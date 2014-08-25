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

namespace CameraMouseSuite
{
    public partial class StandardClickControlPanel : UserControl, CMSConfigPanel
    {

        private CMSClickControlModuleStandard standardClickControl = null;
        public void SetClickControl(CMSClickControlModuleStandard standardClickControl)
        {
            this.standardClickControl = standardClickControl;
            LoadFromControls();
        }

        private bool loadingControls = false;


        public StandardClickControlPanel()
        {
            InitializeComponent();
        }

        private void clicking_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                bool enabled = clicking.Checked;
                this.standardClickControl.ClickEnabled = enabled;
                clickRadius.Enabled = enabled;
                dwell.Enabled = enabled;
                click_sound.Enabled = enabled;
                sendLogAdvancedTracker();
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

                standardClickControl.Radius = val;
                sendLogAdvancedTracker();
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
                standardClickControl.DwellTime = val;
                sendLogAdvancedTracker();
            }
        }

        private void click_sound_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                standardClickControl.PlaySound = click_sound.Checked;
                sendLogAdvancedTracker();
            }
        }


        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            loadingControls = true;

            clicking.Checked = standardClickControl.ClickEnabled;
            clickRadius.Enabled = standardClickControl.ClickEnabled;
            dwell.Enabled = standardClickControl.ClickEnabled;
            click_sound.Enabled = standardClickControl.ClickEnabled;
            click_sound.Checked = standardClickControl.PlaySound;

            long lval = standardClickControl.DwellTime;
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

            double val = standardClickControl.Radius;
            if (val == 0.025)
                this.clickRadius.SelectedItem = "Small";
            else if (val == 0.05)
                this.clickRadius.SelectedItem = "Normal";
            else if (val == 0.075)
                this.clickRadius.SelectedItem = "Large";

            loadingControls = false;
        }

        private SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        #endregion
    }
}
