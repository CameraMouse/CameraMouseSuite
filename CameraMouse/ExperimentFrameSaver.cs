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
    /*
    public class ExperimentFrameSaver
    {
        private static int saveTimeMillis = 1000;
        private static string saveDirectory = "C:/temp";
        private static bool saveEnabled = false;
        private static long lastSendTime = 0;

        public static void Init(int SaveTimeMillis, string SaveDirectory)
        {
            saveEnabled = true;
            saveTimeMillis = SaveTimeMillis;
            saveDirectory = SaveDirectory;
        }
        public static void SaveLogEvent(CMSLogExperimentFrameEvent logEvent)
        {
            try
            {
                lastSendTime = Environment.TickCount;

                string saveFile = saveDirectory + "/" + DateTime.Now.Ticks + ".xml";

                TextWriter tw = new StreamWriter(saveFile);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSLogExperimentFrameEvent));
                xmSer.Serialize(tw, logEvent);
                tw.Close();
            }
            catch
            {
            }
        }
        public static bool CanSaveLogEvent()
        {
            long tickCount = Environment.TickCount;

            return (tickCount - lastSendTime > saveTimeMillis);
        }
        public static bool IsExperimentFrameEnabled()
        {
            return saveEnabled;
        }

    }*/
}
