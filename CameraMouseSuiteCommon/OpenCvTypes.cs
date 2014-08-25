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

using System.Runtime.InteropServices;





namespace CameraMouseSuite

{



	[StructLayout(LayoutKind.Sequential)]

	public struct _IplImage 

	{

		public int  nSize;         /* sizeof(IplImage) */

		public int  ID;            /* version (=0)*/

		public int  nChannels;     /* Most of OpenCV functions support 1,2,3 or 4 channels */

		int  pad11; 

		public int  depth;         /* pixel depth in bits: IPL_DEPTH_8U, IPL_DEPTH_8S, IPL_DEPTH_16S,

                                        IPL_DEPTH_32S, IPL_DEPTH_32F and IPL_DEPTH_64F are supported */

		int  pad12, pad13;

		public int  dataOrder;     /* 0 - public interleaved color channels, 1 - separate color channels.

                                        cvCreateImage can only create interleaved images */

		public int  origin;        /* 0 - top-left origin,

                                        1 - bottom-left origin (Windows bitmaps style) */

		public int  align;         /* Alignment of image rows (4 or 8).

                                        OpenCV ignores it and uses widthStep instead */

		public int  width;         /* image width in pixels */

		public int  height;        /* image height in pixels */



		public IntPtr roi;  //public  struct _IplROI *roi;/* image ROI. when it is not NULL, this specifies image region to process */

		IntPtr pad8, pad9, pad10;   

		public int    imageSize;     /* image data size in bytes

                               (=image->height*image->widthStep

                               in case of interleaved data)*/

		public IntPtr imageData;   /* pointer to aligned image data */

		public int    widthStep;   /* size of aligned image row in bytes */

		int    pad0, pad1, pad2, pad3, pad4, pad5, pad6, pad7; /* ignored by OpenCV */

		public IntPtr imageDataOrigin; /* pointer to a very origin of image data

                                  (not necessarily aligned) -

                                  it is needed for correct image deallocation */

	}







	[StructLayout(LayoutKind.Sequential)]

	public struct CvCamDescription 

	{

		public IntPtr DeviceDescription; // = new byte[100];

		public IntPtr device; // = new byte[100];

		public int  channel;

		public IntPtr ChannelDescription; //  = new byte[100];

		public int  maxwidth;

		public int  maxheight;

		public int  minwidth;

		public int  minheight;

	}





	[StructLayout(LayoutKind.Sequential)]

	public struct CvSize 

	{

		public int Width;

		public int Height;



	

		public CvSize(int width, int height) 

		{ 

			Width = width;  

			Height= height;

		}



		public override string ToString()

		{

			return "("+Width+","+Height+")";

		}



	}




    [StructLayout(LayoutKind.Sequential)]
    public struct CvRect
    {
        public int x, y, width, height;

        public CvRect(int x, int y, int width, int height) 
        {
            this.x=x;
            this.y=y;
            this.width=width;
            this.height=height;
        }

    }

	[StructLayout(LayoutKind.Sequential)]

	public struct CvPoint 

	{

		public int x, y;

		/// <summary>

		/// 2D point with integer coordinates

		/// </summary>

		/// <param name="x"></param>

		/// <param name="y"></param>

		public CvPoint(int x, int y) 

		{

			this.x = x;  

			this.y = y;

		}

		/// <summary>

		/// 2D point with floating-point coordinates

		/// </summary>

		/// <param name="pt"></param>

		public CvPoint(CvPoint2D32f pt) 

		{

			this.x = (int) pt.x;  

			this.y = (int) pt.y;

		}

	}







	[StructLayout(LayoutKind.Sequential)]

	public struct CvPoint2D32f 

	{

		public float x, y;

		/// <summary>

		/// Initializer uses float values

		/// </summary>

		/// <param name="x"></param>

		/// <param name="y"></param>

		public CvPoint2D32f(float x, float y) 

		{

			this.x = x;  

			this.y = y;

		}



		/// <summary>

		/// Initializer, uses values from a CvPoint

		/// </summary>

		/// <param name="pt"></param>

		public CvPoint2D32f(CvPoint pt) 

		{

			this.x = (float) pt.x;  

			this.y = (float) pt.y;

		}

	}

	[StructLayout(LayoutKind.Sequential)]

	public struct CvTermCriteria

	{

		public int   type;     // flags	    maxIter=1 | epsilon=2;

		public int   maxIter;

		public double epsilon;

				

		/// <summary>

		/// Termination occurs after n iterations.

		/// </summary>

		public CvTermCriteria(int nIterations)

		{

			this.type = 1;

			this.maxIter = nIterations;

			this.epsilon = 0.0;

		}

		/// <summary>

		/// Termination occurs when it convergences to less than epsilon.

		/// </summary>

		/// <param name="epsilon"></param>

		public CvTermCriteria(double epsilon)

		{

			this.type = 2;

			this.maxIter = 0;

			this.epsilon = epsilon;

		}



		/// <summary>

		/// Termination occurs when either maxIterations is reached, or convergence

		/// to within epsilon is achieved.

		/// </summary>

		/// <param name="maxIer"></param>

		/// <param name="epsilon"></param>

		public CvTermCriteria(int maxIterations, double epsilon)

		{

			this.type = 3;

			this.maxIter = maxIterations;

			this.epsilon = epsilon;

		}



		/// <summary>

		/// Termination occurs when either maxIterations is reached, or convergence

		/// to within epsilon is achieved.

		/// </summary>

		/// <param name="type"></param>

		/// <param name="maxIterations"></param>

		/// <param name="epsilon"></param>

		public CvTermCriteria(int type, int maxIterations, double epsilon)

		{

			this.type = type;

			this.maxIter = maxIterations;

			this.epsilon = epsilon;

		}

	}

	public enum ColorConversion

	{

		BGR2BGRA    = 0,

		RGB2RGBA    = BGR2BGRA,

		BGRA2BGR    = 1,

		RGBA2RGB    = BGRA2BGR,

		BGR2RGBA    = 2,

		RGB2BGRA    = BGR2RGBA,

		RGBA2BGR    = 3,

		BGRA2RGB    = RGBA2BGR,

		BGR2RGB     = 4,

		RGB2BGR     = BGR2RGB,

		BGRA2RGBA   = 5,

		RGBA2BGRA   = BGRA2RGBA,

		BGR2GRAY    = 6,

		RGB2GRAY    = 7,

		GRAY2BGR    = 8,

		GRAY2RGB    = GRAY2BGR,

		GRAY2BGRA   = 9,

		GRAY2RGBA   = GRAY2BGRA,

		BGRA2GRAY   = 10,

		RGBA2GRAY   = 11,

		BGR2BGR565  = 12,

		RGB2BGR565  = 13,

		BGR5652BGR  = 14,

		BGR5652RGB  = 15,

		BGRA2BGR565 = 16,

		RGBA2BGR565 = 17,

		BGR5652BGRA = 18,

		BGR5652RGBA = 19,

		GRAY2BGR565 = 20,

		BGR5652GRAY = 21,

		BGR2XYZ     = 22,

		RGB2XYZ     = 23,

		XYZ2BGR     = 24,

		XYZ2RGB     = 25,

		BGR2YCrCb   = 26,

		RGB2YCrCb   = 27,

		YCrCb2BGR   = 28,

		YCrCb2RGB   = 29,

		BGR2HSV     = 30,

		RGB2HSV     = 31,

		BGR2Lab     = 34,

		RGB2Lab     = 35,

		BayerBG2BGR = 40,

		BayerGB2BGR = 41,

		BayerRG2BGR = 42,

		BayerGR2BGR = 43,

		BayerBG2RGB = BayerRG2BGR,

		BayerGB2RGB = BayerGR2BGR,

		BayerRG2RGB = BayerBG2BGR,

		BayerGR2RGB = BayerGB2BGR

	}



}