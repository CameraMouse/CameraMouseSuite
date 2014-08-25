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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CameraMouseSuite;

namespace AHMTrackingSuite
{
    public partial class AHMSimpleTrackingPanel : UserControl, CMSConfigPanel
    {

        public AHMSimpleTrackingPanel()
        {
            InitializeComponent();
        }

        private AHMTrackingModule trackingModule = null;

        public void SetModule(AHMTrackingModule trackingModule)
        {
            this.trackingModule = trackingModule;
            LoadFromControls();
        }

        private bool isLoading = false;

        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            if (trackingModule == null)
                return;
            isLoading = true;

            AHMSetupType setupType = trackingModule.SetupType;
            if (setupType.Equals(AHMSetupType.Timing15Sec))
            {
                this.comboBoxSetupType.SelectedItem = "Natural Movement";
            }
            else 
            {
                trackingModule.SetupType = AHMSetupType.Movement30Sec;
                this.comboBoxSetupType.SelectedItem = "Rectangle Movement";
            }

            int updateFrequency = trackingModule.UpdateFrequency;
            if (updateFrequency == 0)
            {
                this.comboBoxUpdateFequency.SelectedItem = "Fast";
            }
            else
            {
                trackingModule.UpdateFrequency = 500;
                this.comboBoxUpdateFequency.SelectedItem = "Slow";
            }

            this.checkBoxAutoStart.Checked = trackingModule.AutoStartMode == AutoStartMode.NoseMouth;

            isLoading = false;
        }

        #endregion

        /*
        private void comboBoxAutoStart_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (isLoading)
                return;

            trackingModule.AutoStartMode = (AutoStartMode)Enum.Parse(typeof(AutoStartMode), comboBoxAutoStart.SelectedItem.ToString());

            sendLogAdvancedTracker();
        }*/

        private void comboBoxUpdateFequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                if (this.comboBoxUpdateFequency.SelectedItem.Equals("Fast"))
                {
                    this.trackingModule.UpdateFrequency = 0;
                }
                else
                {
                    this.trackingModule.UpdateFrequency = 500;
                }
                sendLogAdvancedTracker();
            }
        }

        private void comboBoxSetupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                if (this.comboBoxSetupType.SelectedItem.Equals("Natural Movement"))
                {
                    trackingModule.SetupType = AHMSetupType.Timing15Sec;
                }
                else
                {
                    trackingModule.SetupType = AHMSetupType.Movement30Sec;
                }
                sendLogAdvancedTracker();
            }
        }

        #region CMSConfigPanel Members

        private SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }


        #endregion

        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (checkBoxAutoStart.Checked)
                trackingModule.AutoStartMode = AutoStartMode.NoseMouth;
            else
                trackingModule.AutoStartMode = AutoStartMode.None;
        }

    }
}
