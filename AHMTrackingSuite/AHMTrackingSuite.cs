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
    public delegate double GetGain();

    [CMSIgnoreSuiteAtt()]
    public class AHMTrackingSuite : CMSTrackingSuite
    {
        /*
        public override CMSTrackingSuite Clone()
        {
            AHMTrackingSuite trackingModule = new AHMTrackingSuite();
            trackingModule.AHMTracking = AHMTracking.Clone() as AHMTrackingModule;
            trackingModule.StandardClickControl = MouseControlModule.Clone() as CMSMouseControlModule;
            trackingModule.StandardMouseControl = ClickControlModule.Clone() as CMSClickControlModule;
            return trackingModule;
        }*/

        public AHMTrackingModule AHMTracking
        {
            get
            {
                return trackingModule as AHMTrackingModule;
            }
            set
            {
                trackingModule = value;
                if(trackingModule != null)
                ((AHMTrackingModule)trackingModule).MouseControlModuleStandard = StandardMouseControl;

            }
        }

        public CMSClickControlModuleStandard StandardClickControl
        {
            get
            {
                return this.clickControlModule as CMSClickControlModuleStandard;
            }
            set
            {
                this.clickControlModule = value;
            }
        }

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

        public AHMTrackingSuite()
        {
            AHMTrackingModule ahmTracker = new AHMTrackingModule();
            this.trackingModule = ahmTracker;
            this.mouseControlModule = new CMSMouseControlModuleStandard();
            this.clickControlModule = new CMSClickControlModuleStandard();
            ahmTracker.MouseControlModuleStandard = mouseControlModule as CMSMouseControlModuleStandard;

            this.name = "AHM";
            this.informalName = "Fast Motion Tracker (Advanced)";
            this.description = "This tracker can track very fast motions. Advanced controls are visible.";

        }

        public override void SendSuiteLogEvent()
        {
            if (CMSLogger.CanCreateLogEvent(false, false, false, "AHMLogSuiteEvent"))
            {
                AHMLogSuiteEvent logEvent = new AHMLogSuiteEvent();
                if (logEvent != null)
                {
                    logEvent.TrackingSuite = this;
                    CMSLogger.SendLogEvent(logEvent);
                }
            }
        }
    }
}
