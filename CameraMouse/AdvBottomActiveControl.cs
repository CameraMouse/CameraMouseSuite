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
    public partial class AdvBottomActiveControl : UserControl, CMSConfigPanel
    {
        public AdvBottomActiveControl()
        {
            InitializeComponent();
            
        }

        public event EventHandler EventNotifySound
        {
            add
            {
                notify_sound.CheckedChanged += value;
            }
            remove
            {
                notify_sound.CheckedChanged -= value;
            }
        }
        public event EventHandler EventStopCtrl
        {
            add
            {
                stop_ctrl.CheckedChanged += value;
            }
            remove
            {
                stop_ctrl.CheckedChanged -= value;
            }
        }
        public event EventHandler EventStopScroll
        {
            add
            {
                stopScroll.CheckedChanged += value;
            }
            remove
            {
                stopScroll.CheckedChanged -= value;
            }
        }
        public event EventHandler EventGoCtrl
        {
            add
            {
                go_ctrl.CheckedChanged += value;
            }
            remove
            {
                go_ctrl.CheckedChanged -= value;
            }
        }
        public event EventHandler EventGoScroll
        {
            add
            {
                go_scroll.CheckedChanged += value;
            }
            remove
            {
                go_scroll.CheckedChanged -= value;
            }
        }
        public event EventHandler EventStopOnMove
        {
            add
            {
                stop_on_move.CheckedChanged += value;
            }
            remove
            {
                stop_on_move.CheckedChanged -= value;
            }
        }
        public event EventHandler EventComboBoxSecondsMouseInactivity
        {
            add
            {
                comboBoxSecondsMouseInactivity.SelectedIndexChanged += value;
            }
            remove
            {
                comboBoxSecondsMouseInactivity.SelectedIndexChanged -= value;
            }
        }
        public event EventHandler EventAutoStart
        {
            add
            {
                auto_start.CheckedChanged += value;
            }
            remove
            {
                auto_start.CheckedChanged -= value;
            }
        }
        public event EventHandler EventButtonRestore
        {
            add
            {
                buttonRestore.Click += value;
            }
            remove
            {
                buttonRestore.Click -= value;
            }
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
        public event EventHandler EventButtonLoad
        {
            add
            {
                buttonLoad.Click += value;
            }
            remove
            {
                buttonLoad.Click -= value;
            }
        }
        public event EventHandler EventButtonSave
        {
            add
            {
                buttonSave.Click += value;
            }
            remove
            {
                buttonSave.Click -= value;
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

        private CMSControlTogglerConfig togglerConfig = null;
        public CMSControlTogglerConfig TogglerConfig
        {
            get
            {
                return togglerConfig;
            }
            set
            {
                togglerConfig = value;
            }
        }

        private bool loading = false;
        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            loading = true;
            comboBoxSecondsMouseInactivity.SelectedItem = togglerConfig.AutoStartDelay.ToString();
            auto_start.Checked = togglerConfig.AutoStartControlEnabled;
            go_ctrl.Checked = togglerConfig.CtrlStart;
            go_scroll.Checked = togglerConfig.ScrollStart;
            stopScroll.Checked = togglerConfig.ScrollStop;
            stop_ctrl.Checked = togglerConfig.CtrlStop;
            stop_on_move.Checked = togglerConfig.AutoStopControlEnabled;
            notify_sound.Checked = togglerConfig.PlaySoundOnControlChanges;
            loading = false;
        }

        #endregion



        #region CMSConfigPanel Members


        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
        }

        #endregion
    }
}
