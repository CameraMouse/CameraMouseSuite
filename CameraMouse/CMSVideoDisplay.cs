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

namespace CameraMouseSuite
{
    public enum VideoMessage
    {
        Close
    }

    //public delegate void DisplayMessage(VideoMessage videoMessage);
    //public delegate void MouseUpOnDisplay(MouseEventArgs e);

    public interface CMSVideoDisplay
    {
        Form InvokeNewForm(Type formType);
        void Init(CMSViewAdapter viewAdapter);
        void VideoInputSizeDetermined(Size [] videoInputSizes);
        void SetVideo(Bitmap [] frames);
        void SetTrackingControlMessage(bool control, string extraMessage);
        void ReceiveMessage(string message, Color color);
        void ReceiveMessages(Bitmap[] bitmaps, string[] messages);
        void Quit();
        Form GetParentForm();
    }
}
