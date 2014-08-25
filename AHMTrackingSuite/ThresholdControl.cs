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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHMTrackingSuite
{
    public delegate void PastThreshold();

    public delegate void SetThreshold(int value);

    public partial class ThresholdControl : UserControl
    {
        public ThresholdControl()
        {
            InitializeComponent();
        }

        public int ThresholdValue
        {
            set
            {
                this.trackBarEThreshold.Value = value;
            }
        }

        private double maxValue = 0;
        public double MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }

        private bool isPastThreshold = false;

        public void Reset()
        {
            isPastThreshold = true;
            maxValue = 0;
        }

        private event PastThreshold pastThreshold;
        public event PastThreshold PastThreshold
        {
            add
            {
                pastThreshold += value;
            }
            remove
            {
                pastThreshold -= value;
            }
        }

        private event SetThreshold setThreshold;
        public event SetThreshold SetThreshold
        {
            add
            {
                setThreshold += value;
            }
            remove
            {
                setThreshold -= value;
            }
        }

        public void checkValue(double curValue, bool training)
        {
            if (curValue > maxValue)
            {
                if (training)
                {
                    maxValue = curValue;
                    //this.progressBarError.Maximum = (int)maxValue;
                }
                else
                {
                    curValue = maxValue;
                }
            }

            int val = 0;
            if(maxValue > 0)
                val = (int)(100.0 * curValue / maxValue);

            this.progressBarError.Value = val;

            if (val >= this.trackBarEThreshold.Value)
            {
                if (!isPastThreshold)
                {
                    isPastThreshold = true;
                    if (!training)
                        pastThreshold();
                }
            }
            else
            {
                isPastThreshold = false;
            }
        }

        private void trackBarEThreshold_Scroll(object sender, EventArgs e)
        {
            setThreshold(trackBarEThreshold.Value);            
        }
    }
}
