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
using CameraMouseSuite;
using System.Reflection;

namespace BlinkLinkStandardTrackingSuite
{
    public partial class BlinkLinkClickControlPanel : UserControl, CMSConfigPanel
    {
        

        private BlinkLinkClickControlModule blinkLinkClickControlModule;
        private bool                        loadingControls;

        public BlinkLinkClickControlPanel()
        {
            InitializeComponent();
            blinkLinkClickControlModule    = null;
            loadingControls                = false;

            shortLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.None));
            shortLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftClick));
            shortLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.RightClick));
            shortLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.DoubleClick));

            shortRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.None));
            shortRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftClick));
            shortRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.RightClick));
            shortRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.DoubleClick));

            longLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.None));
            longLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftClick));
            longLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.RightClick));
            longLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftDrag));
            longLeftWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.DoubleClick));

            longRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.None));
            longRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftClick));
            longRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.RightClick));
            longRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftDrag));
            longRightWinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.DoubleClick));

            blinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.None));
            blinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.LeftClick));
            blinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.RightClick));
            blinkActionComboBox.Items.Add(GetEnumDescription(ClickAction.DoubleClick));

            statusWindowComboBox.Items.Add(GetEnumDescription(EyeStatusWindowOption.NoWindow));
            statusWindowComboBox.Items.Add(GetEnumDescription(EyeStatusWindowOption.FollowMouse));
            statusWindowComboBox.Items.Add(GetEnumDescription(EyeStatusWindowOption.Stationary));

            soundComboBox.Items.Add(GetEnumDescription(SoundOption.NoSound));
            soundComboBox.Items.Add(GetEnumDescription(SoundOption.BlinkClicksOnly));
            soundComboBox.Items.Add(GetEnumDescription(SoundOption.AllClicks));
        }

        private static Enum GetEnum(string enumString, Type enumType, Enum defaultValue)
        {
            foreach( Enum e in Enum.GetValues(enumType) )
            {
                if( GetEnumDescription(e) == enumString )
                {
                    return e;
                }
            }

            return defaultValue;
        }

        private static ClickAction GetClickAction(string action)
        {
            return (ClickAction)GetEnum(action, typeof(ClickAction), ClickAction.None);
        }

        private static SoundOption GetSoundOption(string soundOption)
        {
            return (SoundOption)GetEnum(soundOption, typeof(SoundOption), SoundOption.NoSound);
        }

        private static EyeStatusWindowOption GetEyeStatusWindowOption(string windowOption)
        {
            return (EyeStatusWindowOption)GetEnum(windowOption,
                typeof(EyeStatusWindowOption), EyeStatusWindowOption.Stationary);
        }

        private static string GetEnumDescription(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if( name != null )
            {
                FieldInfo field = type.GetField(name);
                if( field != null )
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if( attr != null )
                    {
                        return attr.Description;
                    }
                }
            }
            return value.ToString();
        }

        public void SetClickControl(BlinkLinkClickControlModule module)
        {
            this.blinkLinkClickControlModule = module;
            LoadFromControls();
        }

        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            loadingControls = true;

            shortLeftWinkActionComboBox.SelectedItem
                = (object)GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortLeftWinkAction);

            shortRightWinkActionComboBox.SelectedItem
                = (object)GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortRightWinkAction);

            longLeftWinkActionComboBox.SelectedItem
                = (object)GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.LongLeftWinkAction);

            longRightWinkActionComboBox.SelectedItem
                = (object)GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.LongRightWinkAction);

            blinkActionComboBox.SelectedItem
                = (object)GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.BlinkAction);

            shortWinkTimeTextBox.Text = blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortWinkTime.ToString();
            longWinkTimeTextBox.Text = blinkLinkClickControlModule.BlinkLinkEyeClickData.LongWinkTime.ToString();

            soundComboBox.SelectedItem = GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.SoundOption);
            statusWindowComboBox.SelectedItem = GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.EyeStatusWindowOption);

            switchEyesCheckBox.Checked = blinkLinkClickControlModule.BlinkLinkEyeClickData.SwitchEyes;

            soundComboBox.SelectedItem = GetEnumDescription(blinkLinkClickControlModule.BlinkLinkEyeClickData.SoundOption);

            loadingControls = false;
        }

        SendLogAdvancedTracker sendLogAdvancedTracker = null;

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        #endregion
        
        private void shortWinkTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                try
                {
                    string text = shortWinkTimeTextBox.Text;
                    float tempVal;

                    if (text.Length == 0)
                    {
                        tempVal = BlinkLinkEyeClickData.MinimumShortWinkTime;
                        shortWinkTimeTextBox.Text = BlinkLinkEyeClickData.MinimumShortWinkTime.ToString();
                    }
                    else
                    {
                        tempVal = Math.Max(float.Parse(text), BlinkLinkEyeClickData.MinimumShortWinkTime);
                    }

                    blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortWinkTime = tempVal;
                    sendLogAdvancedTracker();
                }
                catch (Exception)
                {
                    string oldText
                        = blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortWinkTime.ToString();
                    shortWinkTimeTextBox.Text = oldText;

                    shortWinkTimeTextBox.SelectionStart = oldText.Length;
                }
            }
        }

        private void longWinkTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                try
                {
                    string text = longWinkTimeTextBox.Text;
                    float tempVal;

                    if (text.Length == 0)
                    {
                        tempVal = BlinkLinkEyeClickData.MinimumLongWinkTime;
                        longWinkTimeTextBox.Text = BlinkLinkEyeClickData.MinimumLongWinkTime.ToString();
                    }
                    else
                    {
                        tempVal = Math.Max(float.Parse(text), BlinkLinkEyeClickData.MinimumLongWinkTime);
                    }

                    blinkLinkClickControlModule.BlinkLinkEyeClickData.LongWinkTime = tempVal;
                    sendLogAdvancedTracker();
                }
                catch (Exception)
                {
                    string oldText
                        = blinkLinkClickControlModule.BlinkLinkEyeClickData.LongWinkTime.ToString();
                    longWinkTimeTextBox.Text = oldText;
                    longWinkTimeTextBox.SelectionStart = oldText.Length;
                }
            }
        }

        private void shortLeftWinkActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortLeftWinkAction
                    = GetClickAction((string)shortLeftWinkActionComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }

        private void shortRightWinkActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.ShortRightWinkAction
                    = GetClickAction((string)shortRightWinkActionComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }

        private void longLeftWinkActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.LongLeftWinkAction
                    = GetClickAction((string)longLeftWinkActionComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }

        private void longRightWinkActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.LongRightWinkAction
                    = GetClickAction((string)longRightWinkActionComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }

        private void blinkActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.BlinkAction
                    = GetClickAction(blinkActionComboBox.SelectedText);
                sendLogAdvancedTracker();
            }
        }

        private void switchEyesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.SwitchEyes = switchEyesCheckBox.Checked;
                sendLogAdvancedTracker();
            }
        }

        private void soundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.SoundOption = GetSoundOption((string)soundComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }

        private void statusWindowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                blinkLinkClickControlModule.BlinkLinkEyeClickData.EyeStatusWindowOption = GetEyeStatusWindowOption((string)statusWindowComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }

        
    }
}
