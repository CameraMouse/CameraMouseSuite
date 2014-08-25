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

namespace CameraMouseSuite
{
    public class CMSLogAtt : System.Attribute, IComparable<CMSLogAtt>
    {
        public bool Frequent = false;
        public bool Large = false;
        public bool PrivacyConcerns = false;

        public CMSLogAtt() { }
        public CMSLogAtt(bool frequent,bool large, bool privacyConcerns) 
        {
            Frequent = frequent;
            Large = large;
            PrivacyConcerns = privacyConcerns;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (Frequent)
                h++;
            if (Large)
                h += 2;
            if (PrivacyConcerns)
                h += 4;
            return h;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj as CMSLogAtt) == 0;
        }

        #region IComparable<CMSLogAtt> Members

        public int CompareTo(CMSLogAtt other)
        {
            if (other == null)
                return -1;

            if (Frequent != other.Frequent)
                return Frequent ? 1 : -1;
            if (Large != other.Large)
                return Large ? 1 : -1;
            if (PrivacyConcerns != other.PrivacyConcerns)
                return PrivacyConcerns ? 1 : -1;
            return 0;
        }

        #endregion
    }
}
