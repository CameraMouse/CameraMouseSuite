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
using System.Windows.Forms;
using System.Drawing;

namespace CameraMouseSuite
{

    public delegate bool ToggleControl(bool control);
    public delegate CMSState GetState();

    public class CMSControlToggler
    {
        private bool quit = true;

        private Thread thread = null;

        private ToggleControl toggleControl = null;
        public ToggleControl ToggleControl
        {
            get
            {
               return toggleControl;
            }
            set
            {
                toggleControl = value;
            }
        }

        private GetState getState = null;
        public GetState GetState
        {
            get
            {
                return getState;
            }
            set
            {
                getState = value;
            }
        }

        private GetCursorPos getCursorPos = null;
        public GetCursorPos GetCursorPos
        {
            get
            {
                return getCursorPos;
            }
            set
            {
                getCursorPos = value;
            }
        }

        private SoundPlayer soundPlayer = new SoundPlayer();

        private CMSControlTogglerConfig controlTogglerConfig = null;
        public CMSControlTogglerConfig ControlTogglerConfig
        {
            get
            {
                return controlTogglerConfig;
            }
            set
            {
                controlTogglerConfig = value;
            }
        }

        private long lastChangeTime = 0;
        private long lastDif = -1;

        private object keyMutex = new object();
        public void ProcessKeys(Keys keys)
        {
            if (!quit)
            {
                lock (keyMutex)
                {
                    CMSState currentState = getState();

                    if (currentState.Equals(CMSState.Tracking))
                    {
                        if ((this.controlTogglerConfig.CtrlStart && (keys.Equals(Keys.LControlKey) || keys.Equals(Keys.RControlKey))) ||
                           (this.controlTogglerConfig.ScrollStart && keys.Equals(Keys.Scroll)))
                        {
                            if (toggleControl(true))
                            {
                                if (this.controlTogglerConfig.PlaySoundOnControlChanges)
                                    soundPlayer.PlayChangeState();
                            }
                        }
                    }
                    else if (currentState.Equals(CMSState.ControlTracking))
                    {
                        if ((this.controlTogglerConfig.CtrlStop && (keys.Equals(Keys.LControlKey) || keys.Equals(Keys.RControlKey))) ||
                           (this.controlTogglerConfig.ScrollStop && keys.Equals(Keys.Scroll)))
                        {

                            if (toggleControl(false))
                            {
                                if (this.controlTogglerConfig.PlaySoundOnControlChanges)
                                    soundPlayer.PlayChangeState();
                            }
                        }
                    }
                }
            }
        }

        public void Start()
        {
            quit = false;
            thread = new Thread(new ThreadStart(StartThreadLoop));
            thread.Start();

        }

        public void Stop()
        {
            quit = true;
        }

        private void StartThreadLoop()
        {
            while (!quit)
            {
                CMSState curState = getState();
                if (controlTogglerConfig.AutoStartControlEnabled && curState.Equals(CMSState.Tracking))
                {
                    TestControlAutoStart();
                }

                else if (controlTogglerConfig.AutoStopControlEnabled && curState.Equals(CMSState.ControlTracking))
                {
                    TestControlAutoStop();
                }
                Thread.Sleep(controlTogglerConfig.IntervalTime);
            }
        }

        private Point autoStartRetrievedCursorPos = Point.Empty;
        private DateTime lastTestAutoStartTime = new DateTime(0);

        private void TestControlAutoStart()
        {
            DateTime currentTime = DateTime.Now;
            Point curCursorPos = Cursor.Position;
            if (lastTestAutoStartTime.Ticks == 0 || autoStartRetrievedCursorPos.Equals(Point.Empty))
            {
                lastTestAutoStartTime = currentTime;
                autoStartRetrievedCursorPos.X = curCursorPos.X;
                autoStartRetrievedCursorPos.Y = curCursorPos.Y;
                return;
            }

            if (curCursorPos.X == autoStartRetrievedCursorPos.X &&
               curCursorPos.Y == autoStartRetrievedCursorPos.Y)
            {
                TimeSpan ts = currentTime - lastTestAutoStartTime;

                if (ts.Ticks > this.controlTogglerConfig.AutoStartDelay * 10000000)
                {
                    if (toggleControl(true))
                    {
                        if (this.controlTogglerConfig.PlaySoundOnControlChanges)
                            soundPlayer.PlayChangeState();
                        //if (this.controlTogglerConfig.ScrollStart)
                        //    Keyboard.SetState(Keyboard.VirtualKeys.VK_SCROLL, true);
                    }
                }
            }
            else
            {
                lastTestAutoStartTime = currentTime;
                autoStartRetrievedCursorPos.X = curCursorPos.X;
                autoStartRetrievedCursorPos.Y = curCursorPos.Y;
            }

        }

        private void TestControlAutoStop()
        {
            Point currentCursorPos = Cursor.Position;
            PointF trackerCursorPos = GetCursorPos();
            double difx = (double)currentCursorPos.X - trackerCursorPos.X;
            double dify = (double)currentCursorPos.Y - trackerCursorPos.Y;

            if (difx * difx > 50.0 || dify * dify > 50.0)
            {
                if (toggleControl(false))
                {
                    if (this.controlTogglerConfig.PlaySoundOnControlChanges)
                        soundPlayer.PlayChangeState();
                    //if (this.controlTogglerConfig.ScrollStop)
                    //    Keyboard.SetState(Keyboard.VirtualKeys.VK_SCROLL, false);
                }
            }
        }

    }
}
