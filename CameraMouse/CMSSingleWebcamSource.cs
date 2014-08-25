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

    public class CMSSingleWebcamSource : CMSVideoSource
    {
        private WebCam webCam = null;
        private Thread cameraWatch = null; 
        private object mutex = new object();
        private int cameraCount = 0;
        private Form parentForm = null;
        private int quitNum = 0;

        public override void Init(Form parentForm)
        {
            this.cameraTitles = new string[] {"Camera"};

            this.parentForm = parentForm;
            //cameraWatch = new Thread(new ThreadStart(CameraWatcher));
            //cameraWatch.Start();
        }

        public override bool StartSource(string preferedCameraMoniker)
        {
            lock (mutex)
            {
                quitNum++;
                isActive = true;
                cameraWatch = new Thread(new ThreadStart(CameraWatcher));
                cameraWatch.Start();

                if (webCam != null)
                {
                    webCam.Stop();
                    webCam = null;
                }
                if (WebCam.CameraCount > 0)
                {
                    if (preferedCameraMoniker != null && preferedCameraMoniker.Length > 0
                       && WebCam.GetFilter(preferedCameraMoniker) == null)
                        preferedCameraMoniker = null;

                    if (WebCam.CameraCount == 1)
                        preferedCameraMoniker = WebCam.AvailableWebCamMonikers[0].Moniker;
                    else if (preferedCameraMoniker == null || preferedCameraMoniker.Length == 0)
                    {
                        CameraSelector cs = new CameraSelector(WebCam.AvailableWebCamMonikers);
                        cs.ShowDialog();
                        preferedCameraMoniker = cs.SelectedCamera;
                    }

                    webCam = new WebCam(preferedCameraMoniker, parentForm);
                    webCam.CaptureDeviceVideoInputSizeDetermined += videoInputSizeDeterminedFunc;
                    webCam.NewFrame += new WebCamEventHandler(webCam_NewFrame);
                    webCam.Start();
                    cameraCount = WebCam.CameraCount;
                    currentMonikor = preferedCameraMoniker;

                    return true;
                }
                return false;
            }
        }

        private void webCam_NewFrame(object sender, WebCamEventArgs e)
        {
            processFrameFunc(e.Bitmap.Clone() as Bitmap);
        }

        public override void StopSource()
        {
            quitNum++;
            isActive = false;
            if (webCam != null)
            {
                webCam.Stop();
                webCam = null;
            }
        }

        public override void DisplayCameraPropertyPage()
        {
            if (webCam != null && isActive)
            {
                webCam.ShowPropertyPage();
            }

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
                                if (count > 0)
                                {
                                    bool sourceStillPluggedIn = false;
                                    foreach (WebCamDescription monikor in WebCam.AvailableWebCamMonikers)
                                    {
                                        if (monikor.Moniker.Equals(currentMonikor))
                                        {
                                            sourceStillPluggedIn = true;
                                            break;
                                        }
                                    }

                                    if (sourceStillPluggedIn)
                                    {
                                        Thread.Sleep(500);
                                        continue;
                                    }
                                }

                                if (webCam != null)
                                {
                                    webCam.Stop();
                                    webCam = null;
                                }

                                bCameraLost = true;
                                
                            }
                            else if (lastCount == 0 && count > 0)
                            {
                                bCameraFound = true;
                            }
                        }

                        if(bCameraLost)
                            CameraLostFunc(count > 0);
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
