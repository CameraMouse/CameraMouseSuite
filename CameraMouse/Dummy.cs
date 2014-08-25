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
using System.Xml;

namespace CameraMouseSuite
{
    [XmlRoot("D")]
    public class Dummy 
    {
        /*
        private int num = 0;

        [XmlElement(ElementName = "N", Namespace = "http://cameramouse.org/D")]
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
            }
        }
        */
    }

    [XmlRoot("D1")]
    public class Dummy1 : Dummy
    {
        private int num1 = 0;

        [XmlElement(ElementName="N")]
        public int Num1
        {
            get
            {
                return num1;
            }
            set
            {
                num1 = value;
            }
        }
    }

    [XmlRoot("D2")]
    public class Dummy2 : Dummy
    {
        private int num2 = 0;

        [XmlElement(ElementName = "N")]
        public int Num2
        {
            get
            {
                return num2;
            }
            set
            {
                num2 = value;
            }
        }
    }

    [XmlRoot("D3")]
    public class Dummy3
    {
        private XmlElement[] dummies = null;
        
        [XmlArrayItem(ElementName="X")]
        public XmlElement[] Dummies
        {
            get
            {
                return dummies;
            }
            set
            {
                dummies = value;
            }
        }
    }
}
