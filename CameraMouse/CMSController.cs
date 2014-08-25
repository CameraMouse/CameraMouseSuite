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
using System.Drawing;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    
    public delegate void ToggleSetup(bool setup);
    public delegate void SetSelectedTrackingSuite(string newTrackingSuiteName);
    public delegate PointF GetCursorPos();


    public class CMSController
    {
        private Object mutex = null;
        private CMSModel model = null;
        private CMSVideoDisplay videoDisplay = null;
        private CMSVideoSource videoSource = null;
        private CMSState controllerState = CMSState.CameraNotFound;
        private CMSControlToggler controlToggler = null;
        
        /******************** Video Source Events ***********************/

        private void CameraFound()
        {
            lock (mutex)
            {
                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogCameraFoundEvent"))
                {
                    CMSLogCameraFoundEvent cameraFoundEvent = new CMSLogCameraFoundEvent();
                    if (cameraFoundEvent != null)
                        CMSLogger.SendLogEvent(cameraFoundEvent);
                }

                if (controllerState.Equals(CMSState.CameraNotFound))
                {
                    if (videoSource.StartSource())
                    {
                        SetState(CMSState.Setup);
                        model.CurrentMonikor = videoSource.GetCurrentMonikor();
                    }
                    else
                    {
                        SetState(CMSState.CameraNotFound);
                    }
                }
                else
                {
                    throw new Exception("Received Camera Found in state " + controllerState.ToString());
                }
            }
        }

        private void CameraLost(bool containsCameras)
        {
            lock (mutex)
            {
                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogCameraLostEvent"))
                {
                    CMSLogCameraLostEvent cameraLostEvent = new CMSLogCameraLostEvent();
                    if (cameraLostEvent != null)
                        CMSLogger.SendLogEvent(cameraLostEvent);
                }
                if (!controllerState.Equals(CMSState.CameraNotFound))
                {
                    SetState(CMSState.CameraNotFound);
                    if (model.SelectedSuite != null)
                        model.SelectedSuite.Clean();
               
                    if (containsCameras)
                    {
                        if (videoSource.StartSource())
                        {
                            model.CurrentMonikor = videoSource.GetCurrentMonikor();
                            SetState(CMSState.Setup);
                        }
                        else
                        {
                            MessageBox.Show("Camera Lost");
                        }
                    }
                    else
                        MessageBox.Show("Camera Lost");
                }
                else
                {
                    throw new Exception("Received Camera Lost in state " + controllerState.ToString());
                }
            }
        }

        private void VideoInputSizesDetermined(System.Drawing.Size [] videoInputSizes)
        {
            lock (mutex)
            {
                videoDisplay.VideoInputSizeDetermined(videoInputSizes);
                model.FrameDims = videoInputSizes;

                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogVideoSizesDeterminedEvent"))
                {
                    CMSLogVideoSizesDeterminedEvent vidSizesEvent = new CMSLogVideoSizesDeterminedEvent();
                    if (vidSizesEvent != null)
                    {
                        vidSizesEvent.Sizes = videoInputSizes;
                        CMSLogger.SendLogEvent(vidSizesEvent);
                    }
                }
            }
        }

        void ProcessFrameFromSource(System.Drawing.Bitmap [] frames)
        {   
            lock (mutex)
            {
                if (CMSLogger.CanCreateLogEvent(false, false, true, "CMSLogProcessesEvent"))
                {
                    CMSLogProcessesEvent pEvent = new CMSLogProcessesEvent();
                    pEvent.CaptureProcesses();
                    CMSLogger.SendLogEvent(pEvent);
                }

                if (!controllerState.Equals(CMSState.CameraNotFound))
                {
                    foreach(Bitmap frame in frames)
                        frame.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    CMSTrackingSuite currentTracker = model.SelectedSuite;
                    
                    if (currentTracker != null && frames != null && frames.Length > 0)
                    {
                        if (!currentTracker.Initialized)
                        {
                            Size[] sizes = new Size[frames.Length];
                            for (int i = 0; i < frames.Length; i++)
                                sizes[i] = new Size(frames[i].Width, frames[i].Height);
                            currentTracker.Init(sizes);
                        }


                        bool control = controllerState.Equals(CMSState.ControlTracking);
                        
                        /*
                        CMSLogExperimentFrameEvent logExpFrameEvent = null;

                        if (ExperimentFrameSaver.IsExperimentFrameEnabled())
                        {
                            if (ExperimentFrameSaver.CanSaveLogEvent())
                            {
                                logExpFrameEvent = new CMSLogExperimentFrameEvent(frames[0], currentTracker.Name, 0, 0);
                            }
                        }*/

                        currentTracker.ProcessFrame(frames, control);

                        /*
                        if (logExpFrameEvent != null)
                        {
                            if (currentTracker.TrackingModule != null)
                            {
                                PointF curPoint = currentTracker.TrackingModule.ImagePoint;
                                logExpFrameEvent.X = (int)curPoint.X;
                                logExpFrameEvent.Y = (int)curPoint.Y;
                                ExperimentFrameSaver.SaveLogEvent(logExpFrameEvent);
                            }
                        }*/

                        if (CMSLogger.CanCreateLogEvent(true, true, false, "CMSLogFrameEvent"))
                        {
                            CMSLogFrameEvent frameEvent = new CMSLogFrameEvent();
                            if (frameEvent != null)
                            {
                                frameEvent.SetImages(frames);
                                CMSLogger.SendLogEvent(frameEvent);
                            }
                        }
                    }

                    videoDisplay.SetVideo(frames);
                }
            }
        }

        /******************** Video Display Events ***********************/
        
        public void MouseUpOnDisplay(MouseEventArgs e, int cameraNum)
        {
            lock (this.mutex)
            {
                if (model.SelectedSuite != null)
                {
                    Point pt = new Point(e.X,e.Y);
                    model.SelectedSuite.ProcessMouse(pt, e.Button.Equals(MouseButtons.Left), cameraNum);
                }
            }
        }

        public void SetSelectedTrackingSuite(string newTrackingSuiteName)
        {
            lock (mutex)
            {
                if (newTrackingSuiteName == null)
                    return;
                if (newTrackingSuiteName.Equals(model.SelectedSuiteName))
                    return;

                CMSTrackingSuite oldTrackingSuite = this.model.SelectedSuite;
                if (oldTrackingSuite != null)
                {
                    //oldTrackingSuite.ToggleControl(false);
                    oldTrackingSuite.Clean();
                }

                model.SelectedSuiteName = newTrackingSuiteName;
                ToggleSetup(true);
                CMSTrackingSuite newTrackingSuite = this.model.SelectedSuite;
                newTrackingSuite.CMSTrackingSuiteAdapter = new CMSStandardTrackingSuiteAdapter(model, this,this.videoDisplay);
                newTrackingSuite.Init(model.FrameDims);
                newTrackingSuite.SendSuiteLogEvent();
                ReceiveMessagesFromTracker(null, null);
            }
        }

        public PointF GetCursorPos()
        {
            lock (mutex)
            {
                CMSTrackingSuite selectedSuite = model.SelectedSuite;
                if (selectedSuite == null || selectedSuite.MouseControlModule == null)
                    return PointF.Empty;
                
                return selectedSuite.MouseControlModule.MousePointer;
            }
        }

        public void ChooseNewWebCam()
        {
            lock (mutex)
            {
                videoSource.StopSource();

                
                if (model.SelectedSuite != null)
                {
                    model.SelectedSuite.Clean();
                }
                if (videoSource.StartSource())
                {
                    SetState(CMSState.Setup);
                    model.CurrentMonikor = videoSource.GetCurrentMonikor();
                }
                else
                {
                    SetState(CMSState.CameraNotFound);
                }
            }
        }

        /******************** Events from the tracker ***********************/

        public void ToggleSetup(bool setup)
        {
            lock (mutex)
            {
                if ((controllerState.Equals(CMSState.Setup) ||
                    controllerState.Equals(CMSState.Tracking)) && !setup)
                {
                    SetState(CMSState.Tracking);
                    //videoDisplay.SetTrackingControlMessage(false, true);
                }
                else if ((controllerState.Equals(CMSState.ControlTracking) ||
                        controllerState.Equals(CMSState.Tracking) || controllerState.Equals(CMSState.Setup)) && setup)
                {
                    SetState(CMSState.Setup);
                }
            }
        }

        public void ReceiveMessageFromTracker(string message)
        {
            lock (mutex)
            {
                if (controllerState.Equals(CMSState.Setup))
                {
                    this.videoDisplay.ReceiveMessage(message,Color.ForestGreen);
                }
                else if (controllerState.Equals(CMSState.Tracking))
                {
                    this.videoDisplay.SetTrackingControlMessage(false, message);
                }
                else if (controllerState.Equals(CMSState.ControlTracking))
                {
                    this.videoDisplay.SetTrackingControlMessage(true, message);
                }
            }
        }

        public void ReceiveMessagesFromTracker(Bitmap[] bitmaps, string[] messages)
        {
            //lock (mutex)
            //{
                //if (controllerState.Equals(CMSState.Setup))
                //{
                    this.videoDisplay.ReceiveMessages(bitmaps,messages);
                //}
            //}
        }

        /******************** Communication With Other Classes ***********************/

        void ProcessKeys(Keys keys)
        {
            lock (this.mutex)
            {
                if (model.SelectedSuite != null)
                {
                    model.SelectedSuite.ProcessKeys(keys);
                }
            }
        }

        public bool ToggleControl(bool control)
        {
            lock (mutex)
            {
                //CMSTrackingSuite curSuite = model.SelectedSuite;

                //if (curSuite == null)
                //return false;

                bool correctConfig = (controllerState.Equals(CMSState.Tracking) && control) ||
                               (controllerState.Equals(CMSState.ControlTracking) && !control);
                if (!correctConfig)
                    return false;

                if (control)
                {
                    SetState(CMSState.ControlTracking);
                }
                else
                {
                    SetState(CMSState.Tracking);
                }

                return true;
            }
        }

        public CMSState GetState()
        {
            lock (mutex)
            {
                return controllerState;
            }
        }

        /******************** Other Functions ****************/

        private void SetState(CMSState state)
        {
            lock (mutex)
            {
                /*
                if (state.Equals(CMSState.Tracking))
                {
                    videoDisplay.SetTrackingControlMessage(false, "");
                }
                else if (state.Equals(CMSState.ControlTracking))
                {
                    videoDisplay.SetTrackingControlMessage(true, "");
                }
                else if (state.Equals(CMSState.Setup))
                {
                    videoDisplay.ReceiveMessage("", Color.Black);
                    //videoDisplay.SetTrackingControlMessage(false, false);
                }*/
                controllerState = state;
                CMSTrackingSuite curSuite = model.SelectedSuite;
                if (curSuite != null)
                    curSuite.StateChange(state);

                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogStateChangeEvent"))
                {
                    CMSLogStateChangeEvent stateChangeEvent = new CMSLogStateChangeEvent();
                    stateChangeEvent.State = state.ToString();
                    CMSLogger.SendLogEvent(stateChangeEvent);
                }
            }
        }
        
        public void Close()
        {
            lock (mutex)
            {
                
                if (controllerState.Equals(CMSState.Quitting))
                    return;

                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogEndEvent"))
                {
                    CMSLogEndEvent endEvent = new CMSLogEndEvent();
                    endEvent.SetDateTime(DateTime.Now);
                    if (endEvent != null)
                        CMSLogger.SendLogEvent(endEvent);
                }

                videoSource.StopSource();
                controllerState = CMSState.Quitting;
                CMSTrackingSuite selectedSuite = model.SelectedSuite;
                if (selectedSuite != null)
                    selectedSuite.Clean();
                videoDisplay.Quit();
                controlToggler.Stop();
                CMSLogger.StopLogging();
            }
        }

        public void Start(CMSVideoSource videoSource, CMSVideoDisplay videoDisplay, CMSControlToggler controlToggler)
        {
            mutex = new object();

            try
            {
                ProcessKeys[] procKeysDelegates = null;
                lock (mutex)
                {
                    CMSConstants.Init();
                    controllerState = CMSState.Starting;


                    model = new CMSModel();
                    model.Init("./" + CMSConstants.SUITE_LIB_DIR, "./" + CMSConstants.SUITE_CONFIG_DIR,
                               "./" + CMSConstants.MAIN_CONFIG_FILE,
                               "./" + CMSConstants.MAIN_CAMERA_CONFIG_FILE,
                               "./" + CMSConstants.MAIN_LOG_CONFIG_FILE,
                               "./" + CMSConstants.MAIN_ID_CONFIG_FILE);

                    model.Load();
                    model.IncrementAndSaveSessionNum();
                    
                    CMSLogger.SetUidReceivedDelegate(model.UidUpdated);
                    CMSLogger.Init(model.LogConfig, model.IdConfig);

                    //CMSLogger.CanCreateLogEvent(

                    if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogStartEvent"))
                    {
                        CMSLogStartEvent startEvent = new CMSLogStartEvent();
                        startEvent.SetDateTime(DateTime.Now);
                        CMSLogger.SendLogEvent(startEvent);
                    }

                    this.videoSource = videoSource;
                    videoSource.CameraLost += new CameraLost(CameraLost);
                    videoSource.CameraFound += new CameraFound(CameraFound);
                    videoSource.VideoInputSizesDetermined += new VideoInputSizesDetermined(VideoInputSizesDetermined);
                    videoSource.ProcessFrame += new ProcessFrame(ProcessFrameFromSource);
                    videoSource.Init(videoDisplay.GetParentForm());


                    this.videoDisplay = videoDisplay;
                    videoDisplay.Init(new CMSViewAdapter(model, this,videoSource));

                    model.SelectedSuite.CMSTrackingSuiteAdapter = new CMSStandardTrackingSuiteAdapter(model, this,videoDisplay);

                    string currentMoniker = model.CurrentMonikor;
                    if (videoSource.StartSource(currentMoniker))
                    {
                        string newMoniker = videoSource.GetCurrentMonikor();
                        model.CurrentMonikor = newMoniker;
                        controllerState = CMSState.Setup;
                    }
                    else
                    {
                        controllerState = CMSState.CameraNotFound;
                    }


                    this.controlToggler = controlToggler;
                    controlToggler.GetState = GetState;
                    controlToggler.ToggleControl = ToggleControl;
                    controlToggler.GetCursorPos = GetCursorPos;
                    controlToggler.ControlTogglerConfig = model.GeneralConfig.ControlTogglerConfig;
                    controlToggler.Start();

                    
                    procKeysDelegates = new ProcessKeys[2];
                    procKeysDelegates[0] = ProcessKeys;
                    procKeysDelegates[1] = controlToggler.ProcessKeys;

                }
                CMSKeyHook.initHook(procKeysDelegates);
                Application.Run(videoDisplay.GetParentForm());
                CMSKeyHook.removeHook();
            }
            catch (Exception e)
            {
                try
                {
                    if (CMSLogger.CanCreateLogEvent(true, true, false, "CMSLogExceptionEvent"))
                    {
                        CMSLogExceptionEvent exceptionEvent = new CMSLogExceptionEvent();

                        exceptionEvent.SetException(e);
                        CMSLogger.SendLogEvent(exceptionEvent);
                    }
                }
                catch
                {
                }
                MessageBox.Show("Error occurred during startup:" + e.Message);
                Application.Exit();
            }
        } 

    }
}
