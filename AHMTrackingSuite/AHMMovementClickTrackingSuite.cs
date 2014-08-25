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
using System.Linq;
using System.Text;
using CameraMouseSuite;

namespace AHMTrackingSuite
{
    //[CMSIgnoreSuiteAtt()]
    public class AHMMovementClickTrackingSuite : CMSTrackingSuite
    {
        
        public AHMTrackingModule AHMTracking
        {
            get
            {
                return trackingModule as AHMTrackingModule;
            }
            set
            {
                trackingModule = value;

                if (trackingModule != null)
                {
                    ((AHMTrackingModule)trackingModule).PanelType = AHMPanelType.Eyebrow;
                    ((AHMTrackingModule)trackingModule).KernelLightingCorrection = false;
                    ((AHMTrackingModule)trackingModule).SetupType = AHMSetupType.Timing10Sec;
                    ((AHMTrackingModule)trackingModule).NumTemplates = 16;            
                    //((AHMTrackingModule)trackingModule).MouseControlModuleStandard = StandardMouseControl;
                }

            }
        }

        public AHMMovementClickModule AHMMovementClickModule
        {
            get
            {
                return this.clickControlModule as AHMMovementClickModule;
            }
            set
            {
                this.clickControlModule = value;
            }
        }

        /*
        public CMSMouseControlModuleStandard StandardMouseControl
        {
            get
            {
                return this.mouseControlModule as CMSMouseControlModuleStandard;
            }
            set
            {
                this.mouseControlModule = value;
                if (trackingModule != null)
                {
                    ((AHMTrackingModule)trackingModule).MouseControlModuleStandard = StandardMouseControl;
                }
            }
        }
        */

        public AHMMovementClickTrackingSuite()
        {
            AHMTrackingModule ahmTracker = new AHMTrackingModule();
            ahmTracker.PanelType = AHMPanelType.Eyebrow;                
            ahmTracker.KernelLightingCorrection = false;
            ahmTracker.SetupType = AHMSetupType.Timing10Sec;
            ahmTracker.NumTemplates = 16;
            
            this.trackingModule = ahmTracker;            
            this.mouseControlModule = null;
            this.clickControlModule = new AHMMovementClickModule();
            
            this.name = "AHMMovementClick";
            this.informalName = "Eyebrow Clicker";
            this.description = "This tracker causes clicks to occur when the user moves his or her eyebrow. It is intended for users with stationary heads.";
        }

        public override void SendSuiteLogEvent()
        {
            if (CMSLogger.CanCreateLogEvent(false, false, false, "AHMLogSuiteEvent"))
            {
                AHMLogSuiteEvent logEvent = new AHMLogSuiteEvent();
                if (logEvent != null)
                {
                    logEvent.MovementClickTrackingSuite = this;
                    CMSLogger.SendLogEvent(logEvent);
                }
            }
        }

    }
}
