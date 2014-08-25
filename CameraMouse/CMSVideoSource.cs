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
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public delegate void ProcessFrame(Bitmap [] frames);

    public delegate void VideoInputSizesDetermined(Size[] videoInputSizes);

    public delegate void VideoInputSizeDetermined(object sender, Size videoInputSizes);

    public delegate void CameraLost(bool containsCameras);

    public delegate void CameraFound();

    public abstract class CMSVideoSource
    {
        private event ProcessFrame processFrame;
        private event VideoInputSizesDetermined videoInputSizesDetermined;
        private event CameraLost cameraLost;
        private event CameraFound cameraFound;

        public event ProcessFrame ProcessFrame
        {
            add
            {
                processFrame += value;
            }
            remove
            {
                processFrame -= value;
            }
        }
        public event VideoInputSizesDetermined VideoInputSizesDetermined
        {
            add
            {
                videoInputSizesDetermined += value;
            }
            remove
            {
                videoInputSizesDetermined -= value;
            }
        }
        public event CameraLost CameraLost
        {
            add
            {
                cameraLost += value;
            }
            remove
            {
                cameraLost -= value;
            }
        }
        public event CameraFound CameraFound
        {
            add
            {
                cameraFound += value;
            }
            remove
            {
                cameraFound -= value;
            }
        }

        protected bool isActive = false;
        protected string currentMonikor = null;
        protected string[] cameraTitles = null;

        public string[] CameraTitles
        {
            get
            {
                return cameraTitles;
            }
        }

        public string GetCurrentMonikor()
        {
            return currentMonikor;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public abstract void Init(Form parentForm);

        public bool StartSource()
        {
            return StartSource(null);
        }

        public abstract bool StartSource(string preferedCameraMoniker);
        
        public abstract void StopSource();

        public abstract void DisplayCameraPropertyPage();

        protected void videoInputSizeDeterminedFunc(object sender, Size videoInputSize)
        {
            videoInputSizesDetermined(new Size[]{videoInputSize});
        }

        protected void videoInputSizesDeterminedFunc(Size [] videoInputSizes)
        {
            videoInputSizesDetermined(videoInputSizes);
        }

        protected void processFrameFunc(Bitmap b)
        {
            processFrame(new Bitmap[]{b});
        }

        protected void processFrameFunc(Bitmap [] bs)
        {
            processFrame(bs);
        }


        protected void CameraFoundFunc()
        {
            cameraFound();
        }

        protected void CameraLostFunc(bool containsCameras)
        {
            cameraLost(containsCameras);
        }
    }

}
