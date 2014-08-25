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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BlinkLinkStandardTrackingSuite
{
    public class BlinkLinkStandardTrackingModule : CMSTrackingModule
    {
        private const int    PixelDepth             = 8;
        private const int    PixelChannels          = 1;
        private const int    PixelColorChannels     = 3;
        private const int    NumberOfTrackingPoints = 3;
        private const int    EyeLocationImageCount  = 3;
        private const int    MousePointIndex        = 0;
        private const int    LeftEyePointIndex      = 1;
        private const int    RightEyePointIndex     = 2;
        private const string InitMessage            = "Please Blink";

        private Pen mouseTrackingBoxPen = new Pen(System.Drawing.Brushes.LimeGreen, 2);
        private CvSize _pwinsz;
        private CvTermCriteria _criteria;
        private CvImageWrapper _grey=null;
        private CvImageWrapper _prev_grey=null;
        private CvImageWrapper _pyramid=null;
        private CvImageWrapper _prev_pyramid=null;
        private CvImageWrapper _swap_temp=null;
        private CvImageWrapper _curFrame = null;
        private CvPoint2D32f[] _last_track_points=null, _current_track_points=null;
        private CvPoint2D32f leftEyeOffset, rightEyeOffset;
        private int _flowflags = 0;
        private byte[] _status = null;
        private CvSize imageSize = new CvSize(0, 0);
        private EyeLocator eyeLocator = null;
        private PointF[] eyeImagePoints = null;

        private bool validTrackPoints = false;

        [DllImport("cv100.dll")]
        private static extern void cvCalcOpticalFlowPyrLK(
            IntPtr old, IntPtr curr, IntPtr oldPyr, IntPtr currPyr,
            [In, Out] CvPoint2D32f[] oldFeatures,
            [In, Out] CvPoint2D32f[] currFeatures,
            int numFeatures, CvSize winSize, int level,
            [In, Out] byte[] status,
            [In, Out] float[] errors,
            CvTermCriteria term,
            int flags);


        public override void Init(Size[] imageSizes)
        {
            Clean();

            _flowflags |= CMSConstants.CV_LKFLOW_PYR_A_READY;
            validTrackPoints = false;
            _pwinsz = new CvSize(10, 10);
            _status = new byte[NumberOfTrackingPoints];
            imageSize.Width = imageSizes[0].Width;
            imageSize.Height = imageSizes[0].Height;

            _last_track_points = new CvPoint2D32f[NumberOfTrackingPoints];
            _current_track_points = new CvPoint2D32f[NumberOfTrackingPoints];

            _criteria = new CvTermCriteria(CMSConstants.CV_TERMCRIT_ITER | CMSConstants.CV_TERMCRIT_EPS, 20, 0.03);

            _curFrame = CvImageWrapper.CreateImage(imageSize, PixelDepth, PixelColorChannels);

            _grey = CvImageWrapper.CreateImage(imageSize, PixelDepth, PixelChannels);

            _prev_grey = CvImageWrapper.CreateImage(imageSize, PixelDepth, PixelChannels);

            _pyramid = CvImageWrapper.CreateImage(imageSize, PixelDepth, PixelChannels);

            _prev_pyramid = CvImageWrapper.CreateImage(imageSize, PixelDepth, PixelChannels);

            _swap_temp = CvImageWrapper.CreateImage(imageSize, PixelDepth, PixelChannels);

            eyeLocator = new EyeLocator(EyeLocationImageCount);
            eyeLocator.Reset();

            eyeImagePoints = new PointF[2];

            CMSTrackingSuiteAdapter.SendMessage(InitMessage);
        }

        public override void ProcessMouse(Point p, bool leftMouseButton, int cameraNum)
        {
        }

        public override void Process(Bitmap[] frames)
        {
            extraTrackingInfo = null;

            Bitmap frame = frames[0];

            if( frame == null )
                throw new Exception("Frame is null!");

            if( frame.Width != imageSize.Width || frame.Height != imageSize.Height )
                throw new Exception("Invalid frame sizes");


            _curFrame.setImage(frame);

            CvImageWrapper.ConvertImageColor(_curFrame, _grey, ColorConversion.BGR2GRAY);

            if( !validTrackPoints )
            {
                eyeLocator.AddImage(frame);

                if( eyeLocator.TrackingPointsFound )
                {
                    _current_track_points[MousePointIndex] = eyeLocator.MouseTrackingPoint;
                    _current_track_points[LeftEyePointIndex] = eyeLocator.LeftEyeTrackingPoint;
                    _current_track_points[RightEyePointIndex] = eyeLocator.RightEyeTrackingPoint;

                    leftEyeOffset.x = eyeLocator.LeftEyePoint.x - eyeLocator.LeftEyeTrackingPoint.x;
                    leftEyeOffset.y = eyeLocator.LeftEyePoint.y - eyeLocator.LeftEyeTrackingPoint.y;

                    rightEyeOffset.x = eyeLocator.RightEyePoint.x - eyeLocator.RightEyeTrackingPoint.x;
                    rightEyeOffset.y = eyeLocator.RightEyePoint.y - eyeLocator.RightEyeTrackingPoint.y;

                    validTrackPoints = true;

                }
                else
                {
                    trackingSuiteAdapter.SendMessage(InitMessage);
                }
            }

            for( int i = 0; i < NumberOfTrackingPoints; ++i )
            {
                SwapPoints(ref _current_track_points[i], ref _last_track_points[i]);
            }

            cvCalcOpticalFlowPyrLK(_prev_grey._rawPtr, _grey._rawPtr, _prev_pyramid._rawPtr,
                _pyramid._rawPtr, _last_track_points, _current_track_points, NumberOfTrackingPoints, _pwinsz, 3,
                _status, null, _criteria, _flowflags);

            if( validTrackPoints )
            {
                for( int i = 0; i < NumberOfTrackingPoints; ++i )
                {
                    if( _status[i] == 0 )
                    {
                        validTrackPoints = false;
                        trackingSuiteAdapter.ToggleSetup(true);
                        eyeLocator.Reset();
                        break;
                    }
                }
            }

            for( int i = 0; i < NumberOfTrackingPoints; ++i )
            {
                LimitTPDelta(ref _current_track_points[i], _last_track_points[i]);
            }


            SwapImages(ref _grey, ref _prev_grey);
            SwapImages(ref _pyramid, ref _prev_pyramid);

            if( validTrackPoints )
            {
                extraTrackingInfo = new BlinkLinkClickControlModule.BlinkLinkCMSExtraTrackingInfo(
                    new CvPoint2D32f(_current_track_points[LeftEyePointIndex].x + leftEyeOffset.x,
                        _current_track_points[LeftEyePointIndex].y + leftEyeOffset.y),
                    new CvPoint2D32f(_current_track_points[RightEyePointIndex].x + rightEyeOffset.x,
                        _current_track_points[RightEyePointIndex].y + rightEyeOffset.y));

                imagePoint.X = _current_track_points[MousePointIndex].x;
                imagePoint.Y = _current_track_points[MousePointIndex].y;

                eyeImagePoints[0].X = _current_track_points[LeftEyePointIndex].x;
                eyeImagePoints[0].Y = _current_track_points[LeftEyePointIndex].y;

                eyeImagePoints[1].X = _current_track_points[RightEyePointIndex].x;
                eyeImagePoints[1].Y = _current_track_points[RightEyePointIndex].y;
            }
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {
            if( !validTrackPoints )
                return;

            double ratioVideoInputToMaxOutput = trackingSuiteAdapter.GetRatioInputToOutput()[0];
            {

                int w = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput),
                    h = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput);
                mouseTrackingBoxPen.Width = 2f * (float)ratioVideoInputToMaxOutput;

                Graphics tempG = Graphics.FromImage(frames[0]);
                tempG.DrawRectangle(mouseTrackingBoxPen, this.imagePoint.X - w / 2, this.imagePoint.Y - h / 2, w, h);
                tempG.DrawRectangle(mouseTrackingBoxPen, eyeImagePoints[0].X - w / 2, eyeImagePoints[0].Y - h / 2, w, h);
                tempG.DrawRectangle(mouseTrackingBoxPen, eyeImagePoints[1].X - w / 2, eyeImagePoints[1].Y - h / 2, w, h);
                tempG.Dispose();
            }
        }

        public override void Clean()
        {
            _flowflags = 0;
            if( this._curFrame != null )
                CvImageWrapper.ReleaseImage(_curFrame);
            if( _grey != null )
                CvImageWrapper.ReleaseImage(_grey);
            if( _prev_grey != null )
                CvImageWrapper.ReleaseImage(_prev_grey);
            if( _pyramid != null )
                CvImageWrapper.ReleaseImage(_pyramid);
            if( _prev_pyramid != null )
                CvImageWrapper.ReleaseImage(_prev_pyramid);
            if( _swap_temp != null )
                CvImageWrapper.ReleaseImage(_swap_temp);
            if( eyeLocator != null )
            {
                eyeLocator.Dispose();
            }
        }

        public override void ProcessKeys(Keys keys)
        {

            if( keys == Keys.F1 )
            {
                validTrackPoints = false;
                eyeLocator.Reset();
                trackingSuiteAdapter.ToggleSetup(true);
                trackingSuiteAdapter.SendMessage(InitMessage);
            }
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
            if( dist > 35 * 35 )
            {
                cur = last;
            }
        }

        /*
        public override CMSModule Clone()
        {
            return new BlinkLinkStandardTrackingModule();
        }
        */
        public override CMSConfigPanel getPanel()
        {
            return null;
        }

        public override void StateChange(CMSState state)
        {
        }
    }
}
