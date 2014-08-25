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
using System.Xml;

namespace CameraMouseSuite
{
    public class CMSLogConnection
    {
        private CMSRestClient restClient = new CMSRestClient();

        private string endPoint = null;
        public string EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                endPoint = value;
            }
        }

        private WebProxy proxyServer = null;
        public WebProxy ProxyServer
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

        private bool isConnected = false;
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
        }

        private Exception exception = null;
        public Exception NetworkException
        {
            get
            {
                return exception;
            }
        }

        public bool Connect()
        {
            if (uid == 0)
            {
                CMSSynMessage synMessage = new CMSSynMessage();
                synMessage.Id = idConfig;
                synMessage.Uid = 0;

                try
                {
                    if (endPoint == null || endPoint.Length == 0)
                        throw new Exception("Server Url is empty");

                    CMSSynAckMessage synAckMessage = restClient.SendRequest(endPoint, synMessage, typeof(CMSSynAckMessage)) as CMSSynAckMessage;
                    if (synAckMessage == null)
                    {
                        isConnected = false;
                        if(loggerStatusChange != null)
                            loggerStatusChange();
                        return false;
                    }
                    this.uid = synAckMessage.Uid;
                    isConnected = true;
                    exception = null;
                    if (loggerStatusChange != null)
                        loggerStatusChange();
                    return true;
                }
                catch (Exception e)
                {
                    isConnected = false;
                    exception = e;
                    if (loggerStatusChange != null)
                        loggerStatusChange();
                    return false;
                }
            }
            else
            {
                CMSSynMessage synMessage = new CMSSynMessage();
                synMessage.Id = idConfig;
                synMessage.Uid = uid;

                try
                {
                    CMSSynAckMessage synAckMessage = restClient.SendRequest(endPoint, synMessage, typeof(CMSSynAckMessage)) as CMSSynAckMessage;
                    if (synMessage == null)
                    {
                        isConnected = false;
                        if (loggerStatusChange != null)
                            loggerStatusChange();
                        return false;
                    }
                    uid = synAckMessage.Uid;
                    isConnected = true;
                    exception = null;
                    if (loggerStatusChange != null)
                        loggerStatusChange();
                    return true;
                }
                catch (Exception e)
                {
                    isConnected = false;
                    exception = e;
                    if (loggerStatusChange != null)
                        loggerStatusChange();
                    return false;
                }
            }
        }

        private event LoggerStatusChange loggerStatusChange;
        public event LoggerStatusChange LoggerStatusChange
        {
            add
            {
                loggerStatusChange += value;
            }
            remove
            {
                loggerStatusChange -= value;
            }
        }
        
        private object mutex = new object();
        

        /*
        private Queue<CMSLogEvent> logEvents = new Queue<CMSLogEvent>();
   
        public void AddLogEvent(CMSLogEvent logEvent)
        {
            lock (mutex)
            {
                if (logEvents.Count >= maxNumLogEvents)
                    logEvents.Dequeue();
                logEvents.Enqueue(logEvent);
            }
        }
       */
        
        public bool SendLogEvents(LinkedList<object> logEvents)
        {
            if (!isConnected)
                return false;

            if (uid == 0)
                return true;

            CMSLogMessage message = new CMSLogMessage();
            message.Uid = uid;


            object [] les = null;
            lock (mutex)
            {
                les = logEvents.ToArray();
                logEvents.Clear();
            }
            message.Messages = new System.Xml.XmlElement[les.Length];
            for (int i = 0; i < les.Length; i++)
            {
                if (les[i] is CMSLogEvent)
                {
                    CMSLogEvent curLogEvent = les[i] as CMSLogEvent;
                    if (curLogEvent.Uid == 0)
                        curLogEvent.Uid = uid;
                    message.Messages[i] = curLogEvent.ToXml();
                }
                else if (les[i] is XmlElement)
                    message.Messages[i] = (XmlElement)les[i];
            }
            
            try
            {
                CMSAckMessage ackMessage = restClient.SendRequest(endPoint, message, typeof(CMSAckMessage), proxyServer) as CMSAckMessage;
                if (ackMessage == null)
                {
                    exception = new Exception("No Response from server");
                    isConnected = false;
                    return false;
                }
                if(ackMessage.Uid != uid)
                {
                    exception = new Exception("Server responded with different user id");
                    isConnected = false;
                    return false;
                }
                return true;
            }
            catch(Exception e)
            {
                exception = e;
                isConnected = false;
                return false;
            }
        }

    }
}
