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

namespace CameraMouseSuite
{
    public partial class StandardMouseControlPanel : UserControl, CMSConfigPanel
    {
        public StandardMouseControlPanel()
        {
            InitializeComponent();
        }

        private CMSMouseControlModuleStandard standardMouseControl = null;
        public void SetMouseControl(CMSMouseControlModuleStandard standardMouseControl)
        {
            this.standardMouseControl = standardMouseControl;
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
                standardMouseControl.UserHorizontalGain = val;
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
                standardMouseControl.UserVerticalGain = val;
                sendLogAdvancedTracker();
            }
        }

        private void smooth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                string val = smooth.SelectedItem.ToString();
                if (val.Equals("Off"))
                    standardMouseControl.Damping = 1.0;
                else if (val.Equals("Very Low"))
                    standardMouseControl.Damping = 0.95;
                else if (val.Equals("Low"))
                    standardMouseControl.Damping = 0.80;
                else if (val.Equals("Med"))
                    standardMouseControl.Damping = 0.65;
                else if (val.Equals("Med High"))
                    standardMouseControl.Damping = 0.5;
                else if (val.Equals("High"))
                    standardMouseControl.Damping = 0.3;
                else if (val.Equals("Very High"))
                    standardMouseControl.Damping = 0.15;
                else if (val.Equals("Extreme"))
                    standardMouseControl.Damping = 0.05;
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
                standardMouseControl.NorthLimit = Double.Parse(s, CultureInfo.InvariantCulture);
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
                standardMouseControl.WestLimit = Double.Parse(s, CultureInfo.InvariantCulture);
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
                standardMouseControl.EastLimit = Double.Parse(s, CultureInfo.InvariantCulture);
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
                standardMouseControl.SouthLimit = Double.Parse(s, CultureInfo.InvariantCulture);
                sendLogAdvancedTracker();
            }
        }

        private void ReverseMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                standardMouseControl.ReverseHorizontal = this.checkBoxReverseMouse.Checked;
                sendLogAdvancedTracker();
            }
        }


        #region CMSConfigPanel Members

        private SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        public void LoadFromControls()
        {
            loadingControls = true;

            double val = Math.Round(100.0F * standardMouseControl.EastLimit);
            string temp = ((int)(val)).ToString() + "%";
            this.exclude_E.SelectedItem = temp;

            val = Math.Round(100.0F * standardMouseControl.WestLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_W.SelectedItem = temp;

            val = Math.Round(100.0F * standardMouseControl.NorthLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_N.SelectedItem = temp;

            val = Math.Round(100.0F * standardMouseControl.SouthLimit);
            temp = ((int)(val)).ToString() + "%";
            this.exclude_S.SelectedItem = temp;

            val = standardMouseControl.UserHorizontalGain;
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

            val = standardMouseControl.UserVerticalGain;
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

            val = standardMouseControl.Damping;
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

        #endregion
    }
}
