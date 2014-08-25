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
using System.Text;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public class CMSViewAdapter
    {
        private CMSModel model = null;
        private CMSController controller = null;
        private CMSVideoSource videoSource = null;
        public CMSViewAdapter(CMSModel model, CMSController controller, CMSVideoSource videoSource)
        {
            this.videoSource = videoSource;
            this.model = model;
            this.controller = controller;
        }

        public void DisplayCameraPropertyPage()
        {
            videoSource.DisplayCameraPropertyPage();
        }

        public void ChooseNewWebCam()
        {
            controller.ChooseNewWebCam();
            //webCamSource.StopSource();
            //webCamSource.StartSource();
        }

        public CMSControlTogglerConfig ControlTogglerConfig
        {
            get
            {
                return model.GeneralConfig.ControlTogglerConfig;
            }
            set
            {
                model.GeneralConfig.ControlTogglerConfig.UpdateControlTogglerConfig(value);
            }
        }

        public CMSIdentificationConfig IdentificationConfig
        {
            get
            {
                return model.IdConfig.Clone();
            }
            set
            {
                model.IdConfig.UpdateIdentificationConfig(value);
            }
        }

        public CMSLogConfig LogConfig
        {
            get
            {
                return model.LogConfig.Clone();
            }
            set
            {
                model.LogConfig.UpdateUserControlledLogConfigInfo(value);
            }
        }

        public string SelectedSuiteInformalName
        {
            get
            {
                CMSTrackingSuite suite = GetCurrentTrackingSuite();
                if (suite == null)
                    return null;
                return suite.InformalName;
            }
        }

        public string SelectedSuiteName
        {
            get
            {
                return model.GeneralConfig.SelectedSuiteName;
            }
            set
            {
                controller.SetSelectedTrackingSuite(value);
            }
        }

        public double [] RatioVideoInputToOutput
        {
            get
            {
                return model.RatioVideoInputToOutput;
            }
            set
            {
                model.RatioVideoInputToOutput = value;
            }
        }

        public void Quit()
        {
            controller.Close();
        }

        public void MouseUpOnDisplay(MouseEventArgs e, int cameraNum)
        {
            controller.MouseUpOnDisplay(e, cameraNum);
        }

        public CMSTrackingSuiteIdentifier[] TrackingSuiteIdentifiers
        {
            get
            {
                return model.TrackingDirectory.TrackingIdentifiers;
            }
        }

        /*
        public string[] TrackingSuiteInformalNames
        {
            get
            {
                return model.TrackingDirectory.TrackingSuiteInformalNames;
            }
        }

        public string[] TrackingSuiteDescriptions
        {
            get
            {
                return model.TrackingDirectory.TrackSuiteDescriptions;
            }
        }

        public string[] TrackingSuiteNames
        {
            get
            {
                return model.TrackingDirectory.TrackingSuiteNames;
            }
        }
        */

        public string[] CameraTitles
        {
            get
            {
                return videoSource.CameraTitles;
            }
        }

        public void UpdateTrackingSuite(CMSTrackingSuite trackingSuite)
        {
            model.TrackingDirectory.GetTrackingSuite(trackingSuite.Name).Update(trackingSuite);
        }

        public CMSTrackingSuite GetTrackingSuite(string suiteName)
        {
            return model.TrackingDirectory.GetTrackingSuite(suiteName);
        }

        public CMSTrackingSuite GetCurrentTrackingSuite()
        {
            return GetTrackingSuite(SelectedSuiteName);
        }

        public CMSTrackingSuiteStandard StandardTrackingSuite
        {
            get
            {
                return model.TrackingDirectory.GetTrackingSuite(CMSConstants.STANDARD_TRACKING_SUITE_NAME) as CMSTrackingSuiteStandard;
            }
        }

        public void SaveSuiteAndConfig()
        {
            model.SaveGeneralConfig();
            model.SaveCurrentSuite();
        }
        public void LoadSuiteAndTogglerConfig()
        {
            model.LoadControlTogglerConfig();
            model.LoadCurrentSuite();
        }

        public void SaveLogConfig()
        {
            model.SaveLogConfig();
        }
        public void SaveIdConfig()
        {
            model.SaveIdConfig();
        }
    }
}
