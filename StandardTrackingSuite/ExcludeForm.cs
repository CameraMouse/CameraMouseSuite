﻿/*                         Camera Mouse Suite
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
    public partial class ExcludeForm : Form
    {
        public ExcludeForm()
        {
            InitializeComponent();
        }

        public void SetVertical(int xPos, int yLowerPos, int yHigherPos, int width)
        {
            Size = new Size(width, yHigherPos - yLowerPos + 1);
            Location = new Point(xPos, yLowerPos);
        }
        
        public void SetHorizontal(int yPos, int xLowerPos, int xHigherPos, int height)
        {
            Size = new Size(xHigherPos - xLowerPos + 1, height);
            Location = new Point(xLowerPos,yPos);
        }
    }
}