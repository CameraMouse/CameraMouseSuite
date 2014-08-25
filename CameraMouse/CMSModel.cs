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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace CameraMouseSuite
{
    public class CMSModel
    {
        private CMSTrackingSuiteDirectory trackingDirectory;
        public CMSTrackingSuiteDirectory TrackingDirectory
        {
            get
            {
                return trackingDirectory;
            }
        }

        private CMSConfig generalConfig = null;
        public CMSConfig GeneralConfig
        {
            get
            {
                return generalConfig;
            }
        }

        private CMSLogConfig logConfig = null;
        public CMSLogConfig LogConfig
        {
            get
            {
                return logConfig;
            }
            set
            {
                logConfig = value;
            }
        }

        private CMSIdentificationConfig idConfig = null;
        public CMSIdentificationConfig IdConfig
        {
            get
            {
                return idConfig;
            }
            set
            {
                idConfig = value;
            }
        }

        private CMSCameraConfig cameraConfig = null;

        private string suiteLibDirectory = null;
        private string suiteConfigDirectory = null;
        private string generalConfigFile = null;
        private string cameraConfigFile = null;
        private string logConfigFile = null;
        private string idConfigFile = null;

        private Size[] frameDims = null;
        public Size [] FrameDims
        {
            get
            {
                return frameDims;
            }
            set
            {
                frameDims = value;
            }
        }

        private double [] ratioVideoInputToOutput = new double[]{1.0};
        public double [] RatioVideoInputToOutput
        {
            get
            {
                return ratioVideoInputToOutput;
            }
            set
            {
                ratioVideoInputToOutput = value;
            }
        }

        public CMSTrackingSuite SelectedSuite
        {
            get
            {
                return trackingDirectory.GetTrackingSuite(generalConfig.SelectedSuiteName);
            }
        }

        public string SelectedSuiteName
        {
            get
            {
                return generalConfig.SelectedSuiteName;
            }
            set
            {
                generalConfig.SelectedSuiteName = value;
            }
        }

        public string CurrentMonikor
        {
            get
            {
                return cameraConfig.PreferedMoniker;
            }
            set
            {
                cameraConfig.PreferedMoniker = value;
                SaveCameraConfig();
            }
        }
        
        public void Init(string suiteLibDirectory, 
                         string suiteConfigDirectory, 
                         string generalConfigFile,
                         string cameraConfigFile,
                         string logConfigFile,
                         string idConfigFile)
        {
            this.suiteLibDirectory = suiteLibDirectory;
            this.suiteConfigDirectory = suiteConfigDirectory;
            this.generalConfigFile = generalConfigFile;
            this.cameraConfigFile = cameraConfigFile;
            this.logConfigFile = logConfigFile;
            this.idConfigFile = idConfigFile;
            trackingDirectory = new CMSTrackingSuiteDirectory();
        }

        #region Load
        public void Load()
        {

            LoadGeneralConfig();
            LoadCameraConfig();
            LoadLogConfig();
            LoadIdConfig();

            trackingDirectory.Load(CMSConstants.SUITE_CONFIG_DIR, CMSConstants.SUITE_LIB_DIR);

            if (trackingDirectory.GetTrackingSuite(generalConfig.SelectedSuiteName) == null)
            {
                generalConfig.SelectedSuiteName = CMSConstants.STANDARD_TRACKING_SUITE_NAME;
            }
        }
        private void LoadGeneralConfig()
        {
            bool generateFile = true;
            if (File.Exists(generalConfigFile))
            {
                try
                {
                    generateFile = false;
                    TextReader tr = new StreamReader(generalConfigFile);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSConfig));
                    generalConfig = (CMSConfig)xmSer.Deserialize(tr);
                    tr.Close();
                }
                catch (Exception e)
                {
                    generateFile = true;
                    try
                    {
                        File.Delete(generalConfigFile);
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }

            if (generateFile)
            {
                generalConfig = new CMSConfig();
                generalConfig.SelectedSuiteName = CMSConstants.STANDARD_TRACKING_SUITE_NAME;
                generalConfig.ControlTogglerConfig = new CMSControlTogglerConfig();

                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(generalConfigFile)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(generalConfigFile));
                    }
                    FileStream fs = new FileStream(generalConfigFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete | FileShare.ReadWrite);
                    TextWriter tw = new StreamWriter(fs);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSConfig));
                    xmSer.Serialize(tw, generalConfig);
                    tw.Close();
                }
                catch (Exception e)
                {
                }
            }

        }
        private void LoadCameraConfig()
        {
            bool generateFile = true;
            if (File.Exists(cameraConfigFile))
            {
                try
                {
                    generateFile = false;
                    TextReader tr = new StreamReader(cameraConfigFile);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSCameraConfig));
                    cameraConfig = (CMSCameraConfig)xmSer.Deserialize(tr);
                    tr.Close();
                }
                catch (Exception e)
                {
                    generateFile = true;
                    try
                    {
                        File.Delete(cameraConfigFile);
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }

            if (generateFile)
            {
                cameraConfig = new CMSCameraConfig();
                cameraConfig.PreferedMoniker = null;

                FileStream fs = new FileStream(cameraConfigFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete | FileShare.ReadWrite);
                TextWriter tw = new StreamWriter(fs);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSCameraConfig));
                xmSer.Serialize(tw, cameraConfig);
                tw.Close();
            }
        }
        private void LoadIdConfig()
        {
            bool generateFile = true;
            if (File.Exists(idConfigFile))
            {
                try
                {
                    generateFile = false;
                    TextReader tr = new StreamReader(idConfigFile);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSIdentificationConfig));
                    idConfig = (CMSIdentificationConfig)xmSer.Deserialize(tr);

                    tr.Close();
                }
                catch
                {
                    generateFile = true;
                    try
                    {
                        File.Delete(idConfigFile);
                    }
                    catch
                    {
                    }
                }
            }

            if (generateFile)
            {
                idConfig = new CMSIdentificationConfig();

                try
                {
                    FileStream fs = new FileStream(idConfigFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete | FileShare.ReadWrite);
                    TextWriter tw = new StreamWriter(fs);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSIdentificationConfig));
                    xmSer.Serialize(tw, idConfig);
                    tw.Close();
                }
                catch
                {
                }
            }

            if (idConfig != null)
            {
                if (Environment.GetEnvironmentVariables().Contains("NotStudy"))
                {
                    try
                    {
                        idConfig.NotStudy = Boolean.Parse(Environment.GetEnvironmentVariables()["NotStudy"] as string);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    idConfig.NotStudy = false;
                }
            }     
        }
        private void LoadLogConfig()
        {
            bool generateFile = true;
            if (File.Exists(logConfigFile))
            {
                try
                {
                    generateFile = false;
                    TextReader tr = new StreamReader(logConfigFile);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSLogConfig));
                    logConfig = (CMSLogConfig)xmSer.Deserialize(tr);
                    tr.Close();
                }
                catch
                {
                    generateFile = true;
                    try
                    {
                        File.Delete(logConfigFile);
                    }
                    catch
                    {
                    }
                }
            }

            if (generateFile)
            {
                logConfig = new CMSLogConfig();

                try
                {
                    FileStream fs = new FileStream(logConfigFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete | FileShare.ReadWrite);
                    TextWriter tw = new StreamWriter(fs);
                    XmlSerializer xmSer = new XmlSerializer(typeof(CMSLogConfig));
                    xmSer.Serialize(tw, logConfig);
                    tw.Close();
                }
                catch
                {
                }
            }

        }
        public void LoadCurrentSuite()
        {
            CMSTrackingSuite currentSuite = SelectedSuite;
            if (currentSuite == null)
                return;
            trackingDirectory.LoadSuite(suiteConfigDirectory, currentSuite.GetType());
        }
        public void LoadControlTogglerConfig()
        {
            CMSConfig newConfig = null;
            if (File.Exists(generalConfigFile))
            {
                TextReader tr = new StreamReader(CMSConstants.MAIN_CONFIG_FILE);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSConfig));
                newConfig = (CMSConfig)xmSer.Deserialize(tr);
                tr.Close();

            }
            else
            {
                newConfig = new CMSConfig();
            }

            generalConfig.ControlTogglerConfig.UpdateControlTogglerConfig(newConfig.ControlTogglerConfig);
        }
        #endregion

        #region Save
        public void Save()
        {
            trackingDirectory.Save(suiteConfigDirectory);
            SaveGeneralConfig();
            SaveLogConfig();
            SaveIdConfig();
        }

        private object generalSaveMutex = new object();
        public void SaveGeneralConfig()
        {
            lock (generalSaveMutex)
            {
                TextWriter tw = new StreamWriter(generalConfigFile);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSConfig));
                xmSer.Serialize(tw, generalConfig);
                tw.Close();
            }
        }
        
        private object cameraSaveMutex = new object();
        private void SaveCameraConfig()
        {
            lock (cameraSaveMutex)
            {
                TextWriter tw = new StreamWriter(cameraConfigFile);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSCameraConfig));
                xmSer.Serialize(tw, cameraConfig);
                tw.Close();
            }
        }

        private object idSaveMutex = new object();
        public void SaveIdConfig()
        {
            lock (idSaveMutex)
            {
                TextWriter tw = new StreamWriter(idConfigFile);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSIdentificationConfig));
                xmSer.Serialize(tw, idConfig);
                tw.Close();
            }
        }

        private object logSaveMutex = new object();
        public void SaveLogConfig()
        {
            lock (logSaveMutex)
            {
                TextWriter tw = new StreamWriter(logConfigFile);
                XmlSerializer xmSer = new XmlSerializer(typeof(CMSLogConfig));
                xmSer.Serialize(tw, logConfig);
                tw.Close();
            }
        }

        public void SaveCurrentSuite()
        {
            trackingDirectory.SaveSuite(suiteConfigDirectory, SelectedSuiteName);
        }
        #endregion

        public void UidUpdated(long uid)
        {
            logConfig.Uid = uid;
            SaveLogConfig();
        }
        public void IncrementAndSaveSessionNum()
        {
            logConfig.SessionNum = (logConfig.SessionNum + 1) % int.MaxValue;
            SaveLogConfig();
        }

    }
}
