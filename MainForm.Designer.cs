namespace FmodPlayer
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem banksToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
            this.AddBank = new System.Windows.Forms.ToolStripMenuItem();
            this.ListRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ListSearch = new System.Windows.Forms.ToolStripTextBox();
            this.TrackList = new System.Windows.Forms.ListBox();
            this.CtrlPanel = new System.Windows.Forms.Panel();
            this.ParameterValue = new System.Windows.Forms.TrackBar();
            this.ParameterName = new System.Windows.Forms.ComboBox();
            this.ParameterList = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.EventName = new System.Windows.Forms.Label();
            this.StateLabel = new System.Windows.Forms.Label();
            this.EventTimeline = new System.Windows.Forms.TrackBar();
            this.Volume = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CtrlPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.CtrlStop = new System.Windows.Forms.ToolStripMenuItem();
            this.WavRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.Timer16 = new System.Windows.Forms.Timer(this.components);
            banksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CtrlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParameterValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EventTimeline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Volume)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // banksToolStripMenuItem
            // 
            banksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBank});
            banksToolStripMenuItem.Name = "banksToolStripMenuItem";
            banksToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            banksToolStripMenuItem.Text = "Banks";
            // 
            // AddBank
            // 
            this.AddBank.Name = "AddBank";
            this.AddBank.Size = new System.Drawing.Size(93, 22);
            this.AddBank.Text = "Add";
            this.AddBank.Click += new System.EventHandler(this.AddBank_Click);
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ListRefresh,
            searchToolStripMenuItem});
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            refreshToolStripMenuItem.Text = "List";
            // 
            // ListRefresh
            // 
            this.ListRefresh.Name = "ListRefresh";
            this.ListRefresh.Size = new System.Drawing.Size(112, 22);
            this.ListRefresh.Text = "Refresh";
            this.ListRefresh.Click += new System.EventHandler(this.ListRefresh_Click);
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ListSearch});
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            searchToolStripMenuItem.Text = "Search";
            // 
            // ListSearch
            // 
            this.ListSearch.Name = "ListSearch";
            this.ListSearch.Size = new System.Drawing.Size(100, 21);
            this.ListSearch.TextChanged += new System.EventHandler(this.ListSearch_TextChanged);
            // 
            // TrackList
            // 
            this.TrackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackList.FormattingEnabled = true;
            this.TrackList.Location = new System.Drawing.Point(0, 24);
            this.TrackList.Name = "TrackList";
            this.TrackList.Size = new System.Drawing.Size(374, 167);
            this.TrackList.TabIndex = 0;
            this.TrackList.SelectedIndexChanged += new System.EventHandler(this.TrackList_SelectedIndexChanged);
            // 
            // CtrlPanel
            // 
            this.CtrlPanel.Controls.Add(this.ParameterValue);
            this.CtrlPanel.Controls.Add(this.ParameterName);
            this.CtrlPanel.Controls.Add(this.ParameterList);
            this.CtrlPanel.Controls.Add(this.TimeLabel);
            this.CtrlPanel.Controls.Add(this.EventName);
            this.CtrlPanel.Controls.Add(this.StateLabel);
            this.CtrlPanel.Controls.Add(this.EventTimeline);
            this.CtrlPanel.Controls.Add(this.Volume);
            this.CtrlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CtrlPanel.Location = new System.Drawing.Point(0, 191);
            this.CtrlPanel.Name = "CtrlPanel";
            this.CtrlPanel.Size = new System.Drawing.Size(374, 92);
            this.CtrlPanel.TabIndex = 1;
            // 
            // ParameterValue
            // 
            this.ParameterValue.Location = new System.Drawing.Point(195, 53);
            this.ParameterValue.Maximum = 0;
            this.ParameterValue.Name = "ParameterValue";
            this.ParameterValue.Size = new System.Drawing.Size(93, 42);
            this.ParameterValue.TabIndex = 11;
            this.ParameterValue.Scroll += new System.EventHandler(this.ParameterValue_Scroll);
            // 
            // ParameterName
            // 
            this.ParameterName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ParameterName.FormattingEnabled = true;
            this.ParameterName.Location = new System.Drawing.Point(82, 57);
            this.ParameterName.Name = "ParameterName";
            this.ParameterName.Size = new System.Drawing.Size(107, 21);
            this.ParameterName.TabIndex = 10;
            this.ParameterName.SelectedIndexChanged += new System.EventHandler(this.ParameterName_SelectedIndexChanged);
            // 
            // ParameterList
            // 
            this.ParameterList.AutoSize = true;
            this.ParameterList.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ParameterList.Location = new System.Drawing.Point(-3, 60);
            this.ParameterList.Name = "ParameterList";
            this.ParameterList.Size = new System.Drawing.Size(84, 15);
            this.ParameterList.TabIndex = 4;
            this.ParameterList.Text = "Properties:";
            // 
            // TimeLabel
            // 
            this.TimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeLabel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TimeLabel.Location = new System.Drawing.Point(114, 23);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TimeLabel.Size = new System.Drawing.Size(218, 15);
            this.TimeLabel.TabIndex = 3;
            this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EventName
            // 
            this.EventName.AutoSize = true;
            this.EventName.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EventName.Location = new System.Drawing.Point(-3, 38);
            this.EventName.Name = "EventName";
            this.EventName.Size = new System.Drawing.Size(0, 15);
            this.EventName.TabIndex = 2;
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StateLabel.Location = new System.Drawing.Point(-3, 23);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(56, 15);
            this.StateLabel.TabIndex = 1;
            this.StateLabel.Text = "Stopped";
            // 
            // EventTimeline
            // 
            this.EventTimeline.Dock = System.Windows.Forms.DockStyle.Top;
            this.EventTimeline.Location = new System.Drawing.Point(0, 0);
            this.EventTimeline.Name = "EventTimeline";
            this.EventTimeline.Size = new System.Drawing.Size(332, 42);
            this.EventTimeline.TabIndex = 0;
            this.EventTimeline.TickStyle = System.Windows.Forms.TickStyle.None;
            this.EventTimeline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EventTimeline_MouseDown);
            this.EventTimeline.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EventTimeline_MouseUp);
            // 
            // Volume
            // 
            this.Volume.Dock = System.Windows.Forms.DockStyle.Right;
            this.Volume.Location = new System.Drawing.Point(332, 0);
            this.Volume.Maximum = 100;
            this.Volume.Name = "Volume";
            this.Volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Volume.Size = new System.Drawing.Size(42, 92);
            this.Volume.TabIndex = 12;
            this.Volume.TickFrequency = 10;
            this.Volume.Value = 100;
            this.Volume.Scroll += new System.EventHandler(this.Volume_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            banksToolStripMenuItem,
            refreshToolStripMenuItem,
            this.playbackToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(374, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CtrlPlay,
            this.CtrlStop,
            this.WavRecord});
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.playbackToolStripMenuItem.Text = "Playback";
            // 
            // CtrlPlay
            // 
            this.CtrlPlay.Name = "CtrlPlay";
            this.CtrlPlay.Size = new System.Drawing.Size(180, 22);
            this.CtrlPlay.Text = "Play/Reset";
            this.CtrlPlay.Click += new System.EventHandler(this.CtrlPlay_Click);
            // 
            // CtrlStop
            // 
            this.CtrlStop.Name = "CtrlStop";
            this.CtrlStop.Size = new System.Drawing.Size(180, 22);
            this.CtrlStop.Text = "Stop";
            this.CtrlStop.Click += new System.EventHandler(this.CtrlStop_Click);
            // 
            // WavRecord
            // 
            this.WavRecord.Name = "WavRecord";
            this.WavRecord.Size = new System.Drawing.Size(180, 22);
            this.WavRecord.Text = "Record next to WAV";
            this.WavRecord.Click += new System.EventHandler(this.WavRecord_Click);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Interval = 20;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // Timer16
            // 
            this.Timer16.Enabled = true;
            this.Timer16.Interval = 167;
            this.Timer16.Tick += new System.EventHandler(this.Timer16_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 283);
            this.Controls.Add(this.TrackList);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.CtrlPanel);
            this.MinimumSize = new System.Drawing.Size(294, 220);
            this.Name = "MainForm";
            this.Text = "Fmod player";
            this.CtrlPanel.ResumeLayout(false);
            this.CtrlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParameterValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EventTimeline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Volume)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AddBank;
        private System.Windows.Forms.ToolStripMenuItem ListRefresh;
        internal System.Windows.Forms.ListBox TrackList;
        internal System.Windows.Forms.ToolStripTextBox ListSearch;
        internal System.Windows.Forms.Timer UpdateTimer;
        internal System.Windows.Forms.TrackBar EventTimeline;
        internal System.Windows.Forms.Label StateLabel;
        internal System.Windows.Forms.Label EventName;
        internal System.Windows.Forms.Label TimeLabel;
        internal System.Windows.Forms.Label ParameterList;
        internal System.Windows.Forms.Panel CtrlPanel;
        internal System.Windows.Forms.TrackBar ParameterValue;
        internal System.Windows.Forms.ComboBox ParameterName;
        private System.Windows.Forms.ToolStripMenuItem playbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CtrlPlay;
        private System.Windows.Forms.ToolStripMenuItem CtrlStop;
        private System.Windows.Forms.Timer Timer16;
        internal System.Windows.Forms.TrackBar Volume;
        private System.Windows.Forms.ToolStripMenuItem WavRecord;
    }
}

