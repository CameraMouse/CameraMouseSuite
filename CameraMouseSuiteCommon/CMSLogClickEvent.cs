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
using System.Xml.Serialization;

namespace CameraMouseSuite
{
    public enum ClickType
    {
        DlClk,
        LClk,
        LDn,
        LUp,        
        RClk,
        RDn,
        RUp
    }

    [XmlRoot("K")]
    [CMSLogAtt(Frequent = false, Large = false, PrivacyConcerns = false)]
    public class CMSLogClickEvent : CMSLogEvent
    {
        private int x = 0;
        [XmlElement("X")]
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        private int y = 0;
        [XmlElement("Y")]
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        private int width = 0;
        [XmlElement("W")]
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        private int height = 0;
        [XmlElement("H")]
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        private ClickType clickType = ClickType.LClk;
        [XmlElement("Tp")]
        public ClickType ClickType
        {
            get
            {
                return clickType;
            }
            set
            {
                clickType = value;
            }
        }
    }
}
