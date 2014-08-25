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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHMTrackingSuite
{
    public partial class ClickingThresholdForm : Form
    {
        public ClickingThresholdForm()
        {
            InitializeComponent();
        }

        private delegate void SetThresholdValueDelegate(int value);
        public void SetThresholdValue(int value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SetThresholdValueDelegate(SetThresholdValue), new object[] { value });
            }
            else
            {
                thresholdControl1.ThresholdValue = value;
            }
        }

        public event PastThreshold PastThreshold
        {
            add
            {
                thresholdControl1.PastThreshold += value;
            }
            remove
            {
                thresholdControl1.PastThreshold -= value;
            }
        }
        public event SetThreshold SetThreshold
        {
            add
            {
                thresholdControl1.SetThreshold += value;
            }
            remove
            {
                thresholdControl1.SetThreshold -= value;
            }
        }

        public double MaxValue
        {
            get
            {
                return thresholdControl1.MaxValue;
            }
        }

        private delegate void ResetDelegate();
        public void Reset()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ResetDelegate(Reset));
            }
            else
            {
                thresholdControl1.Reset();
            }
        }

        private delegate void CheckValueDelegate(double curValue, bool training);
        public void checkValue(double curValue, bool training)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CheckValueDelegate(checkValue), new object[] { curValue, training });
            }
            else
            {
                thresholdControl1.checkValue(curValue, training);
            }
        }
    }
}
