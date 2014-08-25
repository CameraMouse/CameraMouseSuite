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
using System.Drawing;

namespace AHMTrackingSuite
{
    [XmlRoot("Am")]
    [CMSLogAtt(Frequent = false, Large = false, PrivacyConcerns = false)]
    public class AHMLogSuiteEvent : CMSLogEvent
    {
        private AHMTrackingSuite trackingSuite = null;
        [XmlElement("Su")]
        public AHMTrackingSuite TrackingSuite
        {
            get
            {
                return trackingSuite;
            }
            set
            {
                trackingSuite = value;
            }
        }

        private AHMSimpleTrackingSuite simpleTrackingSuite = null;
        [XmlElement("Sp")]
        public AHMSimpleTrackingSuite SimpleTrackingSuite
        {
            get
            {
                return simpleTrackingSuite;
            }
            set
            {
                simpleTrackingSuite = value;
            }
        }

        private AHMMovementClickTrackingSuite movementClickTrackingSuite = null;
        [XmlElement("Sm")]
        public AHMMovementClickTrackingSuite MovementClickTrackingSuite
        {
            get
            {
                return movementClickTrackingSuite;
            }
            set
            {
                movementClickTrackingSuite = value;
            }
        }

    }

    [XmlRoot("Ar")]
    [CMSLogAtt(Frequent = true, Large = false, PrivacyConcerns = false)]
    public class AHMLogRealtimeEvent : CMSLogEvent
    {
        private float tansSqrdDist = 0.0f;
        [XmlElement("A")]
        public float TanSqrdDist
        {
            get
            {
                return tansSqrdDist;
            }
            set
            {
                tansSqrdDist = value;
            }
        }

        private float projSqrdDist = 0.0f;
        [XmlElement("P")]
        public float ProjSqrdDist
        {
            get
            {
                return projSqrdDist;
            }
            set
            {
                projSqrdDist = value;
            }
        }

        /*
        private float[] weights = null;
        [XmlArray("Ws")]
        [XmlArrayItem("W")]
        public float[] Weights
        {
            get
            {
                return weights;
            }
            set
            {
                weights = value;
            }
        }
        */
    }
    
    [XmlRoot("At")]
    [CMSLogAtt(Frequent = false, Large = true, PrivacyConcerns = false)]
    public class AHMLogTrainingEvent : CMSLogEvent
    {

        private CMSSerializedImage[] trainingImages = null;

        [XmlArray("Ts")]
        [XmlArrayItem("Ti")]
        public CMSSerializedImage[] TrainingImages
        {
            get
            {
                return trainingImages;
            }
            set
            {
                trainingImages = value;
            }
        }

        public void SetTrainingImages(Bitmap[] images)
        {
            trainingImages = new CMSSerializedImage[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                trainingImages[i] = new CMSSerializedImage();
                trainingImages[i].SetImage(images[i]);
            }
        }
    }

    [XmlRoot("Ai")]
    [CMSLogAtt(Frequent = true, Large = true, PrivacyConcerns = false)]
    public class AHMLogRealtimeFeatureImagesEvent : CMSLogEvent
    {
        private CMSSerializedImage curFeature = null;
        private CMSSerializedImage backFeature = null;
        
        [XmlElement("C")]
        public CMSSerializedImage CurFeature
        {
            get
            {
                return curFeature;
            }
            set
            {
                curFeature = value;
            }
        }

        [XmlElement("B")]
        public CMSSerializedImage BackFeature
        {
            get
            {
                return backFeature;
            }
            set
            {
                backFeature = value;
            }
        }
        
        public void SetImages(Bitmap curImage, Bitmap backImage)
        {
            curFeature = new CMSSerializedImage();
            curFeature.SetImage(curImage);

            backFeature = new CMSSerializedImage();
            backFeature.SetImage(backImage);
        }
    }

    [XmlRoot("As")]
    [CMSLogAtt(Frequent = false, Large = false, PrivacyConcerns = false)]
    public class AHMLogStateEvent : CMSLogEvent
    {
        private string state = null;

        [XmlElement("St")]
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
    }
}
