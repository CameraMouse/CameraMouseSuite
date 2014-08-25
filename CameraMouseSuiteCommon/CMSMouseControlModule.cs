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
using System.Xml.Serialization;

namespace CameraMouseSuite
{

    public abstract class CMSMouseControlModule : CMSModule
    {
        protected PointF mousePointer = PointF.Empty;
        
        [XmlIgnore()]
        public PointF MousePointer
        {

            get
            {
                lock(mousePointerLock)
                {
                    return mousePointer;
                }
            }
        }

        protected Size screenSize = Size.Empty;
        [XmlIgnore()]
        public Size ScreenSize
        {
            get
            {
                return screenSize;
            }
            set
            {
                screenSize = value;
            }
        }

        protected Size imageSize = Size.Empty;

        [XmlIgnore()]
        public Size ImageSize
        {
            get
            {
                return imageSize;
            }
            set
            {
                imageSize = value;
            }
        }

        private object mousePointerLock = new object();

        protected void SetCursorPosition(int x, int y)
        {
            lock(mousePointerLock)
            {
                User32.SetCursorPos(x,y);
                mousePointer.X = x;
                mousePointer.Y = y;
            }
        }

        public abstract void ProcessMouse(PointF imagePoint, 
                                          CMSExtraTrackingInfo extraInfo,
                                          Bitmap [] frames);
    }
}
