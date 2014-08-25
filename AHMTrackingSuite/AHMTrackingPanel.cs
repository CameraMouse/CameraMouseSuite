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

    public partial class AHMTrackingPanel : UserControl, CMSConfigPanel
    {
        private AHMTrackingModule trackingModule = null;

        public void SetModule(AHMTrackingModule trackingModule)
        {
            this.trackingModule = trackingModule;
            LoadFromControls();
        }

        public AHMTrackingPanel()
        {
            InitializeComponent();
        }

        private bool isLoading = false;

        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            if(trackingModule==null)
                return;

            isLoading = true;
            this.lightingCorrection.Checked = trackingModule.KernelLightingCorrection;
            this.checkBoxExtraDisplay.Checked = trackingModule.ExtraDisplay;

            AHMSetupType setupType = trackingModule.SetupType;
            if (setupType.Equals(AHMSetupType.Timing15Sec))
            {
                this.comboBoxSetupType.SelectedItem = "Natural Movement";
            }
            else if (setupType.Equals(AHMSetupType.KeyPress))
            {
                this.comboBoxSetupType.SelectedItem = "Key Press";
            }
            else if (setupType.Equals(AHMSetupType.Movement30Sec))
            {
                this.comboBoxSetupType.SelectedItem = "Movement - 30 Sec";
            }
            else if (setupType.Equals(AHMSetupType.Movement45Sec))
            {
                this.comboBoxSetupType.SelectedItem = "Movement - 45 Sec";
            }
            else if (setupType.Equals(AHMSetupType.Movement60Sec))
            {
                this.comboBoxSetupType.SelectedItem = "Movement - 60 Sec";
            }
            else if (setupType.Equals(AHMSetupType.MovementInfinite))
            {
                this.comboBoxSetupType.SelectedItem = "Movement - Infinite";
            }

            int updateFrequency = trackingModule.UpdateFrequency;
            if (updateFrequency == 0)
            {
                this.comboBoxUpdateFequency.SelectedItem = "Every Frame";
            }
            else if (updateFrequency == 1000)
            {
                this.comboBoxUpdateFequency.SelectedItem = "Once Per Second";
            }
            else if (updateFrequency == 500)
            {
                this.comboBoxUpdateFequency.SelectedItem = "Twice Per Second";
            }
            else if (updateFrequency == 333)
            {
                this.comboBoxUpdateFequency.SelectedItem = "Three Per Second";
            }

            this.comboBoxAutoStart.SelectedItem = trackingModule.AutoStartMode.ToString();
            
            this.NumTemplates.SelectedItem = trackingModule.NumTemplates.ToString();
            isLoading = false;
        }

        #endregion
        
        private void NumTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                trackingModule.NumTemplates = Int32.Parse(NumTemplates.SelectedItem.ToString());
                sendLogAdvancedTracker();
            }
        }

        private void lightingCorrection_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                this.trackingModule.KernelLightingCorrection = lightingCorrection.Checked;
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
                else if (this.comboBoxSetupType.SelectedItem.Equals("Key Press"))
                {
                    trackingModule.SetupType = AHMSetupType.KeyPress;
                }
                else if (this.comboBoxSetupType.SelectedItem.Equals("Movement - Infinite"))
                {
                    trackingModule.SetupType = AHMSetupType.MovementInfinite;
                }
                else if (this.comboBoxSetupType.SelectedItem.Equals("Movement - 30 Sec"))
                {
                    trackingModule.SetupType = AHMSetupType.Movement30Sec;
                }
                else if (this.comboBoxSetupType.SelectedItem.Equals("Movement - 45 Sec"))
                {
                    trackingModule.SetupType = AHMSetupType.Movement45Sec;
                }
                else if (this.comboBoxSetupType.SelectedItem.Equals("Movement - 60 Sec"))
                {
                    trackingModule.SetupType = AHMSetupType.Movement60Sec;
                }
                sendLogAdvancedTracker();
            }
        }

        private void checkBoxExtraDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                trackingModule.ExtraDisplay = this.checkBoxExtraDisplay.Checked;
                sendLogAdvancedTracker();
            }
        }

        private void comboBoxUpdateFequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (this.comboBoxUpdateFequency.SelectedItem.Equals("Every Frame"))
            {
                this.trackingModule.UpdateFrequency = 0;
            }
            else if (this.comboBoxUpdateFequency.SelectedItem.Equals("Once Per Second"))
            {
                this.trackingModule.UpdateFrequency = 1000;
            }
            else if (this.comboBoxUpdateFequency.SelectedItem.Equals("Twice Per Second"))
            {
                this.trackingModule.UpdateFrequency = 500;
            }
            else if (this.comboBoxUpdateFequency.SelectedItem.Equals("Three Per Second"))
            {
                this.trackingModule.UpdateFrequency = 333;
            }
            sendLogAdvancedTracker();
        }

        private SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        private void comboBoxAutoStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            trackingModule.AutoStartMode = (AutoStartMode)Enum.Parse(typeof(AutoStartMode),comboBoxAutoStart.SelectedItem.ToString());

            sendLogAdvancedTracker();
        }

    }

}
