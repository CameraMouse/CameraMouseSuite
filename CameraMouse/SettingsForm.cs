/*                         Camera Mouse Suite
 *  Copyright (C) 2014, Samual Epstein
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace CameraMouseSuite
{

    public partial class SettingsForm : Form
    {     
        public SettingsForm()
        {
            InitializeComponent();
        }

        private CMSViewAdapter viewAdapter = null;
        private Button enableButton = null;
        private Label enableLabel;
        private CMSControlTogglerConfig togglerConfig = null;

        public void Init(CMSViewAdapter viewAdapter)
        {
            this.loggingInfoControl1.Init(viewAdapter);
            this.viewAdapter = viewAdapter;
            trackingSuiteStandard = viewAdapter.StandardTrackingSuite;
            togglerConfig = viewAdapter.ControlTogglerConfig;
            InitClassicControls();
            InitAdvancedControls();
            AdjustDisplaysToTracker();
        }

        #region Advanced And Classic View Coordination

        void AdjustDisplaysToTracker()
        {
            string selectedSuiteName = viewAdapter.SelectedSuiteName;
            if (selectedSuiteName.Equals(CMSConstants.STANDARD_TRACKING_SUITE_NAME))
            {
                SetClassicViewState(true);
                SetTrackerSelected(false);
                tabControl.SelectTab(this.tabPageClassicView);
            }
            else if (selectedSuiteName.Equals(CMSConstants.EMPTY_TRACKING_SUITE_NAME))
            {
                SetClassicViewState(false);
                SetTrackerSelected(false);
                tabControl.SelectTab(this.tabPageAdvancedView);
            }
            else
            {
                SetClassicViewState(false);
                SetTrackerSelected(true);
                tabControl.SelectTab(this.tabPageAdvancedView);
            }
            //viewAdapter.
            this.Invalidate();
        }

        void enableButton_Click(object sender, EventArgs e)
        {
            viewAdapter.SelectedSuiteName = CMSConstants.STANDARD_TRACKING_SUITE_NAME;
            AdjustDisplaysToTracker();
        }

        void SetClassicViewState(bool enabled)
        {
            if (enabled)
            {
                this.enableButton.Visible = false;
                this.panelClassicContainer.Visible = true;
                this.enableLabel.Visible = false;
                LoadToControls();
            }
            else
            {
                this.enableButton.Visible = true;
                this.panelClassicContainer.Visible = false;
                this.enableLabel.Visible = true;
            }
        }

        #endregion

        #region Advanced View

        private AdvBottomActiveControl bottomActiveControl = null;
        private AdvBottomInActiveControl bottomInActiveControl = null;
        private AdvTrackerSelectionControl trackerSelectionControl = null;

        public void InitAdvancedControls()
        {

            string informalName = viewAdapter.SelectedSuiteInformalName;
            CMSTrackingSuiteIdentifier [] trackingSuiteIdentifiers = viewAdapter.TrackingSuiteIdentifiers;

            if (trackingSuiteIdentifiers == null || trackingSuiteIdentifiers.Length <= 2)
            {
                tabControl.TabPages.Remove(this.tabPageAdvancedView);
                //tabControl.TabPages.Remove(this.tabPageUpdate);
                return;
            }

            bottomActiveControl = new AdvBottomActiveControl();
            bottomActiveControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            bottomActiveControl.EventAbout += new EventHandler(about_Click);
            bottomActiveControl.EventCameraAdvanced += new EventHandler(camera_advanced_Click);
            bottomActiveControl.EventHelp +=new EventHandler(help_Click);
            bottomActiveControl.EventOk += new EventHandler(Ok_Click);
            bottomActiveControl.EventVideoSelectorBtn += new EventHandler(video_selector_btn_Click);
            bottomActiveControl.EventButtonLoad += new EventHandler(buttonLoad_Click);
            bottomActiveControl.EventButtonRestore += new EventHandler(buttonRestore_Click);
            bottomActiveControl.EventButtonSave += new EventHandler(buttonSave_Click);
            
            bottomActiveControl.EventAutoStart += new EventHandler(auto_start_CheckedChanged);
            bottomActiveControl.EventComboBoxSecondsMouseInactivity += new EventHandler(comboBoxSecondsMouseInactivity_SelectedIndexChanged);
            bottomActiveControl.EventGoCtrl += new EventHandler(go_ctrl_CheckedChanged);
            bottomActiveControl.EventGoScroll += new EventHandler(go_scroll_CheckedChanged);
            bottomActiveControl.EventStopOnMove += new EventHandler(stop_on_move_CheckedChanged);
            bottomActiveControl.EventStopScroll += new EventHandler(stopScroll_CheckedChanged);
            bottomActiveControl.EventStopCtrl += new EventHandler(stop_ctrl_CheckedChanged);
            bottomActiveControl.EventNotifySound += new EventHandler(notify_sound_CheckedChanged);

            bottomActiveControl.TogglerConfig = togglerConfig;
            bottomActiveControl.LoadFromControls();
            bottomActiveControl.Init(SendTrackerLog);

            bottomInActiveControl = new AdvBottomInActiveControl();
            bottomInActiveControl.EventAbout += new EventHandler(about_Click);
            bottomInActiveControl.EventCameraAdvanced += new EventHandler(camera_advanced_Click);
            bottomInActiveControl.EventHelp += new EventHandler(help_Click);
            bottomInActiveControl.EventOk += new EventHandler(Ok_Click);
            bottomInActiveControl.EventVideoSelectorBtn += new EventHandler(video_selector_btn_Click);
            bottomInActiveControl.Location = new Point(6, 292);
            bottomInActiveControl.Anchor = System.Windows.Forms.AnchorStyles.Top;

            trackerSelectionControl = new AdvTrackerSelectionControl();
            trackerSelectionControl.SetNames(informalName, trackingSuiteIdentifiers);
            trackerSelectionControl.EventSelectTrackerButton += new EventHandler(trackerSelectionControl_EventSelectTrackerButton);
            trackerSelectionControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.trackerSelectionControl.Location = new System.Drawing.Point(6, 6);
        }

        void trackerSelectionControl_EventSelectTrackerButton(object sender, EventArgs e)
        {
            string selectedSuiteName = viewAdapter.SelectedSuiteName;
            
            if (selectedSuiteName.Equals(CMSConstants.STANDARD_TRACKING_SUITE_NAME) ||
                selectedSuiteName.Equals(CMSConstants.EMPTY_TRACKING_SUITE_NAME))
            {
                string curName = trackerSelectionControl.SelectedFormalName;
                if (curName == null || curName.Equals(selectedSuiteName))
                    return;
                viewAdapter.SelectedSuiteName = curName;                

            }
            else
            {
                viewAdapter.SelectedSuiteName = CMSConstants.EMPTY_TRACKING_SUITE_NAME;                
            }
            AdjustDisplaysToTracker();
            this.Invalidate();
        }

        private string selectedSuiteName = null;
        
        /// <summary>
        /// Toggles Whether or not the advanced view shows a tracker.
        /// </summary>
        /// <param name="trackerSelected"></param>
        public void SetTrackerSelected(bool trackerSelected)
        {
            if (trackerSelectionControl == null)
                return;

            this.SuspendLayout();
            this.tabControl.SuspendLayout();
            tabPageAdvancedView.SuspendLayout();
            
            trackerSelectionControl.SetActive(!trackerSelected);
            if (trackerSelected)
            {   
                CMSTrackingSuite currentTracker = viewAdapter.GetCurrentTrackingSuite();
               
                if(currentTracker == null)
                    throw new Exception("No current tracker");

                if (currentTracker.Name.Equals(selectedSuiteName))
                {
                    foreach (Control control in tabPageAdvancedView.Controls)
                    {
                        CMSConfigPanel configPanel = control as CMSConfigPanel;
                        if (configPanel != null)
                        {
                            configPanel.Init(SendTrackerLog);
                            configPanel.LoadFromControls();
                        }
                    }
                }
                else
                {

                    selectedSuiteName = currentTracker.Name;

                    tabPageAdvancedView.Controls.Clear();
                    int maxWidth = trackerSelectionControl.Width;
                    int totalHeight = 3 + trackerSelectionControl.Height + this.bottomActiveControl.Height;
                    CMSConfigPanel[] trackerPanels = currentTracker.GetPanels();

                    foreach (CMSConfigPanel configPanel in trackerPanels)
                    {
                        if (configPanel == null)
                            continue;
                        configPanel.Init(SendTrackerLog);
                        Control trackerPanel = configPanel as Control;
                        if (trackerPanel == null)
                            continue;
                        if (trackerPanel.Width > maxWidth)
                            maxWidth = trackerPanel.Width;
                        totalHeight += trackerPanel.Height;
                    }
                    totalHeight += 3;
                    int totalWidth = 12 + maxWidth;
                    tabPageAdvancedView.Size = new Size(totalWidth, totalHeight);
                    tabControl.Size = new Size(totalWidth + 8, totalHeight + 26);
                    Size = new Size(totalWidth + 19, totalHeight + 55);

                    Point trackerSelectionControlPt = new Point((totalWidth - trackerSelectionControl.Width) / 2, 3);
                    trackerSelectionControl.Location = trackerSelectionControlPt;

                    int curHeight = 3 + trackerSelectionControl.Height;

                    tabPageAdvancedView.Controls.Add(trackerSelectionControl);

                    foreach (Control trackerPanel in trackerPanels)
                    {
                        if (trackerPanel == null)
                            continue;
                        Point curPt = new Point((totalWidth - trackerPanel.Width) / 2, curHeight);
                        trackerPanel.Location = curPt;
                        tabPageAdvancedView.Controls.Add(trackerPanel);
                        curHeight += trackerPanel.Height;
                    }
                    Point bottomActivePt = new Point((totalWidth - this.bottomActiveControl.Width) / 2, curHeight);
                    bottomActiveControl.Location = bottomActivePt;
                    tabPageAdvancedView.Controls.Add(bottomActiveControl);
                }
            }
            else
            {
                tabPageAdvancedView.Controls.Clear();
                selectedSuiteName = null;
                trackerSelectionControl.Location = new Point(6, 3);
                tabPageAdvancedView.Controls.Add(trackerSelectionControl);
                Size = new Size(697, 435);
                tabControl.Size = new Size(686, 406);
                tabPageAdvancedView.Size = new Size(678, 380);
                tabPageAdvancedView.Controls.Add(bottomInActiveControl);
            }
            this.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            tabPageAdvancedView.ResumeLayout(false);
            tabPageAdvancedView.PerformLayout();
            Invalidate();
        }

        #endregion

        #region Classic View

        private void InitClassicControls()
        {
            LoadToControls();

            this.enableLabel = new Label();
            this.enableLabel.AutoSize = true;
            this.enableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableLabel.Location = new System.Drawing.Point(181, 91);
            this.enableLabel.Name = "labelDummy";
            this.enableLabel.Size = new System.Drawing.Size(338, 25);
            this.enableLabel.Text = "Classic View currently is disabled.";
            this.enableLabel.Visible = false;
            this.tabControl.TabPages[0].Controls.Add(enableLabel);

            this.enableButton = new Button();
            this.enableButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableButton.Location = new System.Drawing.Point(235, 189);
            this.enableButton.Name = "buttonDummy";
            this.enableButton.Size = new System.Drawing.Size(206, 30);
            this.enableButton.Text = "Enable Classic View";
            this.enableButton.UseVisualStyleBackColor = true;
            this.enableButton.Click += new EventHandler(enableButton_Click);
            this.enableButton.Visible = false;
            this.tabControl.TabPages[0].Controls.Add(enableButton);

        }

        private bool loadingControls = false;
        private CMSTrackingSuiteStandard trackingSuiteStandard = null;
        
        private void LoadToControls()
        {
            if (trackingSuiteStandard == null)
                throw new Exception("Tracking suite is null");

            loadingControls = true;

            CMSTrackingModuleStandard trackingModule = trackingSuiteStandard.StandardTracking;
            CMSMouseControlModuleStandard mouseControlModule = trackingSuiteStandard.StandardMouseControl;
            CMSClickControlModuleStandard clickControlModule = trackingSuiteStandard.StandardClickControl;


            checkBoxReverseMouse.Checked = mouseControlModule.ReverseHorizontal;
            clicking.Checked = clickControlModule.ClickEnabled;
            clickRadius.Enabled = clickControlModule.ClickEnabled;
            dwell.Enabled = clickControlModule.ClickEnabled;
            click_sound.Enabled = clickControlModule.ClickEnabled;
            click_sound.Checked = clickControlModule.PlaySound;

            comboBoxSecondsMouseInactivity.SelectedItem = togglerConfig.AutoStartDelay.ToString();
            auto_start.Checked = togglerConfig.AutoStartControlEnabled;
            go_ctrl.Checked = togglerConfig.CtrlStart;
            go_scroll.Checked = togglerConfig.ScrollStart;
            stopScroll.Checked = togglerConfig.ScrollStop;
            stop_ctrl.Checked = togglerConfig.CtrlStop;
            stop_on_move.Checked = togglerConfig.AutoStopControlEnabled;
            notify_sound.Checked = togglerConfig.PlaySoundOnControlChanges;

            double val = Math.Round(100.0F * mouseControlModule.EastLimit);
            string temp = ((int)(val)).ToString() + "%";
            this.exclude_E.SelectedItem = temp;

            val = Math.Round(100.0F * mouseControlModule.WestLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_W.SelectedItem = temp;

            val = Math.Round(100.0F * mouseControlModule.NorthLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_N.SelectedItem = temp;

            val = Math.Round(100.0F * mouseControlModule.SouthLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_S.SelectedItem = temp;

            long lval = clickControlModule.DwellTime;
            if (lval == 100)
                this.dwell.SelectedItem = "0.1 Sec";
            else if (lval == 250)
                this.dwell.SelectedItem = "0.25 Sec";
            else if (lval == 500)
                this.dwell.SelectedItem = "0.5 Sec";
            else if (lval == 750)
                this.dwell.SelectedItem = "0.75 Sec";
            else if (lval == 1000)
                this.dwell.SelectedItem = "1 Sec";
            else if (lval == 1500)
                this.dwell.SelectedItem = "1.5 Sec";
            else if (lval == 2000)
                this.dwell.SelectedItem = "2 Sec";
            else if (lval == 3000)
                this.dwell.SelectedItem = "3 Sec";

            val = clickControlModule.Radius;
            if (val == 0.025)
                this.clickRadius.SelectedItem = "Small";
            else if (val == 0.05)
                this.clickRadius.SelectedItem = "Normal";
            else if (val == 0.075)
                this.clickRadius.SelectedItem = "Large";

            val = mouseControlModule.UserHorizontalGain;
            if (val == 3.0)
                this.Horiz_gain.SelectedItem = "Very Low";
            else if (val == 4.5)
                this.Horiz_gain.SelectedItem = "Low";
            else if (val == 6.0)
                this.Horiz_gain.SelectedItem = "Med";
            else if (val == 7.5)
                this.Horiz_gain.SelectedItem = "Med High";
            else if (val == 9.0)
                this.Horiz_gain.SelectedItem = "High";
            else if (val == 10.5)
                this.Horiz_gain.SelectedItem = "Very High";
            else if (val == 12.0)
                this.Horiz_gain.SelectedItem = "Extreme";

            val = mouseControlModule.UserVerticalGain;
            if (val == 3.0)
                this.vert_gain.SelectedItem = "Very Low";
            else if (val == 4.5)
                this.vert_gain.SelectedItem = "Low";
            else if (val == 6.0)
                this.vert_gain.SelectedItem = "Med";
            else if (val == 7.5)
                this.vert_gain.SelectedItem = "Med High";
            else if (val == 9.0)
                this.vert_gain.SelectedItem = "High";
            else if (val == 10.5)
                this.vert_gain.SelectedItem = "Very High";
            else if (val == 12.0)
                this.vert_gain.SelectedItem = "Extreme";

            val = mouseControlModule.Damping;
            if (val == 1.0)
                this.smooth.SelectedItem = "Off";
            else if (val == 0.95)
                this.smooth.SelectedItem = "Very Low";
            else if (val == 0.8)
                this.smooth.SelectedItem = "Low";
            else if (val == 0.65)
                this.smooth.SelectedItem = "Med";
            else if (val == 0.5)
                this.smooth.SelectedItem = "Med High";
            else if (val == 0.3)
                this.smooth.SelectedItem = "High";
            else if (val == 0.15)
                this.smooth.SelectedItem = "Very High";
            else if (val == 0.05)
                this.smooth.SelectedItem = "Extreme";

            this.comboBoxAutoStart.SelectedItem = trackingModule.AutoStartMode.ToString();

            loadingControls = false;

        }
       
        #region Classic GUI Widgets Changed

        private void comboBoxAutoStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                trackingSuiteStandard.StandardTracking.AutoStartMode =
                    (AutoStartMode)Enum.Parse(typeof(AutoStartMode), comboBoxAutoStart.SelectedItem.ToString());
                SendTrackerLog();
            }
        }

        private void clicking_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                bool enabled = clicking.Checked;
                this.trackingSuiteStandard.StandardClickControl.ClickEnabled = enabled;
                clickRadius.Enabled = enabled;
                dwell.Enabled = enabled;
                click_sound.Enabled = enabled;
                SendTrackerLog();
            }
        }

        private void clickRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                double val = 0.05;
                string temp = this.clickRadius.SelectedItem.ToString();
                if (temp.Equals("Small"))
                    val = 0.025;
                else if (temp.Equals("Normal"))
                    val = 0.05;
                else if (temp.Equals("Large"))
                    val = 0.075;

                trackingSuiteStandard.StandardClickControl.Radius = val;
                SendTrackerLog();
            }
        }

        private void dwell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                long val = 1000;
                string temp = this.dwell.SelectedItem.ToString();
                if (temp.Equals("0.1 Sec"))
                    val = 100;
                else if (temp.Equals("0.25 Sec"))
                    val = 250;
                else if (temp.Equals("0.5 Sec"))
                    val = 500;
                else if (temp.Equals("0.75 Sec"))
                    val = 750;
                else if (temp.Equals("1 Sec"))
                    val = 1000;
                else if (temp.Equals("1.5 Sec"))
                    val = 1500;
                else if (temp.Equals("2 Sec"))
                    val = 2000;
                else if (temp.Equals("3 Sec"))
                    val = 3000;
                trackingSuiteStandard.StandardClickControl.DwellTime = val;
                SendTrackerLog();
            }
        }

        private void click_sound_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                trackingSuiteStandard.StandardClickControl.PlaySound = click_sound.Checked;
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void Horiz_gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.loadingControls)
            {
                double val = 1;
                string temp = this.Horiz_gain.SelectedItem.ToString();
                if (temp.Equals("Very Low"))
                    val = 3;
                else if (temp.Equals("Low"))
                    val = 4.5;
                else if (temp.Equals("Med"))
                    val = 6.0;
                else if (temp.Equals("Med High"))
                    val = 7.5;
                else if (temp.Equals("High"))
                    val = 9.0;
                else if (temp.Equals("Very High"))
                    val = 10.5;
                else if (temp.Equals("Extreme"))
                    val = 12.0;
                trackingSuiteStandard.StandardMouseControl.UserHorizontalGain = val;
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void vert_gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                double val = 1;
                string temp = this.vert_gain.SelectedItem.ToString();
                if (temp.Equals("Very Low"))
                    val = 3;
                else if (temp.Equals("Low"))
                    val = 4.5;
                else if (temp.Equals("Med"))
                    val = 6.0;
                else if (temp.Equals("Med High"))
                    val = 7.5;
                else if (temp.Equals("High"))
                    val = 9.0;
                else if (temp.Equals("Very High"))
                    val = 10.5;
                else if (temp.Equals("Extreme"))
                    val = 12.0;
                trackingSuiteStandard.StandardMouseControl.UserVerticalGain = val;
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void smooth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string val = smooth.SelectedItem.ToString();
                if (val.Equals("Off"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 1.0;
                else if (val.Equals("Very Low"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.95;
                else if (val.Equals("Low"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.80;
                else if (val.Equals("Med"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.65;
                else if (val.Equals("Med High"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.5;
                else if (val.Equals("High"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.3;
                else if (val.Equals("Very High"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.15;
                else if (val.Equals("Extreme"))
                    trackingSuiteStandard.StandardMouseControl.Damping = 0.05;
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void exclude_N_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string s = exclude_N.SelectedItem.ToString();
                s = s.Substring(0, s.Length - 1);
                if (s.Length == 1)
                    s = "0.0" + s;
                else
                    s = "0." + s;
                trackingSuiteStandard.StandardMouseControl.NorthLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void exclude_W_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string s = exclude_W.SelectedItem.ToString();
                s = s.Substring(0, s.Length - 1);
                if (s.Length == 1)
                    s = "0.0" + s;
                else
                    s = "0." + s;
                trackingSuiteStandard.StandardMouseControl.WestLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void exclude_E_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string s = exclude_E.SelectedItem.ToString();
                s = s.Substring(0, s.Length - 1);
                if (s.Length == 1)
                    s = "0.0" + s;
                else
                    s = "0." + s;
                trackingSuiteStandard.StandardMouseControl.EastLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void exclude_S_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string s = exclude_S.SelectedItem.ToString();
                s = s.Substring(0, s.Length - 1);
                if (s.Length == 1)
                    s = "0.0" + s;
                else
                    s = "0." + s;
                trackingSuiteStandard.StandardMouseControl.SouthLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        private void ReverseMouse_CheckedChanged(object sender, EventArgs e)
        {
            if(!loadingControls)
            {
                trackingSuiteStandard.StandardMouseControl.ReverseHorizontal = this.checkBoxReverseMouse.Checked;
                this.trackingSuiteStandard.SendSuiteLogEvent();
            }
        }

        // Buttons
        #endregion


        #endregion

        #region ToggleControls

        private void auto_start_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox autoStartBox = sender as CheckBox;
                if (autoStartBox == null)
                    return;
                this.togglerConfig.AutoStartControlEnabled = autoStartBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void stop_on_move_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox stopOnMoveBox = sender as CheckBox;
                if (stopOnMoveBox == null)
                    return;
                this.togglerConfig.AutoStopControlEnabled = stopOnMoveBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void go_scroll_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox goScrollBox = sender as CheckBox;
                if (goScrollBox == null)
                    return;
                this.togglerConfig.ScrollStart = goScrollBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void stopScroll_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox stopScrollBox = sender as CheckBox;
                if (stopScrollBox == null)
                    return;
                this.togglerConfig.ScrollStop = stopScrollBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void go_ctrl_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox goCtrlBox = sender as CheckBox;
                if (goCtrlBox == null)
                    return;
                this.togglerConfig.CtrlStart = goCtrlBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void stop_ctrl_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox stopCtrlBox = sender as CheckBox;
                if (stopCtrlBox == null)
                    return;
                this.togglerConfig.CtrlStop = stopCtrlBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void notify_sound_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                CheckBox notifySoundBox = sender as CheckBox;
                if (notifySoundBox == null)
                    return;
                this.togglerConfig.PlaySoundOnControlChanges = notifySoundBox.Checked;
                SendTogglerConfigLogEvent();
            }
        }

        private void comboBoxSecondsMouseInactivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                ComboBox secondsMouseBox = sender as ComboBox;
                if (secondsMouseBox == null)
                    return;
                this.togglerConfig.AutoStartDelay = secondsMouseBox.SelectedIndex + 1;
                SendTogglerConfigLogEvent();
            }
        }

        #endregion

        #region CommonControls

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            CMSTrackingSuite curTrackingSuite = viewAdapter.GetCurrentTrackingSuite();
            CMSTrackingSuite defaultTrackingSuite = System.Activator.CreateInstance(curTrackingSuite.GetType()) as CMSTrackingSuite;
            viewAdapter.UpdateTrackingSuite(defaultTrackingSuite);
            viewAdapter.ControlTogglerConfig = new CMSControlTogglerConfig();
            viewAdapter.GetCurrentTrackingSuite().SendSuiteLogEvent();
            SendTogglerConfigLogEvent();
            AdjustDisplaysToTracker();
        }

        private void video_selector_btn_Click(object sender, EventArgs e)
        {
            viewAdapter.ChooseNewWebCam();
        }

        private void camera_advanced_Click(object sender, EventArgs e)
        {
            viewAdapter.DisplayCameraPropertyPage();
        }

        private void about_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.viewAdapter.SaveSuiteAndConfig();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            viewAdapter.LoadSuiteAndTogglerConfig();
            AdjustDisplaysToTracker();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
        
        private void help_Click(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process P = new System.Diagnostics.Process();
                string filename = Environment.CurrentDirectory+"\\"+CMSConstants.CAMERA_MOUSE_MANUAL_PDF;
                P.StartInfo.FileName = CMSConstants.CAMERA_MOUSE_MANUAL_PDF;
                P.Start();
            }
            catch(Exception ee)
            {
            }



        }

        #endregion

        private void SendTogglerConfigLogEvent()
        {
            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogTogglerConfigEvent"))
            {
                CMSLogTogglerConfigEvent togglerConfigEvent = new CMSLogTogglerConfigEvent();
                togglerConfigEvent.Config = togglerConfig;
                CMSLogger.SendLogEvent(togglerConfigEvent);
            }
        }
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Equals(this.tabPageLogging))
                loggingInfoControl1.OnView();
        }
        private void SendTrackerLog()
        {
            viewAdapter.GetCurrentTrackingSuite().SendSuiteLogEvent();
        }

    }
}
