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
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CameraMouseSuite
{
    public class CMSClickControlModuleStandard : CMSClickControlModule
    {
        private SoundPlayer soundPlayer = new SoundPlayer();
        private object mutex = new object();

        private long dwellTime=500;
        public long DwellTime
        {
            get
            {
                lock (mutex)
                {
                    return dwellTime;
                }
            }
            set
            {
                lock (mutex)
                {
                    dwellTime = value;
                }
            }
        }
        
        private double radius=0.05;
        public double Radius
        {
            get
            {
                lock (mutex)
                {
                    return radius;
                }
            }
            set
            {
                lock (mutex)
                {
                    radius = value;
                }
            }
        }

        private bool clickEnabled=false;
        public bool ClickEnabled
        {
            get
            {
                lock (mutex)
                {
                    return clickEnabled;
                }
            }
            set
            {
                lock (mutex)
                {
                    clickEnabled = value;
                }
            }
        }

        private bool controlEnabled = false;

        private bool playSound;
        public bool PlaySound
        {
            get
            {
                lock (mutex)
                {
                    return playSound;
                }
            }
            set
            {
                lock (mutex)
                {
                    playSound = value;
                }
            }
        }

        private PointF currentMousePoint = PointF.Empty;
        private PointF CurrentMousePoint
        {
            get
            {
                lock (mutex)
                {
                    return new PointF(currentMousePoint.X, currentMousePoint.Y);
                }
            }
            set
            {
                lock (mutex)
                {
                    if (currentMousePoint == null)
                        currentMousePoint = new PointF();
                    currentMousePoint.X = value.X;
                    currentMousePoint.Y = value.Y;
                }
            }
        }

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

        private int sleepTime = 100;
        private double timeElapsed = 0;
        private PointF currentMouseRefPoint = new PointF(0,0);
        private bool prevLoopClicked = false;

        private Thread loopThread = null;

        private void ClickThread()
        {
            while (!Quit)
            {
                PointF curMousePos = CurrentMousePoint;
                timeElapsed += sleepTime;

                double absRadius = Radius * ((double)CMSConstants.SCREEN_WIDTH);

                if (!WithinRadius(curMousePos, currentMouseRefPoint, absRadius))
                {
                    timeElapsed = 0;
                    currentMouseRefPoint.X = curMousePos.X;
                    currentMouseRefPoint.Y = curMousePos.Y;
                    prevLoopClicked = false;
                }

                if (timeElapsed >= this.DwellTime && !prevLoopClicked)
                {
                    if (clickEnabled && controlEnabled)
                    {
                        LeftMouseClick((int)curMousePos.X, (int)curMousePos.Y);
                        if (playSound)
                        {
                            soundPlayer.PlayClick();
                        }
                        prevLoopClicked = true;
                    }
                }
                else if (timeElapsed >= (this.DwellTime + 1000) && prevLoopClicked)
                {
                    timeElapsed = 0;
                    prevLoopClicked = false;
                }
                System.Threading.Thread.Sleep(sleepTime);
            }
        }

        private void LeftMouseClick(int x,int y)
        {
            User32.mouse_event(2,x,y, 0,0); //Right mouse down at x,y
            System.Threading.Thread.Sleep(100);
            User32.mouse_event(4,x,y, 0,0); //Right mouse up at x,y


            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogClickEvent"))
            {
                CMSLogClickEvent clickEvent = new CMSLogClickEvent();
                if (clickEvent != null)
                {
                    clickEvent.X = x;
                    clickEvent.Y = y;
                    clickEvent.Width = CMSConstants.SCREEN_WIDTH;
                    clickEvent.Height = CMSConstants.SCREEN_HEIGHT;
                    clickEvent.ClickType = ClickType.LClk;
                    CMSLogger.SendLogEvent(clickEvent);
                }
            }
        }

        private bool WithinRadius(PointF center, PointF p, double r)
        {
            double dX = center.X - p.X;
            double dY = center.Y - p.Y;
            double dist = Math.Sqrt(dX * dX + dY * dY);
            return (dist < r);
        }

        public override void ProcessClick(System.Drawing.PointF mousePoint, System.Drawing.PointF screenPoint, 
                                          CMSExtraTrackingInfo extraInfo, System.Drawing.Bitmap [] frames)
        {
            if (!state.Equals(CMSState.ControlTracking))
                return;
            CurrentMousePoint = screenPoint;
        }

        /*
        public override void ToggleControl(bool control)
        {
            controlEnabled = control;
        }
        */

        public override void Clean()
        {
            Quit = true;
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {
            
        }

        public override void Init(Size[] imageSizes)
        {
            loopThread = new Thread(new ThreadStart(ClickThread));
            loopThread.Start();
        }

        public override void Update(CMSModule module)
        {
            lock (mutex)
            {

                CMSClickControlModuleStandard clickModule = module as CMSClickControlModuleStandard;
            
                this.DwellTime = clickModule.DwellTime;
                this.PlaySound = clickModule.PlaySound;
                this.Radius = clickModule.Radius;
                this.ClickEnabled = clickModule.ClickEnabled;
            }
        }

        /*
        public override CMSModule Clone()
        {
            lock (mutex)
            {
                CMSClickControlModuleStandard standardClickControl = new CMSClickControlModuleStandard();

                standardClickControl.DwellTime= this.DwellTime;
                standardClickControl.PlaySound = this.PlaySound;
                standardClickControl.Radius = this.Radius;
                standardClickControl.ClickEnabled = this.ClickEnabled;
                
                return standardClickControl;
            }
        }
        */

        public override CMSConfigPanel getPanel()
        {
            StandardClickControlPanel clickPanel = new StandardClickControlPanel();
            clickPanel.SetClickControl(this);
            return clickPanel;
        }

        public override void StateChange(CMSState state)
        {
            controlEnabled = state.Equals(CMSState.ControlTracking);
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {

        }
    }
}


