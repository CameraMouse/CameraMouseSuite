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
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

namespace CameraMouseSuite
{
    /*
    [XmlRoot("Xp")]
    [CMSLogAtt(Frequent = true, Large = true, PrivacyConcerns = false)]
    public class CMSLogExperimentFrameEvent : CMSLogEvent
    {
        public CMSLogExperimentFrameEvent()
        {
        }

        public CMSLogExperimentFrameEvent(Bitmap image, string trackingSuiteName, int x, int y)
        {
            SetImage(image);
            this.trackingSuiteName = trackingSuiteName;
            this.x = x;
            this.y = y;
        }

        private CMSSerializedImage[] frames = null;

        [XmlArray("Fs")]
        [XmlArrayItem("F")]
        public CMSSerializedImage[] Frames
        {
            get
            {
                return frames;
            }
            set
            {
                frames = value;
            }
        }

        public void SetImage(Bitmap image)
        {
            SetImages(new Bitmap[] { image });
        }

        public void SetImages(Bitmap[] images)
        {
            frames = new CMSSerializedImage[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                frames[i] = new CMSSerializedImage();
                frames[i].SetImage(images[i]);
            }
        }

        public Bitmap[] GetImages()
        {
            if (frames == null)
                return null;
            Bitmap[] images = new Bitmap[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                images[i] = frames[i].GetImage();
            }
            return images;
        }

        private string trackingSuiteName = null;
        [XmlElement("Ts")]
        public string TrackingSuiteName
        {
            get
            {
                return trackingSuiteName;
            }
            set
            {
                trackingSuiteName = value;
            }
        }

        private int x = 0;
        [XmlElement("X")]
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        private int y = 0;
        [XmlElement("Y")]
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

    }
    */
}
