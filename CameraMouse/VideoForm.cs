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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CameraMouseSuite
{
    public partial class VideoForm : Form, CMSVideoDisplay
    {
        public VideoForm()
        {
            InitializeComponent();
        }

        private OtherOutputForm otherForm = null;
        private SettingsForm form = null;
        private Bitmap currentFrame = null;
        private Image animationImage = null;
        private bool animating = false;
        private CMSViewAdapter viewAdapter;
        private bool firstWaitingAnimationDisplayFrame = true;
        private bool firstVideoDisplayFrame = true;
        private bool permitVideoOutputWindowResize = false;
        private bool maximized = true;
        private bool isQuit = false;
        private SafeMessagesPass otherMessagesPass = new SafeMessagesPass();
        private SafeMessagePass standardMessagePass = new SafeMessagePass();

        private Thread otherMessagesThread = null;
        private Thread standardMessagesThread = null;
        private Size videoInputSize;
        private object mutex = new object();


        #region Events

        public void Init(CMSViewAdapter viewAdapter)
        {
            animationImage = Properties.Resources.indicator;
            this.viewAdapter = viewAdapter;
            form = new SettingsForm();
            otherForm = new OtherOutputForm();
            //otherForm.Show();
            //otherForm.Visible = false;
            otherMessagesThread = new Thread(new ThreadStart(OtherFormsGo));
            otherMessagesThread.Start();
            standardMessagesThread = new Thread(new ThreadStart(StandardFormsGo));
            standardMessagesThread.Start();

            form.Init(viewAdapter);

        }

        public void VideoInputSizeDetermined(Size [] videoInputSizes)
        {
            this.videoInputSize = videoInputSizes[0];
            permitVideoOutputWindowResize = (((double)videoInputSize.Height / (double)videoInputSize.Width) == .75);

            if (!permitVideoOutputWindowResize)
            {
                this.videoDisplay.Width = (int)videoInputSize.Width;
                this.videoDisplay.Height = (int)videoInputSize.Height;
            }
            else
            {
                maximized = true;
            }

            viewAdapter.RatioVideoInputToOutput  = new double[]{((double)videoInputSize.Width)/((double)videoDisplay.Width)};
        }

        public void SetVideo(Bitmap [] frames)
        {
            currentFrame = frames[0];
            videoDisplay.Invalidate();
        }

        public void Quit()
        {
            isQuit = true;
            otherMessagesPass.SetKill();
            standardMessagePass.SetKill();
            ImageAnimator.StopAnimate(animationImage, new EventHandler(this.AnimateStep));
            if (form.Created)
                form.Close();
            if (otherForm.Created)
                otherForm.Close();
            this.Close();
        }

        public Form GetParentForm()
        {
            return this;
        }

        #endregion

        #region Message Reception

        private delegate void ReceiveMessageDelegate(string message, Color color);
        public void ReceiveMessage(string message, Color color)
        {
            standardMessagePass.SetMessage(message, color, 0);

            /*
            if (this.userMessage.InvokeRequired)
            {
                userMessage.Invoke(new ReceiveMessageDelegate(ReceiveMessage), new object[] { message, color});
            }
            else
            {
                userMessage.ForeColor = color;
                userMessage.Text = message;
            }
            */
        }
        private void StandardFormsGo()
        {
            Color color;
            string message;
            int control;
            while (!isQuit)
            {
                standardMessagePass.GetMessage(out color, out message, out control);
                StandardSetMessage(message, color, control);
            }
        }
        private delegate void StandardSetMessageDelegate(string message, Color color, int control);
        private void StandardSetMessage(string message, Color color, int control)
        {
            if (isQuit)
                return;

            try
            {

                if (InvokeRequired)
                {
                    Invoke(new StandardSetMessageDelegate(StandardSetMessage), new object[] { message, color, control });
                }
                else
                {
                    userMessage.ForeColor = color;
                    userMessage.Text = message;

                    if (control == 1)
                    {
                        controlLabel.Text = CMSConstants.CONTROL_CAMERA_MOUSE;
                    }
                    else if (control == 2)
                    {
                        controlLabel.Text = CMSConstants.CONTROL_MOUSE;
                    }

                }
            }
            catch (Exception e)
            {
            }

        }
        public void SetTrackingControlMessage(bool control, string extraMessage)
        {


            //if (this.controlLabel.InvokeRequired)
            //{
            //try
            //{
            //  controlLabel.Invoke(new SetTrackingControlMessageDelegate(SetTrackingControlMessage), new object[] { control, extraMessage });
            //}
            //catch (Exception e)
            //{
            //}
            //}
            //else
            //{

            string msg = "";

            /*
            if (control)
            {
                controlLabel.Text = CMSConstants.CONTROL_CAMERA_MOUSE;
            }
            else
            {
                controlLabel.Text = CMSConstants.CONTROL_MOUSE;
            }*/



            CMSControlTogglerConfig togglerConfig = viewAdapter.ControlTogglerConfig;
            if (control)
            {
                msg = CMSConstants.TO_STOP_CONTROL + Environment.NewLine;
                if (togglerConfig.ScrollStop)
                    msg += CMSConstants.SCROLL_LOCK_KEY_DESCRIPTION;
                if (togglerConfig.CtrlStop)
                {
                    if (togglerConfig.ScrollStop)
                        msg += ", ";
                    msg += CMSConstants.CTRL_KEY_DESCRIPTION;
                }

                if (togglerConfig.AutoStopControlEnabled)
                {
                    if (togglerConfig.ScrollStop || togglerConfig.CtrlStop)
                        msg += ", ";
                    msg += "Move mouse by hand";
                }

                this.ReceiveMessage(msg, Color.Black);
            }
            else
            {

                msg = CMSConstants.TO_START_CONTROL + Environment.NewLine;
                if (togglerConfig.ScrollStart)
                    msg += CMSConstants.SCROLL_LOCK_KEY_DESCRIPTION;

                if (togglerConfig.CtrlStart)
                {
                    if (togglerConfig.ScrollStart)
                        msg += ", ";
                    msg += CMSConstants.CTRL_KEY_DESCRIPTION;
                }

                if (togglerConfig.AutoStartControlEnabled)
                {
                    if (togglerConfig.CtrlStart || togglerConfig.ScrollStart)
                        msg += ", ";

                    msg += CMSConstants.AUTO_START;
                }


            }

            msg += extraMessage;

            //ReceiveMessage(msg, Color.Black);
            //    }

            standardMessagePass.SetMessage(msg, Color.Black, control ? 1 : 2);

        }

        private delegate void StartOtherFormDelegate();
        private void OtherFormsGo()
        {
            //otherForm = new OtherOutputForm();
            //otherForm.Show();
            //otherForm.Visible = false;
           
            //otherForm = new OtherOutputForm();
            Bitmap [] bitmaps = null;
            string [] messages = null;
            while (!isQuit)
            {
                otherMessagesPass.GetMessages(out bitmaps, out messages);
                if ((bitmaps != null || messages != null) && (!otherForm.Created))
                {
                    CreateOtherForm();
                }
                otherForm.ReceiveMessage(bitmaps, messages);
            }
        }
        public void ReceiveMessages(Bitmap[] bitmaps, string[] messages)
        {
            otherMessagesPass.SetMessages(bitmaps, messages);            
        }

        delegate void CreateOtherFormDelegate();
        void CreateOtherForm()
        {
            if (InvokeRequired)
            {
                Invoke(new CreateOtherFormDelegate(CreateOtherForm));
            }
            else
            {
                otherForm.Show();
            }

        }
   
        #endregion

        #region Window Processing

        private void AnimateStep(object o, EventArgs e)
        {
            this.videoDisplay.Invalidate();
        }

        private void videoDisplay_Paint(object sender, PaintEventArgs e)
        {
            if (currentFrame == null)
            {
                // If this is the first frame of showing the "waiting icon,"
                // then hide the re-size button.
                if (firstWaitingAnimationDisplayFrame)
                {
                    resizeOutputbutton.Visible = false;
                    firstWaitingAnimationDisplayFrame = false;
                    firstVideoDisplayFrame = true;
                }

                int x = this.videoDisplay.Size.Width / 2;
                int y = this.videoDisplay.Size.Height / 2;

                int w = this.animationImage.Size.Width;
                int h = this.animationImage.Size.Height;

                if (!animating)
                {
                    //Begin the animation only once.
                    ImageAnimator.Animate(this.animationImage, new EventHandler(this.AnimateStep));
                    animating = true;
                }

                ImageAnimator.UpdateFrames();

                e.Graphics.DrawImage(animationImage, x - w / 2, y - h / 2, w, h);
            }
            else
            {
                if (animating)
                {
                    animating = false;
                    ImageAnimator.StopAnimate(animationImage, new EventHandler(this.AnimateStep));
                }


                 // If this is the first frame of video dispaly and the re-size
                 // button is to be used, then show it.
                if (firstVideoDisplayFrame)
                {
                    this.resizeOutputbutton.Visible = true;
                    firstWaitingAnimationDisplayFrame = true;
                    firstVideoDisplayFrame = false;
                }

                Graphics tempG = Graphics.FromImage(currentFrame);

                Image curFrameToDisplay = currentFrame;//.Clone() as Image;
                
                // Re-size the image (if necessary)
                e.Graphics.DrawImage(curFrameToDisplay, 0, 0, videoDisplay.Width, videoDisplay.Height);

            }
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            if (form.Visible)
                return;

            if (form.Created)
            {
                form.Visible = true;
                form.BringToFront();
            }
            else
                form.Show();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            viewAdapter.Quit();
        }

        private void resizeOutputbutton_Click(object sender, EventArgs e)
        {
            if (!this.permitVideoOutputWindowResize)
                return;

            maximized = !maximized;
            ResizeFrame(maximized);
        }

        private delegate void ResizeFrameDelegate(bool maximize);

        private void ResizeFrame(bool maximize)
        {
            if (InvokeRequired)
            {
                Invoke(new ResizeFrameDelegate(ResizeFrame), new object[] { maximize });
            }
            else
            {
                if (maximize)
                {
                    videoDisplay.Size = new Size(CMSConstants.VIDEO_DISPLAY_MAX_WIDTH,
                                                 CMSConstants.VIDEO_DISPLAY_MAX_HEIGHT);
                    this.controlPanel.Visible = true;
                    this.TopMost = false;
                    this.toolTipResize.SetToolTip(this.resizeOutputbutton, CMSConstants.MINIMIZE_FORM_TOOLTIP);
                    this.resizeOutputbutton.Image = Properties.Resources.FormMinimize;
                    Size = new Size(CMSConstants.VIDEO_DISPLAY_MAX_WIDTH + 60, CMSConstants.VIDEO_DISPLAY_MAX_HEIGHT + 250);// Set the size of the form
                    this.videoDisplay.Location = new Point(21, 24);
                }
                else
                {
                    

                    videoDisplay.Size = new Size(CMSConstants.VIDEO_DISPLAY_MIN_WIDTH,
                                                 CMSConstants.VIDEO_DISPLAY_MIN_HEIGHT);
                    this.controlPanel.Visible = false;
                    this.TopMost = true;
                    this.toolTipResize.SetToolTip(this.resizeOutputbutton, CMSConstants.MAXIMIZE_FORM_TOOLTIP);
                    this.resizeOutputbutton.Image = Properties.Resources.FormMaximize;
                    Size = new Size(CMSConstants.VIDEO_DISPLAY_MIN_WIDTH + 40, 
                                    CMSConstants.VIDEO_DISPLAY_MIN_HEIGHT + 50 + resizeOutputbutton.Size.Height);// Set the size of the form
                    this.videoDisplay.Location = new Point(16, 5);
                }
                viewAdapter.RatioVideoInputToOutput = new double[]{((double)videoInputSize.Width) / ((double)videoDisplay.Width)};
                int x = this.videoDisplay.Location.X + this.videoDisplay.Width - this.resizeOutputbutton.Size.Width;
                int y = this.videoDisplay.Location.Y + this.videoDisplay.Height + 2;

                this.resizeOutputbutton.Location = new Point(x, y);
                Invalidate();

                if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogWindowChangedEvent"))
                {
                    CMSLogWindowChangedEvent windowChangedEvent = new CMSLogWindowChangedEvent();
                    if (windowChangedEvent != null)
                    {
                        windowChangedEvent.Maximized = maximize;
                        CMSLogger.SendLogEvent(windowChangedEvent);
                    }
                }
            }
        }

        private void videoDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentFrame == null)
                return;

            double ratio = ((double)currentFrame.Width)/((double)videoDisplay.Width);

            int newX = (int)(ratio*(double)e.X);
            int newY = (int)(ratio * (double)e.Y);

            MouseEventArgs e2 = new MouseEventArgs(e.Button, e.Clicks, newX, newY, e.Delta);
            viewAdapter.MouseUpOnDisplay(e2,0);
        }

        private void VideoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            viewAdapter.Quit();
        }

        private delegate Form InvokeNewFormDelegate(Type formType);

        public Form InvokeNewForm(Type formType)
        {
            if (this.InvokeRequired)
            {                
                return Invoke(new InvokeNewFormDelegate(InvokeNewForm), new object[]{ formType}) as Form;
            }
            else
            {
                Form newForm = System.Activator.CreateInstance(formType) as Form;
                newForm.Show();
                return newForm;
            }
        }

        #endregion
    }
}
