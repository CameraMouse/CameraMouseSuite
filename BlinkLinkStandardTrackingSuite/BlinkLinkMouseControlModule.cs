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
using System.Drawing;
using System.Diagnostics;

namespace BlinkLinkStandardTrackingSuite
{
    public class BlinkLinkMouseControlModule : CMSMouseControlModule
    {
        private bool firstFrameInControl = false;
        private int waitTime = 70;
        private long lastTickCount = 0;
        private double imageWidth = 0;
        private double imageHeight = 0;
        private double screenWidth = 0;
        private double screenHeight = 0;

        private PointF imageOriginPoint;
        private PointF prevCursorPos;

        private bool   reverseHorizontal = false;
        private bool   moveMouse = true;
        private bool   pauseMouseEnabled = true;
        private double userHorizontalGain = 6.0;
        private double userVerticalGain = 6.0;
        private double damping = 0.65;
        private double eastLimit = 0.0;
        private double southLimit = 0.0;
        private double westLimit = 0.0;
        private double northLimit = 0.0;
        private bool showPanel = true;

        private BlinkLinkClickControlModule clickControlModule = null;

        public bool ShowPanel
        {
            get
            {
                return showPanel;
            }
            set
            {
                showPanel = value;
            }
        }
        public bool MoveMouse
        {
            get
            {
                return moveMouse;
            }
            set
            {
                moveMouse = value;
            }
        }
        public bool PauseMouseEnabled
        {
            get
            {
                return pauseMouseEnabled;
            }
            set
            {
                pauseMouseEnabled = value;
            }
        }
        public float PauseMouseTime
        {
            get
            {
                return clickControlModule.BlinkLinkEyeClickData.ShortWinkTime / 2;
            }
        }
        public bool ReverseHorizontal
        {
            get
            {
                return reverseHorizontal;
            }
            set
            {
                reverseHorizontal = value;
            }
        }
        public double UserHorizontalGain
        {
            get
            {
                return userHorizontalGain;
            }
            set
            {
                userHorizontalGain = value;
            }
        }
        public double UserVerticalGain
        {
            get
            {
                return userVerticalGain;
            }
            set
            {
                userVerticalGain = value;
            }
        }
        public double Damping
        {
            get
            {
                return damping;
            }
            set
            {
                damping = value;
            }
        }
        public double EastLimit
        {
            get
            {
                return eastLimit;
            }
            set
            {
                eastLimit = value;
                UpdateForms();
            }
        }
        public double SouthLimit
        {
            get
            {
                return southLimit;
            }
            set
            {
                southLimit = value;
                UpdateForms();
            }
        }
        public double WestLimit
        {
            get
            {
                return westLimit;
            }
            set
            {
                westLimit = value;
                UpdateForms();
            }
        }
        public double NorthLimit
        {
            get
            {
                return northLimit;
            }
            set
            {
                northLimit = value;
                UpdateForms();
            }
        }
        public BlinkLinkClickControlModule ClickControlModule
        {
            set
            {
                clickControlModule = value;
            }
        }

        private ExcludeForm eastExcludeForm = null;
        private ExcludeForm westExcludeForm = null;
        private ExcludeForm southExcludeForm = null;
        private ExcludeForm northExcludeForm = null;


        private bool PauseMouse
        {
            get
            {
                return PauseMouseEnabled && (clickControlModule.EyeClosedTime >= (PauseMouseTime * 1000)) && !clickControlModule.IsDragging;
            }
        }

        private void UpdateForms()
        {
            double screenWidth = CMSConstants.SCREEN_WIDTH;
            double screenHeight = CMSConstants.SCREEN_HEIGHT;
            if( eastExcludeForm != null )
            {
                int x = (int)((1.0 - eastLimit) * screenWidth);
                eastExcludeForm.SetVertical(x, (int)(northLimit * screenHeight) - 4,
                                           (int)((1.0 - southLimit) * screenHeight) + 4, 6);
            }

            if( westExcludeForm != null )
            {
                int x = (int)(westLimit * screenWidth) - 6;
                westExcludeForm.SetVertical(x, (int)(northLimit * screenHeight) - 4,
                                           (int)((1.0 - southLimit) * screenHeight) + 4, 6);
            }

            if( southExcludeForm != null )
            {
                int y = (int)((1.0 - southLimit) * screenHeight);
                southExcludeForm.SetHorizontal(y, (int)(westLimit * screenWidth) - 4,
                                           (int)((1.0 - eastLimit) * screenWidth) + 4, 6);
            }

            if( northExcludeForm != null )
            {
                int y = (int)(northLimit * screenHeight) - 6;
                northExcludeForm.SetHorizontal(y, (int)(westLimit * screenWidth) - 4,
                                           (int)((1.0 - eastLimit) * screenWidth) + 4, 6);
            }
        }

        private void ShowExcludeForms()
        {
            if( eastLimit > 0.0 )
            {
                if( eastExcludeForm.Created )
                    eastExcludeForm.Visible = true;
                else
                {
                    Size size = eastExcludeForm.Size;
                    Point pt = eastExcludeForm.Location;
                    eastExcludeForm.Show();
                    eastExcludeForm.Size = size;
                    eastExcludeForm.Location = pt;
                }
            }

            if( westLimit > 0.0 )
            {
                if( westExcludeForm.Created )
                    westExcludeForm.Visible = true;
                else
                {
                    Size size = westExcludeForm.Size;
                    Point pt = westExcludeForm.Location;
                    westExcludeForm.Show();
                    westExcludeForm.Size = size;
                    westExcludeForm.Location = pt;

                }
            }

            if( southLimit > 0.0 )
            {
                if( southExcludeForm.Created )
                    southExcludeForm.Visible = true;
                else
                {
                    Size size = southExcludeForm.Size;
                    Point pt = southExcludeForm.Location;
                    southExcludeForm.Show();
                    southExcludeForm.Size = size;
                    southExcludeForm.Location = pt;
                }
            }

            if( northLimit > 0.0 )
            {
                if( northExcludeForm.Created )
                    northExcludeForm.Visible = true;
                else
                {
                    Size size = northExcludeForm.Size;
                    Point pt = northExcludeForm.Location;
                    northExcludeForm.Show();
                    northExcludeForm.Size = size;
                    northExcludeForm.Location = pt;
                }
            }
        }

        private void HideExcludeForms()
        {
            if( eastExcludeForm != null )
                eastExcludeForm.Visible = false;
            if( westExcludeForm != null )
                westExcludeForm.Visible = false;
            if( northExcludeForm != null )
                northExcludeForm.Visible = false;
            if( southExcludeForm != null )
                southExcludeForm.Visible = false;
        }
        /*
        public override CMSModule Clone()
        {
            BlinkLinkMouseControlModule newModule = new BlinkLinkMouseControlModule();
            newModule.ReverseHorizontal = this.reverseHorizontal;
            newModule.UserHorizontalGain = this.userHorizontalGain;
            newModule.UserVerticalGain = this.userVerticalGain;
            newModule.Damping = this.Damping;
            newModule.EastLimit = this.EastLimit;
            newModule.SouthLimit = this.SouthLimit;
            newModule.WestLimit = this.WestLimit;
            newModule.NorthLimit = this.NorthLimit;
            return newModule;
        }
*/
        private PointF AdjustCursor(PointF cur, PointF prev)
        {
            PointF newCursor = new PointF();
            newCursor.X = (float)((cur.X * damping) + (prev.X * (1.0 - damping)));
            newCursor.Y = (float)((cur.Y * damping) + (prev.Y * (1.0 - damping)));

            double east = (1.0 - eastLimit) * screenWidth;
            double west = (westLimit) * screenWidth;
            double north = northLimit * screenHeight;
            double south = (1.0 - southLimit) * screenHeight;

            if( newCursor.X > east )
            {
                imageOriginPoint.X += 1F;
                newCursor.X = (float)east;
            }
            else if( newCursor.X < west )
            {
                imageOriginPoint.X += -1F;
                newCursor.X = (float)west;
            }
            if( newCursor.Y > south )
            {
                imageOriginPoint.Y += 1F;
                newCursor.Y = (float)south;
            }
            else if( newCursor.Y < north )
            {
                imageOriginPoint.Y += -1F;
                newCursor.Y = (float)north;
            }

            if( newCursor.X >= screenWidth )
                newCursor.X -= 8;
            if( newCursor.Y >= screenHeight )
                newCursor.Y -= 15;
            return newCursor;
        }

        public PointF ComputeRelCursorInWindow(PointF imagePoint, PointF imageOrigin)
        {
            PointF absPoint = ComputeCursor(imagePoint, imageOrigin);
            absPoint.X = absPoint.X / (float)screenWidth;
            absPoint.Y = absPoint.Y / (float)screenHeight;

            absPoint.X = (float)((absPoint.X - westLimit) / ((1.0 - eastLimit - westLimit)));
            absPoint.Y = (float)((absPoint.Y - northLimit) / ((1.0 - southLimit - northLimit)));

            if( absPoint.X < 0 )
                absPoint.X = 0;
            if( absPoint.Y < 0 )
                absPoint.Y = 0;

            if( absPoint.X >= 1 )
                absPoint.X = 1;
            if( absPoint.Y >= 1 )
                absPoint.Y = 1;

            return absPoint;
        }

        public PointF ComputeCursor(PointF imagePoint, PointF imageOrigin)
        {
            double difx = imagePoint.X - imageOrigin.X;
            double dify = imagePoint.Y - imageOrigin.Y;

            difx *= this.userHorizontalGain * screenWidth / imageWidth;
            dify *= this.userVerticalGain * screenHeight / imageHeight;

            if( this.reverseHorizontal )
                difx *= -1.0;

            PointF screenPoint = new PointF();
            screenPoint.X = (float)(this.screenWidth / 2.0 + difx);
            screenPoint.Y = (float)(this.screenHeight / 2.0 + dify);

            return screenPoint;
        }

        public override void Init(Size[] imageSizes)
        {
            int imageWidth = imageSizes[0].Width;
            int imageHeight = imageSizes[0].Height;

            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.screenWidth = CMSConstants.SCREEN_WIDTH;
            this.screenHeight = CMSConstants.SCREEN_HEIGHT;

            eastExcludeForm = new ExcludeForm();
            westExcludeForm = new ExcludeForm();
            southExcludeForm = new ExcludeForm();
            northExcludeForm = new ExcludeForm();

            UpdateForms();
        }

        public override void ProcessMouse(System.Drawing.PointF imagePoint, CMSExtraTrackingInfo extraInfo,
                                          System.Drawing.Bitmap[] frames)
        {

            if( state.Equals(CMSState.ControlTracking) && MoveMouse )
            {
                Bitmap frame = frames[0];
                if( firstFrameInControl )
                {
                    imageOriginPoint = new PointF(imagePoint.X, imagePoint.Y);
                }

                PointF tempCursor = ComputeCursor(new PointF(imagePoint.X, imagePoint.Y), imageOriginPoint);

                if( firstFrameInControl )
                {
                    prevCursorPos.X = tempCursor.X;
                    prevCursorPos.Y = tempCursor.Y;
                    firstFrameInControl = false;
                }
                PointF newCursor = AdjustCursor(tempCursor, prevCursorPos);
                prevCursorPos = newCursor;

                long newTickCount = Environment.TickCount;
                if( newTickCount - this.lastTickCount > this.waitTime )
                {
                    int nx = (int)newCursor.X;
                    int ny = (int)newCursor.Y;
                    if( !PauseMouse )
                    {
                        //User32.SetCursorPos(nx, ny);
                        SetCursorPosition(nx, ny);
                    }
                    //mousePointer.X = nx;
                    //mousePointer.Y = ny;
                    lastTickCount = newTickCount;
                }
            }

            clickControlModule.UpdateEyeStatusWindow();
        }

        public override void Clean()
        {
            firstFrameInControl = true;

            if( eastExcludeForm != null )
                eastExcludeForm.Close();
            if( westExcludeForm != null )
                westExcludeForm.Close();
            if( southExcludeForm != null )
                southExcludeForm.Close();
            if( northExcludeForm != null )
                northExcludeForm.Close();
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {

        }

        public override void Update(CMSModule module)
        {
            BlinkLinkMouseControlModule newConfig = module as BlinkLinkMouseControlModule;

            if( newConfig == null )
                throw new Exception("Invalid Config");

            this.reverseHorizontal = newConfig.ReverseHorizontal;
            this.userHorizontalGain = newConfig.UserHorizontalGain;
            this.userVerticalGain = newConfig.UserVerticalGain;
            this.Damping = newConfig.Damping;
            this.EastLimit = newConfig.EastLimit;
            this.SouthLimit = newConfig.SouthLimit;
            this.WestLimit = newConfig.WestLimit;
            this.NorthLimit = newConfig.NorthLimit;
        }

        public override CMSConfigPanel getPanel()
        {
            if (ShowPanel)
            {
                BlinkLinkMouseControlPanel panel = new BlinkLinkMouseControlPanel();
                panel.SetMouseControl(this);
                return panel;
            }
            return null;
        }

        public override void StateChange(CMSState state)
        {
            if( state.Equals(CMSState.ControlTracking) )
            {
                firstFrameInControl = true;
                ShowExcludeForms();
            }
            else
                HideExcludeForms();
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {
        }
    }
}
