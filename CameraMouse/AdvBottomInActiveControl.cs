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
    public partial class AdvBottomInActiveControl : UserControl
    {
        public AdvBottomInActiveControl()
        {
            InitializeComponent();

        }

        public event EventHandler EventOk
        {
            add
            {
                Ok.Click += value;
            }
            remove
            {
                Ok.Click -= value;
            }
        }

        public event EventHandler EventHelp
        {
            add
            {
                help.Click += value;
            }
            remove
            {
                help.Click -= value;
            }
        }

        public event EventHandler EventAbout
        {
            add
            {
                about.Click += value;
            }
            remove
            {
                about.Click -= value;
            }
        }

        public event EventHandler EventCameraAdvanced
        {
            add
            {
                camera_advanced.Click += value;
            }
            remove
            {
                camera_advanced.Click -= value;
            }
        }

        public event EventHandler EventVideoSelectorBtn
        {
            add
            {
                video_selector_btn.Click += value;
            }
            remove
            {
                video_selector_btn.Click -= value;

            }
        }
    }
}
