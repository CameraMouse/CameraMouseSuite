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
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using CameraMouseSuite;
using AHMUtility;

namespace AHMTrackingSuite
{
    public abstract class AHMSetup
    {
        protected CvSize imageSize = new CvSize(0, 0);
        public CvSize ImageSize
        {
            get
            {
                return imageSize;
            }
            set
            {
                imageSize = value;
            }
        }

        protected CMSTrackingSuiteAdapter trackingSuiteAdapter = null;
        public CMSTrackingSuiteAdapter TrackingSuiteAdapter
        {
            get
            {
                return trackingSuiteAdapter;
            }
            set
            {
                trackingSuiteAdapter = value;
            }
        }

        protected bool drawLucasKanade = false;
        public bool DrawLucasKanade
        {
            get
            {
                return drawLucasKanade;
            }
        }

        //protected AHMFinishedSetup finishedSetup = null;
        /*public AHMFinishedSetup FinishedSetup
        {
            get
            {
                return finishedSetup;
            }
            set
            {
                finishedSetup = value;
            }
        }*/

        protected int numTemplates = 9;
        public int NumTemplates
        {
            get
            {
                return numTemplates;
            }
            set
            {
                numTemplates = value;
            }
        }

        protected int obsSize = 100;
        public int ObsSize
        {
            get
            {
                return obsSize;
            }
            set
            {
                obsSize = value;
            }
        }

        protected double ratioVideoInputToMaxOutput;
        public double RatioVideoInputToMaxOutput
        {
            get
            {
                return ratioVideoInputToMaxOutput;
            }
            set
            {
                ratioVideoInputToMaxOutput = value;
            }
        }

        protected List<CvImageWrapper>[] templatesList = null;
        public List<CvImageWrapper>[] TemplatesList
        {
            get
            {
                return templatesList;
            }
        }

        /*
        protected List<CvImageWrapper> templates = new List<CvImageWrapper>();
        public List<CvImageWrapper> Templates
        {
            get
            {
                return templates;
            }
        }
        */

        public abstract void Init(int numTrackingPoints);
        public abstract void ProcessMouse(System.Drawing.Point p, bool leftMouseButton);
        public abstract void ProcessKeys(System.Windows.Forms.Keys keys);
        public bool Process(PointF imagePoint, System.Drawing.Bitmap[] frames)
        {
            return Process(new PointF[] { imagePoint }, frames);
        }
        public abstract bool Process(PointF [] imagePoints, System.Drawing.Bitmap [] frames);
        public abstract void DrawOnFrame(System.Drawing.Bitmap[] frames);
        public abstract void Clean();
    }

    public class AHMTimingSetup : AHMSetup
    {
        private CvImageWrapper curFrame = null;
        public AHMTimingSetup() { }
        public AHMTimingSetup(long setupTimeSeconds) 
        {
            this.setupTimeMillis = setupTimeSeconds*1000;
        }

        private long setupTimeMillis = 10000;
        public long SetipTimeMillis
        {
            get
            {
                return setupTimeMillis;
            }
            set
            {
                setupTimeMillis = value;
            }
        }

        private long startTickCount = 0;
        private long lastTickCount = 0;
        private long messageTickCount = 0;
        private long tickCountSpacing = 0;

        public override void Init(int numTrackingPoints)
        {
            templatesList = new List<CvImageWrapper>[numTrackingPoints];
            for(int i = 0; i < numTrackingPoints; i++)
                templatesList[i] =  new List<CvImageWrapper>();
            base.drawLucasKanade = true;
            tickCountSpacing = (long)(((double)setupTimeMillis) / ((double)numTemplates));
        }

        public override void ProcessMouse(Point p, bool leftMouseButton)
        {
        }

        public override void ProcessKeys(Keys keys)
        {
        }

        private void CaptureImages(PointF [] imagePoints, Bitmap frame)
        {
            try
            {
                if (curFrame == null)
                    curFrame = new CvImageWrapper(frame);
                else
                    curFrame.setImage(frame);

                for (int i = 0; i < imagePoints.Length; i++)
                {
                    PointF imagePoint = imagePoints[i];

                    CvRect cropDimensions = new CvRect();
                    cropDimensions.x = (int)imagePoint.X - obsSize / 2;
                    cropDimensions.y = (int)imagePoint.Y - obsSize / 2;
                    cropDimensions.width = obsSize;
                    cropDimensions.height = obsSize;

                    CvImageWrapper curObs = curFrame.cropSubImage(cropDimensions);

                    this.templatesList[i].Add(curObs);
                }
            }
            catch (Exception e)
            {
            }

        }

        public override bool Process(PointF [] imagePoints, Bitmap[] frames)
        {
            if (startTickCount == 0)
            {
                startTickCount = lastTickCount = Environment.TickCount;
            }
            
            long curTickCount = Environment.TickCount;
            if (curTickCount - startTickCount > setupTimeMillis)
                return false;

            if (curTickCount - lastTickCount > tickCountSpacing)
            {
                CaptureImages(imagePoints, frames[0]);
                lastTickCount = curTickCount;
            }

            if (curTickCount - messageTickCount > 300)
            {
                messageTickCount = curTickCount;
                int second = (int)((setupTimeMillis/1000)-((double)(curTickCount - startTickCount)) / 1000.0);
                trackingSuiteAdapter.SendMessage("Please move naturally for image collection. There are " + second + " seconds remaining");
            }

            return true;
        }

        public override void DrawOnFrame(Bitmap[] frames)
        {
        }

        public override void Clean()
        {
            foreach(List<CvImageWrapper> templates in templatesList)
                templates.Clear();

            if (curFrame != null)
                CvImageWrapper.ReleaseImage(curFrame);
            curFrame = null;
        }

    }

    public class AHMKeyPressSetup : AHMSetup
    {
        private object mutex = new object();
        private int numKeyPressed = 0;

        private PointF[] curPoints = null;
        private CvImageWrapper curFrame = null;
        private bool finished = false;
        public override void ProcessMouse(Point p, bool leftMouseButton)
        {
        }

        public override void ProcessKeys(Keys keys)
        {

            if (finished)
                return;

            lock (mutex)
            {
                if (keys.Equals(Keys.Shift) || keys.Equals(Keys.ShiftKey) ||
                    keys.Equals(Keys.LShiftKey) || keys.Equals(Keys.RShiftKey))
                {
                    for (int i = 0; i < curPoints.Length; i++)
                    {
                        PointF curPoint = curPoints[i];

                        if (curPoint.X <= obsSize / 2 || curPoint.X >= curFrame.Size.Width - obsSize / 2)
                            return;
                        if (curPoint.Y <= obsSize / 2 || curPoint.Y >= curFrame.Size.Height - obsSize / 2)
                            return;

                        if (curFrame == null)
                            return;


                        CvRect cropDimensions = new CvRect();
                        cropDimensions.x = (int)curPoint.X - obsSize / 2;
                        cropDimensions.y = (int)curPoint.Y - obsSize / 2;
                        cropDimensions.width = obsSize;
                        cropDimensions.height = obsSize;

                        CvImageWrapper curObs = curFrame.cropSubImage(cropDimensions);

                        this.templatesList[i].Add(curObs);
                        numKeyPressed++;
                        if (numKeyPressed == this.numTemplates)
                        {
                            Thread.Sleep(300);
                            finished = true;
                        }
                        else
                        {
                            SendMessage();
                        }
                    }
                }
            }
        }

        public override bool Process(PointF [] imagePoints, System.Drawing.Bitmap [] frames)
        {
            lock (mutex)
            {
                if (finished)
                    return false;

                for (int i = 0; i < imagePoints.Length; i++)
                {
                    curPoints[i].X = imagePoints[i].X;
                    curPoints[i].Y = imagePoints[i].Y;
                }
                if (curFrame == null)
                    curFrame = new CvImageWrapper(frames[0]);
                else
                    curFrame.setImage(frames[0]);
                return true;
            }
        }

        public override void Clean()
        {
            lock (mutex)
            {
                foreach(List<CvImageWrapper> templates in templatesList)
                    templates.Clear();

                if (curFrame != null)
                    CvImageWrapper.ReleaseImage(curFrame);
                curFrame = null;
            }
        }

        public override void DrawOnFrame(System.Drawing.Bitmap[] frames)
        {
            
        }
   
        private void SendMessage()
        {
            trackingSuiteAdapter.SendMessage("Press tab to reset, Shift to capture template, " + numKeyPressed + "/"
                                            + this.numTemplates + " templates received.");
        }

        public override void Init(int numTrackingPoints)
        {
            templatesList = new List<CvImageWrapper>[numTrackingPoints];
            curPoints = new PointF[numTrackingPoints];

            for (int i = 0; i < numTrackingPoints; i++)
            {
                templatesList[i] = new List<CvImageWrapper>();
                curPoints[i] = new PointF();
            }

            
            drawLucasKanade = true;
            SendMessage();
        }

    }

    public class AHMRectangleSetup : AHMSetup
    {
        private object mutex = new object();

        private CMSMouseControlModuleStandard mouseControlStandard;
        public CMSMouseControlModuleStandard MouseControlStandard
        {
            get
            {
                return mouseControlStandard;
            }
            set
            {
                mouseControlStandard = value;
            }
        }

        public AHMRectangleSetup() { }
        public AHMRectangleSetup(int setupTime, CMSMouseControlModuleStandard mouseControlStandard) 
        {
            this.setupTime = setupTime;
            this.mouseControlStandard = mouseControlStandard;
        }

        private int setupTime = 0;
        public int SetupTime
        {
            get
            {
                return setupTime;
            }
            set
            {
                setupTime = value;
            }
        }

        private int secondsCount = 0;
        private Pen trackingBoxPen = new Pen(System.Drawing.Brushes.Black, 2);
        private PointF centerPoint = PointF.Empty;
        private PointF relCurPoint = PointF.Empty;
        private int rectangleLength = 0;
        private int curNumRectangles = 0;
        private bool[,] rectangles = null;
        private CvImageWrapper curFrame = null;
        private bool finished = false;
        private bool shouldSendMessage = false;
        private Bitmap setupFrame = null;
        private bool[,] setupRects = null;
        private void InitSetupFrame()
        {
            lock (mutex)
            {
                Graphics g = Graphics.FromImage(setupFrame);
                g.FillRectangle(Brushes.White, 0, 0, setupFrame.Width, setupFrame.Height);

                int rectangleWidth = setupFrame.Width / rectangleLength;
                int rectangleHeight = setupFrame.Height / rectangleLength;

                int offset = (int)(2.0 * ratioVideoInputToMaxOutput);
                setupRects = new bool[rectangleLength, rectangleLength];
                for (int x = 0; x < rectangleLength; x++)
                {
                    for (int y = 0; y < rectangleLength; y++)
                    {
                        if (rectangles[x, y])
                        {
                            setupRects[x, y] = true;

                            int startX = x * rectangleWidth + offset;
                            int startY = y * rectangleHeight + offset;
                            int curRectWidth = rectangleWidth - 2 * offset;
                            if (startX + curRectWidth >= setupFrame.Width - (offset - 1))
                                curRectWidth = setupFrame.Width - offset - startX;
                            int curRectHeight = rectangleHeight - 2 * offset;
                            if (startY + curRectHeight >= setupFrame.Height - (offset - 1))
                                curRectHeight = setupFrame.Height - offset - startY;

                            g.FillRectangle(Brushes.Blue, startX, startY, curRectWidth, curRectHeight);
                        }
                        else
                        {
                            setupRects[x, y] = false;
                        }
                    }
                }
            }
        }

        private void UpdateSetupFrame()
        {
            lock (mutex)
            {
                int rectangleWidth = setupFrame.Width / rectangleLength;
                int rectangleHeight = setupFrame.Height / rectangleLength;

                int offset = (int)(2.0 * ratioVideoInputToMaxOutput);

                Graphics g = Graphics.FromImage(setupFrame);

                for (int x = 0; x < rectangleLength; x++)
                {
                    for (int y = 0; y < rectangleLength; y++)
                    {
                        if (!rectangles[x, y] && setupRects[x,y])
                        {
                            setupRects[x, y] = false;
                            
                            int startX = x * rectangleWidth + offset;
                            int startY = y * rectangleHeight + offset;
                            int curRectWidth = rectangleWidth-2*offset;
                            if (startX + curRectWidth >= setupFrame.Width - (offset - 1))
                                curRectWidth = setupFrame.Width - offset - startX;
                            int curRectHeight = rectangleHeight-2*offset;
                            if (startY + curRectHeight >= setupFrame.Height - (offset - 1))
                                curRectHeight = setupFrame.Height - offset - startY;

                            g.FillRectangle(Brushes.White, startX, startY, curRectWidth, curRectHeight);
                        }
                    }
                }
            }
        }
        public override void ProcessMouse(Point p, bool leftMouseButton)
        {
            
        }

        public override void ProcessKeys(Keys keys)
        {
            
        }

        public override bool Process(PointF  [] imagePoints, System.Drawing.Bitmap[] frames)
        {
            lock (mutex)
            {

                if (finished)
                    return false;

                if (centerPoint.IsEmpty)
                {
                    centerPoint.X = imagePoints[0].X;
                    centerPoint.Y = imagePoints[0].Y;
                    return true;
                }

                if (curFrame == null)
                    curFrame = new CvImageWrapper(frames[0]);
                else
                    curFrame.setImage(frames[0]);


                if (setupFrame == null)
                {
                    setupFrame = new Bitmap(curFrame.Size.Width, curFrame.Size.Height);
                    InitSetupFrame();
                }

                relCurPoint = mouseControlStandard.ComputeRelCursorInWindow(imagePoints[0], centerPoint);
                int xRectIndex = (int)Math.Floor((double)rectangleLength * relCurPoint.X);
                int yRectIndex = (int)Math.Floor((double)rectangleLength * relCurPoint.Y);

                if (xRectIndex >= rectangleLength)
                    xRectIndex = rectangleLength - 1;
                if (yRectIndex >= rectangleLength)
                    yRectIndex = rectangleLength - 1;
                if (xRectIndex < 0)
                    xRectIndex = 0;
                if (yRectIndex < 0)
                    yRectIndex = 0;

                if (rectangles[xRectIndex, yRectIndex] && !finished)
                {
                    rectangles[xRectIndex, yRectIndex] = false;
                    curNumRectangles++;

                    for (int i = 0; i < imagePoints.Length; i++)
                    {
                        PointF imagePoint = imagePoints[i];

                        if (!(imagePoint.X <= obsSize / 2 || imagePoint.X >= curFrame.Size.Width - obsSize / 2) &&
                            !(imagePoint.Y <= obsSize / 2 || imagePoint.Y >= curFrame.Size.Height - obsSize / 2))
                        {

                            CvRect cropDimensions = new CvRect();
                            cropDimensions.x = (int)imagePoint.X - obsSize / 2;
                            cropDimensions.y = (int)imagePoint.Y - obsSize / 2;
                            cropDimensions.width = obsSize;
                            cropDimensions.height = obsSize;

                            CvImageWrapper curObs = curFrame.cropSubImage(cropDimensions);
                            this.templatesList[i].Add(curObs);
                            UpdateSetupFrame();
                        }
                    }

                    if (curNumRectangles == this.numTemplates)
                    {
                        this.finished = true;
                        return false;
                    }
                    else
                    {
                        SendMessage();
                    }

                }

                else if (shouldSendMessage)
                {
                    shouldSendMessage = false;
                    SendMessage();
                }
                return true;
            }
        }

        public override void Clean()
        {
            lock (mutex)
            {

                if (curFrame != null)
                {
                    //foreach (CvImageWrapper img in templates)
                        //CvImageWrapper.ReleaseImage(img);
                    CvImageWrapper.ReleaseImage(curFrame);
                    curFrame = null;
                }
                
                foreach(List<CvImageWrapper> templates in templatesList)
                    templates.Clear();
                
                if(countdownTimer!=null)
                    this.countdownTimer.Dispose();
            }
        }

        public override void DrawOnFrame(System.Drawing.Bitmap[] frames)
        {
            lock (mutex)
            {
                if(setupFrame == null)
                    return;

                Bitmap back = setupFrame.Clone() as Bitmap;

                if (!relCurPoint.IsEmpty)
                {
                    int startX = (int)(relCurPoint.X * (double)frames[0].Width);
                    int startY = (int)(relCurPoint.Y * (double)frames[0].Height);

                    int w = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput),
                                h = (int)(CMSConstants.STANDARD_BOX_LENGTH * ratioVideoInputToMaxOutput);
                    trackingBoxPen.Width = 2F * (float)ratioVideoInputToMaxOutput;

                    Graphics tempG = Graphics.FromImage(back);
                    tempG.DrawRectangle(trackingBoxPen, startX - w / 2, startY - h / 2, w, h);
                }
                frames[0] = back;
                //b = back;
            }
        }

        private object sendMessageMutex = new object();

        private void SendMessage()
        {
            lock (sendMessageMutex)
            {
                string s = "Move cursor over rectangles," +
                " Press Tab to reset. " + curNumRectangles + "/"
                                             + this.numTemplates + " templates received";

                if (setupTime == 0)
                    s += ".";
                else
                    s += ", " + (setupTime - secondsCount) + " seconds remaining";
                trackingSuiteAdapter.SendMessage(s);
            }
        }

        private object countDownMutex = new object();
        private void CountDown(object state)
        {
            lock (countDownMutex)
            {
                if (finished)
                    return;

                secondsCount++;

                if (secondsCount == setupTime)
                {
                    finished = true;
                }
                else
                {
                    shouldSendMessage = true;
                    //SendMessage();
                }
            }
        }

        System.Threading.Timer countdownTimer;
        
        public override void Init(int numTrackingPoints)
        {
            lock (mutex)
            {

                templatesList = new List<CvImageWrapper>[numTrackingPoints];
                for(int i = 0; i < numTrackingPoints; i++)
                    templatesList[i] =  new List<CvImageWrapper>();
            
                drawLucasKanade = false;
                curNumRectangles = 0;
                centerPoint = PointF.Empty;

                setupFrame = null;
                rectangleLength = (int)Math.Sqrt(numTemplates);
                rectangles = new bool[rectangleLength, rectangleLength];
                for (int x = 0; x < rectangleLength; x++)
                {
                    for (int y = 0; y < rectangleLength; y++)
                    {
                        rectangles[x, y] = true;
                    }
                }
                finished = false;

                if (setupTime > 0)
                {
                    TimerCallback CountdownDelegate = new TimerCallback(CountDown);
                    countdownTimer = new System.Threading.Timer(CountdownDelegate, null, 0, 1000);
                    
                    
                }
                SendMessage();
            }
        }

    }



}
