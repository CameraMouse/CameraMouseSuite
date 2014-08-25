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
using BlinkLinkStandardTrackingSuite;
using System.Reflection;

namespace BlinkLinkStandardTrackingSuite
{                        
    public partial class BlinkLinkClickControlSimplePanel : UserControl, CMSConfigPanel
    {
        private bool loadingControls;
        private BlinkLinkClickControlSimpleModule blinkLinkClickControlSimpleModule;
        
        public BlinkLinkClickControlSimplePanel()
        {
            InitializeComponent();
            loadingControls = false;

            statusWindowComboBox.Items.Add(GetEnumDescription(EyeStatusWindowOption.NoWindow));
            statusWindowComboBox.Items.Add(GetEnumDescription(EyeStatusWindowOption.FollowMouse));
            statusWindowComboBox.Items.Add(GetEnumDescription(EyeStatusWindowOption.Stationary));
        }
        
        public void SetClickControl(BlinkLinkClickControlSimpleModule module)
        {
            this.blinkLinkClickControlSimpleModule = module;
            LoadFromControls();
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

        private static EyeStatusWindowOption GetEyeStatusWindowOption(string windowOption)
        {
            return (EyeStatusWindowOption)GetEnum(windowOption,
                typeof(EyeStatusWindowOption), EyeStatusWindowOption.Stationary);
        }

        protected static string GetEnumDescription(Enum value)
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

        #region CMSConfigPanel Members

        public void LoadFromControls()
        {
            loadingControls = true;

            shortWinkTimeTextBox.Text = blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.ShortWinkTime.ToString();

            statusWindowComboBox.SelectedItem = GetEnumDescription(blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.EyeStatusWindowOption);

            switchEyesCheckBox.Checked = blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.SwitchEyes;

            playSoundCheckBox.Checked = blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.PlaySoundForBlinkOnlyClicks;

            loadingControls = false;
        }

        private SendLogAdvancedTracker sendLogAdvancedTracker; 

        public void Init(SendLogAdvancedTracker sendLogAdvancedTracker)
        {
            this.sendLogAdvancedTracker = sendLogAdvancedTracker;
        }

        #endregion

        private void switchEyesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.SwitchEyes = switchEyesCheckBox.Checked;
                sendLogAdvancedTracker();
            }
        }

        private void playSoundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                if( playSoundCheckBox.Checked )
                {
                    blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.SoundOption = SoundOption.BlinkClicksOnly;
                }
                else
                {
                    blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.SoundOption = SoundOption.NoSound;
                }
                sendLogAdvancedTracker();
            }
        }

        private void shortWinkTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                try
                {
                    string text = shortWinkTimeTextBox.Text;
                    float tempVal;

                    if( text.Length == 0 )
                    {
                        tempVal = BlinkLinkEyeClickData.MinimumShortWinkTime;
                        shortWinkTimeTextBox.Text = BlinkLinkEyeClickData.MinimumShortWinkTime.ToString();
                    }
                    else
                    {
                        tempVal = Math.Max(float.Parse(text), BlinkLinkEyeClickData.MinimumShortWinkTime);
                    }

                    blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.ShortWinkTime = tempVal;
                    sendLogAdvancedTracker();
                }
                catch( Exception )
                {
                    string oldText
                        = blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.ShortWinkTime.ToString();
                    shortWinkTimeTextBox.Text = oldText;

                    shortWinkTimeTextBox.SelectionStart = oldText.Length;
                }
            }
        }

        private void statusWindowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( !loadingControls )
            {
                blinkLinkClickControlSimpleModule.BlinkLinkEyeClickData.EyeStatusWindowOption = GetEyeStatusWindowOption((string)statusWindowComboBox.SelectedItem);
                sendLogAdvancedTracker();
            }
        }
    }
}
