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
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using DirectShowLib;
using System.Windows.Forms;


//using videosource;

namespace CameraMouseSuite
{
    
    public class WebCam
    {
        //A (modified) definition of OleCreatePropertyFrame found here: http://groups.google.no/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/db794e9779144a46/55dbed2bab4cd772?lnk=st&q=[DllImport(%22olepro32.dll%22)]&rnum=1&hl=no#55dbed2bab4cd772
        [DllImport(@"oleaut32.dll")]
        public static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner,
            int x,
            int y,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            int cObjects,
            [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] 
			ref object ppUnk,
            int cPages,
            IntPtr lpPageClsID,
            int lcid,
            int dwReserved,
            IntPtr lpvReserved);

        private CaptureDevice cd;
        private bool started = false;
        private bool initialized = false;

        private dshow.Filter _filter;

        private System.Drawing.Bitmap lastFrame;
        public System.Drawing.Bitmap LastFrame
        {
            get { return lastFrame; }
        }

        // new frame event
        public event WebCamEventHandler NewFrame;

        // This event should be raised when the video input size of the capture
        // device is determined.
        public event VideoInputSizeDetermined CaptureDeviceVideoInputSizeDetermined = null;

        private static string[] KnownBadCams
        {
            get
            {
                ArrayList badies = new ArrayList();
                badies.Add("Creative WebCam Instant (VFW)");
                badies.Add("Live! Cam Optia AF (VFW)");

                return (string[])badies.ToArray(typeof(string));
            }
        }


        public static WebCamDescription[] AvailableWebCamMonikers
        {
            get
            {
                dshow.FilterCollection filters;
                ArrayList cams = new ArrayList();
                ArrayList badies = new ArrayList(KnownBadCams);


                try
                {
                    filters = new dshow.FilterCollection(dshow.Core.FilterCategory.VideoInputDevice);
                    for (int i = 0; i < filters.Count; i++)
                    {
                        // add if it isn't a known problem camera filter
                        if (!badies.Contains(filters[i].Name))
                            cams.Add(new WebCamDescription(filters[i].Name, filters[i].MonikerString));
                    }

                }
                catch
                {
                    // vista seems to throw an excpetion when there are no VideoInputDevices
                    // instead of returning an empty filter collection like XP sp2
                }


                return (WebCamDescription[])cams.ToArray(typeof(WebCamDescription));
            }
        }


        // this should never be called 
        public WebCam()
        {
            //dshow.FilterCollection filters = new dshow.FilterCollection(dshow.Core.FilterCategory.VideoInputDevice);

            //if (filters.Count == 0)
            //{
            //    throw new Exception("No DirectShow filters available for video stream moniker (probably no webcam)");
            //}

            //cd = new CaptureDevice();

            //// why do i always pic 3rd filter?
            //cd.VideoSource = filters[2].MonikerString;
            //cd.NewFrame += new CameraEventHandler(cd_NewFrame);
        }



        Form _parent;

        public WebCam(string moniker, System.Windows.Forms.Form frm)
        {
            _filter = GetFilter(moniker);

            _parent = frm;

            cd = new CaptureDevice();
            cd.VideoSource = _filter.MonikerString;// moniker;
            cd.NewFrame += new CameraEventHandler(cd_NewFrame);
            cd.VideoInputSizeDetermined += new VideoInputSizeDetermined(OnCaptureDeviceVideoInputSizeDetermined);
        }

        private void OnCaptureDeviceVideoInputSizeDetermined(object sender, Size videoInputSize)
        {
            if (CaptureDeviceVideoInputSizeDetermined != null)
                CaptureDeviceVideoInputSizeDetermined(this, videoInputSize);
        }

        public static dshow.Filter GetFilter(string moniker)
        {
            dshow.Filter retval = null;

            dshow.FilterCollection filters = new dshow.FilterCollection(dshow.Core.FilterCategory.VideoInputDevice);

            foreach (dshow.Filter filt in filters)
            {
                if (moniker.CompareTo(filt.MonikerString) == 0)
                {
                    retval = filt;
                    break;
                }
            }

            return retval;
        }


        /// <summary>
        /// Enumerates all filters of the selected category and returns the IBaseFilter for the 
        /// filter described in friendlyname
        /// </summary>
        /// <param name="category">Category of the filter</param>
        /// <param name="friendlyname">Friendly name of the filter</param>
        /// <returns>IBaseFilter for the device</returns>
        private IBaseFilter CreateFilter(Guid category, string friendlyname)
        {
            object source = null;
            Guid iid = typeof(IBaseFilter).GUID;
            foreach (DsDevice device in DsDevice.GetDevicesOfCat(category))
            {
                if (device.Name.CompareTo(friendlyname) == 0)
                {
                    device.Mon.BindToObject(null, null, ref iid, out source);
                    break;
                }
            }

            return (IBaseFilter)source;
        }




        /// <summary>
        /// Displays a property page for a filter
        /// </summary>
        /// <param name="dev">The filter for which to display a property page</param
        /// 
        /// 
        /// >
        /// 

        IBaseFilter theDevice = null;

        public void DisplayPropertyPage()
        {

            if (theDevice != null)
            {
                Marshal.ReleaseComObject(theDevice);
                theDevice = null;
            }
            //Create the filter for the selected video input device
            string devicepath = _filter.Name; // cd.VideoSource;   //comboBox1.SelectedItem.ToString();
            theDevice = CreateFilter(FilterCategory.VideoInputDevice, devicepath);

            IBaseFilter dev = theDevice;

            //Get the ISpecifyPropertyPages for the filter
            ISpecifyPropertyPages pProp = dev as ISpecifyPropertyPages;
            int hr = 0;

            if (pProp == null)
            {
                //If the filter doesn't implement ISpecifyPropertyPages, try displaying IAMVfwCompressDialogs instead!
                IAMVfwCompressDialogs compressDialog = dev as IAMVfwCompressDialogs;
                if (compressDialog != null)
                {

                    hr = compressDialog.ShowDialog(VfwCompressDialogs.Config, IntPtr.Zero);
                    DsError.ThrowExceptionForHR(hr);
                }
                return;
            }

            //Get the name of the filter from the FilterInfo struct
            FilterInfo filterInfo;
            hr = dev.QueryFilterInfo(out filterInfo);
            DsError.ThrowExceptionForHR(hr);

            // Get the propertypages from the property bag
            DsCAUUID caGUID;
            hr = pProp.GetPages(out caGUID);
            DsError.ThrowExceptionForHR(hr);

            // Check for property pages on the output pin
            IPin pPin = DsFindPin.ByDirection(dev, PinDirection.Output, 0);
            ISpecifyPropertyPages pProp2 = pPin as ISpecifyPropertyPages;
            if (pProp2 != null)
            {
                DsCAUUID caGUID2;
                hr = pProp2.GetPages(out caGUID2);
                DsError.ThrowExceptionForHR(hr);

                if (caGUID2.cElems > 0)
                {
                    int soGuid = Marshal.SizeOf(typeof(Guid));

                    // Create a new buffer to hold all the GUIDs
                    IntPtr p1 = Marshal.AllocCoTaskMem((caGUID.cElems + caGUID2.cElems) * soGuid);

                    // Copy over the pages from the Filter
                    for (int x = 0; x < caGUID.cElems * soGuid; x++)
                    {
                        Marshal.WriteByte(p1, x, Marshal.ReadByte(caGUID.pElems, x));
                    }

                    // Add the pages from the pin
                    for (int x = 0; x < caGUID2.cElems * soGuid; x++)
                    {
                        Marshal.WriteByte(p1, x + (caGUID.cElems * soGuid), Marshal.ReadByte(caGUID2.pElems, x));
                    }

                    // Release the old memory
                    Marshal.FreeCoTaskMem(caGUID.pElems);
                    Marshal.FreeCoTaskMem(caGUID2.pElems);

                    // Reset caGUID to include both
                    caGUID.pElems = p1;
                    caGUID.cElems += caGUID2.cElems;
                }
            }

            // Create and display the OlePropertyFrame
            try
            {
                object oDevice = (object)dev;
                hr = OleCreatePropertyFrame(_parent.Handle, 10, 10, filterInfo.achName, 1, ref oDevice, caGUID.cElems, caGUID.pElems, 0, 0, IntPtr.Zero);
            }
            catch (Exception E)
            {
                //  DsError.ThrowExceptionForHR(hr);
            }

            // Release COM objects
            Marshal.FreeCoTaskMem(caGUID.pElems);
            Marshal.ReleaseComObject(pProp);
            if (filterInfo.pGraph != null)
            {
                Marshal.ReleaseComObject(filterInfo.pGraph);
            }
        }



        public void ShowPropertyPage()
        {
            dshow.FilterCollection filters = new dshow.FilterCollection(dshow.Core.FilterCategory.VideoInputDevice);
            //    dshow.Core.IBaseFilter dev = (dshow.Core.IBaseFilter)filters[2];

            //      ISpecifyPropertyPages pProp = dev as ISpecifyPropertyPages;
        }

        public bool Running
        {
            get
            {
                return cd.Running;
            }
        }

        public void Start()
        {
            if (!started)
            {
                cd.Start();
                started = true;
                initialized = true;
            }
        }

        public void Stop()
        {
            cd.Stop();
            started = false;
        }

        public static int CameraCount
        {
            get
            {
                return WebCam.AvailableWebCamMonikers.Length;
            }
        }

        private void cd_NewFrame(object sender, CameraEventArgs e)
        {
            lastFrame = e.Bitmap;

            if (NewFrame != null)
            {
                NewFrame(this, new WebCamEventArgs(e.Bitmap));
            }
        }
    }


    // NewFrame delegate
    public delegate void WebCamEventHandler(object sender, WebCamEventArgs e);

    /// <summary>
    /// Camera event arguments
    /// </summary>
    public class WebCamEventArgs : EventArgs
    {
        private System.Drawing.Bitmap bmp;

        // Constructor
        public WebCamEventArgs(System.Drawing.Bitmap bmp)
        {
            this.bmp = bmp;
        }

        // Bitmap property
        public System.Drawing.Bitmap Bitmap
        {
            get { return bmp; }
        }
    }

    public class WebCamDescription
    {
        private string name;
        public string Name
        {
            get { return name; }
        }

        private string moniker;
        public string Moniker
        {
            get { return moniker; }
        }

        public WebCamDescription(string name, string moniker)
        {
            this.name = name;
            this.moniker = moniker;
        }
    }
}
