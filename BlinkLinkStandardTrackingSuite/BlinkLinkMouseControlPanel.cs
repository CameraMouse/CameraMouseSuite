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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    public partial class BlinkLinkMouseControlPanel : UserControl, CMSConfigPanel
    {
        public BlinkLinkMouseControlPanel()
        {
            InitializeComponent();
        }

        private BlinkLinkMouseControlModule blinkLinkClickControlModule = null;
        public void SetMouseControl(BlinkLinkMouseControlModule standardMouseControl)
        {
            this.blinkLinkClickControlModule = standardMouseControl;
            LoadFromControls();
        }

        private bool loadingControls = false;

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
                blinkLinkClickControlModule.UserHorizontalGain = val;
                sendLogAdvancedTracker();
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
                blinkLinkClickControlModule.UserVerticalGain = val;
                sendLogAdvancedTracker();
            }
        }

        private void smooth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string val = smooth.SelectedItem.ToString();
                if (val.Equals("Off"))
                    blinkLinkClickControlModule.Damping = 1.0;
                else if (val.Equals("Very Low"))
                    blinkLinkClickControlModule.Damping = 0.95;
                else if (val.Equals("Low"))
                    blinkLinkClickControlModule.Damping = 0.80;
                else if (val.Equals("Med"))
                    blinkLinkClickControlModule.Damping = 0.65;
                else if (val.Equals("Med High"))
                    blinkLinkClickControlModule.Damping = 0.5;
                else if (val.Equals("High"))
                    blinkLinkClickControlModule.Damping = 0.3;
                else if (val.Equals("Very High"))
                    blinkLinkClickControlModule.Damping = 0.15;
                else if (val.Equals("Extreme"))
                    blinkLinkClickControlModule.Damping = 0.05;
                sendLogAdvancedTracker();
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
                blinkLinkClickControlModule.NorthLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                sendLogAdvancedTracker();
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
                blinkLinkClickControlModule.WestLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                sendLogAdvancedTracker();
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
                blinkLinkClickControlModule.EastLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                sendLogAdvancedTracker();
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
                blinkLinkClickControlModule.SouthLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                sendLogAdvancedTracker();
            }
        }

        private void ReverseMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                blinkLinkClickControlModule.ReverseHorizontal = this.checkBoxReverseMouse.Checked;
                sendLogAdvancedTracker();
            }
        }

        private void moveMouseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                bool check = moveMouseCheckBox.Checked;
                blinkLinkClickControlModule.MoveMouse = check;
                ChangeMouseControlItemsEnabled(check);
                sendLogAdvancedTracker();
            }
        }

        private void ChangeMouseControlItemsEnabled(bool enabled)
        {
            smooth.Enabled = enabled;
            vert_gain.Enabled = enabled;
            Horiz_gain.Enabled = enabled;
            exclude_E.Enabled = enabled;
            exclude_N.Enabled = enabled;
            exclude_S.Enabled = enabled;
            exclude_W.Enabled = enabled;
            checkBoxReverseMouse.Enabled = enabled;

            pauseMouseCheckBox.Checked = blinkLinkClickControlModule.PauseMouseEnabled;

            pauseMouseCheckBox.Enabled = enabled;

        }

        private void pauseMouseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                blinkLinkClickControlModule.PauseMouseEnabled = pauseMouseCheckBox.Checked;
                sendLogAdvancedTracker();
            }
        }

        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            loadingControls = true;

            moveMouseCheckBox.Checked = blinkLinkClickControlModule.MoveMouse;
            ChangeMouseControlItemsEnabled(blinkLinkClickControlModule.MoveMouse);

            double val = Math.Round(100.0F * blinkLinkClickControlModule.EastLimit);
            string temp = ((int)(val)).ToString() + "%";
            this.exclude_E.SelectedItem = temp;

            val = Math.Round(100.0F * blinkLinkClickControlModule.WestLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_W.SelectedItem = temp;

            val = Math.Round(100.0F * blinkLinkClickControlModule.NorthLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_N.SelectedItem = temp;

            val = Math.Round(100.0F * blinkLinkClickControlModule.SouthLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_S.SelectedItem = temp;

            val = blinkLinkClickControlModule.UserHorizontalGain;
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

            val = blinkLinkClickControlModule.UserVerticalGain;
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

            val = blinkLinkClickControlModule.Damping;
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
            loadingControls = false;
        }

        SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        #endregion
    }
}
