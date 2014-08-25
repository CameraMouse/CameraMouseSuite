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
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;

namespace CameraMouseSuite
{
    public delegate void UidReceived(long uid);

    public delegate void LoggerStatusChange();

    public class CMSLogger
    {
        #region Local

        private long lastDateTimeStampSendTime = 0;

        private long dateTimeStampWaitPeriodMillis = 4000;
        public long DateTimeStampWaitPeriodMillis
        {
            get
            {
                return dateTimeStampWaitPeriodMillis;
            }
            set
            {
                dateTimeStampWaitPeriodMillis = value;
            }
        }

        private bool neverConnect = false;
        public bool NeverConnect
        {
            get
            {
                return neverConnect;
            }
            set
            {
                neverConnect = value;
            }
        }

        private object logMutex = new object();
        private long startTimeInMillis = Environment.TickCount;
        private CMSLogTimingProfile timingProfile = new CMSLogTimingProfile();

        private long logUid = 0;

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

        private bool isRunning = false;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public bool IsConnected
        {
            get
            {
                if (connection == null)
                    return false;
                return connection.IsConnected;
            }
        }

        public Exception NetworkException
        {
            get
            {
                if (connection == null)
                    return null;
                return connection.NetworkException;
            }
        }

        private CMSFileLogger fileLogger = null;
        private CMSLogConnection connection = null;

        private Thread startThread = null;
        private Timer logEventProcessorTimer = null;
        private Timer fileProcessorTimer = null;

        private LinkedList<CMSLogEvent> logEvents = new LinkedList<CMSLogEvent>();

        private void Start()
        {
            if (!isRunning)
                LoggerStatusChange();
            isRunning = true;
            startThread = new Thread(new ThreadStart(StartThread));
            startThread.Start();            
        }

        private void StartThread()
        {

            if (!idConfig.HasConsent)
            {
                Stop();
                return;
            }

            connection = new CMSLogConnection();
            connection.EndPoint = logConfig.LogServer;
            connection.IdConfig = idConfig;
            connection.Uid = logConfig.Uid;
            connection.LoggerStatusChange += this.LoggerStatusChange;
            connection.ProxyServer = logConfig.CreateProxy();

            if (!neverConnect)
            {
                if (logConfig.Uid == 0)
                {
                    if (!connection.Connect())
                    {
                        Stop();
                        return;
                    }

                    logConfig.Uid = connection.Uid;
                    if (uidReceived != null)
                        uidReceived(logConfig.Uid);
                }
                else
                {
                    if (connection.Connect())
                    {
                        logConfig.Uid = connection.Uid;
                        if (uidReceived != null)
                            uidReceived(logConfig.Uid);
                    }
                }
            }

            fileLogger = new CMSFileLogger();
            fileLogger.Directory = "./" + CMSConstants.LOG_DIR;
            fileLogger.MBLimitLogDirectory = logConfig.MBLimitLogDirectory;
            fileLogger.MBLimitLogFile = logConfig.MBLimitLogFile;

            //logEvents = new LinkedList<CMSLogEvent>();

            if(logEventProcessorTimer != null)
                logEventProcessorTimer.Dispose();

            TimerCallback logCallBack = new TimerCallback(ProcessLogEvents);
            logEventProcessorTimer = new System.Threading.Timer(logCallBack, null, 5000, logConfig.LogEventWaitTime);

            if (fileProcessorTimer != null)
                fileProcessorTimer.Dispose();

            TimerCallback monitorCallBack = new TimerCallback(Monitor);
            fileProcessorTimer = new System.Threading.Timer(monitorCallBack, null, 5000, logConfig.FileMonitorWaitTime);
        }

        private object monitorMutex = new object();
        private string curFileName = null;

        private LinkedList<XmlElement> monitorLogEvents = new LinkedList<XmlElement>();
        private void Monitor(object state)
        {
            lock (monitorMutex)
            {

                if (!isRunning)
                    return;

                if (neverConnect)
                    return;

                if (!IsConnected)
                {
                    if (curFileName != null)
                    {
                        curFileName = null;
                        monitorLogEvents.Clear();
                    }
                    return;
                }

                if (curFileName != null)
                {
                    if (monitorLogEvents.Count == 0)
                    {
                        try
                        {
                            if(File.Exists(curFileName))
                                File.Delete(curFileName);
                            curFileName = null;
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                DirectoryInfo di = new DirectoryInfo("./" + CMSConstants.LOG_DIR);
                FileInfo[] files = di.GetFiles();
                if (files == null || files.Length == 0)
                    return;

                FileInfo oldestFile = null;
                long fileSmallestNum = Int64.MaxValue;

                foreach (FileInfo fi in files)
                {
                    string name = fi.Name;
                    if (!name.StartsWith("x"))
                        continue;
                    bool deleteable = name.StartsWith("x");
                    int periodIndex = name.IndexOf('.');
                    long serialNum = Int64.Parse(name.Substring(1, periodIndex - 1));
                    if (serialNum < fileSmallestNum)
                    {
                        fileSmallestNum = serialNum;
                        oldestFile = fi;
                    }
                }


                if (oldestFile == null)
                    return;

                try
                {
                    curFileName = oldestFile.FullName;
                    using (FileStream fileStream = new FileStream(curFileName, FileMode.Open))
                    {
                        using (GZipStream gStream = new GZipStream(fileStream, CompressionMode.Decompress))
                        {
                            /*
                            const int size = 4096;
                            byte[] bytes = new byte[4096];
                            int numBytes;
                            while ((numBytes = gStream.Read(bytes, 0, size)) > 0)
                            {
                                int prevIndex = 0;
                                for (int i = 0; i < numBytes; i++)
                                {
                                    byte b = bytes[i];

                                    //if(b < 32 && b != 9 && b != 10 && b != 13)
                                    if (b == 31)
                                    {
                                        if (prevIndex < i)
                                            ms.Write(bytes, prevIndex, i - prevIndex);
                                        prevIndex = i + 1;
                                    }
                                }
                                if (prevIndex < numBytes)
                                {
                                    ms.Write(bytes, prevIndex, numBytes - prevIndex);
                                }
                            }
                            */

                            //ms.WriteTo(new FileStream("C:/temp/out.xml", FileMode.Create));

                            //ms.Position = 0;
                            XmlSerializer xmSer = new XmlSerializer(typeof(CMSLogMessage));
                            CMSLogMessage logMessage = xmSer.Deserialize(gStream) as CMSLogMessage;
                            if (logMessage == null)
                            {
                                curFileName = null;
                            }
                            foreach (XmlElement logXml in logMessage.Messages)
                                monitorLogEvents.AddLast(logXml);
                        }
                    }

                }
                catch
                {

                }
            }
        }

        private long lastReconnectTime = 0;
        private object processLogEventsMutex = new object();
        private void ProcessLogEvents(object state)
        {
            if (!isRunning)
                return;

            lock (processLogEventsMutex)
            {
                LinkedList<object> curLogEvents = new LinkedList<object>();
                LinkedList<CMSLogEvent> tempLogEvents = new LinkedList<CMSLogEvent>();
                lock (logMutex)
                {
                    int count = 0;
                    int newCount = 0;
                    foreach (CMSLogEvent logEvent in logEvents)
                    {                        
                        count++;
                        if (count < logConfig.MaxNumLogEventInMessage)
                            curLogEvents.AddLast(logEvent);
                        else if (newCount < logConfig.MaxNumLogEventInMemory)
                        {
                            tempLogEvents.AddLast(logEvent);
                            newCount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    logEvents = tempLogEvents;
                }

                while (curLogEvents.Count < logConfig.MaxNumLogEventInMessage && monitorLogEvents.Count > 0)
                {
                    curLogEvents.AddLast(monitorLogEvents.First.Value);
                    monitorLogEvents.RemoveFirst();
                }

                if (curLogEvents.Count == 0)
                    return;

                if (IsConnected)
                {
                    if (connection.SendLogEvents(curLogEvents))
                        return;                    
                }

                if (!neverConnect)
                {
                    long curTickCount = Environment.TickCount;
                    if (curTickCount - lastReconnectTime > logConfig.ReconnectTiming)
                    {
                        curTickCount = lastReconnectTime;
                        if (connection.Connect())
                        {
                            foreach (CMSLogEvent logEvent in curLogEvents)
                                SendLog(logEvent);
                            return;
                        }
                    }
                }

                fileLogger.WriteLogEvents(curLogEvents);
            }
        }

        public void Stop()  
        {
            if (isRunning)
                LoggerStatusChange();
            isRunning = false;
            if(fileLogger!=null)
                fileLogger.Close();
            if(logEventProcessorTimer != null)
                logEventProcessorTimer.Dispose();
            if (fileProcessorTimer != null)
                fileProcessorTimer.Dispose();
            monitorLogEvents.Clear();
            logEvents.Clear();
        }

        private void UpdateTimingProfile()
        {
            CMSLogBehavior profile = logConfig.LoggingBehavior;
            bool sendPCMessages = logConfig.EnablePCMessages;

            timingProfile.Clear();
            if (profile.Equals(CMSLogBehavior.NoLogging))
            {
            }
            else if (profile.Equals(CMSLogBehavior.OnlyInfrequentLogging))
            {
                timingProfile.UnConnectedTimings[new CMSLogAtt(false, false, false)] = 0;
                timingProfile.ConnectedTimings[new CMSLogAtt(false, false, false)] = 0;

                if (sendPCMessages)
                {
                    timingProfile.UnConnectedTimings[new CMSLogAtt(false, false, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(false, false, true)] = 15000;
                }
            }
            else if (profile.Equals(CMSLogBehavior.AllSmallMessages))
            {
                timingProfile.UnConnectedTimings[new CMSLogAtt(false, false, false)] = 0;
                timingProfile.UnConnectedTimings[new CMSLogAtt(true, false, false)] = 250;
                timingProfile.ConnectedTimings[new CMSLogAtt(false, false, false)] = 0;
                timingProfile.ConnectedTimings[new CMSLogAtt(true, false, false)] = 100;

                if (sendPCMessages)
                {
                    timingProfile.UnConnectedTimings[new CMSLogAtt(false, false, true)] = 15000;
                    timingProfile.UnConnectedTimings[new CMSLogAtt(true, false, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(false, false, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(true, false, true)] = 15000;
                }
            }
            else if (profile.Equals(CMSLogBehavior.FullLogging))
            {
                timingProfile.UnConnectedTimings[new CMSLogAtt(false, false, false)] = 0;
                timingProfile.UnConnectedTimings[new CMSLogAtt(false, true, false)] = 0;
                timingProfile.UnConnectedTimings[new CMSLogAtt(true, false, false)] = 250;
                timingProfile.ConnectedTimings[new CMSLogAtt(false, false, false)] = 0;
                timingProfile.ConnectedTimings[new CMSLogAtt(false, true, false)] = 0;
                timingProfile.ConnectedTimings[new CMSLogAtt(true, false, false)] = 100;
                timingProfile.ConnectedTimings[new CMSLogAtt(true, true, false)] = 30000;

                if (sendPCMessages)
                {
                    timingProfile.UnConnectedTimings[new CMSLogAtt(false, false, true)] = 15000;
                    timingProfile.UnConnectedTimings[new CMSLogAtt(false, true, true)] = 15000;
                    timingProfile.UnConnectedTimings[new CMSLogAtt(true, false, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(false, false, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(false, true, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(true, false, true)] = 15000;
                    timingProfile.ConnectedTimings[new CMSLogAtt(true, true, true)] = 30000;
                }
            }
        }
        public void SendLog(CMSLogEvent logEvent)
        {
            lock (logMutex)
            {
                long tickCount = Environment.TickCount;
                if (tickCount - lastDateTimeStampSendTime > dateTimeStampWaitPeriodMillis)
                {
                    lastDateTimeStampSendTime = tickCount;
                    logEvent.SetDateTime(DateTime.Now);
                }
                logEvent.SessionNum = logConfig.SessionNum;
                logEvent.TimeInMillis = tickCount - startTimeInMillis;
                logEvent.Uid = logConfig.Uid;
                logEvent.LogId = ++logUid;
                logEvents.AddLast(logEvent);
            }
        }
        private CMSLogAtt cachedAtt = new CMSLogAtt();
        public bool CanGetNewLog(bool frequent, bool large, bool pcConcerns, string logName)
        {
            cachedAtt.Frequent = frequent;
            cachedAtt.Large = large;
            cachedAtt.PrivacyConcerns = pcConcerns;
            long curTime = Environment.TickCount;

            return timingProfile.Send(cachedAtt, logName, curTime, IsConnected || neverConnect);                
        }
        public bool CanGetNewLog(Type logEventType)
        {
            lock (logMutex)
            {
                long curTime = Environment.TickCount;
                Attribute[] attrs = System.Attribute.GetCustomAttributes(logEventType);

                CMSLogAtt attr = null;
                foreach (Attribute curAttr in attrs)
                {
                    if (curAttr is CMSLogAtt)
                    {
                        attr = curAttr as CMSLogAtt;
                        break;
                    }
                }

                return timingProfile.Send(attr, logEventType.ToString(), curTime, IsConnected || neverConnect);
            }
        }
        public CMSLogEvent GetNewLog(Type logEventType)
        {
            lock (logMutex)
            {
                long curTime = Environment.TickCount;
                Attribute[] attrs = System.Attribute.GetCustomAttributes(logEventType);

                CMSLogAtt attr = null;
                foreach (Attribute curAttr in attrs)
                {
                    if (curAttr is CMSLogAtt)
                    {
                        attr = curAttr as CMSLogAtt;
                        break; 
                    }
                }

                if (!timingProfile.Send(attr, logEventType.ToString(), curTime, IsConnected || neverConnect))
                    return null;

                CMSLogEvent logEvent = Activator.CreateInstance(logEventType) as CMSLogEvent;
                /*
                if (includeDateTimeStamps)
                    logEvent.SetDateTime(DateTime.Now);
                logEvent.SessionNum = logConfig.SessionNum;
                logEvent.TimeInMillis = curTime - startTimeInMillis;
                logEvent.Uid = logConfig.Uid;
                logEvent.LogId = ++logUid;
                */
                
                return logEvent;
            }
        }

        private void UpdateConnection()
        {
            if (connection != null)
            {
                connection.EndPoint = logConfig.LogServer;
                connection.IdConfig = idConfig;
                connection.ProxyServer = logConfig.CreateProxy();

                if (IsRunning && !IsConnected && !neverConnect)
                {
                    if (connection.Connect())
                    {
                        logConfig.Uid = connection.Uid;
                        if (uidReceived != null)
                            uidReceived(logConfig.Uid);
                    }
                }
            }  
        }
        public void Update()
        {
            UpdateTimingProfile();
            UpdateConnection();

            if ((!logConfig.LoggingBehavior.Equals(CMSLogBehavior.NoLogging)) && ! IsRunning)
            {
                Start();
            }
            else if (logConfig.LoggingBehavior.Equals(CMSLogBehavior.NoLogging) && IsRunning)
            {
                Stop();
            }
        }

        private void LoggerStatusChange()
        {
            if (loggerStatusChange != null)
                loggerStatusChange();
        }

        #endregion

        #region Static

        private static CMSLogger logger = null;

        private static UidReceived uidReceived = null;

        private static event LoggerStatusChange loggerStatusChange;

        public static void AddLoggerStatusChangeListener(LoggerStatusChange lsc)
        {
            loggerStatusChange += lsc;
        }

        public static void SetUidReceivedDelegate(UidReceived ur)
        {
            uidReceived = ur;
        }

        public static bool IsLoggerRunning()
        {
            return logger.IsRunning;
        }

        public static bool IsLoggerConnected()
        {
            return logger.IsConnected;
        }

        public static Exception GetNetworkException()
        {
            return logger.NetworkException;
        }

        public static void UpdateLogger()
        {
            logger.Update();                        
        }

        public static void Init(CMSLogConfig logConfig, CMSIdentificationConfig idConfig)
        {
            logger = new CMSLogger();
            logger.LogConfig = logConfig;
            logger.IdConfig = idConfig;
            System.Collections.IDictionary environment = Environment.GetEnvironmentVariables();
            if (environment.Contains("DateTimeSendPeriod"))
            {
                long sendPeriod = Int32.Parse(Environment.GetEnvironmentVariables()["DateTimeSendPeriod"] as string);
                logger.DateTimeStampWaitPeriodMillis = sendPeriod;                
            }
            if (environment.Contains("NeverConnect"))
            {
                bool neverConnect = Boolean.Parse(Environment.GetEnvironmentVariables()["NeverConnect"] as string);
                logger.NeverConnect = neverConnect;
            }
            logger.Update();
        }


        public static bool CanCreateLogEvent(bool frequent, bool large, bool pcConcerns, string logName)
        {
            if (logger != null && logger.IsRunning)
                return logger.CanGetNewLog(frequent, large, pcConcerns, logName);
            return false;
        }

        public static bool CanCreateLogEvent(Type logType)
        {
            if (logger != null && logger.IsRunning)
                return logger.CanGetNewLog(logType);
            return false;
        }

        /*
        public static CMSLogEvent GetNewLogEvent(Type logEventType)
        {
            if (!CanCreateLogEvent(logEventType))
                return null;

            return Activator.CreateInstance(logEventType)as CMSLogEvent;
        }
        */

        public static void SendLogEvent(CMSLogEvent logEvent)
        {
            if (logger != null && logger.IsRunning)
            {

                logger.SendLog(logEvent);
            }
        }

        public static void StopLogging()
        {
            if (logger != null)
                logger.Stop();
        }
        #endregion
    }

    class CMSFileLogger
    {
        private string directory = null;
        public string Directory
        {
            get
            {
                return directory;
            }
            set
            {
                directory = value;
            }
        }

        private CMSIndividualFileLogger individualFileLogger = null;

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

        private int mbLimitLogFile = 25;
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

        private object mutex = new object();

        private bool isOpen = true;

        public void Close()
        {
            lock (mutex)
            {
                if (!isOpen)
                    return;

                try
                {
                    if (individualFileLogger != null)
                    {
                        individualFileLogger.Close();
                        individualFileLogger = null;
                    }
                }
                catch
                {
                }

                isOpen = false;
            }
        }

        public bool WriteLogEvents(LinkedList<object> logEvents)
        {
            lock (mutex)
            {
                if (!isOpen)
                    return false;

                if (individualFileLogger == null)
                {
                    if (!CreateNewIndividualLogger())
                    {
                        isOpen = false;
                        return false;
                    }
                }

                bool isFinished = false;
                bool isOkay = individualFileLogger.WriteLogEvents(logEvents, out isFinished);

                if (!isOkay)
                {
                    isOpen = false;
                    return false;
                }

                if (isFinished)
                    individualFileLogger = null;

                return true;
            }
        }

        private bool CreateNewIndividualLogger()
        {
            try
            {
                long length = 0;
                DirectoryInfo di = new DirectoryInfo(directory);

                FileInfo oldestFile = null;
                long fileSmallestNum = Int64.MaxValue;
                long fileLargestNum = Int64.MinValue;
                bool foundFile = false;

                foreach (FileInfo fi in di.GetFiles())
                {
                    length += fi.Length;
                    string name = fi.Name;
                    bool deleteable = name.StartsWith("x");
                    int periodIndex = name.IndexOf('.');
                    foundFile |= deleteable;
                    long serialNum = Int64.Parse(name.Substring(1, periodIndex - 1));
                    if (serialNum < fileSmallestNum && deleteable)
                    {
                        fileSmallestNum = serialNum;
                        oldestFile = fi;
                    }
                    if (serialNum > fileLargestNum)
                    {
                        fileLargestNum = serialNum;
                    }
                }

                if (length > (long)mbLimitLogDirectory * (long)1000000)
                    File.Delete(oldestFile.FullName);

                if (!foundFile)
                    fileLargestNum = 0;

                individualFileLogger = new CMSIndividualFileLogger();
                string file = directory + "/x" + (fileLargestNum + 1) + ".gz";
                string tempFile = directory + "/t" + (fileLargestNum + 1) + ".gz";
                if (!individualFileLogger.Init(mbLimitLogFile * 2000000, file, tempFile))
                    return false;
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
    
    class CMSIndividualFileLogger
    {
        public CMSIndividualFileLogger() { }

        private object mutex = new object();
        private GZipStream fileStream = null;

        private bool isOpen = false;
        private long curCharacterCount = 0;
        private string filename = null;
        private string tempFilename = null;

        public bool Init(long characterCount, string filename, string tempFilename)
        {
            lock (mutex)
            {
                try
                {
                    this.tempFilename = tempFilename;
                    this.filename = filename;
                    this.characterCount = characterCount;
                    fileStream = new GZipStream(new FileStream(tempFilename, FileMode.Create), CompressionMode.Compress);

                    isOpen = true;
                    string startString = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<CMS><Ms>\n");
                    byte[] start = System.Text.ASCIIEncoding.ASCII.GetBytes(startString);
                    fileStream.Write(start, 0, start.Length);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool Close()
        {
            lock (mutex)
            {
                try
                {
                    if (!isOpen)
                        return false;

                    isOpen = false;
                    string endString = ("</Ms></CMS>");
                    byte[] end = System.Text.ASCIIEncoding.ASCII.GetBytes(endString);
                    fileStream.Write(end, 0, end.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    File.Move(tempFilename, filename);

                }
                catch
                {
                    return false;
                }
                return true;
                
            }
        }

        public bool WriteLogEvents(LinkedList<object> logEvents, out bool finished)
        {
            lock (mutex)
            {
                finished = false;
                try
                {
                    if (!isOpen)
                        return false;

                    foreach (object o in logEvents)
                    {
                        if (o is CMSLogEvent)
                        {
                            string MStart = ("<M>");
                            byte[] MStartBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(MStart);
                            fileStream.Write(MStartBytes,0, MStartBytes.Length);

                            CMSLogEvent logEvent = (CMSLogEvent)o;
                            byte[] toWrite = logEvent.ToXmlBytes();
                            fileStream.Write(toWrite, 0, toWrite.Length);
                            curCharacterCount += toWrite.Length;

                            string MEnd = ("</M>");
                            byte[] MEndBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(MEnd);
                            fileStream.Write(MEndBytes, 0, MEndBytes.Length);
                        }
                    }
                    fileStream.Flush();

                    if (curCharacterCount < characterCount)
                        return true;

                    finished = true;

                    return Close();
                }
                catch(Exception e)
                {

                    try
                    {
                        Close();
                    }
                    catch
                    {
                    }

                    return false;
                }
            }
        }

        private long characterCount = 0;

    }

    public enum CMSLogBehavior
    {
        NoLogging,
        OnlyInfrequentLogging,
        AllSmallMessages,
        FullLogging
    }

    class CMSLogTimingProfile
    {
        protected SortedList<CMSLogAtt, long> unConnectedTimings = new SortedList<CMSLogAtt, long>();
        protected SortedList<CMSLogAtt, long> connectedTimings = new SortedList<CMSLogAtt, long>();

        public SortedList<CMSLogAtt, long> UnConnectedTimings 
        {
            get
            {
                return unConnectedTimings;
            }
        }
        public SortedList<CMSLogAtt, long> ConnectedTimings
        {
            get
            {
                return connectedTimings;
            }
        }

        protected SortedList<string, long> lastTiming = new SortedList<string, long>();
        public bool Send(CMSLogAtt att, string typeName, long time, bool connected)
        {
            SortedList<CMSLogAtt, long> timings = null;
            if (connected)
                timings = connectedTimings;
            else
                timings = unConnectedTimings;

            if (!timings.ContainsKey(att))
                return false;

            long dif = timings[att];
            if (dif == 0)
                return true;

            if (lastTiming.ContainsKey(typeName))
            {                
                long oldTime = lastTiming[typeName];                
                if (time - oldTime < dif)
                    return false;
                lastTiming[typeName] = time;
                return true;
            }
            else
            {
                lastTiming[typeName] = time;
                return true;
            }
        }

        public void Clear()
        {
            unConnectedTimings.Clear();
            connectedTimings.Clear();
        }
    }

}


