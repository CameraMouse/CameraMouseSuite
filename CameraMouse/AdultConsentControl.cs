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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public partial class AdultConsentControl : UserControl
    {
        public AdultConsentControl()
        {
            InitializeComponent();
        }

        private CMSIdentificationConfig idConfig = null;
        public CMSIdentificationConfig IdConfig
        {
            get
            {
                return idConfig;
            }
            set
            {
                idConfig = value;
            }
        }

        private bool isLoading = false;

        public void Init()
        {
            isLoading = true;
            richTextBox1.Rtf = ConsentResources.AdultRTF;
         
            //textBoxAdultConsentName.Text = idConfig.ConsentAdultSignature;
            //textBoxAdultConsentWitnessName.Text = idConfig.ConsentAdultWitness;
            //if(idConfig.ConsentAdultSignature!= null && idConfig.ConsentAdultSignature.Length > 0)
                //textBoxAdultConsentDate.Text = idConfig.ConsentAdultDate;
            //else
                //textBoxConsentWitnessDate.Text = idConfig.ConsentAdultDate;

            if (idConfig.HasConsent)
            {
                //this.textBoxAdultConsentName.ReadOnly = true;
                //this.textBoxAdultConsentWitnessName.ReadOnly = true;
                //this.textBoxAdultConsentDate.ReadOnly = true;
                //this.textBoxConsentWitnessDate.ReadOnly = true;
                this.buttonGoBack.Visible = false;
                this.buttonAgree.Text = "Ok";
            }
            else
            {
                //this.textBoxAdultConsentName.ReadOnly = false;
                //this.textBoxAdultConsentWitnessName.ReadOnly = false;
                //this.textBoxAdultConsentDate.ReadOnly = false;
                //this.textBoxConsentWitnessDate.ReadOnly = false;
                this.buttonGoBack.Visible = true;
                this.buttonAgree.Text = "I Agree";
            }

            isLoading = false;            
        }

        /*
        private void textBoxAdultConsentName_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.ConsentAdultSignature = textBoxAdultConsentName.Text.Trim();
        }

        private void textBoxAdultConsentDate_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;


            idConfig.ConsentAdultDate = this.textBoxAdultConsentDate.Text.Trim();
        }

        private void textBoxAdultConsentWitnessName_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.ConsentAdultWitness = this.textBoxAdultConsentWitnessName.Text.Trim();
        }

        private void textBoxConsentWitnessDate_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.ConsentAdultDate = this.textBoxConsentWitnessDate.Text.Trim();
        }
        */

        public event EventHandler AgreeClick
        {
            add
            {
                this.buttonAgree.Click += value;
            }
            remove
            {
                this.buttonAgree.Click -= value;
            }
        }

        public event EventHandler GoBackClick
        {
            add
            {
                this.buttonGoBack.Click += value;
            }
            remove
            {
                this.buttonGoBack.Click -= value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                System.Diagnostics.Process.Start("wordpad.exe " + Environment.CurrentDirectory+"/"+CMSConstants.ADULT_INFO_RTF_FILE);
            }
            catch(Exception ee)
            {
                MessageBox.Show("Error, could not open document viewer");
            }*/

            /*
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "pdf";
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filename = sfd.FileName;
                if (filename == null || filename.Trim().Length == 0)
                    return;

                try
                {
                    System.IO.File.Copy(CMSConstants.ADULT_INFO_PDF_FILE, filename);
                    MessageBox.Show("Information form has been copied");
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Error: " + ee);
                }
            }*/

        }

    }
}
