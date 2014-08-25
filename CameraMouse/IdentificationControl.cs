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
    public partial class IdentificationControl : UserControl
    {
        public IdentificationControl()
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

        public void Init(bool goingToConsent)
        {
            isLoading = true;

            textBoxCity.Text = idConfig.City;
            textBoxCountry.Text = idConfig.Country;
            textBoxEMail.Text = idConfig.Email;

            if (idConfig.FirstName == null || idConfig.FirstName.Trim().Length == 0)
                idConfig.FirstName = "Anonymous";
            
            if (idConfig.LastName == null || idConfig.LastName.Trim().Length == 0)
                idConfig.LastName = "Anonymous";
            
            textBoxFirstName.Text = idConfig.FirstName;
            textBoxLastName.Text = idConfig.LastName;

            textBoxState.Text = idConfig.StateProvince;
            this.textBoxCondition.Text = idConfig.Condition;

            if (idConfig.AgeGroup == AgeGroup.MinorSixYounger)
                comboBoxAge.SelectedIndex = 0;
            else if (idConfig.AgeGroup == AgeGroup.MinorSevenOlder)
                comboBoxAge.SelectedIndex = 1;
            else
                comboBoxAge.SelectedIndex = 2;


            if (goingToConsent && ! idConfig.NotStudy)
            {
                buttonOk.Text = "Go to Information Page";
            }
            else
            {
                buttonOk.Text = "Go to Research Page";
            }

            isLoading = false;
        }

        private void textBoxFirstName_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.FirstName = textBoxFirstName.Text.Trim();
        }
        private void textBoxLastName_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.LastName = this.textBoxLastName.Text.Trim();
        }
        private void textBoxEMail_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.Email = this.textBoxEMail.Text.Trim();
        }
        private void textBoxCity_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.City = this.textBoxCity.Text.Trim();
        }


        private void textBoxCondition_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;
            idConfig.Condition = this.textBoxCondition.Text.Trim();
        }

        private void textBoxState_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.StateProvince = this.textBoxState.Text.Trim();
        }
        private void textBoxCountry_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            idConfig.Country = this.textBoxCountry.Text.Trim();

        }
        private void comboBoxAge_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            switch (this.comboBoxAge.SelectedIndex)
            {
                case 0:
                    idConfig.AgeGroup = AgeGroup.MinorSixYounger;
                    break;

                case 1:
                    idConfig.AgeGroup = AgeGroup.MinorSevenOlder;
                    break;
                
                case 2:
                    idConfig.AgeGroup = AgeGroup.Adult;
                    break;
                default:
                    idConfig.AgeGroup = AgeGroup.Adult;
                    break;
            }
        }

        public event EventHandler OkClick
        {
            add
            {
                this.buttonOk.Click += value;
            }
            remove
            {
                this.buttonOk.Click -= value;
            }
        }
        public event EventHandler CancelClick
        {
            add
            {
                this.buttonCancel.Click += value;
            }
            remove
            {
                this.buttonCancel.Click -= value;
            }
        }


    }
}
