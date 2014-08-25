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
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace CameraMouseSuite
{

    public class ExperimentClickFrameSaver
    {
        private static string saveDirectory = "C:/temp";
        private static bool saveEnabled = false;
        
        private static List<ExperimentFrame> frames = new List<ExperimentFrame>();

        public static void Init(string SaveDirectory)
        {
            saveEnabled = true;
            saveDirectory = SaveDirectory;
        }

        private static object mutex = new object();
        public static void SaveEvent(ExperimentFrame frame)
        {
            try
            {
                lock(mutex)
                {
                    frames.Add(frame);
                    if(frames.Count > 100)
                    {
                        string saveFile = saveDirectory + "/" + DateTime.Now.Ticks + ".csv";
                        using(TextWriter tw = new StreamWriter(saveFile))
                        {
                            foreach (ExperimentFrame curFrame in frames)
                                tw.WriteLine(curFrame.RelativeYVal + "," + curFrame.EMAYVal +","+ curFrame.Threshold + "," + curFrame.Click);
                        }
                        frames.Clear();
                    }                    
                }
            }
            catch
            {
            }
        }
        public static bool IsExperimentFrameEnabled()
        {
            return saveEnabled;
        }
    }

    public class ExperimentFrame
    {
        public ExperimentFrame() { }
        public ExperimentFrame(double RelativeYVal, double EMAYVal, double Threshold, int Click) 
        {
            this.Click = Click;
            this.RelativeYVal = RelativeYVal;
            this.EMAYVal = EMAYVal;
            this.Threshold = Threshold;
        }

        public int Click = 0;
        public double RelativeYVal = 0;
        public double EMAYVal = 0;
        public double Threshold = 0;
    }

}


