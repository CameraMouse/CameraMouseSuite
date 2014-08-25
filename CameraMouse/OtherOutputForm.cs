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

namespace CameraMouseSuite
{

    public partial class OtherOutputForm : Form
    {
        //private List<PictureAndTextControl> picTexControls = new List<PictureAndTextControl>();
        private List<PictureBox> picControls = new List<PictureBox>();
        private List<Label> labels = new List<Label>();
        private object mutex = new object();
        public OtherOutputForm()
        {
            InitializeComponent();
        }

        /*
        public void ReceiveMessage(Bitmap[] bitmaps, string[] messages)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SendMessages(ReceiveMessage), new object[] { bitmaps, messages });
            }
            else
            {
                if (!Visible)
                    Visible = true;

                lock (mutex)
                {
                    if (bitmaps.Length >= 1)
                    {
                        pictureBox1.Image = bitmaps[0];
                    }

                    if (bitmaps.Length >= 2)
                    {
                        pictureBox2.Image = bitmaps[1];
                    }

                    if (messages.Length >= 1)
                    {
                        label1.Text = messages[0];
                    }
                    //else
                    //{
                    //  label1.Text = "";
                    //}

                    if (messages.Length >= 2)
                    {
                        label2.Text = messages[1];
                    }
                    //else
                    //{
                    //label2.Text = "";
                    //}

                    Invalidate();
                }
            }
        }
        */

        
        public void ReceiveMessage(Bitmap[] bitmaps, string[] messages)
        {
            try
            {
                if (IsDisposed)
                    return;

                if (this.InvokeRequired)
                {
                    Invoke(new SendMessages(ReceiveMessage), new object[] { bitmaps, messages });
                }
                else
                {
                    if (bitmaps == null || messages == null)
                    {
                        try
                        {
                            Visible = false;
                        }
                        catch (Exception e)
                        {
                        }
                        return;
                    }

                    if (!Visible)
                    {
                        try
                        {

                            Visible = true;
                        }
                        catch (Exception e)
                        {
                            return;
                        }
                    }



                    if (bitmaps.Length < this.picControls.Count || messages.Length < labels.Count)
                    {
                        SuspendLayout();
                        this.Controls.Clear();
                        picControls.Clear();
                        labels.Clear();
                        ResumeLayout();
                        PerformLayout();
                    }

                    if (bitmaps.Length == 0 && messages.Length == 0)
                    {
                    }
                    else if (bitmaps.Length == 0 && messages.Length > 0)
                    {
                        SetMessages(messages);
                    }
                    else
                    {
                        SuspendLayout();
                        if (picControls.Count < bitmaps.Length)
                        {

                            while (picControls.Count < bitmaps.Length)
                            {
                                //PictureAndTextControl picTex = new PictureAndTextControl();
                                PictureBox picBox = new PictureBox();
                                picBox.SizeMode = PictureBoxSizeMode.CenterImage;
                                //picTexControls.Add(picTex);
                                picControls.Add(picBox);
                                Controls.Add(picBox);
                            }
                            while (labels.Count < messages.Length)
                            {
                                Label newLabel = new Label();
                                labels.Add(newLabel);
                                Controls.Add(newLabel);
                            }
                        }

                        int width = 10;
                        int maxHeight = 0;
                        for (int i = 0; i < bitmaps.Length; i++)
                        {
                            if (messages.Length <= i)
                                continue;
                            string text = messages[i];
                            labels[i].Text = text;
                            labels[i].Location = new Point(width, 10);

                            PictureBox picBox = picControls[i];
                            picBox.Image = bitmaps[i].Clone() as Image;
                            picBox.Size = new Size(bitmaps[i].Width + 1, bitmaps[i].Height + 1);
                            picBox.Location = new Point(width, 30);

                            width += picBox.Width + 10;
                            if (picBox.Height > maxHeight)
                                maxHeight = picBox.Height;

                            /*
                            PictureAndTextControl picTex = picTexControls[i];
                            picTex.Image = bitmaps[i].Clone() as Image;
                            if (i < messages.Length)
                                picTex.Text = messages[i];
                            else
                                picTex.Text = "";
                            picTex.Location = new Point(width, 4);
                            width += picTex.Width + 4;
                            if (picTex.Height > maxHeight)
                                maxHeight = picTex.Height;
                             */
                        }
                        Size = new Size(width + 10, maxHeight + 70);

                        ResumeLayout();
                        PerformLayout();
                    }
                }
                Invalidate();
            }
            catch (Exception e)
            {
            }
        }

        private void SetMessages(string[] messages)
        {
            if (labels.Count < messages.Length)
            {
                SuspendLayout();
                while (labels.Count < messages.Length)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(4, labels.Count * 10);
                    labels.Add(label);
                    Controls.Add(label);
                }

                ResumeLayout();
                PerformLayout();
            }

            int maxWidth = 0;
            int height = 4;
            for (int i = 0; i < messages.Length; i++)
            {
                labels[i].Text = messages[i];
                labels[i].Location = new Point(4, height);
                height += labels[i].Height + 4;
                if (labels[i].Width > maxWidth)
                    maxWidth = labels[i].Width;
            }

            Size = new Size(maxWidth+8, height+4);
        }
    }
}
