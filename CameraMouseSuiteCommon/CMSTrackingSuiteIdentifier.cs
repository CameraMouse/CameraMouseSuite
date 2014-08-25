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
    public class CMSTrackingSuiteIdentifier : IComparable<CMSTrackingSuiteIdentifier>
    {
        protected string name = null;
        protected string description = null;
        protected string informalName = null;

        public CMSTrackingSuiteIdentifier(string name, string description, string informalName)
        {
            this.name = name;
            this.description = description;
            this.informalName = informalName;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public string InformalName
        {
            get
            {
                return informalName;
            }
        }

        #region IComparable<CMSTrackingSuiteIdentifier> Members

        public int CompareTo(CMSTrackingSuiteIdentifier other)
        {
            if (informalName == null)
                return -1;
            return informalName.CompareTo(other.InformalName);
        }

        #endregion
    }
}
