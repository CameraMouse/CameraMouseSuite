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
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public enum CMSCameraProfile
    {
        OneCamera,
        LeftRight,
        LeftCenterRight,
        TwoCams,
        ThreeCams,
        FourCams,
        FiveCams,
        SixCams
    }

    public class CMSMultipleWebcamSource : CMSVideoSource
    {
        private Thread cameraWatch = null;
        private object mutex = new object();
        private Form parentForm = null;
        private int quitNum = 0;
        private int dominantWebCam = 0;

        private WebCam[] webCams = null;
        private Bitmap[] frames = null;
        private Size[] videoSizes = null;
        
        private CMSCameraProfile profile = CMSCameraProfile.LeftRight;
        public CMSCameraProfile Profile
        {
            get
            {
                return profile;
            }
            set
            {
                profile = value;
            }
        }

        public override void Init(System.Windows.Forms.Form parentForm)
        {
            currentMonikor = null;
            this.parentForm = parentForm;
            
        }
        
        private void SetupProfile()
        {
            if (profile.Equals(CMSCameraProfile.LeftRight))
            {
                dominantWebCam = 0;
                cameraTitles =  new string[] { "Left", "Right" };
            }
            else if (profile.Equals(CMSCameraProfile.LeftCenterRight))
            {
                dominantWebCam = 1;
                cameraTitles = new string[] { "Left", "Center", "Right" };
            }
            else if (profile.Equals(CMSCameraProfile.TwoCams))
            {
                dominantWebCam = 0;
                cameraTitles = new string[] { "Cam One", "Cam Two" };
            }
            else if (profile.Equals(CMSCameraProfile.ThreeCams))
            {
                dominantWebCam = 0;
                cameraTitles = new string[] { "Cam One", "Cam Two", "Cam Three" };
            }
            else if (profile.Equals(CMSCameraProfile.FourCams))
            {
                dominantWebCam = 0;
                cameraTitles = new string[] { "Cam One", "Cam Two", "Cam Three", "Cam Four" };
            }
            else if (profile.Equals(CMSCameraProfile.FiveCams))
            {
                dominantWebCam = 0;
                cameraTitles = new string[] { "Cam One", "Cam Two", "Cam Three", "Cam Four", "Cam Five" };
            }
            else if (profile.Equals(CMSCameraProfile.SixCams))
            {
                dominantWebCam = 0;
                cameraTitles = new string[] { "Cam One", "Cam Two", "Cam Three", "Cam Four", "Cam Five", "Cam Six" };
            }
            else
                throw new Exception("Unknown Camera Profile");
        }

        public override bool StartSource(string preferedCameraMoniker)
        {
            quitNum++;

            if (WebCam.CameraCount > 0)
            {
                SetupProfile();
                
                MultipleCameraSelector mcs = new MultipleCameraSelector(cameraTitles,WebCam.AvailableWebCamMonikers);
                mcs.ShowDialog();

                string[] cameraSelections = mcs.SelectedCameras;

                if (cameraSelections == null)
                    return false;

                frames = new Bitmap[cameraSelections.Length];
                videoSizes = new Size[cameraSelections.Length];

                for (int i = 0; i < cameraSelections.Length; i++)
                {
                    frames[i] = null;
                    videoSizes[i] = Size.Empty;
                }

                cameraWatch = new Thread(new ThreadStart(CameraWatcher));
                cameraWatch.Start();

                webCams = new WebCam[cameraSelections.Length];
                for (int i = 0; i < webCams.Length; i++)
                {
                    webCams[i] = new WebCam(cameraSelections[i], parentForm);
                    webCams[i].CaptureDeviceVideoInputSizeDetermined += new VideoInputSizeDetermined(CMSMultipleWebcamSource_CaptureDeviceVideoInputSizeDetermined);
                    webCams[i].NewFrame += new WebCamEventHandler(CMSMultipleWebcamSource_NewFrame);
                    webCams[i].Start();
                }
                return true;
            }
            else
                return false;
        }

        void CMSMultipleWebcamSource_NewFrame(object sender, WebCamEventArgs e)
        {
            bool hasNull = false;
            int index = 0;

            for (int i = 0; i < webCams.Length; i++)
            {
                if (webCams[i].Equals(sender))
                {
                    index = i;
                    frames[i] = e.Bitmap.Clone() as Bitmap;
                }
                if (frames[i] == null)
                    hasNull = true;
            }

            if (index == this.dominantWebCam && !hasNull)
            {
                Bitmap[] sendFrames = new Bitmap[frames.Length];
                for (int i = 0; i < frames.Length; i++)
                    sendFrames[i] = frames[i].Clone() as Bitmap;
                base.processFrameFunc(sendFrames);
            }
        }

        void CMSMultipleWebcamSource_CaptureDeviceVideoInputSizeDetermined(object sender, Size videoInputSize)
        {
            bool hasNull = false;
            
            for (int i = 0; i < webCams.Length; i++)
            {
                if (webCams[i].Equals(sender))
                {
                    videoSizes[i] = videoInputSize;
                }
                if (videoSizes[i] == null)
                    hasNull = true;
            }

            if (!hasNull)
            {
                base.videoInputSizesDeterminedFunc(videoSizes);
            }
        }

        public override void StopSource()
        {
            quitNum++;
            isActive = false;
            if (webCams != null)
            {
                foreach (WebCam webCam in webCams)
                {
                    if (webCam != null)
                        webCam.Stop();
                }
                webCams = null;
            }
        }

        public override void DisplayCameraPropertyPage()
        {
            
        }

        private void CameraWatcher()
        {
            bool firstTime = true;
            int lastCount = 0;
            int curQuitNum = quitNum;
            try
            {
                while (curQuitNum == quitNum)
                {
                    if (isActive)
                    {
                        bool bCameraLost = false;
                        bool bCameraFound = false;
                        int count;

                        lock (mutex)
                        {
                            if (firstTime)
                            {
                                firstTime = false;
                                lastCount = WebCam.CameraCount;
                            }

                            count = WebCam.CameraCount;

                            if (count < lastCount)
                            {
                                if(webCams != null)
                                {
                                    foreach(WebCam webCam in webCams)
                                        webCam.Stop();
                                    webCams = null;
                                }

                                bCameraLost = true;

                            }
                            else if (lastCount == 0 && count > 0)
                            {
                                bCameraFound = true;
                            }
                        }

                        if (bCameraLost)
                            CameraLostFunc(true);
                        if (bCameraFound)
                            CameraFoundFunc();

                        lastCount = count;
                    }
                    else
                    {
                        firstTime = true;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
            }
        }

    }
}
