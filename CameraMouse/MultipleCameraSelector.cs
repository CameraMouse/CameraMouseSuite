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
    public partial class MultipleCameraSelector : Form
    {
        private WebCamDescription[] cams = null;
        private ComboBox[] comboBoxes = null;

        private Label[] labels = null;

        private string[] selectedCameras = null;

        public string[] SelectedCameras
        {
            get
            {
                return selectedCameras;
            }
        }

        public MultipleCameraSelector(string [] cameraTitles, WebCamDescription[] cams)
        {
            InitializeComponent();
            this.cams = cams;
            PopulateCameras(cameraTitles);
            
        }

        private void PopulateCameras(string[] cameraTitles)
        {
            string[] camNames = new string[cams.Length];

            for (int i = 0; i < cams.Length; i++)
            {

                if (cams[i].Name.Length > 40)
                    camNames[i] = cams[i].Name.Substring(0, 40);
                else
                    camNames[i] = cams[i].Name;
            }

            comboBoxes = new ComboBox[cameraTitles.Length];
            labels = new Label[cameraTitles.Length];

            for (int i = 0; i < cameraTitles.Length; i++)
            {
                labels[i] = new Label();
                labels[i].AutoSize = true;
                labels[i].Text = cameraTitles[i] +":";
                labels[i].Location = new Point(12,40 + 20 * i);

                comboBoxes[i] = new ComboBox();
                comboBoxes[i].Items.AddRange(camNames);
                comboBoxes[i].Location = new Point(86, 37 + 20 * i);
                comboBoxes[i].Size = new Size(240, 20);

                this.panel.Controls.Add(labels[i]);
                this.panel.Controls.Add(comboBoxes[i]);
            }
        }

        private void OK_btn_Click(object sender, EventArgs e)
        {
            selectedCameras = new string[comboBoxes.Length];

            SortedList<int, object> checker = new SortedList<int, object>();

            for (int i = 0; i < comboBoxes.Length; i++)
            {
                int selectedIndex = comboBoxes[i].SelectedIndex;
                if (checker.ContainsKey(selectedIndex))
                {
                    this.textBox1.Text = "Cannot assign same camera to two slots";
                    selectedCameras = null;
                    return;
                }
                checker[selectedIndex] = null;
                selectedCameras[i] = cams[selectedIndex].Moniker;
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            selectedCameras = null;
            Close();
        }

    }
}
