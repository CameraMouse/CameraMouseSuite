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

namespace CameraMouseSuite
{
    using System;
    using System.Drawing.Imaging;

    // NewFrame delegate
    public delegate void CameraEventHandler(object sender, CameraEventArgs e);

    /// <summary>
    /// Camera event arguments
    /// </summary>
    public class CameraEventArgs : EventArgs
    {
        private System.Drawing.Bitmap bmp;

        // Constructor
        public CameraEventArgs(System.Drawing.Bitmap bmp)
        {
            this.bmp = bmp;
        }

        // Bitmap property
        public System.Drawing.Bitmap Bitmap
        {
            get { return bmp; }
        }
    }
}