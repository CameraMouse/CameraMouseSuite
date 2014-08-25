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
    public class CMSStandardTrackingSuiteAdapter : CMSTrackingSuiteAdapter
    {
        private CMSModel model = null;
        private CMSController controller = null;
        private CMSVideoDisplay view = null;
        public CMSStandardTrackingSuiteAdapter(CMSModel model, CMSController controller, CMSVideoDisplay view)
        {
            this.model = model;
            this.controller = controller;
            this.view = view;
        }
                
        #region CMSTrackingSuiteAdapter Members

        public Form CreateForm(Type formType)
        {
            return view.InvokeNewForm(formType);
        }

        public void ToggleSetup(bool setup)
        {
            controller.ToggleSetup(setup);
                SendMessage("");
        }

        public void SendMessage(string message)
        {
            controller.ReceiveMessageFromTracker(message);
        }

        public void SendMessages(Bitmap[] bitmaps, string[] messages)
        {
            controller.ReceiveMessagesFromTracker(bitmaps, messages);
        }

        public double [] GetRatioInputToOutput()
        {
            return model.RatioVideoInputToOutput;
        }

        public void ToggleControl(bool control)
        {
            controller.ToggleControl(control);
        }

        #endregion
    }
}
