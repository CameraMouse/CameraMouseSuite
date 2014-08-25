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

    public partial class AHMClickMovementPanel : UserControl, CMSConfigPanel
    {
        public AHMClickMovementPanel()
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

            AutoStartMode autoStartMode = trackingModule.AutoStartMode;
            if (autoStartMode == AutoStartMode.None || autoStartMode == AutoStartMode.NoseMouth)
                this.comboBoxAutoStart.SelectedItem = "None";
            else if (autoStartMode == AutoStartMode.LeftEye)
                this.comboBoxAutoStart.SelectedItem = "Left Eyebrow";
            else if (autoStartMode == AutoStartMode.RightEye)
                this.comboBoxAutoStart.SelectedItem = "Right Eyebrow";
            
            isLoading = false;   
        }

        private SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        #endregion

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

        private void comboBoxAutoStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {

                if (comboBoxAutoStart.SelectedItem.Equals("None"))
                {
                    trackingModule.AutoStartMode = AutoStartMode.None;
                }
                else if (comboBoxAutoStart.SelectedItem.Equals("Left Eyebrow"))
                {
                    trackingModule.AutoStartMode = AutoStartMode.LeftEye;
                }
                else if (comboBoxAutoStart.SelectedItem.Equals("Right Eyebrow"))
                {
                    trackingModule.AutoStartMode = AutoStartMode.RightEye;
                }

                if (sendLogAdvancedTracker != null)
                    sendLogAdvancedTracker();
            }
        }

    }

}
