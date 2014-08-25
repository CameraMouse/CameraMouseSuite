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
using System.Xml.Serialization;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    [CMSLogAtt(false,true,false)]
    [XmlRoot("Blt")]
    public class BlinkLinkLogTemplatesEvent : CMSLogEvent
    {
        private bool isOpenTemplates = false;
        [XmlElement("Open")]
        public bool IsOpenTemplates
        {
            get
            {
                return isOpenTemplates;
            }
            set
            {
                isOpenTemplates = value;
            }
        }

        private int selectedTemplate = -1;
        [XmlElement("Stm")]
        public int SelectedTemplate
        {
            get
            {
                return selectedTemplate;
            }
            set
            {
                selectedTemplate = value;
            }
        }

        private CMSSerializedImage[] templates;
        [XmlArray("Tms")]
        [XmlArrayItem("Tm")]
        public CMSSerializedImage[] Templates
        {
            get
            {
                return templates;
            }
            set
            {
                templates = value;
            }
        }

        public void SetTemplates(FastBitmap.NccTemplate[] nccTemplates)
        {
            templates = new CMSSerializedImage[nccTemplates.Length];

            for(int i = 0; i < templates.Length; i++)
            {
                templates[i] = new CMSSerializedImage();
                templates[i].SetImage(nccTemplates[i].Bitmap);
            }
        }
    }
}
