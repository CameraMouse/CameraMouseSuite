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
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Threading;
using System.Windows.Forms;
using AHMUtility;
using AHMTrackingSuite;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    public class BlinkLinkAHMTrackingModule : CMSTrackingModule
    {
        private const int NumberOfTrackingPoints = 3;
        private const int EyeLocationImageCount = 3;
        private const int MousePointIndex = 0;
        private const int LeftEyePointIndex = 1;
        private const int RightEyePointIndex = 2;


        private object mutex = new object();

        private EyeLocator eyeLocator = null;
        private long eyeLocatorTickCount = 0;
        private bool autoStartEnded = false;

        #region Common

        private PointF[] setupPoints = null;

        private AHMTrackingState state = AHMTrackingState.NoFeature;

        private void SetState(AHMTrackingState newState)
        {
            string message = null;
            lock (mutex)
            {
                if (CMSLogger.CanCreateLogEvent(false, false, false, "AHMLogStateEvent"))
                {
                    AHMLogStateEvent stateEvent = new AHMLogStateEvent();
                    if (stateEvent != null)
                    {
                        stateEvent.State = newState.ToString();
                        CMSLogger.SendLogEvent(stateEvent);
                    }
                }

                if (newState.Equals(AHMTrackingState.NoFeature))
                {
                    autoStartEnded = false;
                    eyeLocatorTickCount = Environment.TickCount;
                    lastClickPoint = Point.Empty;
                    imagePoint = PointF.Empty;

                    if(eyeLocator != null)
                        eyeLocator.Reset();

                    if (state.Equals(AHMTrackingState.AHMSetup))
                        ahmSetup.Clean();
                    else if (state.Equals(AHMTrackingState.Tracking))
                    {
                        for (int i = 0; i < cameraMouseAssists.Length; i++)
                        {
                            if (cameraMouseAssists[i] != null)
                            {
                                cameraMouseAssists[i].Dispose();
                                cameraMouseAssists[i] = null;
                            }
                        }
                    }

                    trackingSuiteAdapter.ToggleSetup(true);
                    state = AHMTrackingState.NoFeature;
                    message = "Select Feature to Start Setup";

                }
                else if (newState.Equals(AHMTrackingState.AHMSetup))
                {
                    StartAHMSetup();
                    state = AHMTrackingState.AHMSetup;
                }
                else if (newState.Equals(AHMTrackingState.Tracking))
                {
                    //if (state.Equals(AHMTrackingState.AHMSetup))
                    //ahmSetup.Clean();

                    state = AHMTrackingState.Tracking;

                    //trackingSuiteAdapter.ToggleSetup(false);
                    //message = ", Press Tab to reset";
                    //trackingSuiteAdapter.ToggleControl(true);
                }
            }

            if (message != null)
                this.trackingSuiteAdapter.SendMessage(message);
        }

        public override void Init(Size[] imageSizes)
        {

            eyeLocator = new EyeLocator(EyeLocationImageCount);
            eyeLocator.Reset();
            eyeLocatorTickCount = Environment.TickCount;
            lastClickPoint = Point.Empty;

            cameraMouseAssists = new AHMCameraMouseAssist[NumberOfTrackingPoints];

            int imageWidth = imageSizes[0].Width;
            int imageHeight = imageSizes[0].Height;

            lock (mutex)
            {
                CleanStandard();
                InitStandard(imageWidth, imageHeight);
            }

            CMSTrackingSuiteAdapter.SendMessage(CMSConstants.PLEASE_CLICK_TF);
        }

        private void DrawPointOnFrame(Bitmap b)
        {
            lock (mutex)
            {

                double ratioVideoInputToMaxOutput = trackingSuiteAdapter.GetRatioInputToOutput()[0];
                {

                    int w = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput),
                        h = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput);
                    trackingBoxPen.Width = 2F * (float)ratioVideoInputToMaxOutput;

                    Graphics tempG = Graphics.FromImage(b);
                    tempG.DrawRectangle(trackingBoxPen, this.imagePoint.X - w / 2, this.imagePoint.Y - h / 2, w, h);

                }

            }
        }

        public override void Clean()
        {
            lock (mutex)
            {
                SetState(AHMTrackingState.NoFeature);
                CleanStandard();
                CleanAHM();
                if(eyeLocator != null)
                    eyeLocator.Dispose();
            }
        }

        #endregion

        #region IO

        private Point lastClickPoint = Point.Empty;

        public override void ProcessMouse(System.Drawing.Point p, bool leftMouseButton, int cameraNum)
        {
            /*
            if (cameraNum != 0)
                return;

            lock (mutex)
            {
                if (p.X < 0 || p.X >= this.imageSize.Width ||
                        p.Y < 0 || p.Y >= this.imageSize.Height)
                    return;

                if (state.Equals(AHMTrackingState.AHMSetup))
                {
                    ahmSetup.ProcessMouse(p, leftMouseButton);
                }
                else
                {
                    lastClickPoint.X = p.X;
                    lastClickPoint.Y = p.Y;
                    imagePoint.X = p.X;
                    imagePoint.Y = p.Y;
                    _current_track_points[0].x = p.X;
                    _current_track_points[0].y = p.Y;
                    _last_track_points[0].x = p.X;
                    _last_track_points[0].y = p.Y;

                    if (state.Equals(AHMTrackingState.NoFeature))
                        SetState(AHMTrackingState.Feature);
                }
            }
                */
            
        }

        public override void ProcessKeys(System.Windows.Forms.Keys keys)
        {
            lock (mutex)
            {
                /*
                if (state.Equals(AHMTrackingState.Tracking))
                {
                    if (keys.Equals(Keys.F1))
                    {
                        if (!lastClickPoint.IsEmpty)
                        {
                            imagePoint.X = lastClickPoint.X;
                            imagePoint.Y = lastClickPoint.Y;
                            _current_track_points[0].x = lastClickPoint.X;
                            _current_track_points[0].y = lastClickPoint.Y;
                            _last_track_points[0].x = lastClickPoint.X;
                            _last_track_points[0].y = lastClickPoint.Y;
                        }
                    }
                }*/

                if (keys.Equals(Keys.F1))
                {
                    if (state.Equals(AHMTrackingState.AHMSetup) ||
                        state.Equals(AHMTrackingState.Tracking))
                    {
                        SetState(AHMTrackingState.NoFeature);
                    }
                    //else if (state.Equals(AHMTrackingState.Feature))
                    //{
                      //  SetState(AHMTrackingState.AHMSetup);
                    //}
                }
                else if (state.Equals(AHMTrackingState.AHMSetup))
                {
                    ahmSetup.ProcessKeys(keys);
                }
            }
        }
        
        public override void Process(System.Drawing.Bitmap[] frames)
        {
            try
            {
                lock (mutex)
                {
                    Bitmap frame = frames[0];

                    if (frame == null)
                        throw new Exception("Frame is null!");

                    if (frame.Width != imageSize.Width || frame.Height != imageSize.Height)
                        throw new Exception("Invalid frame sizes");

                    _curFrame.setImage(frame);


                    ProcessLucasKanade();

                    //bool drawOnFrame = true;
                    if (state.Equals(AHMTrackingState.NoFeature))
                    {
                        //drawOnFrame = false;
                        extraTrackingInfo = null;
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
                                _current_track_points[MousePointIndex] = eyeLocator.MouseTrackingPoint;
                                _current_track_points[LeftEyePointIndex] = eyeLocator.LeftEyeTrackingPoint;
                                _current_track_points[RightEyePointIndex] = eyeLocator.RightEyeTrackingPoint;

                                leftEyeOffset.x = eyeLocator.LeftEyePoint.x - eyeLocator.LeftEyeTrackingPoint.x;
                                leftEyeOffset.y = eyeLocator.LeftEyePoint.y - eyeLocator.LeftEyeTrackingPoint.y;

                                rightEyeOffset.x = eyeLocator.RightEyePoint.x - eyeLocator.RightEyeTrackingPoint.x;
                                rightEyeOffset.y = eyeLocator.RightEyePoint.y - eyeLocator.RightEyeTrackingPoint.y;



                                SetState(AHMTrackingState.AHMSetup);
                            }
                        }
                        else
                        {
                            int second = (int)Math.Round(((double)(10000 - (eyeLocatorNewTickCount - eyeLocatorTickCount))) / 1000.0);
                            trackingSuiteAdapter.SendMessage("Auto Start in " + second + " seconds");
                        }

                    }
                    else if (state.Equals(AHMTrackingState.Tracking))
                    {
                        //drawOnFrame = false;
                        ProcessAHM();
                        extraTrackingInfo = new BlinkLinkClickControlModule.BlinkLinkCMSExtraTrackingInfo(
                        new CvPoint2D32f(_current_track_points[LeftEyePointIndex].x + leftEyeOffset.x,
                            _current_track_points[LeftEyePointIndex].y + leftEyeOffset.y),
                        new CvPoint2D32f(_current_track_points[RightEyePointIndex].x + rightEyeOffset.x,
                            _current_track_points[RightEyePointIndex].y + rightEyeOffset.y));

                    }
                    else if (state.Equals(AHMTrackingState.AHMSetup))
                    {
                        extraTrackingInfo = null;
                        if (setupPoints == null)
                            setupPoints = new PointF[NumberOfTrackingPoints];
                        for (int i = 0; i < NumberOfTrackingPoints; i++)
                        {
                            setupPoints[i].X = _current_track_points[i].x;
                            setupPoints[i].Y = _current_track_points[i].y;
                        }

                        if (ahmSetup.Process(setupPoints, frames))
                        {
                            //ahmSetup.DrawOnFrame(frames);
                            //drawOnFrame = ahmSetup.DrawLucasKanade;
                        }
                        else
                        {
                            Thread t = new Thread(new ThreadStart(SetupFinished));
                            t.Start();

                            //SetupFinished();
                            //drawOnFrame = false;
                            SetState(AHMTrackingState.Tracking);
                        }
                    }

                    //if (drawOnFrame)
                      //  DrawPointOnFrame(frame);

                }
            }
            catch (Exception e)
            {
            }

        }

        #endregion

        #region Public Properties

        private bool simplePanel = false;
        public bool SimplePanel
        {
            get
            {
                return simplePanel;
            }
            set
            {
                simplePanel = value;
            }
        }

        private bool showMessages = false;

        private CvSize imageSize = new CvSize(0, 0);

        private CMSMouseControlModuleStandard mouseControlModuleStandard = null;
        [XmlIgnore()]
        public CMSMouseControlModuleStandard MouseControlModuleStandard
        {
            get
            {
                return mouseControlModuleStandard;
            }
            set
            {
                mouseControlModuleStandard = value;
            }
        }

        private bool kernelLightingCorrection = false;
        public bool KernelLightingCorrection
        {
            get
            {
                lock (mutex)
                {
                    return kernelLightingCorrection;
                }
            }
            set
            {
                lock (mutex)
                {
                    kernelLightingCorrection = value;
                }
            }
        }

        private int numTemplates = 16;
        public int NumTemplates
        {
            get
            {
                lock (mutex)
                {
                    return numTemplates;
                }
            }
            set
            {
                lock (mutex)
                {
                    numTemplates = value;
                }
            }
        }

        private AHMSetupType setupType = AHMSetupType.Timing15Sec;
        public AHMSetupType SetupType
        {
            get
            {
                lock (mutex)
                {
                    return setupType;
                }
            }
            set
            {
                lock (mutex)
                {
                    setupType = value;
                }
            }
        }

        private int obsSize = 100;
        public int ObsSize
        {
            get
            {
                lock (mutex)
                {
                    return obsSize;
                }
            }
            set
            {
                lock (mutex)
                {
                    obsSize = value;
                }
            }
        }

        private int updateFrequency = 500;
        public int UpdateFrequency
        {
            get
            {
                return updateFrequency;
            }
            set
            {
                updateFrequency = value;
            }
        }
        
        #endregion

        #region AHM

        private AHMSetup ahmSetup = null;
        private AHMCameraMouseAssist [] cameraMouseAssists = null;

        private void StartAHMSetup()
        {
            lock (mutex)
            {
                for (int i = 0; i < NumberOfTrackingPoints; i++)
                {
                    
                    if (cameraMouseAssists[i] != null)
                    {
                        cameraMouseAssists[i].Dispose();
                        cameraMouseAssists[i] = null;
                    }
                    cameraMouseAssists[i] = new AHMCameraMouseAssist();
                }

                /*
                if (setupType.Equals(AHMSetupType.KeyPress))
                    ahmSetup = new AHMKeyPressSetup();
                else if (setupType.Equals(AHMSetupType.Movement30Sec))
                    ahmSetup = new AHMRectangleSetup(30, mouseControlModuleStandard);
                else if (setupType.Equals(AHMSetupType.Movement45Sec))
                    ahmSetup = new AHMRectangleSetup(45, mouseControlModuleStandard);
                else if (setupType.Equals(AHMSetupType.Movement60Sec))
                    ahmSetup = new AHMRectangleSetup(60, mouseControlModuleStandard);
                else if (setupType.Equals(AHMSetupType.MovementInfinite))
                    ahmSetup = new AHMRectangleSetup(0, mouseControlModuleStandard);
                else if (setupType.Equals(AHMSetupType.Timing15Sec))
                    */
                    ahmSetup = new AHMTimingSetup(10);

                ahmSetup.NumTemplates = numTemplates;
                ahmSetup.TrackingSuiteAdapter = trackingSuiteAdapter;
                //ahmSetup.FinishedSetup = SetupFinished;
                ahmSetup.ImageSize = imageSize;
                ahmSetup.ObsSize = obsSize;
                ahmSetup.RatioVideoInputToMaxOutput = trackingSuiteAdapter.GetRatioInputToOutput()[0];
                ahmSetup.Init(NumberOfTrackingPoints);
            }
        }

        private void CleanAHM()
        {
            lock (mutex)
            {
                if (_AHMBackFeature != null)
                {
                    CvImageWrapper.ReleaseImage(_AHMBackFeature);
                    _AHMBackFeature = null;
                }

                if (_AHMCurFeature != null)
                {
                    CvImageWrapper.ReleaseImage(_AHMCurFeature);
                    _AHMCurFeature = null;
                }

                if (_AHMRealtimeObs != null)
                {
                    CvImageWrapper.ReleaseImage(_AHMRealtimeObs);
                    _AHMRealtimeObs = null;
                }

                if (cameraMouseAssists != null)
                {
                    for (int i = 0; i < NumberOfTrackingPoints; i++)
                    {
                        if (cameraMouseAssists[i] != null)
                        {
                            cameraMouseAssists[i].Dispose();
                            cameraMouseAssists[i] = null;
                        }
                    }
                }

                if (ahmSetup != null)
                {
                    ahmSetup.Clean();
                    ahmSetup = null;
                }
            }
        }

        private Bitmap[] extraImages = new Bitmap[2];
        private CvImageWrapper _AHMCurFeature, _AHMBackFeature, _AHMRealtimeObs;
        private double[] _AHMWeights = null;
        private float _AHMRelXPos = -1, _AHMRelYPos = -1;
        private CvRect _AHMRect;

        private long ticks = 0;

        private void ProcessAHM()
        {
            lock (mutex)
            {
                if (cameraMouseAssists == null)
                    return;

                double ccMinProjSqdDist = 0.0;
                double ccTanSqdDist = 0.0;

                try
                {
                    List<Bitmap> images = null;
                    if (showMessages)
                        images = new List<Bitmap>();

                    long newTicks = Environment.TickCount;

                    if (newTicks - ticks > updateFrequency || updateFrequency == 0)
                    {
                        ticks = newTicks;
                        for (int i = 0; i < NumberOfTrackingPoints; i++)
                        {
                            AHMCameraMouseAssist cameraMouseAssist = cameraMouseAssists[i];
                            if (cameraMouseAssist == null)
                                continue;

                            if (cameraMouseAssist != null && cameraMouseAssist.isReady())
                            {

                                if (_AHMWeights == null || _AHMWeights.Length != this.numTemplates)
                                    _AHMWeights = new double[numTemplates];

                                float relX = 0.0f;
                                float relY = 0.0f;

                                if ((this._current_track_points[i].x >= obsSize / 2 + 10) && (_current_track_points[i].y >= obsSize / 2 + 10) &&
                                 ((_current_track_points[i].x + obsSize / 2 + 10) < imageSize.Width) &&
                                 ((_current_track_points[i].y + obsSize / 2 + 10) < imageSize.Height))
                                {
                                    if (this._AHMRealtimeObs == null)
                                        this._AHMRealtimeObs = CvImageWrapper.CreateImage(new CvSize(obsSize + 20, obsSize + 20), 8, 3);
                                    if (this._AHMBackFeature == null)
                                        this._AHMBackFeature = CvImageWrapper.CreateImage(new CvSize(obsSize, obsSize), 8, 3);
                                    if (this._AHMCurFeature == null)
                                        this._AHMCurFeature = CvImageWrapper.CreateImage(new CvSize(obsSize, obsSize), 8, 3);

                                    _AHMRect.x = (int)_current_track_points[i].x - obsSize / 2 - 10;
                                    _AHMRect.y = (int)_current_track_points[i].y - obsSize / 2 - 10; ;
                                    _AHMRect.height = obsSize + 20;
                                    _AHMRect.width = obsSize + 20;

                                    _curFrame.cropSubImage(_AHMRect, this._AHMRealtimeObs);

                                    unsafe
                                    {

                                        int dx = 0, dy = 0;

                                        IntPtr pMinProjSqdDist = new IntPtr(&ccMinProjSqdDist);
                                        IntPtr pTanSqdDist = new IntPtr(&ccTanSqdDist);
                                        IntPtr pDx = new IntPtr(&dx);
                                        IntPtr pDy = new IntPtr(&dy);

                                        cameraMouseAssist.computeRelativePos(pMinProjSqdDist, pTanSqdDist, this._AHMRealtimeObs._rawPtr, pDx, pDy, _AHMWeights);

                                        /*
                                        if (CMSLogger.CanCreateLogEvent(true, false, false, "AHMLogRealtimeEvent"))
                                        {
                                            AHMLogRealtimeEvent lEvent = new AHMLogRealtimeEvent();
                                            if (lEvent != null)
                                            {
                                                lEvent.ProjSqrdDist = (float)ccMinProjSqdDist;
                                                lEvent.TanSqrdDist = (float)ccTanSqdDist;
                                                CMSLogger.SendLogEvent(lEvent);
                                            }
                                        }*/


                                        _current_track_points[i].x = _current_track_points[i].x + dx;
                                        _current_track_points[i].y = _current_track_points[i].y + dy;

                                        if (i == 0)
                                        {
                                            imagePoint.X = _current_track_points[0].x;
                                            imagePoint.Y = _current_track_points[0].y;
                                        }
                                        else
                                        {
                                            eyeImagePoints[i - 1].X = (int)_current_track_points[i].x;
                                            eyeImagePoints[i - 1].Y = (int)_current_track_points[i].y;
                                        }




                                        if (showMessages)
                                        {
                                            cameraMouseAssist.retrieveBackFeature(this._AHMBackFeature._rawPtr);
                                            cameraMouseAssist.retrieveCurFeature(this._AHMCurFeature._rawPtr);

                                            images.Add(_AHMBackFeature.GetBitMap().Clone() as Bitmap);
                                            images.Add(_AHMCurFeature.GetBitMap().Clone() as Bitmap);
                                        }

                                        /*
                                        AHMLogRealtimeFeatureImagesEvent realtimeEvent = null;
                                        if (CMSLogger.CanCreateLogEvent(true, true, false, "AHMLogRealtimeFeatureImagesEvent"))
                                        {
                                            realtimeEvent = new AHMLogRealtimeFeatureImagesEvent();
                                        }
                                        if (realtimeEvent != null)
                                        {
                                            cameraMouseAssist.retrieveBackFeature(this._AHMBackFeature._rawPtr);
                                            cameraMouseAssist.retrieveCurFeature(this._AHMCurFeature._rawPtr);

                                            extraImages[0] = _AHMBackFeature.GetBitMap().Clone() as Bitmap;
                                            extraImages[1] = _AHMCurFeature.GetBitMap().Clone() as Bitmap;

                                            extraImages[0].RotateFlip(RotateFlipType.RotateNoneFlipX);
                                            extraImages[1].RotateFlip(RotateFlipType.RotateNoneFlipX);

                                            realtimeEvent.SetImages(extraImages[0], extraImages[1]);
                                            CMSLogger.SendLogEvent(realtimeEvent);
                                        }*/
                                    }
                                }
                            }
                        }
                    }
                    if (showMessages && images.Count > 0)
                        trackingSuiteAdapter.SendMessages(images.ToArray(), new string[] { "Left Back", "Left Cur", "Right Back", "Right Cur", "Mouth Back", "Mouth Cur" });
                }
                catch (Exception e)
                {
                }
            }
        }
     
        private void SetupFinished()
        {
            lock (mutex)
            {
                cameraMouseAssists = new AHMCameraMouseAssist[NumberOfTrackingPoints];

                for (int j = 0; j < NumberOfTrackingPoints; j++)
                {
                    cameraMouseAssists[j] = new AHMCameraMouseAssist();

                    IntPtr[] images = new IntPtr[this.ahmSetup.TemplatesList[j].Count];

                    int i = 0;
                    foreach (CvImageWrapper imageWrapper in this.ahmSetup.TemplatesList[j])
                        images[i++] = imageWrapper._rawPtr;

                    int numSubset = (int)Math.Ceiling(Math.Sqrt(this.ahmSetup.TemplatesList[j].Count));

                    ahmSetup.TemplatesList[j].Clear();

                    int kernelType = 0;
                    if (kernelLightingCorrection)
                        kernelType = 2;

                    double dimension = this.cameraMouseAssists[j].init(images, numSubset, 10, 1, 0.6, kernelType);
                }
                ahmSetup.Clean();
            }
        }

        #endregion

        #region Standard Camera Mouse

        private const int PIXEL_DEPTH = 8;
        private const int PIXEL_CHANNELS = 1;
        private const int PIXEL_COLOR_CHANNELS = 3;

        private PointF[] eyeImagePoints = null;
        private CvPoint2D32f leftEyeOffset, rightEyeOffset;

        private Pen trackingBoxPen = new Pen(System.Drawing.Brushes.LimeGreen, 2);
        //private PointF trackingPoint = new PointF(0,0);
        private CvSize _pwinsz;
        private CvTermCriteria _criteria;
        private CvImageWrapper _grey = null;
        private CvImageWrapper _prev_grey = null;
        private CvImageWrapper _pyramid = null;
        private CvImageWrapper _prev_pyramid = null;
        private CvImageWrapper _swap_temp = null;
        private CvImageWrapper _curFrame = null;
        private CvPoint2D32f[] _last_track_points = null, _current_track_points = null;
        private int _flowflags = 0;
        private byte[] _status = null;

        //private bool validTrackPoint = false;

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

        /*
        [DllImport("cv100.dll")]
        private static extern void cvCalcOpticalFlowPyrLK(
            IntPtr old, IntPtr curr, IntPtr oldPyr, IntPtr currPyr,
            [In, Out] CvPoint2D32f[] oldFeatures,
            [In, Out] CvPoint2D32f[] currFeatures,
            int numFeatures, CvSize winSize, int level,
            [In, Out] byte[] status,
            [In, Out] float[] errors,
            CvTermCriteria term,
            int flags);*/

        private void CleanStandard()
        {
            lock (mutex)
            {
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
        }

        private void InitStandard(int imageWidth, int imageHeight)
        {
            lock (mutex)
            {
                _flowflags |= CMSConstants.CV_LKFLOW_PYR_A_READY;
                //validTrackPoint = false;
                _pwinsz = new CvSize(10, 10);
                _status = new byte[NumberOfTrackingPoints];
                imageSize.Width = imageWidth;
                imageSize.Height = imageHeight;

                _last_track_points = new CvPoint2D32f[NumberOfTrackingPoints];
                _current_track_points = new CvPoint2D32f[NumberOfTrackingPoints];

                _criteria = new CvTermCriteria(CMSConstants.CV_TERMCRIT_ITER | CMSConstants.CV_TERMCRIT_EPS, 20, 0.03);

                _curFrame = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_COLOR_CHANNELS);

                _grey = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

                _prev_grey = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

                _pyramid = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

                _prev_pyramid = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

                _swap_temp = CvImageWrapper.CreateImage(imageSize, PIXEL_DEPTH, PIXEL_CHANNELS);

                eyeImagePoints = new PointF[2];
            }
        }

        private void ProcessLucasKanade()
        {
            lock (mutex)
            {
                CvImageWrapper.ConvertImageColor(_curFrame, _grey, ColorConversion.BGR2GRAY);

                for (int i = 0; i < NumberOfTrackingPoints; ++i)
                {
                    SwapPoints(ref _current_track_points[i], ref _last_track_points[i]);
                }
                

                cvCalcOpticalFlowPyrLK(_prev_grey._rawPtr, _grey._rawPtr, _prev_pyramid._rawPtr,
                    _pyramid._rawPtr, _last_track_points, _current_track_points, NumberOfTrackingPoints, _pwinsz, 3,
                    _status, null, _criteria, _flowflags);

                if (!state.Equals(AHMTrackingState.NoFeature))
                {
                    for (int i = 0; i < NumberOfTrackingPoints; ++i)
                    {
                        if (_status[i] == 0)
                            SetState(AHMTrackingState.NoFeature);
                    }
                }

                for(int i = 0; i < NumberOfTrackingPoints; i++)
                    LimitTPDelta(ref _current_track_points[i], _last_track_points[i]);

                //CvPoint2D32f p = _current_track_points[0];


                SwapImages(ref _grey, ref _prev_grey);
                SwapImages(ref _pyramid, ref _prev_pyramid);

                if (!state.Equals(AHMTrackingState.NoFeature))
                {
                    imagePoint.X = _current_track_points[0].x;
                    imagePoint.Y = _current_track_points[0].y;

                    eyeImagePoints[0].X = _current_track_points[LeftEyePointIndex].x;
                    eyeImagePoints[0].Y = _current_track_points[LeftEyePointIndex].y;
                    
                    eyeImagePoints[1].X = _current_track_points[RightEyePointIndex].x;
                    eyeImagePoints[1].Y = _current_track_points[RightEyePointIndex].y;

                }
            }
        }

        private void SwapPoints(ref CvPoint2D32f a, ref CvPoint2D32f b)
        {
            lock (mutex)
            {
                CvPoint2D32f temp;
                temp = a;
                a = b;
                b = temp;
            }
        }

        private void SwapImages(ref CvImageWrapper a, ref CvImageWrapper b)
        {
            lock (mutex)
            {
                CvImageWrapper temp;
                temp = a;
                a = b;
                b = temp;
            }
        }

        // find magnitude of change between cur and last TrackPoint in 
        // camera image coordinates. If too far limit change.
        private void LimitTPDelta(ref CvPoint2D32f cur, CvPoint2D32f last)
        {
            lock (mutex)
            {
                double difX = cur.x - last.x;
                double difY = cur.y - last.y;
                double dist = difX * difX + difY * difY;
                //double dist = Math.Sqrt(difX*difX + difY*difY);
                if (dist > 35 * 35)
                {
                    cur = last;
                }
            }
        }

        #endregion

        #region Other Functions

        public override void Update(CMSModule module)
        {
            BlinkLinkAHMTrackingModule trackModule = module as BlinkLinkAHMTrackingModule;
            KernelLightingCorrection = trackModule.KernelLightingCorrection;
            NumTemplates = trackModule.NumTemplates;
            SetupType = trackModule.SetupType;
            ObsSize = trackModule.ObsSize;
            UpdateFrequency = trackModule.UpdateFrequency;
            //SetState(AHMTrackingState.NoFeature);
        }
        /*
        public override CMSModule Clone()
        {
            lock (mutex)
            {
                AHMTrackingModule track = new AHMTrackingModule();
                track.KernelLightingCorrection = KernelLightingCorrection;
                track.NumTemplates = NumTemplates;
                track.SetupType = SetupType;
                track.ObsSize = ObsSize;

                return track;
            }
        }
        */
        public override CMSConfigPanel getPanel()
        {
            lock (mutex)
            {
                return null;
            }
        }

        public override void StateChange(CMSState state)
        {
            //if (state.Equals(CMSState.Tracking) || state.Equals(CMSState.ControlTracking))
              //  trackingSuiteAdapter.SendMessage(", Press Tab to reset");
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {
            if (!(state.Equals(AHMTrackingState.Tracking) || state.Equals(AHMTrackingState.AHMSetup)))
                return;

            double ratioVideoInputToMaxOutput = trackingSuiteAdapter.GetRatioInputToOutput()[0];
            {

                int w = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput),
                    h = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput);
                trackingBoxPen.Width = 2F * (float)ratioVideoInputToMaxOutput;

                Graphics tempG = Graphics.FromImage(frames[0]);
                tempG.DrawRectangle(trackingBoxPen, this.imagePoint.X - w / 2, this.imagePoint.Y - h / 2, w, h);
                tempG.DrawRectangle(trackingBoxPen, eyeImagePoints[0].X - w / 2, eyeImagePoints[0].Y - h / 2, w, h);
                tempG.DrawRectangle(trackingBoxPen, eyeImagePoints[1].X - w / 2, eyeImagePoints[1].Y - h / 2, w, h);
                tempG.Dispose();
            }
        }

        #endregion

    }
}
