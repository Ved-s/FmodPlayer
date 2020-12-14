using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using FMOD;
using FMOD.Studio;

namespace FmodPlayer
{
    static class Program
    {
        static FMOD.Studio.System FmodSystem;
        static Dictionary<string, FMOD.Studio.EventInstance> EventCache = new Dictionary<string, EventInstance>();
        static Dictionary<string, FMOD.Studio.EventDescription> EventDescriptionCache = new Dictionary<string, EventDescription>();
        
        static List<string> LoadedMasterBankDirs = new List<string>();

        static FMOD.Studio.EventInstance CurrentEvent;
        static FMOD.Studio.EventDescription CurrentDescription;


        static MainForm mainForm;

        static TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
        static int counter;
        static bool sixteenthNote;
        static bool wavewriting = false;
        static bool wavewriting_started = false;

        [STAThread]
        static void Main()
        {
            FmodErrorCheck(FMOD.Studio.System.create(out FmodSystem));
            FMOD.System lls;
            FmodErrorCheck(FmodSystem.getLowLevelSystem(out lls));
            //FmodErrorCheck(lls.setDSPBufferSize(480, 2), "System.setDSPBufferSize");
            FmodErrorCheck(FmodSystem.initialize(1024, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm = new MainForm());
        }
        static EventInstance GetEventInstance(string path) 
        {
            return GetEventInstance(path, out _);
        }
        static EventInstance GetEventInstance(string path, out EventDescription eventDescription)
        {
            try
            {
                EventInstance result;
                if (EventCache.TryGetValue(path, out result)) { EventDescriptionCache.TryGetValue(path, out eventDescription); return result; }
                eventDescription = GetEventDescription(path);
                FmodErrorCheck(eventDescription.createInstance(out result));
                result.set3DAttributes(default);
                EventCache.Add(path, result);
                return result;
            }
            catch (FmodException) { eventDescription = null; return null; }
        }
        static EventDescription GetEventDescription(string path)
        {
            try
            {
                EventDescription result;
                if (EventDescriptionCache.TryGetValue(path, out result)) return result;
                FmodErrorCheck(FmodSystem.getEvent(path, out result));
                FmodErrorCheck(result.loadSampleData());
                EventDescriptionCache.Add(path, result);
                return result;

            }
            catch (FmodException) { return null; }
        }
        static void FmodErrorCheck(RESULT res, string where = null)
        {
            if (res == RESULT.OK) return;
            MessageBox.Show(mainForm, "FMOD error" + (where == null ? "" : " at " + where) + ": " + res, "FMOD");
            throw new FmodException(res.ToString());
        }

        internal static void AddBank(string path, bool loadStrings = false) 
        {
            string directory = Path.GetDirectoryName(path);
            if (!LoadedMasterBankDirs.Contains(directory))
            {
                LoadedMasterBankDirs.Add(directory);
                AddBank(directory + "/Master Bank.bank", true);
            }


            Bank b;
            FmodErrorCheck(FmodSystem.loadBankFile(path, LOAD_BANK_FLAGS.NORMAL, out b));
            FmodErrorCheck(b.loadSampleData());
            if (loadStrings) FmodErrorCheck(FmodSystem.loadBankFile(path.Replace(".bank", ".strings.bank"), LOAD_BANK_FLAGS.NORMAL, out _));
            UpdateSamples();
        }
        internal static void StartEvent(string @event) 
        {
            if (CurrentEvent != null && @event == null)
            {
                FmodErrorCheck(CurrentEvent.stop(STOP_MODE.IMMEDIATE));
                CurrentEvent = null;
                return;
            }
            if (CurrentEvent != null) FmodErrorCheck(CurrentEvent.stop(STOP_MODE.ALLOWFADEOUT));
            if (@event == null) return;
            mainForm.EventName.Text = @event;
            CurrentEvent = GetEventInstance(@event, out CurrentDescription);
            int eventLength;
            FmodErrorCheck(CurrentDescription.getLength(out eventLength));
            mainForm.EventTimeline.Maximum = eventLength;
            SelectProperty(null);
            sixteenthNote = false;
            counter = -1;
            UpdateProperties(true);



            FmodErrorCheck(CurrentEvent.setVolume((float)mainForm.Volume.Value / 100f));
            RecordStartPrep();
            FmodErrorCheck(CurrentEvent.start());
            FmodSystem.update();


        }
        internal static void UpdateSamples()
        {

            mainForm.TrackList.Items.Clear();
            Bank[] banks;

            FmodErrorCheck(FmodSystem.getBankList(out banks), "System.getBanksList");
            List<string> Events = new List<string>();
            foreach (Bank b in banks)
            {
                EventDescription[] eds;
                FmodErrorCheck(b.getEventList(out eds), "Bank.getEventList");
                FmodSystem.update();
                foreach (EventDescription evd in eds)
                {
                    string path;
                    FmodErrorCheck(evd.getPath(out path), "EventDescription.getPath");
                    if (path.StartsWith("event:/") && path.Contains(mainForm.ListSearch.Text))
                    {

                        Events.Add(path);
                    }

                }
            }
            Events.Sort();
            mainForm.TrackList.Items.AddRange(Events.ToArray());


        }
        internal static void UpdateTimeline() 
        {
            try
            {
                PLAYBACK_STATE state = PLAYBACK_STATE.STOPPED;
                if (CurrentEvent != null) FmodErrorCheck(CurrentEvent.getPlaybackState(out state));


                mainForm.StateLabel.Text =  (wavewriting? (wavewriting_started?"[Recording now] ":"[Recording next] ") : "") + textInfo.ToTitleCase(state.ToString().ToLower());
                if ((state == PLAYBACK_STATE.SUSTAINING || state == PLAYBACK_STATE.PLAYING ) && !mainForm.TrackbarMoving) 
                {

                    int timeline;
                    FmodErrorCheck(CurrentEvent.getTimelinePosition(out timeline));
                    mainForm.EventTimeline.Value = Math.Min( timeline, mainForm.EventTimeline.Maximum);
                    mainForm.TimeLabel.Text = $"{ParseTime(timeline)} / {ParseTime(mainForm.EventTimeline.Maximum)}";
                }
                if (state == PLAYBACK_STATE.STOPPED && wavewriting && wavewriting_started) 
                {
                    wavewriting = false;
                    FmodSystem.update();
                    FMOD.System lls;
                    FmodErrorCheck(FmodSystem.getLowLevelSystem(out lls));
                    FmodErrorCheck(lls.setOutput(OUTPUTTYPE.AUTODETECT));
                    wavewriting_started = false;
                    FmodSystem.update();
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Save recorded WAV";
                    sfd.Filter = "WAV|*.wav";
                    sfd.FileName = mainForm.EventName.Text.Replace("event:/", "").Replace("/", "_")+".wav";
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    File.Copy("fmodoutput.wav",sfd.FileName, true);
                }
            }
            catch (FmodException) { }
        }
        internal static void SetTimeline(int value) 
        {
            if (CurrentEvent == null) return;
            try
            {
                FmodErrorCheck( CurrentEvent.setTimelinePosition(value));
            }
            catch (FmodException) { }
           
        }
        static string ParseTime(int time) 
        {
            int ms = time % 1000;
            time /= 1000;
            int s = time % 60;
            time /= 60;
            int m = time % 60;
            int h = time / 60;

            return ((h > 0) ? h.ToString("0")+":" : "") + ((m > 0 || h > 0) ? m.ToString("00")+":" : "") + s.ToString("00")+"." + ms.ToString("000");
        }
        internal static void SetProperty(string name, float value) 
        {
            try 
            {
                if (CurrentEvent == null) return;
                FmodErrorCheck(CurrentEvent.setParameterValue(name, value));
                FmodErrorCheck(FmodSystem.update());
            } catch (FmodException) { }
        }
        internal static void UpdateProperties(bool combo = false) 
        {
            if (CurrentEvent == null) return;
            CurrentEvent.getParameterCount(out int parameterCount);
            string props = "Properties:\n\n";
            if (combo) mainForm.ParameterName.Items.Clear();
            for (int i = 0; i < parameterCount; i++)
            {
                ParameterInstance parameter;
                CurrentEvent.getParameterByIndex(i, out parameter);
                PARAMETER_DESCRIPTION description;
                parameter.getDescription(out description);
                float value;
                parameter.getValue(out value);
                props += $"{description.name}: "+((description.name == "sixteenth_note") ? (counter.ToString() + "/257") :value.ToString())+"\n";
                if (combo) mainForm.ParameterName.Items.Add(description.name);
                if (combo && description.name == "sixteenth_note") sixteenthNote = true;
            }
            mainForm.ParameterList.Text = props;
            mainForm.CtrlPanel.Height = mainForm.ParameterList.Height + 65;
        }
        internal static void SelectProperty(string name) 
        {
            if (CurrentEvent == null) return;
            try
            {
                if (name == null) 
                {
                    mainForm.ParameterValue.Minimum =
                    mainForm.ParameterValue.Value =
                    mainForm.ParameterValue.Maximum = 0;
                    mainForm.ParameterName.SelectedItem = null;
                    return;
                }
                FmodErrorCheck(CurrentEvent.getParameter(name, out ParameterInstance param));
                FmodErrorCheck(param.getDescription(out PARAMETER_DESCRIPTION descr));
                mainForm.ParameterValue.Maximum = (int)descr.maximum;
                mainForm.ParameterValue.Minimum = (int)descr.minimum;
                FmodErrorCheck(param.getValue(out float value));
                mainForm.ParameterValue.Value = (int)value;
            }
            catch (FmodException) { }
        }
        internal static void StopEvt() 
        {
            if (CurrentEvent == null) return;
            try { FmodErrorCheck(CurrentEvent.stop(STOP_MODE.IMMEDIATE)); FmodSystem.update(); }
            catch (FmodException) { }
        }
        internal static void PlayEvt() 
        {
            counter = -1;
            if (CurrentEvent == null) return;
            try { RecordStartPrep(); FmodErrorCheck( CurrentEvent.start()); FmodSystem.update(); }
            catch (FmodException) { }
        }
        internal static void TimerSixteen() 
        {
            if (sixteenthNote)
            {
                if (counter >= 0) SetProperty("sixteenth_note", counter);
                counter++;
                if (counter >= 257) 
                {
                    counter = 0;
                    CurrentEvent.stop(STOP_MODE.IMMEDIATE);
                }
                UpdateProperties();
            }
        }
        internal static void SetVolume(int value) 
        {
            if (CurrentEvent == null) return;
            try { FmodErrorCheck(CurrentEvent.setVolume(((float)value) / 100f)); FmodSystem.update(); }
            catch (FmodException) { }
        }
        internal static void SetWaveOut() 
        {
            if (wavewriting_started) return;
            wavewriting = !wavewriting;

        }
        static void RecordStartPrep() 
        {
            if (!wavewriting || wavewriting_started) return;
            FMOD.System lls;
            FmodErrorCheck(FmodSystem.getLowLevelSystem(out lls));
            FmodErrorCheck(lls.setOutput(OUTPUTTYPE.WAVWRITER));
            wavewriting_started = true;
        }




    }
    
}
