using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
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
        static List<string> LastListedEvents;

        static FMOD.Studio.EventInstance CurrentEvent;
        static FMOD.Studio.EventDescription CurrentDescription;


        static Timer tmr16;

        static int counter;
        static bool sixteenthNote;
        static bool wavewriting = false;
        static bool wavewriting_started = false;
        static string wavewriting_path = null;


        static void Main(string[] args)
        {
            FmodErrorCheck(FMOD.Studio.System.create(out FmodSystem));
            FMOD.System lls;
            FmodErrorCheck(FmodSystem.getLowLevelSystem(out lls));
            FmodErrorCheck(FmodSystem.initialize(1024, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));
            ConsoleMain();
            
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
            string text = "FMOD error" + (where == null ? "" : " at " + where) + ": " + res;
            Console.WriteLine(text);
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
        }
        internal static void StartEvent(string @event, bool start = true) 
        {
            if (CurrentEvent != null && @event == null)
            {
                FmodErrorCheck(CurrentEvent.stop(STOP_MODE.IMMEDIATE));
                CurrentEvent = null;
                return;
            }
            if (CurrentEvent != null) FmodErrorCheck(CurrentEvent.stop(STOP_MODE.ALLOWFADEOUT));
            if (@event == null) return;
            CurrentEvent = GetEventInstance(@event, out CurrentDescription);
            sixteenthNote = false;
            counter = -1;

            UpdateProperties(true);
            RecordStartPrep();
            if (start) FmodErrorCheck(CurrentEvent.start());
            FmodSystem.update();


        }
        internal static void UpdateSamples(string filter = "")
        {
            Bank[] banks;

            FmodErrorCheck(FmodSystem.getBankList(out banks), "System.getBanksList");
            List<string> Events = new List<string>();

            Console.WriteLine("Listing events...");
            foreach (Bank b in banks)
            {
                EventDescription[] eds;
                FmodErrorCheck(b.getEventList(out eds), "Bank.getEventList");
                FmodSystem.update();
                foreach (EventDescription evd in eds)
                {
                    string path;
                    FmodErrorCheck(evd.getPath(out path), "EventDescription.getPath");
                    if (path.StartsWith("event:/") && path.Contains(filter))
                    {
                        Events.Add(path);
                    }

                }
            }
            
            Events.Sort();
            LastListedEvents = Events;
            for (int i = 0; i < Events.Count; i++) Console.WriteLine($"{i}: {Events[i]}");
            
            


        }
        //internal static void UpdateTimeline() 
        //{
        //    try
        //    {
        //        PLAYBACK_STATE state = PLAYBACK_STATE.STOPPED;
        //        if (CurrentEvent != null) FmodErrorCheck(CurrentEvent.getPlaybackState(out state));
        //
        //
        //        mainForm.StateLabel.Text =  (wavewriting? (wavewriting_started?"[Recording now] ":"[Recording next] ") : "") + textInfo.ToTitleCase(state.ToString().ToLower());
        //        if ((state == PLAYBACK_STATE.SUSTAINING || state == PLAYBACK_STATE.PLAYING ) && !mainForm.TrackbarMoving) 
        //        {
        //
        //            int timeline;
        //            FmodErrorCheck(CurrentEvent.getTimelinePosition(out timeline));
        //            mainForm.EventTimeline.Value = Math.Min( timeline, mainForm.EventTimeline.Maximum);
        //            mainForm.TimeLabel.Text = $"{ParseTime(timeline)} / {ParseTime(mainForm.EventTimeline.Maximum)}";
        //        }
        //        if (state == PLAYBACK_STATE.STOPPED && wavewriting && wavewriting_started) 
        //        {
        //            wavewriting = false;
        //            FmodSystem.update();
        //            FMOD.System lls;
        //            FmodErrorCheck(FmodSystem.getLowLevelSystem(out lls));
        //            FmodErrorCheck(lls.setOutput(OUTPUTTYPE.AUTODETECT));
        //            wavewriting_started = false;
        //            FmodSystem.update();
        //            SaveFileDialog sfd = new SaveFileDialog();
        //            sfd.Title = "Save recorded WAV";
        //            sfd.Filter = "WAV|*.wav";
        //            sfd.FileName = mainForm.EventName.Text.Replace("event:/", "").Replace("/", "_")+".wav";
        //            if (sfd.ShowDialog() != DialogResult.OK) return;
        //            File.Copy("fmodoutput.wav",sfd.FileName, true);
        //        }
        //    }
        //    catch (FmodException) { }
        //}
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
        internal static void UpdateProperties(bool search16 = false) 
        {
            if (CurrentEvent == null) return;
            CurrentEvent.getParameterCount(out int parameterCount);
            string props = "Properties:\n\n";
            for (int i = 0; i < parameterCount; i++)
            {
                ParameterInstance parameter;
                CurrentEvent.getParameterByIndex(i, out parameter);
                PARAMETER_DESCRIPTION description;
                parameter.getDescription(out description);
                if (search16 && description.name == "sixteenth_note") { sixteenthNote = true; return; }
                else if (search16) continue;
                float value;
                parameter.getValue(out value);
                if (!search16) props += $"{description.name}: "+value.ToString()+ $" (max:{description.minimum}, min:{description.maximum})"+ "\n";
            }

            if (!search16) Console.WriteLine(props);
        }
        internal static void StopEvt() 
        {
            if (CurrentEvent == null) return;
            try { FmodErrorCheck(CurrentEvent.stop(STOP_MODE.IMMEDIATE)); FmodSystem.update(); }
            catch (FmodException) { }
        }
        internal static void PlayEvt() 
        {
            counter = 1;
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
            }
        }
        internal static void SetVolume(int value) 
        {
            if (CurrentEvent == null) return;
            try { FmodErrorCheck(CurrentEvent.setVolume(((float)value) / 100f)); FmodSystem.update(); }
            catch (FmodException) { }
        }
        internal static void SetWaveOut(string path) 
        {
            wavewriting_path = path;
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

        static void ConsoleMain() 
        {
            tmr16 = new Timer();
            tmr16.Interval = 167;
            tmr16.AutoReset = true;
            tmr16.Elapsed += (s, e) => TimerSixteen();
            tmr16.Start();
            Console.Write("Welcome to FmodPlayer (Console version).\nType \"help\" for list of commands.\n\n");
            while (true) 
            {
                try
                {
                    Console.Write("> ");
                    string[] input = SplitString(Console.ReadLine());
                    bool success = false;
                    foreach (CommandDescription cdes in Commands)
                    {
                        if (cdes.Name.StartsWith(input[0]))
                        {
                            success = true;
                            cdes.Callback(input);
                            break;
                        }
                    }
                    if (!success) Console.WriteLine("Unknown command! Type \"help\" for list of commands.");
                }
                catch (Exception e) 
                {
                    Console.WriteLine($"Exception! [{e.GetType().Name}]: {e.Message}\n{e.StackTrace}");
                }
                
            }
        }
        
        static string[] SplitString(string str)
        {
            List<string> strings = new List<string>();
            string stor = "";
            bool quotes = false;
            foreach (char c in str)
            {
                if (c == ' ' && !quotes)
                {
                    strings.Add(stor);
                    stor = "";
                }
                else if (c == '"') quotes = !quotes;
                else stor += c;
            }
            strings.Add(stor);
            return strings.ToArray();
        }

        /// <summary>
        /// Get argument index if it is correct. 
        /// Returns -1 if wrong argument, -2 if optional argument not presented, otherwise argument index
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="posArgs">Allowed arguments</param>
        /// <param name="args">Full argument list</param>
        /// <param name="arg">Argument index</param>
        /// <param name="optional">Whether argument optional or not</param>
        /// <param name="error">Print error in console</param>
        /// <returns></returns>
        static int GetArgumentIndexValue(string name, string[] posArgs, string[] args, int arg, bool optional = false, bool error = true)
        {
           
            if (args.Length > arg)
            {
                for (int n = 0; n < posArgs.Length; n++) if (posArgs[n].StartsWith(args[arg])) return n;

            }
            else if (optional) return -2;
            if (error) Console.WriteLine($"Error in {name}. Wrong argument at position {arg}: it must be {FormatStrArrayAsEnum(posArgs)}");

            return -1;
        }
        static string FormatStrArrayAsEnum(string[] v) 
        {
            string result = "";
            for (int i = 0; i < v.Length; i++)
            {
                if (i != 0) 
                {
                    if (i == v.Length - 1) result += " or ";
                    else result += ", ";
                }
                result += v[i];
            }
            return result;
        }

        class CommandDescription 
        {
            public string Name;
            public string Usage;
            public string Description;
            public Action<string[]> Callback;
        }

        static CommandDescription[] Commands = new CommandDescription[]
        {
            new CommandDescription()
            {
                Name = "help",
                Usage = "[command]",
                Description = "If no command specified, outputs list of commands, otherwise info about command",
                Callback = (args) => 
                {
                    string[] cmds = Commands.Select((c) => { return c.Name; }).ToArray();
                    int idx = GetArgumentIndexValue("help", cmds, args, 1, true, false);
                    if (idx == -1) 
                    {
                        Console.WriteLine($"Wrong command \"{args[1]}\".");
                    }
                    if (idx < 0)
                    {
                        Console.WriteLine("Available commands:\n"+string.Join("\n",cmds));
                        return;
                    }
                    CommandDescription cdes = Commands[idx];
                    Console.WriteLine($"Help on command \"{cdes.Name}\"");
                    Console.WriteLine($"\tUsage: \"{cdes.Name}"+((cdes.Usage != null)? " "+cdes.Usage : ""));
                    Console.WriteLine($"\t {cdes.Description}");

                }
            },
            new CommandDescription()
            {
                Name = "list",
                Usage = "banks|events|properties [filter]",
                Description = "List all loaded events or banks.",
                Callback = (args) => 
                {
                    int idx = GetArgumentIndexValue("list", new string[]{ "banks", "events", "properties" }, args, 1);
                    string filter = "";
                    if (args.Length >= 3) filter = args[2];
                    switch (idx) 
                    {
                        case 0: // list banks
                            throw new NotImplementedException();
                        case 1: // list events
                            UpdateSamples(filter);
                            break;
                        case 2: // list properties of selected event
                            UpdateProperties();
                            break;
                        default:
                            return;
                    }
                }
            },
            new CommandDescription() 
            {
                Name = "add-bank",
                Usage = "pathToBank",
                Description = "Load FMOD bank",
                Callback = (args) => 
                {
                    if (args.Length == 1) { Console.WriteLine("pathToBank argument required!"); return; }
                    AddBank(args[1]);
                }
            },
            new CommandDescription() 
            {
                Name = "select-event",
                Usage = "<Id>|<Path>",
                Description = "Select event by id from last list command or bu its path",
                Callback = (args) => 
                {
                    if (args.Length == 1) 
                    {
                        string p = "";
                        FmodErrorCheck(CurrentDescription.getPath(out p));
                        Console.WriteLine("Selected event: "+p);
                        return;
                    }
                    string path = args[1];
                    if (int.TryParse(path, out int idx)) 
                    {
                        path = LastListedEvents[idx];
                    }
                    StartEvent(path, false);
                }
            },
            new CommandDescription() 
            {
                Name = "play",
                Description = "Play selected event",
                Callback = (args) => 
                {
                    PlayEvt();
                }
            },
            new CommandDescription()
            {
                Name = "stop",
                Description = "Stop selected event",
                Callback = (args) =>
                {
                    StopEvt();
                }
            },
            new CommandDescription()
            {
                Name = "prop",
                Usage = "<Property> <Value>",
                Description = "Set property in selected event",
                Callback = (args) =>
                {
                    if (args.Length < 3) {Console.WriteLine("Two arguments required!");return; }
                    float v;
                    if (!float.TryParse(args[2],out v)) {Console.WriteLine("Value must be float!");return;}
                    SetProperty(args[1],v);
                }
            },
            
        };

        class FmodException : Exception
        {
            public FmodException(string arg) : base(arg) { }
        }


    }
    
}
