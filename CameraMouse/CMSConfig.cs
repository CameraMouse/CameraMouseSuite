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
using System.Text;

namespace CameraMouseSuite
{
    public class CMSConfig
    {
 
        private string selectedSuiteName = null;
        public string SelectedSuiteName
        {
            get
            {
                return selectedSuiteName;
            }
            set
            {
                selectedSuiteName = value;
            }
        }

        private CMSControlTogglerConfig controlTogglerConfig = new CMSControlTogglerConfig();
        public CMSControlTogglerConfig ControlTogglerConfig
        {
            get
            {
                return controlTogglerConfig;
            }
            set
            {
                controlTogglerConfig = value;
            }
        }

        /*
        private CMSLogConfig logConfig = new CMSLogConfig();
        public CMSLogConfig LogConfig
        {
            get
            {
                return logConfig;
            }
            set
            {
                logConfig = value;
            }
        }

        private CMSIdentificationConfig idConfig = new CMSIdentificationConfig();
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
        */

    }

   
   
    public class CMSControlTogglerConfig
    {
        /*
        public CMSControlTogglerConfig Clone()
        {
            CMSControlTogglerConfig tmp = new CMSControlTogglerConfig();
            tmp.IntervalTime = IntervalTime;
            tmp.AutoStartControlEnabled = AutoStartControlEnabled;
            tmp.AutoStopControlEnabled = AutoStopControlEnabled;
            tmp.AutoStartDelay = AutoStartDelay;
            tmp.ScrollStart = ScrollStart;
            tmp.CtrlStart = CtrlStart;
            tmp.CtrlStop = CtrlStop;
            tmp.ScrollStop = ScrollStop;
            tmp.PlaySoundOnControlChanges = PlaySoundOnControlChanges;
            return tmp;
        }
        */

        private object mutex = new object();
        /*public object Mutex
        {
            get
            {
                return mutex;
            }
        }
        */

        private int intervalTime = 70;
        public int IntervalTime
        {
            get
            {
                lock (mutex)
                {
                    return intervalTime;
                }
            }
            set
            {
                lock (mutex)
                {
                    intervalTime = value;
                }
            }
        }

        private bool autoStartControlEnabled = false;
        public bool AutoStartControlEnabled
        {
            get
            {
                lock (mutex)
                {
                    return autoStartControlEnabled;
                }
            }
            set
            {
                lock (mutex)
                {
                    autoStartControlEnabled = value;
                }
            }
        }

        private bool autoStopControlEnabled = false;
        public bool AutoStopControlEnabled
        {
            get
            {
                lock (mutex)
                {
                    return autoStopControlEnabled;
                }
            }
            set
            {
                lock (mutex)
                {
                    autoStopControlEnabled = value;
                }
            }
        }

        private double autoStartDelay = 4.0;
        public double AutoStartDelay
        {
            get
            {
                lock (mutex)
                {
                    return autoStartDelay;
                }
            }
            set
            {
                lock (mutex)
                {
                    autoStartDelay = value;
                }
            }
        }

        private bool scrollStart = true;
        public bool ScrollStart
        {
            get
            {
                lock (mutex)
                {
                    return scrollStart;
                }
            }
            set
            {
                lock (mutex)
                {
                    scrollStart = value;
                }
            }
        }

        private bool ctrlStart = false;
        public bool CtrlStart
        {
            get
            {
                lock (mutex)
                {
                    return ctrlStart;

                }
            }
            set
            {
                lock (mutex)
                {
                    ctrlStart = value;
                }
            }
        }

        private bool ctrlStop = false;
        public bool CtrlStop
        {
            get
            {
                lock (mutex)
                {
                    return ctrlStop;
                }
            }
            set
            {
                lock (mutex)
                {
                    ctrlStop = value;
                }
            }
        }

        private bool scrollStop = true;
        public bool ScrollStop
        {
            get
            {
                lock (mutex)
                {
                    return scrollStop;
                }
            }
            set
            {
                lock (mutex)
                {
                    scrollStop = value;
                }
            }
        }

        private bool playSoundOnControlChanges = false;
        public bool PlaySoundOnControlChanges
        {
            get
            {
                lock (mutex)
                {
                    return playSoundOnControlChanges;
                }
            }
            set
            {
                lock (mutex)
                {
                    playSoundOnControlChanges = value;
                }
            }
        }

        public void UpdateControlTogglerConfig(CMSControlTogglerConfig togglerConfig)
        {
            lock (mutex)
            {
                IntervalTime = togglerConfig.IntervalTime;
                AutoStartControlEnabled = togglerConfig.AutoStartControlEnabled;
                AutoStopControlEnabled = togglerConfig.AutoStopControlEnabled;
                AutoStartDelay = togglerConfig.AutoStartDelay;
                ScrollStart = togglerConfig.ScrollStart;
                CtrlStart = togglerConfig.CtrlStart;
                CtrlStop = togglerConfig.CtrlStop;
                ScrollStop = togglerConfig.ScrollStop;
                PlaySoundOnControlChanges = togglerConfig.PlaySoundOnControlChanges;
            }
        }
    }

}
