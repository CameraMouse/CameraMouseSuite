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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BlinkLinkStandardTrackingSuite
{
    public partial class EyeStatusWindow : Form
    {
        public enum EyeStatus
        {
            Uninitialized,
            Open,
            Closed,
        }

        public enum MouseState
        {
            NoAction,
            LeftClickWaiting,
            RightClickWaiting,
            LeftClick,
            RightClick,
            DoubleClick,
            DoubleClickWaiting,
            DragStart,
            DragEnd,
        }

        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int YOffset = -20;

        #region Private Data Members

        private Bitmap          uninitializedEyeImage;
        private Bitmap          openEyeImage;
        private Bitmap          closedEyeImage;
        private Bitmap          leftClickMouseImage;
        private Bitmap          rightClickMouseImage;
        private Bitmap          doubleClickMouseImage;
        private Bitmap          dragStartMouseImage;
        private Bitmap          dragEndMouseImage;
        private Bitmap          noActionMouseImage;
        private Bitmap          leftClickWaitingImage;
        private Bitmap          rightClickWaitingImage;

        #endregion

        public EyeStatusWindow()
        {
            InitializeComponent();

            #region Load Images

            uninitializedEyeImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.initializationEye;
            openEyeImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.openEye;
            closedEyeImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.closedEye;
            leftClickMouseImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.leftClickMouse;
            rightClickMouseImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.rightClickMouse;
            doubleClickMouseImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.doubleClickMouse;
            dragStartMouseImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.dragStartMouse;
            dragEndMouseImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.dragEndMouse;
            leftClickWaitingImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.leftClickWaitingImage;
            rightClickWaitingImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.rightClickWaitingImage;
            noActionMouseImage = global::BlinkLinkStandardTrackingSuite.Properties.Resources.noActionMouse;

            #endregion
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                // Configure the CreateParams structure here
                cp.ExStyle = cp.ExStyle | WS_EX_NOACTIVATE;
                return cp;
            }
        }

        public bool Border
        {
            get
            {
                return FormBorderStyle != FormBorderStyle.None;
            }
            set
            {
                SetBorder(value);
            }
        }

        private delegate void UpdateLocationDelegate();
        private delegate void SetVisibilityDelegate(bool visibile);
        private delegate void SetBorderDelegate(bool border);
        private delegate void UpdateImageDelegate(Bitmap img);
        private delegate void UpdateImagesDelegate(Bitmap leftEyeImage, Bitmap rightEyeImage);

        private void EyeStatusWindow_Load(object sender, EventArgs e)
        {
            UpdateLocation();
            ResetImages();
        }

        public void UpdateLocation()
        {
            if( InvokeRequired )
            {
                BeginInvoke(new UpdateLocationDelegate(UpdateLocation));
            }
            else
            {
                Point loc = Cursor.Position;

                loc.Y += YOffset - Height;
                loc.X -= (Width / 2);

                Location = loc;
            }
        }
        private void SetEyeStatusImages(Bitmap leftEyeImage, Bitmap rightEyeImage)
        {
            if( leftEyeStatusPictureBox.InvokeRequired || rightEyeStatusPictureBox.InvokeRequired )
            {
                BeginInvoke(new UpdateImagesDelegate(SetEyeStatusImages), leftEyeImage, rightEyeImage);
            }
            else
            {
                leftEyeStatusPictureBox.Image  = leftEyeImage;
                rightEyeStatusPictureBox.Image = rightEyeImage;
                Invalidate();
            }
        }
        private void SetMouseImage(Bitmap img)
        {
            if( mousePictureBox.InvokeRequired )
            {
                BeginInvoke(new UpdateImageDelegate(SetMouseImage), img); 
            }
            else
            {
                mousePictureBox.Image = img;
                Invalidate();
            }
        }
        public void ResetImages()
        {
            SetMouseImage(noActionMouseImage);
            SetEyeStatusImages(uninitializedEyeImage, uninitializedEyeImage);
        }
        private Bitmap EyeStatusToBitmap(EyeStatus eyeStatus)
        {
            switch( eyeStatus )
            {
                case EyeStatus.Closed:
                    {
                        return closedEyeImage;
                    }

                case EyeStatus.Open:
                    {
                        return openEyeImage;
                    }

                default:
                    {
                        return uninitializedEyeImage;
                    }
            }
        }
        public void SetEyeStatusImages(EyeStatus leftEye, EyeStatus rightEye)
        {
            SetEyeStatusImages(EyeStatusToBitmap(leftEye), EyeStatusToBitmap(rightEye));
        }
        public void SetMouseImage(MouseState mouseState)
        {
            SetMouseImage(MouseStateToBitmap(mouseState));
        }
        public Bitmap MouseStateToBitmap(MouseState mouseState)
        {
            switch( mouseState )
            {
                case MouseState.DoubleClick:
                    {
                        return doubleClickMouseImage;
                    }

                case MouseState.DoubleClickWaiting:
                    {
                        return leftClickWaitingImage;
                    }

                case MouseState.DragEnd:
                    {
                        return dragEndMouseImage;
                    }

                case MouseState.DragStart:
                    {
                        return dragStartMouseImage;
                    }

                case MouseState.LeftClick:
                    {
                        return leftClickMouseImage;
                    }

                case MouseState.LeftClickWaiting:
                    {
                        return leftClickWaitingImage;
                    }

                case MouseState.RightClick:
                    {
                        return rightClickMouseImage;
                    }

                case MouseState.RightClickWaiting:
                    {
                        return rightClickWaitingImage;
                    }

                default:
                    {
                        return noActionMouseImage;
                    }
            }
        }
        public void SetVisibility(bool visible)
        {
            if( InvokeRequired )
            {
                BeginInvoke(new SetVisibilityDelegate(SetVisibility), visible);
            }
            else
            {
                Visible = visible;
            }
        }
        private void SetBorder(bool border)
        {
            if( InvokeRequired )
            {
                BeginInvoke(new SetBorderDelegate(SetBorder), border);
            }
            else
            {
                if( border )
                {
                    FormBorderStyle = FormBorderStyle.FixedDialog;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.None;
                }
            }
        }
    }
}
