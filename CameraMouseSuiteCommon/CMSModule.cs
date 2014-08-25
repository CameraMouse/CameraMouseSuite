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
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CameraMouseSuite
{
    public abstract class CMSModule
    {
        protected static int idmaker = 0;
        protected int id = ++idmaker;

        protected CMSState state = CMSState.Setup;
        public CMSState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        protected CMSTrackingSuiteAdapter trackingSuiteAdapter;

        [XmlIgnore()]
        public CMSTrackingSuiteAdapter CMSTrackingSuiteAdapter
        {
            get
            {
                return trackingSuiteAdapter;
            }
            set
            {
                trackingSuiteAdapter = value;
            }
        }

        public abstract CMSConfigPanel getPanel();
        public abstract void Clean();
        public abstract void ProcessKeys(Keys keys);
        public abstract void Init(System.Drawing.Size [] imageSizes);
        public abstract void Update(CMSModule module);
        public abstract void StateChange(CMSState state);
        public abstract void DrawOnFrame(Bitmap[] frames);
 
        //public abstract CMSModule Clone();

    }
}
