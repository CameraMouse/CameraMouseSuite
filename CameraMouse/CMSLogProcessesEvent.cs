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
using System.Diagnostics;

namespace CameraMouseSuite
{
    [XmlRoot("Pss")]
    [CMSLogAtt(Frequent = false, Large = false, PrivacyConcerns = true)]
    public class CMSLogProcessesEvent : CMSLogEvent
    {
        private string[] processes;
        [XmlArray("Ps")]
        [XmlArrayItem("P")]
        public string[] Processes
        {
            get
            {
                return processes;
            }
            set
            {
                processes = value;
            }
        }

        public bool CaptureProcesses()
        {
            List<string> pList = new List<string>();
            try
            {
                foreach (Process p in Process.GetProcesses("."))
                {
                    try
                    {
                        if (p.MainWindowTitle.Length > 0)
                            pList.Add(p.ProcessName.ToString());

                    }
                    catch { }
                }
            }
            catch
            {
                return false;
            }
            processes = pList.ToArray();
            return true;
        }
    }
}
