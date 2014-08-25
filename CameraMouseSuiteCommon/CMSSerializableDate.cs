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
using System.Xml.Serialization;

namespace CameraMouseSuite
{
    [XmlRoot("Dt")]
    public class CMSSerializableDate
    {
        private long universalFileTime = 0;

        [XmlElement("Ut")]
        public long UniversalFileTime
        {
            get
            {
                return universalFileTime;
            }
            set
            {
                universalFileTime = value;
            }
        }

        private string timeZone = null;
        [XmlElement("Tz")]
        public string TimeZome
        {
            get
            {
                return timeZone;
            }
            set
            {
                timeZone = value;
            }
        }

        public void SetDate(DateTime dt)
        {
            universalFileTime = dt.ToFileTimeUtc();
            timeZone = TimeZone.CurrentTimeZone.StandardName;
        }

        public DateTime GetDate()
        {
            return DateTime.FromFileTimeUtc(universalFileTime);
        }

        public CMSSerializableDate() { }
        public CMSSerializableDate(DateTime dt) 
        {
            SetDate(dt);
        }
    }
}
