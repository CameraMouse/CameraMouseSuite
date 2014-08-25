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
using System.Xml;
using System.Xml.Serialization;

namespace CameraMouseSuite
{
    [XmlRoot("CMS")]
    public class CMSLogMessage
    {
        private long uid = 0;
        [XmlElement("Uid")]
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

        private XmlElement [] messages = null;
        [XmlArray("Msg")]
        [XmlArrayItem("Mg")]
        public XmlElement [] Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
            }
        }
    }

    [XmlRoot("CMSSyn")]
    public class CMSSynMessage
    {
        private CMSIdentificationConfig id = null;
        [XmlElement("Info")]
        public CMSIdentificationConfig Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private long uid = 0;        
        [XmlElement("Uid")]
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
    }

    [XmlRoot("CMSError")]
    public class CMSErrorMessage
    {
        private string error = null;
        [XmlElement("Message")]
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }
    }
    
    [XmlRoot("CMSSynAck")]
    public class CMSSynAckMessage
    {
        private long uid = 0;
        [XmlElement("Uid")]
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
    }

    [XmlRoot("CMSAck")]
    public class CMSAckMessage
    {
        private long uid = 0;
        [XmlElement("Uid")]
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
    }

}
