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
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using CameraMouseSuite;
using System.ComponentModel;

namespace BlinkLinkStandardTrackingSuite
{
    public class EyeClicker : IDisposable
    {
        private const string EyesFoundMessage     = "Eyes Found";
        private const string ResetMessage         = "Resetting Eye Templates";

        private class UpdateData : IDisposable
        {
            public Point      LeftEyeLocation;
            public Point      RightEyeLocation;
            public int        XDistBetweenEyes;
            public FastBitmap Image;
            public bool       PerformClicks;

            public UpdateData(Point leftEyeLocation, Point rightEyeLocation,
            int xDistBetweenEyes, FastBitmap image, bool performClicks)
            {
                LeftEyeLocation = leftEyeLocation;
                RightEyeLocation = rightEyeLocation;
                XDistBetweenEyes = xDistBetweenEyes;
                Image = image;
                PerformClicks = performClicks;
            }

            #region IDisposable Members

            public void Dispose()
            {
                Image.Dispose();
            }

            #endregion
        }

        #region Private Constant Data Members

        private const int   TimeToDisplayErrorMessage   = 0;
        private const int   TimeToDisplayRestartMessage = 0;
        private const int   TimeToDisplayStartMessage   = 1000;
        private const float RatioToStopMouse            = 0.2f;

        #endregion

        #region Private Enumerations

        private enum Stages
        {
            Reset,
            Start,
            Initialization,
            Running
        }


        #endregion

        #region Private Data Members

        private          IBlinkDetector               leftEyeBlinkDetector;
        private          IBlinkDetector               rightEyeBlinkDetector;
        private volatile Stages                       currentStage;
        private          float                        stageTime;
        private          bool                         firstUpdate;
        private          CMSTrackingSuiteAdapter      cmsTrackingSuiteAdapter;
        private          BlinkLinkEyeClickData        blinkLinkEyeClickData;
        private          Stopwatch                    timer;
        private          Thread                       workerThread;
        private          EventWaitHandle              waitHandler;
        private          object                       updateDataMutex;
        private volatile UpdateData                   updateData;
        private          object                       workerUpdateDataMutex;
        private volatile UpdateData                   workerUpdateData;
        private          bool                         xDistanceBetweenEyesSet;
        private          EyeStatusWindow              eyeStatusWindow;
        private          bool                         eyeClickerResetOnly;
        private          EyeClickerFiniteStateMachine clickerFSM;


        #endregion

        #region Properties

        private Stages CurrentStage
        {
            set
            {
                currentStage = value;
                stageTime = 0;

                switch( currentStage )
                {

                    case Stages.Start:
                        {
                            if( eyeClickerResetOnly )
                            {
                                cmsTrackingSuiteAdapter.SendMessage(ResetMessage);
                            }
                            else
                            {
                                cmsTrackingSuiteAdapter.SendMessage(EyesFoundMessage);
                                SystemSounds.Beep.Play();
                            }
                        }
                        break;

                    case Stages.Initialization:
                        {
                        }
                        break;

                    case Stages.Running:
                        {
                            cmsTrackingSuiteAdapter.ToggleSetup(false);
                            cmsTrackingSuiteAdapter.ToggleControl(true);
                            clickerFSM.Start();
                        }
                        break;
                }
            }
        }

        public bool Running
        {
            get
            {
                return currentStage == Stages.Running;
            }
        }

        public bool Initializing
        {
            get
            {
                return currentStage == Stages.Initialization;
            }
        }

        public float EyeClosedTime
        {
            get
            {
                return clickerFSM.EyeClosedTime;
            }
        }

        public bool IsDragging
        {
            get
            {
                return clickerFSM.IsDragging;
            }
        }

        #endregion

        private delegate void showDelegate();

        public EyeClicker(CMSTrackingSuiteAdapter cmsTrackingSuiteAdapter,
            BlinkLinkEyeClickData blinkLinkEyeClickData)
        {
            workerUpdateDataMutex = new object();
            eyeStatusWindow = (EyeStatusWindow)cmsTrackingSuiteAdapter.CreateForm(typeof(EyeStatusWindow));
            eyeClickerResetOnly = false;
            timer = new Stopwatch();
            this.blinkLinkEyeClickData = blinkLinkEyeClickData;
            this.cmsTrackingSuiteAdapter = cmsTrackingSuiteAdapter;
            leftEyeBlinkDetector = new NccBlinkDetector(true, cmsTrackingSuiteAdapter);
            rightEyeBlinkDetector = new NccBlinkDetector(false, cmsTrackingSuiteAdapter);
            stageTime = 0;
            firstUpdate = true;
            currentStage = Stages.Start;
            waitHandler = new AutoResetEvent(false);
            updateDataMutex = new object();
            updateData = null;
            xDistanceBetweenEyesSet = false;
            workerThread = new Thread(new ThreadStart(workerThreadMain));
            workerThread.Priority = ThreadPriority.Highest;
            workerThread.Start();
            clickerFSM = new EyeClickerFiniteStateMachine(blinkLinkEyeClickData, eyeStatusWindow);
        }

        private void workerThreadMain()
        {
            while( true )
            {
                waitHandler.WaitOne();

                lock( updateDataMutex )
                {
                    workerUpdateData = updateData;
                    updateData = null;
                }

                Update();
            }
        }

        private void SetXDistanceBetweenEyes(int dist)
        {
            leftEyeBlinkDetector.SetXDistanceBetweenEyes(dist);
            rightEyeBlinkDetector.SetXDistanceBetweenEyes(dist);
        }

        public void Update(Point leftEyeLocation, Point rightEyeLocation, int xDistBetweenEyes,
                                FastBitmap img, bool performClicks)
        {
            UpdateData temp = new UpdateData(leftEyeLocation, rightEyeLocation, xDistBetweenEyes, img, performClicks);
            lock( updateDataMutex )
            {
                if( updateData != null )
                {
                    updateData.Dispose();
                }
                updateData = temp;
                waitHandler.Set();
            }
        }

        public void Update()
        {
            lock( workerUpdateDataMutex )
            {
                if( workerUpdateData == null )
                {
                    return;
                }


                if( firstUpdate )
                {
                    firstUpdate = false;
                    CurrentStage = Stages.Start;
                    timer.Reset();
                    timer.Start();
                }

                if( !xDistanceBetweenEyesSet )
                {
                    xDistanceBetweenEyesSet = true;
                    SetXDistanceBetweenEyes(workerUpdateData.XDistBetweenEyes);
                }

                float elapsedTime = timer.ElapsedMilliseconds;
                timer.Reset();
                timer.Start();

                stageTime += elapsedTime;

                if( currentStage == Stages.Running || currentStage == Stages.Initialization )
                {
                    leftEyeBlinkDetector.Update(workerUpdateData.Image,
                        blinkLinkEyeClickData.SwitchEyes ? workerUpdateData.RightEyeLocation : workerUpdateData.LeftEyeLocation, elapsedTime);

                    rightEyeBlinkDetector.Update(workerUpdateData.Image,
                        blinkLinkEyeClickData.SwitchEyes ? workerUpdateData.LeftEyeLocation : workerUpdateData.RightEyeLocation, elapsedTime);
                }

                switch( currentStage )
                {
                    case Stages.Initialization:
                        {
                            if( leftEyeBlinkDetector.IsReady && rightEyeBlinkDetector.IsReady )
                            {
                                SystemSounds.Beep.Play();
                                CurrentStage = Stages.Running;
                            }
                        }
                        break;

                    case Stages.Start:
                        {
                            if( stageTime > TimeToDisplayStartMessage )
                            {
                                CurrentStage = Stages.Initialization;
                            }
                        }
                        break;

                    case Stages.Running:
                        {
                            stageTime = 0;

                            SetEyeImages();

                            if( workerUpdateData.PerformClicks )
                            {
                                clickerFSM.Update(leftEyeBlinkDetector.EyeOpen, rightEyeBlinkDetector.EyeOpen, elapsedTime);
                            }
                            else
                            {
                                clickerFSM.Reset();
                            }
                        }
                        break;

                    default:
                        {
                            Debug.Assert(false);
                        }
                        break;
                }

                workerUpdateData.Dispose();
            }
        }

        private void SetEyeImages()
        {
            SetEyeImages(leftEyeBlinkDetector.EyeOpen, rightEyeBlinkDetector.EyeOpen);
        }

        private void SetEyeImages(bool leftEyeOpen, bool rightEyeOpen)
        {
            eyeStatusWindow.SetEyeStatusImages(
                leftEyeOpen ? EyeStatusWindow.EyeStatus.Open : EyeStatusWindow.EyeStatus.Closed,
                rightEyeOpen ? EyeStatusWindow.EyeStatus.Open : EyeStatusWindow.EyeStatus.Closed);
        }

        public void Reset(bool eyeClickerResetOnly)
        {
            CurrentStage = Stages.Reset;
            this.eyeClickerResetOnly = eyeClickerResetOnly;
            if( eyeClickerResetOnly )
            {
                cmsTrackingSuiteAdapter.ToggleSetup(true);
            }
            clickerFSM.Reset();
            SetEyeImages(true, true);
            xDistanceBetweenEyesSet = false;
            firstUpdate = true;
            leftEyeBlinkDetector.Restart();
            rightEyeBlinkDetector.Restart();
            try
            {
                workerThread.Abort();
            }
            catch (ThreadStateException)
            {
                workerThread.Resume();
            }
            workerThread.Join();
            waitHandler.Reset();
            updateData = null;
            lock( workerUpdateDataMutex )
            {
                workerUpdateData = null;
            }
            workerThread = new Thread(new ThreadStart(workerThreadMain));
            workerThread.Priority = ThreadPriority.Highest;
            workerThread.Start();
        }

        public void UpdateEyeStatusWindow()
        {
            if( blinkLinkEyeClickData.ShowEyeStatusWindow != eyeStatusWindow.Visible )
            {
                eyeStatusWindow.SetVisibility(blinkLinkEyeClickData.ShowEyeStatusWindow);
            }

            if( blinkLinkEyeClickData.ShowEyeStatusWindow )
            {
                if( blinkLinkEyeClickData.MoveEyeStatusWindow == eyeStatusWindow.Border )
                {
                    eyeStatusWindow.Border = !blinkLinkEyeClickData.MoveEyeStatusWindow;
                }
            }

            if( blinkLinkEyeClickData.ShowEyeStatusWindow && blinkLinkEyeClickData.MoveEyeStatusWindow )
            {
                eyeStatusWindow.UpdateLocation();
            }
        }

        #region IDisposable Members

        private delegate void DisposeDelegate();

        public void Dispose()
        {
            try
            {
                workerThread.Abort();
            }
            catch (ThreadStateException)
            {
                workerThread.Resume();
            }
            workerThread.Join();
            leftEyeBlinkDetector.Dispose();
            rightEyeBlinkDetector.Dispose();

            if( eyeStatusWindow.InvokeRequired )
            {
                eyeStatusWindow.BeginInvoke(new DisposeDelegate(eyeStatusWindow.Dispose));
            }
            else
            {
                eyeStatusWindow.Dispose();
            }
        }

        #endregion

        public unsafe void LabelImage(Bitmap img, double ratioInputToOutput)
        {
            leftEyeBlinkDetector.LabelImage(img, ratioInputToOutput);
            rightEyeBlinkDetector.LabelImage(img, ratioInputToOutput);
        }
    }
}
