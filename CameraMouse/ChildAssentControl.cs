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
    public partial class ChildAssentControl : UserControl
    {
        public ChildAssentControl()
        {
            InitializeComponent();
        }

        /*
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.ConsentChildWitness = textBoxName.Text.Trim();
        }
        private void textBoxDate_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            this.idConfig.ConsentChildDate = textBoxDate.Text.Trim();
        }
        private void textBoxRelationship_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            this.idConfig.ConsentChildWitnessRelationship = textBoxRelationship.Text.Trim();
        }
        */

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
            richTextBox1.Rtf = ConsentResources.ChildRTF;

            if (idConfig.HasConsent)
            {
                //this.textBoxDate.ReadOnly = true;
                //this.textBoxName.ReadOnly = true;
                //this.textBoxRelationship.ReadOnly = true;
                this.buttonGoBack.Visible = false;
                this.buttonAgree.Text = "Ok";
            }
            else
            {
                //this.textBoxDate.ReadOnly = false;
                //this.textBoxName.ReadOnly = false;
                //this.textBoxRelationship.ReadOnly = false;

                this.buttonGoBack.Visible = true;
                this.buttonAgree.Text = "I Agree";
            }

            isLoading = false;
        }

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

    }
}
