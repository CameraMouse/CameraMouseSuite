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
using System.Xml.Serialization;
using System.IO;

namespace CameraMouseSuite
{
    public class CMSRestClient
    {

        private XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

        public CMSRestClient()
        {
            ns.Add("", "");

        }

        private HttpWebRequest CreateRESTWebRequest(string endPoint, long contentLength)
        {
            HttpWebRequest request = WebRequest.Create(endPoint) as HttpWebRequest;
            request.Method = "POST";
            request.ContentLength = contentLength;
            request.ContentType = "text/xml";
            request.ProtocolVersion = HttpVersion.Version11;            
            return request;
        }

        public object SendRequest(string endPoint, object message, Type outputType)
        {
            return SendRequest(endPoint, message, outputType, null);
        }

        public object SendRequest(string endPoint, object message, Type outputType, IWebProxy proxy)
        {
            HttpWebRequest req =null;


            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xmSer = new XmlSerializer(message.GetType());
                xmSer.Serialize(ms, message, ns);

                req = CreateRESTWebRequest(endPoint,ms.Length);
                
                if (proxy != null)
                    req.Proxy = proxy;

                using (Stream requestStream = req.GetRequestStream())
                {
                    ms.WriteTo(requestStream);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Get failed. Received HTTP " + response.StatusCode);

                using (Stream responseStream = response.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //using (StreamReader sr = new StreamReader(responseStream))
                        //{
                        const int size = 4096;
                        byte[] bytes = new byte[4096];
                        int numBytes;
                        while ((numBytes = responseStream.Read(bytes, 0, size)) > 0)
                        {
                            ms.Write(bytes, 0, numBytes);
                        }
                        //while (sr.Peek() >= 0)
                        //{
                        //ms.WriteByte((byte)sr.Read());
                        //}
                        //}

                        try
                        {
                            ms.Position = 0;
                            XmlSerializer xmSer = new XmlSerializer(outputType);
                            return xmSer.Deserialize(ms);
                        }
                        catch
                        {
                            ms.Position = 0;
                            XmlSerializer xmSer = new XmlSerializer(typeof(CMSErrorMessage));
                            CMSErrorMessage errorMessage = xmSer.Deserialize(ms) as CMSErrorMessage;
                            throw new Exception(errorMessage.Error);
                        }

                    }
                }
            }
        }

    }


}
