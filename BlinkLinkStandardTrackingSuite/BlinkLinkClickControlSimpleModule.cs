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
using BlinkLinkStandardTrackingSuite;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    public class BlinkLinkClickControlSimpleModule : BlinkLinkClickControlModule
    {
        public BlinkLinkClickControlSimpleModule()
            : base()
        {
            this.BlinkLinkEyeClickData = new BlinkLinkEyeClickData(ClickAction.None, ClickAction.None, ClickAction.None, ClickAction.None, ClickAction.LeftClick, 1.5f,
                1000f, SoundOption.BlinkClicksOnly, EyeStatusWindowOption.NoWindow, false);
        }

        public override void Init(System.Drawing.Size[] imageSizes)
        {
            base.Init(imageSizes);
        }

        public override CMSConfigPanel getPanel()
        {
            BlinkLinkClickControlSimplePanel clickPanel = new BlinkLinkClickControlSimplePanel();
            clickPanel.SetClickControl(this);
            return clickPanel;
        }
    }
}
