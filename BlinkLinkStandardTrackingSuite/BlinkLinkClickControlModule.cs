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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BlinkLinkStandardTrackingSuite
{
    public class BlinkLinkClickControlModule : CMSClickControlModule
    {
        private const string RunningMessage = ": Press F1 to reset everything, F2 to retrain blinking.";
        
        public class BlinkLinkCMSExtraTrackingInfo : CMSExtraTrackingInfo
        {
            public Point LeftEyePoint;
            public Point RightEyePoint;

            public BlinkLinkCMSExtraTrackingInfo(CvPoint2D32f leftEyePoint, CvPoint2D32f rightEyePoint)
            {
                this.LeftEyePoint = new Point((int)leftEyePoint.x, (int)leftEyePoint.y);
                this.RightEyePoint = new Point((int)rightEyePoint.x, (int)rightEyePoint.y); ;
            }
        }

        private EyeClicker            eyeClicker;
        private BlinkLinkEyeClickData blinkLinkEyeClickData;
        private bool                  extraInfoPassedLastProcess;

        public BlinkLinkClickControlModule()
        {
            blinkLinkEyeClickData = new BlinkLinkEyeClickData();
            extraInfoPassedLastProcess = false;
        }

        public BlinkLinkEyeClickData BlinkLinkEyeClickData
        {
            get
            {
                return blinkLinkEyeClickData;
            }

            set
            {
                blinkLinkEyeClickData = value;
            }
        }

        public float EyeClosedTime
        {
            get
            {
                return eyeClicker.EyeClosedTime;
            }
        }

        public bool IsDragging
        {
            get
            {
                return eyeClicker.IsDragging;
            }
        }

        public override void ProcessClick(PointF mousePoint, PointF screenPoint, CMSExtraTrackingInfo extraInfo, Bitmap[] frames)
        {
            if( extraInfo != null )
            {
                FastBitmap img = new FastBitmap(frames[0]);
                BlinkLinkCMSExtraTrackingInfo blinkLinkExtraTrackingInfo = (BlinkLinkCMSExtraTrackingInfo)extraInfo;

                eyeClicker.Update(blinkLinkExtraTrackingInfo.LeftEyePoint,
                                  blinkLinkExtraTrackingInfo.RightEyePoint,
                                  Math.Abs(blinkLinkExtraTrackingInfo.LeftEyePoint.X - blinkLinkExtraTrackingInfo.RightEyePoint.X),
                                  img, State == CMSState.ControlTracking);

                extraInfoPassedLastProcess = true;
            }
            else
            {
                if( extraInfoPassedLastProcess )
                {
                    eyeClicker.Reset(false);
                    extraInfoPassedLastProcess = false;
                }
            }
        }

        public override CMSConfigPanel getPanel()
        {
            BlinkLinkClickControlPanel clickPanel = new BlinkLinkClickControlPanel();
            clickPanel.SetClickControl(this);
            return clickPanel;
        }

        public override void Clean()
        {
            if (eyeClicker != null)
            {
                eyeClicker.Dispose();
            }
        }

        public override void ProcessKeys(Keys keys)
        {
            if( keys.Equals(Keys.F2) )
            {
                eyeClicker.Reset(true);
            }
        }

        public override void Init(Size[] imageSizes)
        {
            eyeClicker = new EyeClicker(CMSTrackingSuiteAdapter, blinkLinkEyeClickData);
        }

        public override void Update(CMSModule module)
        {
            blinkLinkEyeClickData.LockEyeClickData();
            try
            {
                BlinkLinkClickControlModule clickModule = module as BlinkLinkClickControlModule;

                blinkLinkEyeClickData.SetData(clickModule.blinkLinkEyeClickData);
            }
            finally
            {
                blinkLinkEyeClickData.UnlockEyeClickData();
            }
        }
        /*
        public override CMSModule Clone()
        {
            blinkLinkEyeClickData.LockEyeClickData();
            try
            {
                BlinkLinkClickControlModule clickModule = new BlinkLinkClickControlModule();

                clickModule.blinkLinkEyeClickData.SetData(blinkLinkEyeClickData);
                return clickModule;
            }
            finally
            {
                blinkLinkEyeClickData.UnlockEyeClickData();
            }
        }
        */
        public override void StateChange(CMSState state)
        {
            if (state == CMSState.ControlTracking || state == CMSState.Tracking)
            {
                trackingSuiteAdapter.SendMessage(RunningMessage);
            }
        }

        public void UpdateEyeStatusWindow()
        {
            eyeClicker.UpdateEyeStatusWindow();
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {
            eyeClicker.LabelImage(frames[0], trackingSuiteAdapter.GetRatioInputToOutput()[0]);
        }
    }
}
