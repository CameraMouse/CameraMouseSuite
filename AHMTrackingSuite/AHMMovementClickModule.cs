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
using System.Drawing;
using System.Threading;
using CameraMouseSuite;

namespace AHMTrackingSuite
{
    public class AHMMovementClickModule : CMSClickControlModule
    {
        private ClickingThresholdForm clickingThresholdForm = null;

        private AHMovingAverage movingAverage = null;

        private long prevClickTickCount = 0;


        private int threshold = 100;
        public int Threshold
        {
            get
            {
                return threshold;
            }
            set
            {
                threshold = value;
            }
        }

        private object mutex = new object();

        private bool quit = false;
        private bool Quit
        {
            get
            {
                lock (mutex)
                {
                    return quit;
                }
            }
            set
            {
                lock (mutex)
                {
                    quit = value;
                }
            }
        }

        private bool clickEvent = false;
        private bool ClickEvent
        {
            get
            {
                lock (mutex)
                {
                    return clickEvent;
                }
            }
            set
            {
                lock (mutex)
                {
                    clickEvent = value;
                }
            }
        }

        private PointF prevMousePoint = PointF.Empty;

        public override void ProcessClick(System.Drawing.PointF mousePoint, System.Drawing.PointF screenPoint, CMSExtraTrackingInfo extraInfo, System.Drawing.Bitmap[] frames)
        {
            try
            {
                if (prevMousePoint.IsEmpty)
                {
                    prevMousePoint = mousePoint;
                    clickingThresholdForm.checkValue(0, false);
                    return;
                }


                AHMTrackingState curState = AHMTrackingState.NoFeature;

                if (extraInfo != null)
                    curState = ((AHMStateExtraInfo)extraInfo).TrackingState;

                if (curState == AHMTrackingState.NoFeature || curState.Equals(AHMTrackingState.Feature))
                {
                    clickingThresholdForm.Reset();
                    prevMousePoint = mousePoint;
                    clickingThresholdForm.checkValue(0, false);
                    return;
                }

                double dx = prevMousePoint.X - mousePoint.X;
                double dy = prevMousePoint.Y - mousePoint.Y;
                if (dy < 0)
                    dy = 0;
                double dist = dy;

                bool isTraining = curState.Equals(AHMTrackingState.AHMSetup);

                movingAverage.AddPoint(dist);
                clickingThresholdForm.checkValue(movingAverage.EMAverage, isTraining);
                prevMousePoint = mousePoint;


                double thresh = ((double)threshold / 100.0) * clickingThresholdForm.MaxValue;

                if (ExperimentClickFrameSaver.IsExperimentFrameEnabled())
                    ExperimentClickFrameSaver.SaveEvent(new ExperimentFrame(dist, movingAverage.EMAverage, thresh,ClickEvent ? 1 : 0));
            }
            catch
            {
            }
        }

        public override CMSConfigPanel getPanel()
        {
            return null;
        }

        public override void Clean()
        {
            if (clickingThresholdForm != null && clickingThresholdForm.Created)
            {
                clickingThresholdForm.Close();
                clickingThresholdForm = null;
            }

            Quit = true;
            ClickEvent = false;
            movingAverage = null;
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {
            return;
        }

        public override void Init(System.Drawing.Size[] imageSizes)
        {
            prevMousePoint = PointF.Empty;

            clickingThresholdForm = trackingSuiteAdapter.CreateForm(typeof(ClickingThresholdForm)) as ClickingThresholdForm;
            clickingThresholdForm.SetThresholdValue(threshold);            
            clickingThresholdForm.SetThreshold += new SetThreshold(extraStateInfoForm_SetThreshold);
            clickingThresholdForm.PastThreshold += new PastThreshold(extraStateInfoForm_PastThreshold);
            
            
            movingAverage = new AHMovingAverage();
            movingAverage.Init(20, 20);

            Thread t = new Thread(new ThreadStart(ClickThread));
            t.Start();
        }

        private void ClickThread()
        {
            while (!Quit)
            {
                if (ClickEvent)
                {
                    ClickEvent = false;

                    Point p = System.Windows.Forms.Cursor.Position;
                    User32.mouse_event(2, p.X, p.Y, 0, 0); //Right mouse down at x,y
                    System.Threading.Thread.Sleep(100);
                    User32.mouse_event(4, p.X, p.Y, 0, 0); //Right mouse up at x,y

                    if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogClickEvent"))
                    {
                        CMSLogClickEvent clickEvent = new CMSLogClickEvent();
                        if (clickEvent != null)
                        {
                            clickEvent.X = p.X;
                            clickEvent.Y = p.Y;
                            clickEvent.Width = CMSConstants.SCREEN_WIDTH;
                            clickEvent.Height = CMSConstants.SCREEN_HEIGHT;
                            clickEvent.ClickType = ClickType.LClk;
                            CMSLogger.SendLogEvent(clickEvent);
                        }
                    }
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        void extraStateInfoForm_PastThreshold()
        {
            long curTickCount = Environment.TickCount;

            if (curTickCount - prevClickTickCount > 1000)
            {
                prevClickTickCount = curTickCount;

                ClickEvent = true;                
            }

        }
        void extraStateInfoForm_SetThreshold(int value)
        {
            //base.
            threshold = value;
        }

        public override void Update(CMSModule module)
        {
            AHMMovementClickModule m = module as AHMMovementClickModule;
            if (m == null)
                return;
            Threshold = m.Threshold;
        }
        public override void StateChange(CMSState state)
        {
        }
        public override void DrawOnFrame(System.Drawing.Bitmap[] frames)
        {
        }

    }
}
