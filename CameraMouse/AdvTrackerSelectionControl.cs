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
    public partial class AdvTrackerSelectionControl : UserControl
    {
        public AdvTrackerSelectionControl()
        {
            InitializeComponent();
        }

        private bool loading = false;
        private SortedList<string, string> descriptionLookup = new SortedList<string, string>();
        private string[] formalNames = null;
        private string selectedTrackerName = null;

        public void SetNames(string selectedInformalName, CMSTrackingSuiteIdentifier[] ids)
        {

            loading = true;

            List<CMSTrackingSuiteIdentifier> idList = new List<CMSTrackingSuiteIdentifier>();
            foreach (CMSTrackingSuiteIdentifier curId in ids)
            {
                if (curId.Name.Equals(CMSConstants.EMPTY_TRACKING_SUITE_NAME))
                    continue;
                idList.Add(curId);
            }

            formalNames = new string[idList.Count];
            string[] informalNames = new string[idList.Count];

            for (int i = 0; i < idList.Count; i++)
            {
                formalNames[i] = idList[i].Name;
                informalNames[i] = idList[i].InformalName;
            }

            for (int i = 0; i < idList.Count; i++)
                descriptionLookup[idList[i].InformalName] = idList[i].Description;
            textBoxAdvDescription.Clear();


            listBoxAdvTrackers.Items.AddRange(informalNames);
            selectedTrackerName = selectedInformalName;
            loading = false;

        }

        /*
        public void SetNames(string selectedInformalName, string[] informalNames, string [] formalNames, string[] descriptions)
        {
            loading = true;
            this.formalNames = formalNames;
            for (int i = 0; i < informalNames.Length; i++)
                descriptionLookup[informalNames[i]] = descriptions[i];
            textBoxAdvDescription.Clear();            

            listBoxAdvTrackers.Items.AddRange(informalNames);
            selectedTrackerName = selectedInformalName;
            loading = false;
        }
        */

        public string SelectedFormalName
        {
            get
            {
                int i = listBoxAdvTrackers.SelectedIndex;
                if (i < 0 || i >= formalNames.Length)
                    return null;
                return formalNames[i];
            }
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                buttonAdvSelectNewTracker.Text = "Select Tracker";
                
                labelTitle.Text = "Tracker Selection";
                //labelTitle.Location = new Point(245, 16);

                labelAdvAvailableTrackers.Visible = true;
                labelAdvDescription.Visible = true;
                
                textBoxAdvDescription.Visible = true;
                listBoxAdvTrackers.Visible = true;
                Size = new Size(665, 289); 
                groupBoxTrackerSelection.Size = new Size(650, 278);
            }
            else
            {
                buttonAdvSelectNewTracker.Text = "Select New Tracker";

                labelTitle.Text = selectedTrackerName;
                //labelTitle.Location = new Point(167, 16);

                labelAdvAvailableTrackers.Visible = false;
                labelAdvDescription.Visible = false;

                textBoxAdvDescription.Visible = false;
                listBoxAdvTrackers.Visible = false;
                Size = new Size(665, 69);
                groupBoxTrackerSelection.Size = new Size(650, 48);
            }
        }

        public string SelectedTracker
        {
            get
            {
                return selectedTrackerName;
            }
        }

        public void SetActiveName(string selectedTrackerName)
        {
            this.selectedTrackerName = selectedTrackerName;
            listBoxAdvTrackers.SelectedItem = selectedTrackerName;
        }

        private void listBoxTrackers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading)
                return;

            string selectedItem = this.listBoxAdvTrackers.SelectedItem as string;
            if (selectedItem == null || selectedItem.Length == 0)
                return;

            selectedTrackerName = selectedItem;

            textBoxAdvDescription.Text = descriptionLookup[selectedItem];
        }

        private event EventHandler selectTrackerEvent;
        public event EventHandler EventSelectTrackerButton
        {
            add
            {
                this.buttonAdvSelectNewTracker.Click += value;
                selectTrackerEvent += value;
            }
            remove
            {
                this.buttonAdvSelectNewTracker.Click -= value;
                selectTrackerEvent -= value;
            }
        }

        private void listBoxAdvTrackers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxAdvTrackers.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                selectTrackerEvent(this, new EventArgs());
            }
        }

    }
}
