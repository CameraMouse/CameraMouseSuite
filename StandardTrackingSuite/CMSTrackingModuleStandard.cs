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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public class CMSTrackingModuleStandard : CMSTrackingModule
    {
        private const int PIXEL_DEPTH = 8;
        private const int PIXEL_CHANNELS = 1;
        private const int PIXEL_COLOR_CHANNELS = 3;

        private Pen trackingBoxPen = new Pen(System.Drawing.Brushes.LimeGreen, 2);
        //private PointF trackingPoint = new PointF(0,0);
        private CvSize _pwinsz;
        private CvTermCriteria _criteria;
        private CvImageWrapper _grey=null;
        private CvImageWrapper _prev_grey=null;
        private CvImageWrapper _pyramid=null;
        private CvImageWrapper _prev_pyramid=null;
        private CvImageWrapper _swap_temp=null;
        private CvImageWrapper _curFrame = null;
        private CvPoint2D32f[] _last_track_points=null, _current_track_points=null;
        private int _flowflags = 0;        
        private byte[] _status = null;
        private CvSize imageSize = new CvSize(0,0);

        private bool validTrackPoint = false;

        private EyeLocator eyeLocator = null;
        private long eyeLocatorTickCount = 0;

        private AutoStartMode autoStartMode = AutoStartMode.None;
        public AutoStartMode AutoStartMode
        {
            get
            {
                return autoStartMode;
            }
            set
            {
                eyeLocatorTickCount = Environment.TickCount;
                autoStartMode = value;
            }
        }

        [DllImport("cv100.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void cvCalcOpticalFlowPyrLK(
            IntPtr old, IntPtr curr, IntPtr oldPyr, IntPtr currPyr,
            [In, Out] CvPoint2D32f[] oldFeatures,
            [In, Out] CvPoint2D32f[] currFeatures,
            int numFeatures, CvSize winSize, int level,
            [In, Out] byte[] status,
            [In, Out] float[] errors,
            CvTermCriteria term,
            int flags);
        
        public override void Init(Size [] imageSizes)
        {
            Clean();

            int imageWidth = imageSizes[0].Width;
            int imageHeight = imageSizes[0].Height;

            eyeLocator = new EyeLocator(3);
            eyeLocator.Reset();
            eyeLocatorTickCount = Environment.TickCount;

            _flowflags |= CMSConstants.CV_LKFLOW_PYR_A_READY;
            validTrackPoint = false;
            _pwinsz = new CvSize(10, 10);
            _status = new byte[1];
            imageSize.Width = imageWidth;
            imageSize.Height = imageHeight;

            _last_track_points = new CvPoint2D32f[1];
            _current_track_points = new CvPoint2D32f[1];
            
            _criteria = new CvTermCriteria(CMSConstants.CV_TERMCRIT_ITER | CMSConstants.CV_TERMCRIT_EPS, 20, 0.03);

            _curFrame = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_COLOR_CHANNELS);

			_grey = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

			_prev_grey = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);
	
			_pyramid = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

			_prev_pyramid = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

			_swap_temp = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

            CMSTrackingSuiteAdapter.SendMessage(CMSConstants.PLEASE_CLICK_TF);
        }

        public override void ProcessMouse(System.Drawing.Point p, bool leftMouseButton, int cameraNum)
        {
            if (cameraNum != 0)
                return;

            if (p.X < 0 || p.X >= this.imageSize.Width ||
                p.Y < 0 || p.Y >= this.imageSize.Height)
            {
                return;
            }

            imagePoint.X = p.X;
            imagePoint.Y = p.Y;
            _current_track_points[0].x = p.X;
            _current_track_points[0].y = p.Y;
            _last_track_points[0].x = p.X;
            _last_track_points[0].y = p.Y;
            if (!validTrackPoint)
            {
                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogStandardStateEvent"))
                {
                    CMSLogStandardStateEvent logEvent = new CMSLogStandardStateEvent();
                    if (logEvent != null)
                    {
                        logEvent.ValidTrackingPoint = true;
                        CMSLogger.SendLogEvent(logEvent);
                    }
                }
            }
            validTrackPoint = true;
            trackingSuiteAdapter.ToggleSetup(false);

        }

        private bool autoStartEnded = false;

        public override void Process(System.Drawing.Bitmap [] frames)
        {
            Bitmap frame = frames[0];
            if (frame == null)
                throw new Exception("Frame is null!");

            if (frame.Width != imageSize.Width || frame.Height != imageSize.Height)
                throw new Exception("Invalid frame sizes");


            _curFrame.setImage(frame);

            CvImageWrapper.ConvertImageColor(_curFrame, _grey, ColorConversion.BGR2GRAY);

            SwapPoints(ref _current_track_points[0], ref _last_track_points[0]);

            cvCalcOpticalFlowPyrLK(_prev_grey._rawPtr, _grey._rawPtr, _prev_pyramid._rawPtr,
                _pyramid._rawPtr, _last_track_points, _current_track_points, 1, _pwinsz, 3,
                _status, null, _criteria, _flowflags);

            if (validTrackPoint && _status[0] == 0)
            {
                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogStandardStateEvent"))
                {
                    CMSLogStandardStateEvent logEvent = new CMSLogStandardStateEvent();
                    if (logEvent != null)
                    {
                        logEvent.ValidTrackingPoint = false;
                        CMSLogger.SendLogEvent(logEvent);
                    }
                }

                eyeLocatorTickCount = Environment.TickCount;
                validTrackPoint = false;
                imagePoint = PointF.Empty;
                trackingSuiteAdapter.ToggleSetup(true);
                trackingSuiteAdapter.SendMessage(CMSConstants.PLEASE_CLICK_TF);
            }


            LimitTPDelta(ref _current_track_points[0], _last_track_points[0]);

            //CvPoint2D32f p = _current_track_points[0];

         
            SwapImages(ref _grey, ref _prev_grey);
            SwapImages(ref _pyramid, ref _prev_pyramid);

            if (validTrackPoint)
            {
                imagePoint.X = _current_track_points[0].x;
                imagePoint.Y = _current_track_points[0].y;
                DrawOnFrame(frame);
            }
            else
            {
                if (!autoStartMode.Equals(AutoStartMode.None))
                {
                    long eyeLocatorNewTickCount = Environment.TickCount;

                    if (eyeLocatorNewTickCount - eyeLocatorTickCount > 10000)
                    {
                        if (!autoStartEnded)
                        {
                            trackingSuiteAdapter.SendMessage("Please Blink");
                            autoStartEnded = true;
                        }

                        eyeLocator.AddImage(frame);
                        if (eyeLocator.TrackingPointsFound)
                        {
                            CvPoint2D32f p = new CvPoint2D32f();

                            if (autoStartMode.Equals(AutoStartMode.LeftEye))
                            {
                                p = eyeLocator.LeftEyeTrackingPoint;
                            }
                            else if (autoStartMode.Equals(AutoStartMode.RightEye))
                            {
                                p = eyeLocator.RightEyeTrackingPoint;
                            }
                            else if (autoStartMode.Equals(AutoStartMode.NoseMouth))
                            {
                                p = eyeLocator.MouseTrackingPoint;
                            }

                            eyeLocator.Reset();

                            imagePoint.X = (int)p.x;
                            imagePoint.Y = (int)p.y;
                            _current_track_points[0].x = p.x;
                            _current_track_points[0].y = p.y;
                            _last_track_points[0].x = p.x;
                            _last_track_points[0].y = p.y;

                            validTrackPoint = true;
                            trackingSuiteAdapter.ToggleSetup(false);
                            trackingSuiteAdapter.ToggleControl(true);
                        }
                    }
                    else
                    {
                        autoStartEnded = false;
                        int second = (int)Math.Round(((double)(10000 - (eyeLocatorNewTickCount - eyeLocatorTickCount))) / 1000.0);
                        trackingSuiteAdapter.SendMessage("Auto Start in " + second + " seconds");
                    }
                }
            }
        }

        private void DrawOnFrame(Bitmap b)
        {
            if(!validTrackPoint)
                return;

            double ratioVideoInputToMaxOutput = trackingSuiteAdapter.GetRatioInputToOutput()[0];
            {
                
                int w = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput),
                    h = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput);
                trackingBoxPen.Width = 2F * (float)ratioVideoInputToMaxOutput;

                Graphics tempG = Graphics.FromImage(b);
                tempG.DrawRectangle(trackingBoxPen, this.imagePoint.X - w / 2, this.imagePoint.Y - h / 2, w, h);
                
            }
        }

        //public override bool ReadyForControl()
        //{
        //    return validTrackPoint;
        //}

    
        public override void Clean()
        {

            if (eyeLocator != null)
            {
                eyeLocator.Dispose();
                eyeLocator = null;
            }


            _flowflags = 0;
            if (this._curFrame != null)
                CvImageWrapper.ReleaseImage(_curFrame);
            if (_grey != null)
                CvImageWrapper.ReleaseImage(_grey);
            if (_prev_grey != null)
                CvImageWrapper.ReleaseImage(_prev_grey);
            if (_pyramid != null)
                CvImageWrapper.ReleaseImage(_pyramid);
            if (_prev_pyramid != null)
                CvImageWrapper.ReleaseImage(_prev_pyramid);
            if (_swap_temp != null)
                CvImageWrapper.ReleaseImage(_swap_temp);
        }
        
        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {
        }

        public override void Update(CMSModule module)
        {
        }

        private void SwapPoints(ref CvPoint2D32f a, ref CvPoint2D32f b)
        {
            CvPoint2D32f temp;
            temp = a;
            a = b;
            b = temp;
        }

        private void SwapImages(ref CvImageWrapper a, ref CvImageWrapper b)
        {
            CvImageWrapper temp;
            temp = a;
            a = b;
            b = temp;
        }

        // find magnitude of change between cur and last TrackPoint in 
        // camera image coordinates. If too far limit change.
        private void LimitTPDelta(ref CvPoint2D32f cur, CvPoint2D32f last)
        {
            double difX = cur.x - last.x;
            double difY = cur.y - last.y;
            double dist = difX * difX + difY * difY;
            //double dist = Math.Sqrt(difX*difX + difY*difY);
            if (dist > 35*35)
            {
                cur = last;
            }
        }

        /*
        public override CMSModule Clone()
        {
            return new CMSTrackingModuleStandard();
        }
        */
        
        public override CMSConfigPanel getPanel()
        {
            return null;
        }

        public override void StateChange(CMSState state)
        {        
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {

        }
    }
}
