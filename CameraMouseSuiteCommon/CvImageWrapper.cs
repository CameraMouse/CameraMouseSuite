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

using System.Drawing;

using System.Drawing.Imaging;







namespace CameraMouseSuite

{
	

	public unsafe class CvImageWrapper

	{

        [DllImport("cxcore100.dll")]

        private static extern void cvCopy(IntPtr src, IntPtr dst, IntPtr mask);


        [DllImport("cxcore100.dll")]

        private static extern void cvResetImageROI(IntPtr img);


        [DllImport("cxcore100.dll")] 

        private static extern void cvSetImageROI(IntPtr img, CvRect rect);



		[DllImport("cxcore100.dll")] 

		private static extern IntPtr cvCreateImage(CvSize sz,int pdepth, int pchan);



		[DllImport("cxcore100.dll")] 

		private static extern int cvReleaseImage(ref IntPtr p); 



		[DllImport("cv100.dll")] 

		private static extern void cvCvtColor(IntPtr src, IntPtr dst, ColorConversion code);



        [DllImport("kernel32.dll")]

        static extern void RtlMoveMemory(IntPtr dest, IntPtr src, uint len);



        [DllImport("msvcrt.dll")]

        static extern IntPtr memset(IntPtr dest, int val, int len);


		public IntPtr _rawPtr;

		protected CvSize _size;


		public CvImageWrapper(IntPtr ptr)

		{

			_rawPtr = ptr;



			if(_rawPtr != IntPtr.Zero)

			{

				int w = ((_IplImage *)_rawPtr)->width;

				int h = ((_IplImage *)_rawPtr)->height;

				_size = new CvSize(w,h); 

			}

			else

			{

				_size = new CvSize(0,0); 

			}

		}

        public CvImageWrapper(Bitmap bmp)

        {
            IntPtr ptr = cvCreateImage(new CvSize(bmp.Size.Width, bmp.Size.Height), 8, 3);

            int struct_size = ((_IplImage*)ptr)->nSize;

                

            ((_IplImage*)ptr)->ID = 0;

            ((_IplImage*)ptr)->align = 8;

            ((_IplImage*)ptr)->nSize = struct_size;

            ((_IplImage*)ptr)->dataOrder = 0;

            ((_IplImage*)ptr)->nChannels = 3;

            ((_IplImage*)ptr)->depth = 8;

            ((_IplImage*)ptr)->imageSize = 3 * bmp.Size.Width * bmp.Size.Height;

            ((_IplImage*)ptr)->width = bmp.Size.Width;

            ((_IplImage*)ptr)->height = bmp.Size.Height;

            ((_IplImage*)ptr)->widthStep = bmp.Size.Width * 3;

            ((_IplImage*)ptr)->origin = 1;

            ((_IplImage*)ptr)->roi = new IntPtr(0);

            ((_IplImage*)ptr)->imageDataOrigin = ((_IplImage*)ptr)->imageData;



            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),

                                                ImageLockMode.ReadOnly, bmp.PixelFormat);

       

            RtlMoveMemory((IntPtr)((_IplImage*)ptr)->imageData, bmpData.Scan0, (uint)((_IplImage*)ptr)->imageSize);



            //Unlock the pixels

            bmp.UnlockBits(bmpData);



            _rawPtr = ptr;



            if (_rawPtr != IntPtr.Zero)

            {

                int w = ((_IplImage*)_rawPtr)->width;

                int h = ((_IplImage*)_rawPtr)->height;

                _size = new CvSize(w, h);

            }

            else

            {

                _size = new CvSize(0, 0);

            }

           

        }

		public CvSize Size

		{

			get

			{

				return _size;

			}

		}

        public CvImageWrapper cropSubImage(CvRect rect)
        {
            Bitmap b = GetBitMap();
            Bitmap b2 = new Bitmap(rect.width,rect.height,b.PixelFormat);

            for (int x = 0; x < rect.width; x++)
            {
                for (int y = 0; y < rect.height; y++)
                {
                    b2.SetPixel(x, y, b.GetPixel(x + rect.x, y + rect.y));
                }
            }

            return new CvImageWrapper(b2);
        }

        public void cropSubImage(CvRect rect, CvImageWrapper croppedImage)
        {
            if (rect.x < 0 || rect.y < 0 ||
               (rect.x + rect.width > this._size.Width) ||
                (rect.y + rect.height > this._size.Height))
                throw new Exception("invalid rect: " + rect);

            cvSetImageROI(this._rawPtr, rect);
            cvCopy(this._rawPtr, croppedImage._rawPtr, IntPtr.Zero);
            cvResetImageROI(this._rawPtr);
            
        }

        public void setImage(Bitmap bmp)
        {

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),

                                                ImageLockMode.ReadOnly, bmp.PixelFormat);



            RtlMoveMemory((IntPtr)((_IplImage*)_rawPtr)->imageData, bmpData.Scan0, (uint)((_IplImage*)_rawPtr)->imageSize);



            //Unlock the pixels

            bmp.UnlockBits(bmpData);
        }

		public static CvImageWrapper CreateImage(CvSize sz, int pdepth, int pchan)

		{

			IntPtr p = cvCreateImage(sz, pdepth, pchan);

			return new CvImageWrapper(p);

		}

		public static void ReleaseImage(CvImageWrapper img)

		{

			cvReleaseImage(ref img._rawPtr);

		}	

		public static void ConvertImageColor(CvImageWrapper src, CvImageWrapper dest, ColorConversion code)

		{

			cvCvtColor(src._rawPtr, dest._rawPtr, code);

		}

        // stupid extra copy because I haven't figured out how to copy from IntPtr to IntPtr.

		public Bitmap GetBitMap()

		{

			int sz = ((_IplImage *)_rawPtr)->imageSize;

			//	int wd = ((_IplImage *)img._rawPtr)->width;

			int wd = ((_IplImage *)_rawPtr)->widthStep;



			//System.Diagnostics.Debug.WriteLine("img size = " + sz.ToString());

			//	System.Diagnostics.Debug.WriteLine("img size = " + sz.ToString());



			byte [] data = new byte[sz];

			Marshal.Copy(((_IplImage *)_rawPtr)->imageData, data, 0, sz);



			//create the Bitmap

			Bitmap bmp = new Bitmap( Size.Width, Size.Height, PixelFormat.Format24bppRgb);  



			//Create a BitmapData and Lock all pixels to be written

			BitmapData bmpData = bmp.LockBits(

				new Rectangle(0, 0, bmp.Width, bmp.Height),   

				ImageLockMode.WriteOnly, bmp.PixelFormat);

 

			//Copy the data from the byte array into BitmapData.Scan0

			Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

                

			//Unlock the pixels

			bmp.UnlockBits(bmpData);



			data = null;

			

			return bmp;

		}

        public CvImageWrapper Clone()
        {
            return new CvImageWrapper(GetBitMap());
        }
	}

}

