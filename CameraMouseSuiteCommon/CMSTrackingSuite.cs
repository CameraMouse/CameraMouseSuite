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
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public delegate void SendMessage(string message);
    public delegate void SendMessages(Bitmap[] bitmaps, string[] messages);

    public abstract class CMSTrackingSuite 
    {
        protected object mutex = new object();
        protected string name = null;
        protected string description = null;
        protected string informalName = null;

        protected CMSTrackingModule trackingModule = null;
        protected CMSMouseControlModule mouseControlModule = null;
        protected CMSClickControlModule clickControlModule = null;

        protected bool initialized = false;
        
        public bool Initialized
        {
            get
            {
                lock (mutex)
                {
                    return initialized;
                }
            }
        }

        [XmlIgnore()]
        public string Name
        {
            get
            {
                lock (mutex)
                {
                    return name;
                }
            }            
        }

        [XmlIgnore()]
        public string Description
        {
            get
            {
                return description;
            }
        }

        [XmlIgnore()]
        public string InformalName
        {
            get
            {
                return informalName;
            }
        }

        [XmlIgnore()]
        public CMSTrackingModule TrackingModule
        {
            get
            {
                return trackingModule;
            }
            set
            {
                trackingModule = value;
            }
        }

        [XmlIgnore()]
        public CMSMouseControlModule MouseControlModule
        {
            get
            {
                return mouseControlModule;
            }
            set
            {
                mouseControlModule = value;
            }
        }

        [XmlIgnore()]
        public CMSClickControlModule ClickControlModule
        {
            get
            {
                return clickControlModule;
            }
            set
            {
                clickControlModule = value;
            }
        }

        public CMSTrackingSuiteIdentifier GetIdentifier()
        {
            return new CMSTrackingSuiteIdentifier(name,description,informalName);
        }

        public CMSConfigPanel[] GetPanels()
        {
            CMSConfigPanel[] panels = new CMSConfigPanel[3];

            if(trackingModule!=null)
                panels[0] = this.trackingModule.getPanel();
            if(mouseControlModule!=null)
                panels[1] = this.mouseControlModule.getPanel();
            if(clickControlModule!=null)
                panels[2] = this.clickControlModule.getPanel();
            
            return panels;
        }

        public void Init(Size [] imageSizes)
        {
            
            lock (mutex)
            {
                if (trackingModule != null)
                {
                    trackingModule.State = CMSState.Setup;
                    trackingModule.Init(imageSizes);
                }

                if (mouseControlModule != null)
                {
                    mouseControlModule.State = CMSState.Setup;
                    mouseControlModule.Init(imageSizes);
                }

                if (clickControlModule != null)
                {
                    clickControlModule.State = CMSState.Setup;
                    clickControlModule.Init(imageSizes);
                }
                initialized = true;
            }
        }

        public void ProcessMouse(Point p, bool leftMouseButton, int cameraNum)
        {
            lock (mutex)
            {
                if(trackingModule != null)
                    trackingModule.ProcessMouse(p, leftMouseButton, cameraNum);
            }
        }

        public void ProcessKeys(Keys keys)
        {
            if (trackingModule != null)
                trackingModule.ProcessKeys(keys);

            if (mouseControlModule != null)
                mouseControlModule.ProcessKeys(keys);

            if (clickControlModule != null)
                clickControlModule.ProcessKeys(keys);
        }

         
        public void ProcessFrame(Bitmap [] frames, bool control)
        {
            lock (mutex)
            {
                PointF imagePoint = PointF.Empty;
                PointF screenPoint = PointF.Empty;
                CMSExtraTrackingInfo extraInfo = null;

                if (trackingModule != null)
                {
                    trackingModule.Process(frames);
                    imagePoint = trackingModule.ImagePoint;
                    extraInfo = trackingModule.ExtraTrackingInfo;
                    if (!trackingModule.ImagePoint.IsEmpty)
                    {
                        if (CMSLogger.CanCreateLogEvent(true, false, false, "CMSLogPointerEvent"))
                        {
                            CMSLogPointerEvent ptrEvent = new CMSLogPointerEvent();
                            if (ptrEvent != null)
                            {
                                ptrEvent.X = (int)trackingModule.ImagePoint.X;
                                ptrEvent.Y = (int)trackingModule.ImagePoint.Y;
                                CMSLogger.SendLogEvent(ptrEvent);
                            }
                        }
                    }
                }

                if (mouseControlModule != null)
                {
                    mouseControlModule.ProcessMouse(imagePoint,
                                                        extraInfo,
                                                        frames);
                    screenPoint = mouseControlModule.MousePointer;
                }

                if (clickControlModule != null)
                {
                    clickControlModule.ProcessClick(imagePoint,
                                                    screenPoint,
                                                    extraInfo,
                                                    frames);
                }

                if(trackingModule!=null)
                    trackingModule.DrawOnFrame(frames);
                if(mouseControlModule != null)
                    mouseControlModule.DrawOnFrame(frames);

                if(clickControlModule != null)
                    clickControlModule.DrawOnFrame(frames);


                if (control)
                {
                    if (CMSLogger.CanCreateLogEvent(true, false, false, "CMSLogCursorEvent"))
                    {
                        CMSLogCursorEvent csrEvent = new CMSLogCursorEvent();

                        if (csrEvent != null)
                        {
                            csrEvent.X = (int)mouseControlModule.MousePointer.X;
                            csrEvent.Y = (int)mouseControlModule.MousePointer.Y;
                            CMSLogger.SendLogEvent(csrEvent);
                        }
                    }
                }
            }
        }

        public void StateChange(CMSState state)
        {
            lock (mutex)
            {
                if(state.Equals(CMSState.Tracking) ||
                    state.Equals(CMSState.ControlTracking))
                    CMSTrackingSuiteAdapter.SendMessage("");

                if (trackingModule != null)
                {
                    trackingModule.State = state;
                    trackingModule.StateChange(state);
                }

                if (mouseControlModule != null)
                {
                    mouseControlModule.State = state;
                    mouseControlModule.StateChange(state);

                }

                if (clickControlModule != null)
                {
                    clickControlModule.State = state;
                    clickControlModule.StateChange(state);
                }
            }
        }

        [XmlIgnore()]
        public CMSTrackingSuiteAdapter CMSTrackingSuiteAdapter
        {
            get
            {
                if (trackingModule != null)
                    return this.trackingModule.CMSTrackingSuiteAdapter;
                else
                    return null;
            }
            set
            {
                if(trackingModule!=null)
                    this.trackingModule.CMSTrackingSuiteAdapter = value;

                if(mouseControlModule!=null)
                    this.mouseControlModule.CMSTrackingSuiteAdapter = value;

                if(clickControlModule!=null)
                    this.clickControlModule.CMSTrackingSuiteAdapter = value;
            }
        }

        //public abstract CMSTrackingSuite Clone();

        public void Update(CMSTrackingSuite newSuite)
        {
            lock (mutex)
            {
                if(trackingModule!=null)
                    this.trackingModule.Update(newSuite.TrackingModule);
                if(mouseControlModule!=null)
                    this.mouseControlModule.Update(newSuite.MouseControlModule);
                if(clickControlModule!=null)
                    this.clickControlModule.Update(newSuite.ClickControlModule);
            }
        }

        public void Clean()
        {
            initialized = false;
            if (trackingModule != null)
                trackingModule.Clean();
            if (mouseControlModule != null)
                mouseControlModule.Clean();
            if (clickControlModule != null)
                clickControlModule.Clean();
        }

        public abstract void SendSuiteLogEvent();
    }
}
