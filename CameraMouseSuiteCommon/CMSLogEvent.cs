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
using System.Xml;
using System.IO;

namespace CameraMouseSuite
{

    
    public abstract class CMSLogEvent
    {
        private long timeInMillis = 0;        
        [XmlElement("T")]
        public long TimeInMillis
        {
            get
            {
                return timeInMillis;
            }
            set
            {
                timeInMillis = value;
            }
        }

        private int sessionNum = 0;
        [XmlElement("S")]
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

        private long uid = 0;
        [XmlElement("I")]
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

        private long logId = 0;
        [XmlElement("L")]
        
        public long LogId
        {
            get
            {
                return logId;
            }
            set
            {
                logId = value;
            }
        }

        private CMSSerializableDate dateTime = null;
        [XmlElement("D")]
        public CMSSerializableDate DateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                dateTime = value;
            }
        }

        public void SetDateTime(DateTime dt)
        {
            dateTime = new CMSSerializableDate(dt);
        }

        public XmlElement ToXml()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer xmSer = new XmlSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            xmSer.Serialize(ms, this, ns);
            XmlDocument xDoc = new XmlDocument();
            ms.Position = 0;
            xDoc.Load(ms);
            XmlElement xml = xDoc.LastChild as XmlElement;
            ms.Close();
            return xml;
        }

        public byte[] ToXmlBytes()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer xmSer = new XmlSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            xmSer.Serialize(ms, this, ns);
            long length = ms.Length;
            int offset = 23;
            ms.Position = offset;
            byte[] bytes = new byte[length - offset];
            ms.Read(bytes, 0, (int)(length - offset));            
            ms.Close();
            return bytes;
        }

        public string ToXmlString()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer xmSer = new XmlSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            xmSer.Serialize(ms, this, ns);
            ms.Position = 38;
            byte[] bytes = new byte[ms.Length - 38];
            ms.Read(bytes, 38, (int)(ms.Length - 38));
            return System.Text.ASCIIEncoding.ASCII.GetString(bytes);
        }

    }

}
