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
using System.Net;

namespace CameraMouseSuite
{
    public class CMSLogConfig
    {
        public CMSLogConfig Clone()
        {
            CMSLogConfig logConfig = new CMSLogConfig();
            logConfig.MBLimitLogDirectory = MBLimitLogDirectory;
            logConfig.MBLimitLogFile = MBLimitLogFile;
            logConfig.LogServer = LogServer;
            logConfig.EnablePCMessages = EnablePCMessages;
            logConfig.LoggingBehavior = LoggingBehavior;
            logConfig.ProxyServer = ProxyServer;
            logConfig.ProxyUsername = ProxyUsername;
            logConfig.ProxyPassword = ProxyPassword;
            logConfig.MaxNumLogEventInMessage = MaxNumLogEventInMessage;
            logConfig.MaxNumLogEventInMemory = MaxNumLogEventInMemory;
            logConfig.LogEventWaitTime = LogEventWaitTime;
            logConfig.FileMonitorWaitTime = FileMonitorWaitTime;
            logConfig.ReconnectTiming = ReconnectTiming;
            logConfig.Uid = Uid;
            logConfig.SessionNum = SessionNum;
            return logConfig;
        }

        private int mbLimitLogDirectory = 400;
        public int MBLimitLogDirectory
        {
            get
            {
                return mbLimitLogDirectory;
            }
            set
            {
                mbLimitLogDirectory = value;
            }
        }

        private int mbLimitLogFile = 50;
        public int MBLimitLogFile
        {
            get
            {
                return mbLimitLogFile;
            }
            set
            {
                mbLimitLogFile = value;
            }
        }

        private string proxyServer = null;
        public string ProxyServer
        {
            get
            {
                return proxyServer;
            }
            set
            {
                proxyServer = value;
            }
        }

        private string proxyUsername = null;
        public string ProxyUsername
        {
            get
            {
                return proxyUsername;
            }
            set
            {
                proxyUsername = value;
            }
        }

        private string proxyPassword = null;
        public string ProxyPassword
        {
            get
            {
                return proxyPassword;
            }
            set
            {
                proxyPassword = value;
            }
        }

        public WebProxy CreateProxy()
        {
            if (proxyServer == null || proxyServer.Length == 0)
                return null;
            WebProxy myProxy=new WebProxy();
            myProxy.Address = new Uri(proxyServer);

            if (proxyUsername != null && proxyUsername.Length > 0)
            {
                myProxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
            }

            return myProxy;
        }

        private string logServer = CMSConstants.DEFAULT_LOG_SERVER_URI;
        public string LogServer
        {
            get
            {
                return logServer;
            }
            set
            {
                logServer = value;
            }
        }

        private bool enablePCMessages = false;
        public bool EnablePCMessages
        {
            get
            {
                return enablePCMessages;
            }
            set
            {
                enablePCMessages = value;
            }
        }

        private int maxNumLogEventInMessage = 100;
        public int MaxNumLogEventInMessage
        {
            get
            {
                return maxNumLogEventInMessage;
            }
            set
            {
                maxNumLogEventInMessage = value;
            }
        }

        private int maxNumLogEventInMemory = 1000;
        public int MaxNumLogEventInMemory
        {
            get
            {
                return maxNumLogEventInMemory;
            }
            set
            {
                maxNumLogEventInMemory = value;
            }
        }

        private int logEventWaitTime = 500;
        public int LogEventWaitTime
        {
            get
            {
                return logEventWaitTime;
            }
            set
            {
                logEventWaitTime = value;
            }
        }

        private int fileMonitorWaitTime = 10000;
        public int FileMonitorWaitTime
        {
            get
            {
                return fileMonitorWaitTime;
            }
            set
            {
                fileMonitorWaitTime = value;
            }
        }

        private int reconnectTiming = 5000;
        public int ReconnectTiming
        {
            get
            {
                return reconnectTiming;
            }
            set
            {
                reconnectTiming = value;
            }
        }

        private long uid = 0;
        public long Uid
        {
            get
            {
                return uid;
            }
            set
            {
                uid = value;
            }
        }

        private int sessionNum = 0;
        public int SessionNum
        {
            get
            {
                return sessionNum;
            }
            set
            {
                sessionNum = value;
            }
        }


        private CMSLogBehavior loggingBehavior = CMSLogBehavior.NoLogging;
        public CMSLogBehavior LoggingBehavior
        {
            get
            {
                return loggingBehavior;
            }
            set
            {
                loggingBehavior = value;
            }
        }


        public void UpdateUserControlledLogConfigInfo(CMSLogConfig logConfig)
        {
            LogServer = logConfig.LogServer;
            EnablePCMessages = logConfig.EnablePCMessages;
            LoggingBehavior = logConfig.LoggingBehavior;
            ProxyServer = logConfig.ProxyServer;
            ProxyUsername = logConfig.ProxyUsername;
            ProxyPassword = logConfig.ProxyPassword;
        }

    }

}
