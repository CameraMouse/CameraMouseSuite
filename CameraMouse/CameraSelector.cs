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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CameraMouseSuite
{
    /// <summary>
    /// Summary description for CameraSelector.
    /// </summary>
    public unsafe class CameraSelector : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Panel main_panel;
        private System.Windows.Forms.Button OK_btn;
        private System.Windows.Forms.Panel radio_btn_panel;
        private System.Windows.Forms.TextBox textBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// DLL calls
        /// </summary>
        [DllImport("CvCam100.dll")]
        private static extern int cvcamGetCamerasCount();

        [DllImport("CvCam100.dll")]
        private static extern void cvcamSetProperty(int cid, string pname, ref int temp);

        [DllImport("CvCam100.dll")]
        private static extern void cvcamSetProperty(int cid, string pname, IntPtr ptr);

        [DllImport("CvCam100.dll")]
        private static extern void cvcamGetProperty(int cid, string pname, ref int temp);

        [DllImport("CvCam100.dll")]
        private static extern void cvcamGetProperty(int cid, string pname, byte* ptr);


        [DllImport("msvcrt.dll")]
        private static extern int mbstowcs(char* dest, byte* src, int count);

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);


        private string _errmsg;

        private string _camera;

        private WebCamDescription[] _cams;


        unsafe public CameraSelector(WebCamDescription[] cams)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            _cams = cams;

            if (_cams.Length == 0)
            {
                _errmsg = "No Cameras Found: a camera for video source is required.";
            }
            else if (_cams.Length < 0)
            {
                _errmsg = "No Cameras Found: Error occured during initialization, " +
                    "attempt restart. May require reboot of computer.";
            }
            else
            {
                PopulateList();
            }
        }


        private void PopulateList()
        {
            int i;
            for (i = 0; i < _cams.Length; i++)
            {

                RadioButton rb = new RadioButton();
                if (_cams[i].Name.Length > 40)
                    rb.Text = _cams[i].Name.Substring(0, 40);
                else
                    rb.Text = _cams[i].Name;

                rb.Location = new Point(10, (i + 1) * 20);
                rb.Size = new Size(240, 20);
                rb.Name = _cams[i].Moniker;

                radio_btn_panel.Controls.Add(rb);
            }

        }


        public string ErrorMsg
        {
            get
            {
                return _errmsg;
            }
        }

        public string SelectedCamera
        {
            get
            {
                return _camera;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CameraSelector));
            this.main_panel = new System.Windows.Forms.Panel();
            this.radio_btn_panel = new System.Windows.Forms.Panel();
            this.OK_btn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.main_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_panel
            // 
            this.main_panel.Controls.Add(this.textBox1);
            this.main_panel.Controls.Add(this.radio_btn_panel);
            this.main_panel.Controls.Add(this.OK_btn);
            this.main_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_panel.Location = new System.Drawing.Point(0, 0);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(296, 294);
            this.main_panel.TabIndex = 0;
            // 
            // radio_btn_panel
            // 
            this.radio_btn_panel.AutoScroll = true;
            this.radio_btn_panel.Location = new System.Drawing.Point(13, 22);
            this.radio_btn_panel.Name = "radio_btn_panel";
            this.radio_btn_panel.Size = new System.Drawing.Size(271, 140);
            this.radio_btn_panel.TabIndex = 2;
            // 
            // OK_btn
            // 
            this.OK_btn.Location = new System.Drawing.Point(116, 242);
            this.OK_btn.Name = "OK_btn";
            this.OK_btn.Size = new System.Drawing.Size(64, 32);
            this.OK_btn.TabIndex = 1;
            this.OK_btn.Text = "OK";
            this.OK_btn.Click += new System.EventHandler(this.OK_btn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(13, 175);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(272, 52);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Please select a camera source to use. ";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CameraSelector
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 294);
            this.ControlBox = false;
            this.Controls.Add(this.main_panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CameraSelector";
            this.Text = "Camera Selector";
            this.main_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        private bool SourceSelected()
        {
            try
            {
                string camera_name;
         
                foreach (RadioButton rb in radio_btn_panel.Controls)
                {
                    if (rb.Checked)
                    {
                        camera_name = rb.Text;
                        _camera = rb.Name;
                        return true;
                    }

                }

                return false;
            }
            catch
            {

            }

            return false;
        }


        private void OK_btn_Click(object sender, System.EventArgs e)
        {
            if (SourceSelected())
                Close();
            else
            {
                textBox1.Text = "You must choose a camera source to proceed";
                Beep(1900, 200);
                Beep(1600, 200);
            }
        }

    }
}
