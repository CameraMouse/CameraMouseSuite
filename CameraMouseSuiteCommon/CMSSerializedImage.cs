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
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

namespace CameraMouseSuite
{
    [XmlRoot("Image")]
    public class CMSSerializedImage
    {
        private PixelFormat format;
        [XmlElement("Fmt")]
        public PixelFormat Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        private int width;
        [XmlElement("W")]
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        private int height;
        [XmlElement("H")]
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /*
        private int actualWidth;
        [XmlElement("Wa")]
        public int ActualWidth
        {
            get
            {
                return actualWidth;
            }
            set
            {
                actualWidth = value;
            }
        }

        private int actualHeight;
        [XmlElement("Wh")]
        public int ActualHeight
        {
            get
            {
                return actualHeight;
            }
            set
            {
                actualHeight = value;

            }
        }
        */

        private byte[] uncompressedData = null;
        private byte[] data = null;
        [XmlElement("D", typeof(byte[]), DataType = "base64Binary")]
        public byte[] Data
        {
            get
            {
                if ((data == null || data.Length == 0) &&
                    (uncompressedData != null && uncompressedData.Length > 0))
                {
                    data = Compress(uncompressedData);
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public void SetImage(Bitmap image)
        {

            width = image.Width;
            height = image.Height;

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                image.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * image.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            image.UnlockBits(bmpData);

            format = image.PixelFormat;
            //this.data = Compress(rgbValues);
            this.uncompressedData = rgbValues;

            /*
            using (MemoryStream ms = new MemoryStream(rgbValues.Length))
            {
                ms.Write(rgbValues, 0, rgbValues.Length);
                using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress))
                {
                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        byte[] bts = new byte[4096];
                        int numBytes;
                        while ((numBytes = gs.Read(bts, 0, 4096)) > 0)
                            ms2.Write(bts, 0, numBytes);

                        this.data = ms2.GetBuffer();
                    }
                    
                }
            }*/
            //this.data = rgbValues;
        }

        public Bitmap GetImage()
        {
            //if(data == null)
                //return null;

            //if(data.Length == 0)
                //return null;

            //byte[] data2 = Decompress(data);



            byte[] data2 = null;

            if ((uncompressedData != null) && (uncompressedData.Length > 0))
            {
                data2 = uncompressedData;
            }
            else if ((data != null) && (data.Length > 0))
            {
                data2 = Decompress(data);
            }
            else
                return null;

            /*
            using (MemoryStream ms = new MemoryStream(data.Length))
            {
                ms.Write(data, 0, data.Length);
                using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress))
                {
                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        byte[] bts = new byte[4096];
                        int numBytes;
                        while ((numBytes = gs.Read(bts, 0, 4096)) > 0)
                            ms2.Write(bts, 0, numBytes);

                        data2 = ms2.GetBuffer();
                    }
                }
            }*/

            Bitmap b = new Bitmap(width, height, format);

            Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * b.Height;
            System.Runtime.InteropServices.Marshal.Copy(data2, 0, ptr, bytes);
            b.UnlockBits(bmpData);
            return b;
        }

        public static byte[] Compress(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(buffer, 0, buffer.Length);
            zip.Close();
            ms.Position = 0;

            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return gzBuffer;
        }

        public static byte[] Decompress(byte[] gzBuffer)
        {
            MemoryStream ms = new MemoryStream();
            int msgLength = BitConverter.ToInt32(gzBuffer, 0);
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

            byte[] buffer = new byte[msgLength];

            ms.Position = 0;
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
            zip.Read(buffer, 0, buffer.Length);

            return buffer;
        }
    }
}
