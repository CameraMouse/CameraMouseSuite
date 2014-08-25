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

namespace CameraMouseSuite
{
    

    public interface CMSExtraTrackingInfo
    {
    }

    public abstract class CMSTrackingModule : CMSModule
    {
        protected PointF imagePoint=PointF.Empty;
        protected CMSExtraTrackingInfo extraTrackingInfo = null;
        
        public PointF ImagePoint
        {
            get
            {
                return imagePoint;
            }
        }
        public CMSExtraTrackingInfo ExtraTrackingInfo
        {
            get
            {
                return extraTrackingInfo;
            }
        }
        
        public abstract void ProcessMouse(Point p, bool leftMouseButton, int cameraNum);
        public abstract void Process(Bitmap [] frames);
        //public abstract bool ReadyForControl();
    }

}
