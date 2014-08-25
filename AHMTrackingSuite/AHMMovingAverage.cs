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
using System.Linq;
using System.Text;

namespace AHMTrackingSuite
{
    public class AHMovingAverage
    {
        private int period = 0;

        private int startTime = 0;
        private double multiplier = 0;

        private double ema = 0;

        private long count = 0;

        private double initialSum = 0.0;

        public bool IsActive
        {
            get
            {
                return count >= startTime;
            }
        }

        public double EMAverage
        {
            get
            {
                return ema;
            }
        }

        public void Reset()
        {
            ema = 0;
            count = 0;
            initialSum = 0.0;
        }

        public void Init(int period, int startTime)
        {
            this.period = period;

            multiplier = 2 / ((double)period + 1.0);


            this.startTime = startTime;
            if (period < startTime)
                this.startTime = period;
        }

        public void SetPoint(double val)
        {
            ema = val;
        }

        public void AddPoint(double val)
        {
            count++;
            if (count <= startTime)
            {
                initialSum += val;
            }
            if (count == startTime)
            {
                ema = initialSum / (double)startTime;
                initialSum = 0.0;
            }
            else
            {
                ema = (val - ema) * multiplier + ema;
            }
        }

    }
}
