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
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace CameraMouseSuite
{
    /*
    [XmlRoot("TestDate")]
    public class TestDate
    {

        private string tester = null;

        [XmlElement()]        
        public string Tester
        {
            get
            {
                return tester;
            }
            set
            {
                tester = value;
            }
        }
    }
    */

    public class CameraMouseSuite
    {
        /*
        public static void Test()
        {

            XmlElement[] dummies = new XmlElement[2];

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer xmSer = new XmlSerializer(typeof(Dummy1));
            MemoryStream ms = new MemoryStream();
            xmSer.Serialize(ms, new Dummy1(), ns);
            XmlDocument xDoc1 = new XmlDocument();

            ms.Position = 0;
            xDoc1.Load(ms);
            dummies[0] = xDoc1.LastChild as XmlElement;

            XmlSerializerNamespaces ns2 = new XmlSerializerNamespaces();
            ns2.Add("", "");

            XmlSerializer xmSer2 = new XmlSerializer(typeof(Dummy2));
            MemoryStream ms2 = new MemoryStream();
            xmSer2.Serialize(ms2, new Dummy2(), ns2);
            ms2.Position = 0;
            XmlDocument xDoc2 = new XmlDocument();
            xDoc2.Load(ms2);
            dummies[1] = xDoc2.LastChild as XmlElement;

            Dummy3 d3 = new Dummy3();
            d3.Dummies = dummies;

            XmlSerializerNamespaces ns3 = new XmlSerializerNamespaces();
            ns3.Add("", "");
            XmlSerializer xmSer3 = new XmlSerializer(typeof(Dummy3));
            TextWriter tw = new StreamWriter("C:/temp/d3.xml");
            xmSer3.Serialize(tw, d3, ns3);
        }

        public static void Test2()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter();
            foreach (Process p in Process.GetProcesses("."))
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        sw.Write("\r\n");
                        sw.Write("\r\n Process Name:" + p.ProcessName.ToString());                                                
                    }
                }
                catch { }
            }
        }

        public static void Test3()
        {
            DirectoryInfo di = new DirectoryInfo("c:/temp");
            FileInfo[] fis = di.GetFiles();
            int g = 0;
            g++;
        }

        public static void Test4()
        {
            TestDate t = new TestDate();
            //t.Tester = "This is my String";
            using (FileStream fs = new FileStream("C:/temp/out.xml", FileMode.Create))
            {
                XmlSerializer xmSer = new XmlSerializer(typeof(TestDate));
                xmSer.Serialize(fs,t);
            }
        }
        */

        public static void Test5()
        {
            Bitmap b = new Bitmap(200, 200);
            for (int x = 0; x < 200; x++)
            {
                for (int y = 0; y < 200; y++)
                {
                    if (((x + y) % 2) == 0)
                        b.SetPixel(x, y, Color.White);
                    else
                        b.SetPixel(x, y, Color.Black);
                }
            }
            b.Save("C:/temp/b.bmp");
            CMSSerializedImage img = new CMSSerializedImage();
            img.SetImage(b);
            XmlSerializer xmSer = new XmlSerializer(typeof(CMSSerializedImage));
            using (FileStream f = new FileStream("C:/temp/out2.xml",FileMode.Create))
            {
                xmSer.Serialize(f, img);
            }

            Bitmap b2;
            using (FileStream f = new FileStream("C:/temp/out2.xml", FileMode.Open))
            {
                CMSSerializedImage img2 = xmSer.Deserialize(f) as CMSSerializedImage;
                b2 = img2.GetImage();
            }

            b2.Save("C:/temp/b2.bmp");
        }


        private static void ProcessCommandLineArguments(String[] args)
        {
            foreach (String arg in args)
            {
                int i = arg.IndexOf("=");
                if(i==-1)
                    continue;
                String key = arg.Substring(0,i);
                String val = arg.Substring(i + 1, arg.Length - i-1);
                Environment.SetEnvironmentVariable(key, val);
            }
            
        }

        public static void Main(String[] args)
        {
            ProcessCommandLineArguments(args);
            
            
            Environment.SetEnvironmentVariable("NotStudy", "true");

            if (Environment.GetEnvironmentVariables().Contains("ExpSaveDir"))
            {
                string saveDir = Environment.GetEnvironmentVariables()["ExpSaveDir"] as string;
                ExperimentClickFrameSaver.Init(saveDir);
            }            

            /*
            if (Environment.GetEnvironmentVariables().Contains("ExpSaveDir") &&
                Environment.GetEnvironmentVariables().Contains("ExpSaveTime"))
            {
                string saveDir = Environment.GetEnvironmentVariables()["ExpSaveDir"] as string;
                string saveTime = Environment.GetEnvironmentVariables()["ExpSaveTime"] as string;

                int iSaveTime = Int32.Parse(saveTime);
                ExperimentFrameSaver.Init(iSaveTime, saveDir);
            }
            */

            CMSCameraProfile profile = CMSCameraProfile.OneCamera;

            if (Environment.GetEnvironmentVariables().Contains("CameraProfile"))
            {
                try
                {
                    string pa = Environment.GetEnvironmentVariables()["CameraProfile"] as string;
                    profile = (CMSCameraProfile)Enum.Parse(typeof(CMSCameraProfile), pa);
                }
                catch (Exception e)
                {
                }
            }

            if (profile.Equals(CMSCameraProfile.OneCamera))
            {
                CMSController controller = new CMSController();
                VideoForm vf = new VideoForm();
                CMSSingleWebcamSource source = new CMSSingleWebcamSource();
                CMSControlToggler ct = new CMSControlToggler();
                controller.Start(source, vf, ct);
            }
            else if (profile.Equals(CMSCameraProfile.LeftRight))
            {
                CMSController controller = new CMSController();
                LeftRightWindowsForm lrvf = new LeftRightWindowsForm();
                CMSMultipleWebcamSource source = new CMSMultipleWebcamSource();
                source.Profile = CMSCameraProfile.LeftRight;
                CMSControlToggler ct = new CMSControlToggler();
                controller.Start(source, lrvf, ct);
            }
            else
            {
                CMSController controller = new CMSController();
                CMSMultipleCameraForm f = new CMSMultipleCameraForm();
                CMSMultipleWebcamSource source = new CMSMultipleWebcamSource();
                source.Profile = profile;
                CMSControlToggler ct = new CMSControlToggler();
                controller.Start(source, f, ct);
            }
        }

    }
}