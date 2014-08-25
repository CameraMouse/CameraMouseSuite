using System;

using System.Collections;

using System.Drawing;



//using videosource;



namespace CameraMouse

{

 

    public class WebCam

    {

        private CaptureDevice cd;

        private bool started = false;

        private bool initialized = false;





        private System.Drawing.Bitmap lastFrame;

        public System.Drawing.Bitmap LastFrame

        {

            get { return lastFrame; }

        }



        // new frame event

        public event WebCamEventHandler NewFrame;





        private static string[] KnownBadCams

        {

            get

            {

                ArrayList badies = new ArrayList();

                badies.Add("Creative WebCam Instant (VFW)");

                badies.Add("Live! Cam Optia AF (VFW)");



                return (string[]) badies.ToArray(typeof(string));

            }

        }





        public static WebCamDescription[] AvailableWebCamMonikers

        {

            get

            {

                dshow.FilterCollection filters = new dshow.FilterCollection(dshow.Core.FilterCategory.VideoInputDevice);



                ArrayList cams = new ArrayList();

                

                ArrayList badies = new ArrayList(KnownBadCams);



                for (int i = 0; i < filters.Count; i++)

                {

                    // add if it isn't a known problem camera filter

                    if (!badies.Contains(filters[i].Name))

                        cams.Add(new WebCamDescription(filters[i].Name, filters[i].MonikerString));

                }



                return (WebCamDescription[]) cams.ToArray(typeof(WebCamDescription)) ;

            }

        }





        public WebCam()

        {

            dshow.FilterCollection filters = new dshow.FilterCollection(dshow.Core.FilterCategory.VideoInputDevice);



            if (filters.Count == 0)

            {

                throw new Exception("No DirectShow filters available for video stream moniker (probably no webcam)");

            }



            cd = new CaptureDevice();



            cd.VideoSource = filters[2].MonikerString;

            cd.NewFrame += new CameraEventHandler(cd_NewFrame);

        }



        public WebCam(string moniker)

        {

            cd = new CaptureDevice();

            cd.VideoSource = moniker;

            cd.NewFrame += new CameraEventHandler(cd_NewFrame);  

        }



        public int FramesReceived
        {
            get
            {
                return cd.FramesReceived;
            }
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

                NewFrame(sender, new WebCamEventArgs(e.Bitmap));

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

