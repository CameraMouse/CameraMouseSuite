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
using System.Threading;
using System.ComponentModel;

namespace BlinkLinkStandardTrackingSuite
{

    #region Enumeration Types

    public enum SoundOption
    {
        [DescriptionAttribute("No Sound")]
        NoSound,

        [DescriptionAttribute("All Clicks")]
        AllClicks,

        [DescriptionAttribute("Blink Clicks Only")]
        BlinkClicksOnly
    }

    public enum EyeStatusWindowOption
    {
        [DescriptionAttribute("Don't Show")]
        NoWindow,

        [DescriptionAttribute("Stationary")]
        Stationary,

        [DescriptionAttribute("Follow Mouse")]
        FollowMouse,
    }

    #endregion

    public class BlinkLinkEyeClickData
    {
        #region Public Constant Data Members

        public const float  MinimumShortWinkTime = 0.1f;
        public const float  MinimumLongWinkTime  = 0.2f;

        #endregion

        #region Private Data Members

        private volatile ClickAction           shortLeftWinkAction;
        private volatile ClickAction           shortRightWinkAction;
        private volatile ClickAction           longLeftWinkAction;
        private volatile ClickAction           longRightWinkAction;
        private volatile ClickAction           blinkAction;
        private volatile float                 shortWinkTime;
        private volatile float                 longWinkTime;
        private volatile SoundOption           soundOption;
        private volatile EyeStatusWindowOption eyeStatusWindowOption;
        private volatile bool                  switchEyes;
        private          object                mutex;

        #endregion

        #region Properties

        public ClickAction ShortLeftWinkAction
        {
            get
            {
                lock( mutex )
                {
                    return shortLeftWinkAction;
                }
            }

            set
            {
                lock( mutex )
                {
                    switch( value )
                    {
                        case ClickAction.None:
                        case ClickAction.LeftClick:
                        case ClickAction.RightClick:
                        case ClickAction.DoubleClick:
                            {
                                shortLeftWinkAction = value;
                                return;
                            }
                    }

                    shortLeftWinkAction = ClickAction.None;
                }
            }
        }

        public ClickAction ShortRightWinkAction
        {
            get
            {
                lock( mutex )
                {
                    return shortRightWinkAction;
                }
            }

            set
            {
                lock( mutex )
                {
                    switch( value )
                    {
                        case ClickAction.None:
                        case ClickAction.LeftClick:
                        case ClickAction.RightClick:
                        case ClickAction.DoubleClick:
                            {
                                shortRightWinkAction = value;
                                return;
                            }
                    }

                    shortRightWinkAction = ClickAction.None;
                }
            }
        }

        public ClickAction LongLeftWinkAction
        {
            get
            {
                lock( mutex )
                {
                    return longLeftWinkAction;
                }
            }

            set
            {
                lock( mutex )
                {
                    longLeftWinkAction = value;
                }
            }
        }

        public SoundOption SoundOption
        {
            get
            {
                return soundOption;
            }
            set
            {
                soundOption = value;
            }
        }

        public EyeStatusWindowOption EyeStatusWindowOption
        {
            get
            {
                return eyeStatusWindowOption;
            }
            set
            {
                eyeStatusWindowOption = value;
            }
        }

        public ClickAction LongRightWinkAction
        {
            get
            {
                lock( mutex )
                {
                    return longRightWinkAction;
                }
            }

            set
            {
                lock( mutex )
                {
                    longRightWinkAction = value;
                }
            }
        }

        public ClickAction BlinkAction
        {
            get
            {
                lock( mutex )
                {
                    return blinkAction;
                }
            }

            set
            {
                lock( mutex )
                {
                    switch( value )
                    {
                        case ClickAction.LeftClick:
                        case ClickAction.RightClick:
                        case ClickAction.DoubleClick:
                            {
                                blinkAction = value;
                                return;
                            }
                    }

                    blinkAction = ClickAction.None;
                }
            }
        }

        public float ShortWinkTime
        {
            get
            {
                lock( mutex )
                {
                    return shortWinkTime;
                }
            }

            set
            {
                lock( mutex )
                {
                    if( value >= MinimumShortWinkTime )
                    {
                        shortWinkTime = value;
                    }
                }
            }
        }

        public float LongWinkTime
        {
            get
            {
                lock( mutex )
                {
                    return longWinkTime;
                }
            }

            set
            {

                lock( mutex )
                {
                    if( value >= MinimumLongWinkTime )
                    {
                        longWinkTime = value;
                    }
                }
            }
        }

        public bool PlaySoundForAllClicks
        {
            get
            {
                return soundOption == SoundOption.AllClicks;
            }
        }

        public bool PlaySoundForBlinkOnlyClicks
        {
            get
            {
                return soundOption == SoundOption.BlinkClicksOnly;
            }
            
        }

        public bool ShowEyeStatusWindow
        {
            get
            {
                return eyeStatusWindowOption == EyeStatusWindowOption.FollowMouse || eyeStatusWindowOption == EyeStatusWindowOption.Stationary;
            }
        }

        public bool MoveEyeStatusWindow
        {
            get
            {
                return eyeStatusWindowOption == EyeStatusWindowOption.FollowMouse;
            }
        }

        public bool SwitchEyes
        {
            get
            {
                return switchEyes;
            }
            set
            {
                switchEyes = value;
            }
        }

        #endregion

        #region Constructors

        public BlinkLinkEyeClickData(
            ClickAction shortLeftWinkAction,
            ClickAction shortRightWinkAction,
            ClickAction longLeftWinkAction,
            ClickAction longRightWinkAction,
            ClickAction blinkAction,
            float shortWinkTime,
            float longWinkTime,
            SoundOption soundOption,
            EyeStatusWindowOption eyeStatusWindowOption,
            bool switchEyes)
        {
            mutex = new object();
            ShortLeftWinkAction = shortLeftWinkAction;
            ShortRightWinkAction = shortRightWinkAction;
            LongLeftWinkAction = longLeftWinkAction;
            LongRightWinkAction = longRightWinkAction;
            ShortWinkTime = shortWinkTime;
            LongWinkTime = longWinkTime;
            SoundOption = soundOption;
            BlinkAction = blinkAction;
            SwitchEyes = switchEyes;
            EyeStatusWindowOption = eyeStatusWindowOption;
        }

        public BlinkLinkEyeClickData() :
            this(ClickAction.LeftClick, ClickAction.RightClick, ClickAction.LeftDrag,
                ClickAction.RightClick, ClickAction.DoubleClick, 1.5f, 2.5f, SoundOption.BlinkClicksOnly, EyeStatusWindowOption.NoWindow, false)
        {
        }

        public BlinkLinkEyeClickData(BlinkLinkEyeClickData other) :
            this(other.shortLeftWinkAction, other.shortRightWinkAction, other.longLeftWinkAction,
                other.longRightWinkAction, other.blinkAction, other.shortWinkTime, other.longWinkTime,
                other.SoundOption, other.EyeStatusWindowOption, other.SwitchEyes)
        {
        }

        #endregion

        #region Public Functions

        public void SetData(BlinkLinkEyeClickData other)
        {
            ShortLeftWinkAction = other.ShortLeftWinkAction;
            ShortRightWinkAction = other.ShortRightWinkAction;
            LongLeftWinkAction = other.LongLeftWinkAction;
            LongRightWinkAction = other.LongRightWinkAction;
            BlinkAction = other.BlinkAction;
            ShortWinkTime = other.ShortWinkTime;
            LongWinkTime = other.LongWinkTime;
            SoundOption = other.SoundOption;
            EyeStatusWindowOption = other.EyeStatusWindowOption;
            SwitchEyes = other.SwitchEyes;
        }

        public void LockEyeClickData()
        {
            Monitor.Enter(mutex);
        }

        public void UnlockEyeClickData()
        {
            Monitor.Exit(mutex);
        }

        #endregion
    }
}
