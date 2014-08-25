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
    public partial class ParticipationRequestControl : UserControl
    {
        public ParticipationRequestControl()
        {
            InitializeComponent();
        }

        /*
        public void Init(bool notStudy)
        {
            if (notStudy)
            {
                
            }
        }
        */

        public event EventHandler AgreeClick
        {
            add
            {
                this.buttonAccept.Click += value;
            }
            remove
            {
                this.buttonAccept.Click -= value;
            }
        }
    }
}
