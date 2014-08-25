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
    public partial class CMSMultipleCameraForm : Form, CMSVideoDisplay
    {
        private OtherOutputForm otherForm = null;
        private SettingsForm form = null;
        private Bitmap currentFrame = null;
        private CMSViewAdapter viewAdapter;
        private bool isQuit = false;
        private SafeMessagesPass otherMessagesPass = new SafeMessagesPass();
        private SafeMessagePass standardMessagePass = new SafeMessagePass();

        private Thread otherMessagesThread = null;
        private Thread standardMessagesThread = null;
        private Size [] videoInputSizes;
        private object mutex = new object();

        private int cameraIndex = 0;
        private string[] cameraTitles = null;

        public CMSMultipleCameraForm()
        {
            InitializeComponent();
        }

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
            Bitmap[] bitmaps = null;
            string[] messages = null;
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

        #region Windows Processing

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            cameraIndex = (cameraIndex + 1) % cameraTitles.Length;
            SetCamButton();
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

        private void videoDisplay_Paint(object sender, PaintEventArgs e)
        {
            if (currentFrame != null)
            {
                Image curFrameToDisplay = currentFrame;//.Clone() as Image;
                e.Graphics.DrawImage(curFrameToDisplay, 0, 0, videoDisplay.Width, videoDisplay.Height);
            }
        }

        private void videoDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentFrame == null)
                return;

            double ratio = ((double)currentFrame.Width) / ((double)videoDisplay.Width);

            int newX = (int)(ratio * (double)e.X);
            int newY = (int)(ratio * (double)e.Y);

            MouseEventArgs e2 = new MouseEventArgs(e.Button, e.Clicks, newX, newY, e.Delta);
            viewAdapter.MouseUpOnDisplay(e2, cameraIndex);
        }

        private void CMSMultipleCameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            viewAdapter.Quit();
        }

        #endregion

        #region CMSVideoDisplay Members

        private delegate void SetCamButtonDelegate();

        private void SetCamButton()
        {
            if (buttonCamera.InvokeRequired)
            {
                buttonCamera.Invoke(new SetCamButtonDelegate(SetCamButton));
            }
            else
            {
                this.buttonCamera.Text = cameraTitles[cameraIndex];
            }
        }

        public void Init(CMSViewAdapter viewAdapter)
        {
            this.viewAdapter = viewAdapter;
            form = new SettingsForm();
            otherForm = new OtherOutputForm();
            otherMessagesThread = new Thread(new ThreadStart(OtherFormsGo));
            otherMessagesThread.Start();
            standardMessagesThread = new Thread(new ThreadStart(StandardFormsGo));
            standardMessagesThread.Start();
            form.Init(viewAdapter);           
        }

        public void VideoInputSizeDetermined(Size[] videoInputSizes)
        {
            this.videoInputSizes = videoInputSizes;

            double[] ratios = new double[videoInputSizes.Length];
            for (int i = 0; i < videoInputSizes.Length; i++)
                ratios[i] = (double)videoInputSizes[i].Width / (double)this.videoDisplay.Width;

            viewAdapter.RatioVideoInputToOutput = ratios;

            cameraTitles = viewAdapter.CameraTitles;

            SetCamButton();
        }

        public void SetVideo(Bitmap[] frames)
        {
            currentFrame = frames[cameraIndex];
            videoDisplay.Invalidate();
        }

        public void Quit()
        {
            isQuit = true;
            otherMessagesPass.SetKill();
            standardMessagePass.SetKill();
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

        private delegate Form InvokeNewFormDelegate(Type formType);

        public Form InvokeNewForm(Type formType)
        {
            if (this.InvokeRequired)
            {
                return Invoke(new InvokeNewFormDelegate(InvokeNewForm), new object[] { formType }) as Form;
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
