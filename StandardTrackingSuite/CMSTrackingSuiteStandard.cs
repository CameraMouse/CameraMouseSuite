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

namespace CameraMouseSuite
{
    public class CMSTrackingSuiteStandard : CMSTrackingSuite
    {

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
            }
        }

        public CMSTrackingModuleStandard StandardTracking
        {
            get
            {
                return this.trackingModule as CMSTrackingModuleStandard;
            }
            set
            {
                trackingModule = value;
            }
        }

        public CMSTrackingSuiteStandard() : base()
        {
            this.trackingModule = new CMSTrackingModuleStandard();
            this.mouseControlModule = new CMSMouseControlModuleStandard();
            this.clickControlModule = new CMSClickControlModuleStandard();
            this.name = CMSConstants.STANDARD_TRACKING_SUITE_NAME;
            this.informalName = CMSConstants.STANDARD_TRACKING_SUITE_INFORMAL_NAME;
            this.description = CMSConstants.STANDARD_TRACKING_SUITE_DECSCRIPTION;
        }

        /*public override CMSTrackingSuite Clone()
        {
            CMSTrackingSuiteStandard trackingModule = new CMSTrackingSuiteStandard();
            trackingModule.TrackingModule = StandardTracking.Clone() as CMSTrackingModule;
            trackingModule.MouseControlModule = MouseControlModule.Clone() as CMSMouseControlModule;
            trackingModule.ClickControlModule = ClickControlModule.Clone() as CMSClickControlModule;
            return trackingModule;
        }*/


        public override void SendSuiteLogEvent()
        {
            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogStandardSuiteEvent"))
            {

                CMSLogStandardSuiteEvent logEvent = new CMSLogStandardSuiteEvent();
                if (logEvent != null)
                {
                    logEvent.StandardSuite = this;
                    CMSLogger.SendLogEvent(logEvent);
                }
            }
        }
    }
}
