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
using System.Collections;
using System.Text;
using System.Threading;
using System.Drawing;

namespace CameraMouseSuite
{
    class SafeMessagesPass
    {
        public SafeMessagesPass()
        {

            _newItemEvent = new AutoResetEvent(false);
            _exitThreadEvent = new ManualResetEvent(false);
            _eventArray = new WaitHandle[2];
            _eventArray[0] = _newItemEvent;
            _eventArray[1] = _exitThreadEvent;
        }

        public EventWaitHandle ExitThreadEvent
        {
            get { return _exitThreadEvent; }
        }
        public EventWaitHandle NewItemEvent
        {
            get { return _newItemEvent; }
        }
        public WaitHandle[] EventArray
        {
            get { return _eventArray; }
        }

        private EventWaitHandle _newItemEvent;
        private EventWaitHandle _exitThreadEvent;
        private WaitHandle[] _eventArray;

        private Bitmap[] bitmaps = null;
        private string[] messages = null;
        private object mutex = new object();
     
        public void SetKill()
        {
            lock (mutex)
            {
                bitmaps = null;
                messages = null;
            }

            ExitThreadEvent.Set();
        }
        public void SetMessages(Bitmap[] bitmaps, string[] messages)
        {
            lock (mutex)
            {
                this.bitmaps = bitmaps;
                this.messages = messages;
                NewItemEvent.Set();
            }
        }
        public void GetMessages(out Bitmap[] bitmaps, out string[] messages)
        {
            WaitHandle.WaitAny(EventArray);
            lock (mutex)
            {
                bitmaps = this.bitmaps;
                messages = this.messages;
            }
        }

    }

    class SafeMessagePass
    {
        public SafeMessagePass()
        {

            _newItemEvent = new AutoResetEvent(false);
            _exitThreadEvent = new ManualResetEvent(false);
            _eventArray = new WaitHandle[2];
            _eventArray[0] = _newItemEvent;
            _eventArray[1] = _exitThreadEvent;
        }

        public EventWaitHandle ExitThreadEvent
        {
            get { return _exitThreadEvent; }
        }
        public EventWaitHandle NewItemEvent
        {
            get { return _newItemEvent; }
        }
        public WaitHandle[] EventArray
        {
            get { return _eventArray; }
        }

        private EventWaitHandle _newItemEvent;
        private EventWaitHandle _exitThreadEvent;
        private WaitHandle[] _eventArray;

        private Color color;
        private string message = null;
        private object mutex = new object();
        int control = 0;

        public void SetKill()
        {
            ExitThreadEvent.Set();        
           
        }

        public void SetMessage(string message, Color color, int control)
        {
            lock (mutex)
            {
                this.message = message;
                this.color = color;
                this.control = control;
                NewItemEvent.Set();
            }
        }
        
        public void GetMessage(out Color color, out string message, out int control)
        {
            WaitHandle.WaitAny(EventArray);
            lock (mutex)
            {
                message = this.message;
                color = this.color;
                control = this.control;
            }
        }

    }
}
