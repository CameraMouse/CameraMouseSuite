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
using System.Drawing;

namespace CameraMouseSuite
{
    public class CMSEmptyTrackingSuite : CMSTrackingSuite
    {
        /*
        public override CMSTrackingSuite Clone()
        {
            return new CMSEmptyTrackingSuite();
        }
*/

        public CMSEmptyTrackingSuite()
            : base()
        {
            trackingModule = new CMSEmptyTrackingModule();
            clickControlModule = new CMSEmptyClickControlModule();
            mouseControlModule = new CMSEmptyMouseControlModule();
            name = CMSConstants.EMPTY_TRACKING_SUITE_NAME;
            informalName = CMSConstants.EMPTY_TRACKING_SUITE_INFORMAL_NAME;
            description = CMSConstants.EMPTY_TRACKING_SUITE_DESCRIPTION;
        }

        public override void SendSuiteLogEvent()
        {
            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogEmptySuiteEvent"))
            {
                CMSLogEmptySuiteEvent logEvent = new CMSLogEmptySuiteEvent();

                if (logEvent != null)
                    CMSLogger.SendLogEvent(logEvent);
            }
        }
    }

    public class CMSEmptyTrackingModule : CMSTrackingModule
    {

        public override void ProcessMouse(System.Drawing.Point p, bool leftMouseButton, int cameraNum)
        {
        }

        public override void Process(System.Drawing.Bitmap [] frames)
        {
        }

        public override CMSConfigPanel getPanel()
        {
            return null;
        }

        public override void Clean()
        {
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {
        }

        public override void Init(System.Drawing.Size [] sizes)
        {
            CMSTrackingSuiteAdapter.SendMessage(CMSConstants.EMPTY_TRACKING_SUITE_MESSAGE);
        }

        public override void Update(CMSModule module)
        {
        }
        /*
        public override CMSModule Clone()
        {
            return null;
        }
        */
        public override void StateChange(CMSState state)
        {
            
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {

        }
    }

    public class CMSEmptyMouseControlModule : CMSMouseControlModule
    {
        public override void ProcessMouse(System.Drawing.PointF imagePoint, CMSExtraTrackingInfo extraInfo, System.Drawing.Bitmap [] frames)
        {
        }

        public override CMSConfigPanel getPanel()
        {
            return null;
        }

        public override void Clean()
        {
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        { 
        }

        public override void Init(System.Drawing.Size [] sizes)
        {
        }

        public override void Update(CMSModule module)
        {
        }
        /*
        public override CMSModule Clone()
        {
            return new CMSEmptyMouseControlModule();
        }
        */

        public override void StateChange(CMSState state)
        {
            
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {

        }
    }

    public class CMSEmptyClickControlModule : CMSClickControlModule
    {
        public override void ProcessClick(System.Drawing.PointF mousePoint, System.Drawing.PointF screenPoint, CMSExtraTrackingInfo extraInfo, System.Drawing.Bitmap [] frames)
        {   
        }

        public override CMSConfigPanel getPanel()
        {
            return null;
        }

        public override void Clean()
        {
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {
        }

        public override void Init(System.Drawing.Size [] sizes)
        {
        }

        public override void Update(CMSModule module)
        {
        }
        /*
        public override CMSModule Clone()
        {
            return new CMSEmptyClickControlModule();
        }
        */
        public override void StateChange(CMSState state)
        {
            
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {

        }
    }
}