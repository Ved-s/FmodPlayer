using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FMOD;
using FMOD.Studio;

namespace FmodPlayer
{
    public partial class MainForm : Form
    {

        internal bool TrackbarMoving;

        public MainForm()
        {
            
            
            InitializeComponent();
        }
        
        private void AddBank_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "FMOD bank|*.bank|All|*.*";
                ofd.Title = "Select FMOD bank";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Program.AddBank(ofd.FileName);
                }
            }
            catch (FmodException) { }

        }
        
        private void TrackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                Program.StartEvent((string)TrackList.SelectedItem);
                
            }
            catch (FmodException) { }
        }
        private void ListRefresh_Click(object sender, EventArgs e)
        {
            Program.UpdateSamples();
        }
        private void ListSearch_TextChanged(object sender, EventArgs e)
        {
            Program.UpdateSamples();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            Program.UpdateTimeline();
        }

        private void EventTimeline_MouseDown(object sender, MouseEventArgs e)
        {
            TrackbarMoving = true;
        }

        private void EventTimeline_MouseUp(object sender, MouseEventArgs e)
        {
            Program.SetTimeline(EventTimeline.Value);
            TrackbarMoving = false;
        }


        private void ParameterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.SelectProperty((string)ParameterName.SelectedItem);
        }

        private void ParameterValue_Scroll(object sender, EventArgs e)
        {
            Program.SetProperty((string)ParameterName.SelectedItem, ParameterValue.Value);
            Program.UpdateProperties();
        }

        private void CtrlPlay_Click(object sender, EventArgs e)
        {
            Program.PlayEvt();
        }

        private void CtrlStop_Click(object sender, EventArgs e)
        {
            Program.StopEvt();
        }

        private void Timer16_Tick(object sender, EventArgs e)
        {
            Program.TimerSixteen();
        }

        private void Volume_Scroll(object sender, EventArgs e)
        {
            Program.SetVolume(Volume.Value);

        }

        private void WavRecord_Click(object sender, EventArgs e)
        {
            Program.SetWaveOut();
        }
    }
    class FmodException : Exception
    {
        public FmodException( string arg) : base(arg) { }
    }
}
