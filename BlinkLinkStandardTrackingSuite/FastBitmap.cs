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
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    public struct ColorARGB
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;

        public ColorARGB(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public ColorARGB(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }

        public float GetIntensity()
        {
            return (R + G + B) / 3f;
        }
    }

    unsafe public class FastBitmap : IDisposable, ICloneable
    {
        [DllImport("cv100.dll")]
        private static extern void cvSmooth(IntPtr src, IntPtr dst, int smoothtype,
            int param1, int param2, double param3, double param4);

        public class ConnectedComponentsResults
        {
            private int width;
            private int height;
            private int[,] labels;
            private int[] sizes;
            private Point[] centers;
            private Rectangle[] boundingBoxes;

            public ConnectedComponentsResults(int width, int height, int[,] labels, int[] sizes, Point[] centers, Rectangle[] boundingBoxes)
            {
                this.width = width;
                this.height = height;
                this.labels = labels;
                this.sizes = sizes;
                this.centers = centers;
                this.boundingBoxes = boundingBoxes;
            }

            public int Width
            {
                get
                {
                    return width;
                }
            }

            public int Height
            {
                get
                {
                    return height;
                }
            }

            public int[,] Labels
            {
                get
                {
                    return labels;
                }
            }

            public int[] Sizes
            {
                get
                {
                    return sizes;
                }
            }

            public Point[] Centers
            {
                get
                {
                    return centers;
                }
            }

            public Rectangle[] BoundingBoxes
            {
                get
                {
                    return boundingBoxes;
                }
            }

            public int NumberOfComponents
            {
                get
                {
                    if( sizes == null )
                    {
                        return 0;
                    }
                    else
                    {
                        return sizes.Length;
                    }
                }
            }

        }

        public class NccTemplate
        {
            private float[] template;
            private float mean;
            private float sigma;
            private int width;
            private int height;
            private int xPos;
            private int yPos;

            #region Properties

            public int Width
            {
                get
                {
                    return width;
                }
            }

            public int Height
            {
                get
                {
                    return height;
                }
            }

            public int X
            {
                get
                {
                    return xPos;
                }
            }

            public int Y
            {
                get
                {
                    return yPos;
                }
            }

            public Size Size
            {
                get
                {
                    return new Size(Width, Height);
                }
            }

            public float this[int i, int j]
            {
                get
                {
                    return template[i + width * j];
                }
            }

            public int Area
            {
                get
                {
                    return width * height;
                }
            }

            public FastBitmap FastBitmap
            {
                get
                {
                    FastBitmap result = new FastBitmap(width, height);

                    for( int i = 0; i < width; ++i )
                    {
                        for( int j = 0; j < height; ++j )
                        {
                            result.SetPixel(i, j, Color.FromArgb((int)template[i + width * j], (int)template[i + width * j], (int)template[i + width * j]));
                        }
                    }

                    return result;
                }
            }

            public Bitmap Bitmap
            {
                get
                {
                    FastBitmap temp = FastBitmap;
                    Bitmap result = temp.Bitmap;
                    temp.Dispose();

                    return result;
                }
            }

            #endregion

            public NccTemplate(float[,] intensityMap, Rectangle intensityMapRect,
                int x, int y, int width, int height)
            {
                if( x + width >= intensityMapRect.Width || x < 0 || y + height >= intensityMapRect.Height || y < 0 )
                {
                    throw new ArgumentOutOfRangeException("Template dimensions excede intensity map dimensions");
                }

                xPos = x + intensityMapRect.X;
                yPos = y + intensityMapRect.Y;

                this.width = width;
                this.height = height;

                float squaredMean = 0;
                template = new float[width * height];

                int xEnd = x + width;
                int yEnd = y + height;
                int index = 0;
                // x + width * y
                // i + width * j


                for( int j = y; j < yEnd; ++j )
                {
                    for( int i = x; i < xEnd; ++i )
                    {
                        float intensity = intensityMap[i, j];
                        template[index++] = intensity;
                        mean += intensity;
                        squaredMean += (intensity * intensity);
                    }
                }

                mean = mean / template.Length;
                squaredMean = squaredMean / template.Length;

                sigma = (float)Math.Sqrt(squaredMean - (mean * mean));
            }

            public NccTemplate(FastBitmap img, int x, int y, int width, int height)
            {
                if( x + width >= img.Width || x < 0 || y + height >= img.Height || y < 0 )
                {
                    throw new ArgumentOutOfRangeException("Template dimensions excede image dimensions");
                }

                xPos = x;
                yPos = y;

                this.width = width;
                this.height = height;

                float squaredMean = 0;
                template = new float[width * height];

                int xEnd = x + width;
                int yEnd = y + height;
                int index = 0;
                // x + width * y
                // i + width * j


                for( int j = y; j < yEnd; ++j )
                {
                    for( int i = x; i < xEnd; ++i )
                    {
                        float intensity = img.GetIntensity(i, j);
                        template[index++] = intensity;
                        mean += intensity;
                        squaredMean += (intensity * intensity);
                    }
                }

                mean = mean / template.Length;
                squaredMean = squaredMean / template.Length;

                sigma = (float)Math.Sqrt(squaredMean - (mean * mean));
            }

            public NccTemplate(int width, int height)
            {
                template = new float[width * height];
                mean = 0;
                sigma = 0;
                this.width = width;
                this.height = height;
                xPos = 0;
                yPos = 0;
            }

            public void SetNccTemplate(float[,] intensityMap, Rectangle intensityMapRect, int x, int y)
            {
                if( x + width >= intensityMapRect.Width || x < 0 || y + height >= intensityMapRect.Height || y < 0 )
                {
                    throw new ArgumentOutOfRangeException("Template dimensions excede intensity map dimensions");
                }

                xPos = x + intensityMapRect.X;
                yPos = y + intensityMapRect.Y;


                float squaredMean = 0;
                mean = 0;

                int xEnd = x + width;
                int yEnd = y + height;
                int index = 0;
                // x + width * y
                // i + width * j


                for( int j = y; j < yEnd; ++j )
                {
                    for( int i = x; i < xEnd; ++i )
                    {
                        float intensity = intensityMap[i, j];
                        template[index++] = intensity;
                        mean += intensity;
                        squaredMean += (intensity * intensity);
                    }
                }

                mean = mean / template.Length;
                squaredMean = squaredMean / template.Length;

                sigma = (float)Math.Sqrt(squaredMean - (mean * mean));
            }

            public void MergeTemplates(NccTemplate mergingTemplate, float alpha)
            {
                if( alpha < 0 || alpha > 1 )
                {
                    throw new ArgumentOutOfRangeException("Alpha must be between 0 and 1 inclusively");
                }

                float beta = 1 - alpha;
                float squaredMean = 0;

                mean = 0;

                for( int i = 0; i < template.Length; ++i )
                {
                    float intensity = beta * template[i] + alpha * mergingTemplate.template[i];
                    template[i] = intensity;
                    mean += intensity;
                    squaredMean += (intensity * intensity);
                }

                mean = mean / (template.Length);
                squaredMean = squaredMean / (template.Length);

                sigma = (float)Math.Sqrt(squaredMean - (mean * mean));
            }

            public float GetNcc(float[] template, float mean, float sigma)
            {
                //r= 1/n Σ_i ((s_i - mean(s)) * (m_i- mean(m)) / (σ_s σ_m))

                float ncc = 0;

                for( int i = 0; i < template.Length; ++i )
                {
                    ncc += ((this.template[i] - this.mean) * (template[i] - mean));
                }

                return ncc / (width * height * this.sigma * sigma);
            }

            public float GetNcc(NccTemplate template)
            {
                return GetNcc(template.template, template.mean, template.sigma);
            }

            public float GetNcc(float[,] intensityMap, int x, int y, float mean, float sigma)
            {
                //r= 1/n Σ_i ((s_i - mean(s)) * (m_i- mean(m)) / (σ_s σ_m))

                float ncc = 0;
                int index = 0;

                for( int j = 0; j < height; ++j )
                {
                    for( int i = 0; i < width; ++i )
                    {

                        ncc += ((this.template[index++] - this.mean) * (intensityMap[i + x, j + y] - mean));
                    }
                }

                return ncc / (width * height * this.sigma * sigma);
            }

            public float[] GetTopNccScores(NccTemplate[] templates, int[] bestMatchIndices)
            {
                float[] bestNccScores = new float[bestMatchIndices.Length];
                bestNccScores[0] = GetNcc(templates[0]);
                bestMatchIndices[0] = 0;

                for( int i = 1; i < bestMatchIndices.Length; ++i )
                {
                    bestNccScores[i] = -2;
                }

                for( int i = 1; i < templates.Length; ++i )
                {
                    float tempNcc = GetNcc(templates[i]);
                    UpdateNccAndPointArray(bestNccScores, bestMatchIndices, tempNcc, i);
                }

                return bestNccScores;
            }

            private static void UpdateNccAndPointArray(float[] nccs, int[] indices, float newNcc, int newIndex)
            {
                int newLoc = 0;

                while( newLoc < nccs.Length )
                {
                    if( newNcc > nccs[newLoc] )
                    {
                        break;
                    }
                    ++newLoc;
                }

                if( newLoc < nccs.Length )
                {
                    for( int i = nccs.Length - 1; i > newLoc; --i )
                    {
                        nccs[i] = nccs[i - 1];
                        indices[i] = indices[i - 1];
                    }

                    nccs[newLoc] = newNcc;
                    indices[newLoc] = newIndex;
                }
            }
        }

        private Bitmap bitmap;
        private BitmapData bitmapData;
        private int width;
        private int height;
        private ColorARGB* startingPosition;
        private ColorARGB*[] imageBackbone;


        public FastBitmap(int width, int height)
        {
            bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Load();
        }

        public FastBitmap(string fileName)
        {
            bitmap = new Bitmap(fileName);
            Load();
        }

        public FastBitmap(Bitmap bitmap)
        {
            this.bitmap = (Bitmap)bitmap.Clone();
            Load();
        }

        public FastBitmap(FastBitmap img)
        {
            bitmap = (Bitmap)img.bitmap.Clone();
            Load();
        }

        public void Dispose()
        {
            Unlock();
            bitmap.Dispose();
        }

        public ColorARGB this[int x, int y]
        {
            get
            {
                if( x < 0 || x >= width )
                {
                    throw new ArgumentOutOfRangeException("x must be non-negative and less than width");
                }

                if( y < 0 || y >= height )
                {
                    throw new ArgumentOutOfRangeException("y must be non-negative and less than height");
                }

                return *(imageBackbone[y] + x);
            }
            set
            {
                if( x < 0 || x >= width )
                {
                    throw new ArgumentOutOfRangeException("x must be non-negative and less than width");
                }

                if( y < 0 || y >= height )
                {
                    throw new ArgumentOutOfRangeException("y must be non-negative and less than height");
                }

                *(imageBackbone[y] + x) = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public ColorARGB* StartingPosition
        {
            get
            {
                return startingPosition;
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                if( startingPosition == null )
                {
                    return (Bitmap)bitmap.Clone();
                }
                else
                {
                    Unlock();
                    Bitmap copy = (Bitmap)bitmap.Clone();
                    Lock();
                    return copy;
                }

            }
        }


        public void Unlock()
        {
            if( startingPosition != null )
            {
                bitmap.UnlockBits(bitmapData);
                startingPosition = null;
            }
        }

        public void Load()
        {
            width = bitmap.Width;
            height = bitmap.Height;
            startingPosition = null;
            Lock();
        }

        public void Lock()
        {
            if( startingPosition == null )
            {
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                startingPosition = (ColorARGB*)bitmapData.Scan0;

                imageBackbone = new ColorARGB*[Width];

                for( int i = 0; i < Width; ++i )
                {
                    imageBackbone[i] = startingPosition + i * width;
                }
            }
        }

        public Color GetPixel(int x, int y)
        {
            if( x < 0 || x >= width )
            {
                throw new ArgumentOutOfRangeException("x must be non-negative and less than width");
            }

            if( y < 0 || y >= height )
            {
                throw new ArgumentOutOfRangeException("y must be non-negative and less than height");
            }

            ColorARGB* position = imageBackbone[y] + x;
            return Color.FromArgb(position->A, position->R, position->G, position->B);
        }

        public float GetIntensity(int x, int y)
        {
            if( x < 0 || x >= width )
            {
                throw new ArgumentOutOfRangeException("x must be non-negative and less than width");
            }

            if( y < 0 || y >= height )
            {
                throw new ArgumentOutOfRangeException("y must be non-negative and less than height");
            }

            ColorARGB* position = imageBackbone[y] + x;
            return (position->R + position->G + position->B) / 3.0f;
        }

        public void GetIntensityMap(int x, int y, int width, int height, float[,] intensityMap)
        {
            int xEnd = x + width;
            int yEnd = y + height;

            if( x < 0 || y < 0|| xEnd >= Width || yEnd >= Height )
            {
                throw new ArgumentOutOfRangeException("Intensity map dimensions execde image dimensions");
            }

            int xIndex = 0;
            int yIndex = 0;

            for( int i = x; i < xEnd; ++i )
            {
                for( int j = y; j < yEnd; ++j )
                {
                    ColorARGB* position = imageBackbone[j] + i;
                    intensityMap[xIndex, yIndex++] = (position->R + position->G + position->B) / 3.0f;
                }
                xIndex++;
                yIndex = 0;
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            if( x < 0 || x >= width )
            {
                throw new ArgumentOutOfRangeException("x must be non-negative and less than width");
            }

            if( y < 0 || y >= height )
            {
                throw new ArgumentOutOfRangeException("y must be non-negative and less than height");
            }

            ColorARGB* position = imageBackbone[y] + x;
            position->A = color.A;
            position->R = color.R;
            position->G = color.G;
            position->B = color.B;
        }

        public void DrawRectangle(int x, int y, int width, int height, Color c, bool filled)
        {
            if( filled )
            {
                for( int i = 0; i < width; ++i )
                {
                    for( int j = 0; j < height; ++j )
                    {
                        SetPixel(x + i, y + j, c);
                    }
                }
            }
            else
            {
                for( int i = 0; i < width; ++i )
                {
                    SetPixel(x + i, y, c);
                    SetPixel(x + i, y + height - 1, c);
                }

                for( int i = 1; i < height - 1; ++i )
                {
                    SetPixel(x, y + i, c);
                    SetPixel(x + width - 1, y + i, c);
                }
            }
        }

        public void DrawRectangle(Point p, int width, int height, Color c, bool filled)
        {
            DrawRectangle(p.X, p.Y, width, height, c, filled);
        }

        public void DrawRectangle(Rectangle r, Color c, bool filled)
        {
            DrawRectangle(r.X, r.Y, r.Width, r.Height, c, filled);
        }

        public FastBitmap GaussianSmooth(int kernel)
        {
            CvImageWrapper temp = new CvImageWrapper(Bitmap);
            CvImageWrapper tempTwo = CvImageWrapper.CreateImage(new CvSize(width, height), 8, 3);
            cvSmooth(temp._rawPtr, tempTwo._rawPtr, 2, kernel, kernel, 0, 0);
            return new FastBitmap(tempTwo.GetBitMap());
        }

        public FastBitmap GetDifferenceImage(FastBitmap otherImg, byte threshold)
        {
            if( (otherImg.width != width) || (otherImg.height != height) )
            {
                throw new ArgumentException("Widths and heights must match");
            }

            FastBitmap result = new FastBitmap(width, height);

            ColorARGB* currentPos = startingPosition;
            ColorARGB* otherCurrentPos = otherImg.startingPosition;
            ColorARGB* resultCurrentPos = result.startingPosition;

            for( int i = 0; i < width * height; ++i )
            {
                int intensity = (currentPos->R + currentPos->G + currentPos->B) / 3;
                int otherIntensity = (otherCurrentPos->R + otherCurrentPos->G + otherCurrentPos->B) / 3;
                byte diff = (byte)Math.Abs(intensity - otherIntensity);
                resultCurrentPos->A = 255;

                if( Math.Abs(intensity - otherIntensity) > threshold )
                {
                    resultCurrentPos->R = 255;
                    resultCurrentPos->G = 255;
                    resultCurrentPos->B = 255;
                }
                else
                {
                    resultCurrentPos->R = 0;
                    resultCurrentPos->G = 0;
                    resultCurrentPos->B = 0;
                }


                ++currentPos;
                ++otherCurrentPos;
                ++resultCurrentPos;
            }

            return result;
        }


        public FastBitmap GetDifferenceImage(FastBitmap otherImg)
        {
            if( (otherImg.width != width) || (otherImg.height != height) )
            {
                throw new ArgumentException("Widths and heights must match");
            }

            FastBitmap result = new FastBitmap(width, height);

            ColorARGB* currentPos = startingPosition;
            ColorARGB* otherCurrentPos = otherImg.startingPosition;
            ColorARGB* resultCurrentPos = result.startingPosition;

            for( int i = 0; i < width * height; ++i )
            {
                int intensity = (currentPos->R + currentPos->G + currentPos->B) / 3;
                int otherIntensity = (otherCurrentPos->R + otherCurrentPos->G + otherCurrentPos->B) / 3;
                byte diff = (byte)Math.Abs(intensity - otherIntensity);
                resultCurrentPos->A = 255;

                resultCurrentPos->R = diff;
                resultCurrentPos->G = diff;
                resultCurrentPos->B = diff;


                ++currentPos;
                ++otherCurrentPos;
                ++resultCurrentPos;
            }

            return result;
        }


        public ConnectedComponentsResults FindConnectedComponents(int threshold, int fillSize, int minimumComponentSize)
        {
            int[,] connComponents = new int[width, height];
            int nextLabel = 0;
            int[] componentSizes = null;
            Point[] componentCenters = null;
            Rectangle[] componentBoundingBoxes = null;
            bool[,] binaryImg = new bool[width, height];

            --minimumComponentSize;

            ColorARGB* currentPosition = startingPosition;
            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    binaryImg[x, y] = ((currentPosition->R + currentPosition->G + currentPosition->B) / 3) > threshold;
                    ++currentPosition;
                }
            }

            if( fillSize > 0 )
            {
                for( int x = 1; x < width; ++x )
                {
                    for( int y = 1; y < height; ++y )
                    {
                        if( !binaryImg[x, y] && (binaryImg[x - 1, y] || binaryImg[x, y - 1]) )
                        {
                            bool cont = false;
                            for( int i = x + 1; i < Math.Min(width, x + fillSize + 1); ++i )
                            {
                                if( binaryImg[i, y] )
                                {
                                    cont = true;
                                    break;
                                }
                            }

                            if( cont )
                            {
                                for( int j = y + 1; j < Math.Min(height, y + fillSize + 1); ++j )
                                {
                                    if( binaryImg[x, j] )
                                    {
                                        binaryImg[x, y] = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            for( int x = 0; x < width; ++x )
            {
                for( int y = 0; y < height; ++y )
                {
                    connComponents[x, y] = -1;
                }
            }

            Queue<Point> pointQueue = new Queue<Point>();

            for( int x = 0; x < width; ++x )
            {
                for( int y = 0; y < height; ++y )
                {
                    if( binaryImg[x, y] && (connComponents[x, y] == -1) )
                    {
                        pointQueue.Clear();
                        Point[] tempPointArray;
                        Point tempPoint = new Point(x, y);
                        pointQueue.Enqueue(tempPoint);
                        int tempArrayIndex = 0;

                        if( minimumComponentSize > 0 )
                        {
                            tempPointArray = new Point[minimumComponentSize];
                            tempPointArray[tempArrayIndex] = tempPoint;
                            ++tempArrayIndex;
                            connComponents[x, y] = -2;
                        }
                        else
                        {
                            tempPointArray = null;
                            tempArrayIndex = minimumComponentSize;
                            connComponents[x, y] = nextLabel;
                        }


                        while( pointQueue.Count > 0 )
                        {
                            Point p = pointQueue.Dequeue();

                            for( int i = Math.Max(0, p.X - 1); i < Math.Min(p.X + 2, width); ++i )
                            {
                                for( int j = Math.Max(0, p.Y - 1); j < Math.Min(p.Y + 2, height); ++j )
                                {
                                    if( binaryImg[i, j] && (connComponents[i, j] == -1) )
                                    {
                                        if( tempArrayIndex == minimumComponentSize )
                                        {
                                            if( tempPointArray != null )
                                            {
                                                foreach( Point point in tempPointArray )
                                                {
                                                    connComponents[point.X, point.Y] = nextLabel;
                                                }
                                                tempPointArray = null;

                                                connComponents[i, j] = nextLabel;
                                                pointQueue.Enqueue(new Point(i, j));

                                            }
                                            else
                                            {
                                                connComponents[i, j] = nextLabel;
                                                pointQueue.Enqueue(new Point(i, j));
                                            }
                                        }
                                        else
                                        {
                                            Point tempP = new Point(i, j);
                                            pointQueue.Enqueue(tempP);
                                            tempPointArray[tempArrayIndex] = tempP;
                                            ++tempArrayIndex;
                                            connComponents[i, j] = -2;
                                        }

                                    }
                                }
                            }
                        }

                        if( tempPointArray == null )
                        {
                            ++nextLabel;
                        }
                        else
                        {
                            foreach( Point point in tempPointArray )
                            {
                                connComponents[point.X, point.Y] = -1;
                            }
                        }
                    }
                }
            }


            if( nextLabel != 0 )
            {
                componentSizes = new int[nextLabel];
                componentCenters = new Point[nextLabel];
                componentBoundingBoxes = new Rectangle[nextLabel];
                int[] xSum = new int[nextLabel];
                int[] ySum = new int[nextLabel];
                int[] xCount = new int[nextLabel];
                int[] yCount = new int[nextLabel];
                int[] left = new int[nextLabel];
                int[] right = new int[nextLabel];
                int[] top = new int[nextLabel];
                int[] bottom = new int[nextLabel];

                for( int i = 0; i < nextLabel; ++i )
                {
                    left[i] = int.MaxValue;
                    top[i] = int.MaxValue;
                }

                for( int x = 0; x < width; ++x )
                {
                    for( int y = 0; y < height; ++y )
                    {
                        if( connComponents[x, y] != -1 )
                        {
                            ++componentSizes[connComponents[x, y]];
                            xSum[connComponents[x, y]] += x;
                            ySum[connComponents[x, y]] += y;

                            if( x < left[connComponents[x, y]] )
                            {
                                left[connComponents[x, y]] = x;
                            }

                            if( x > right[connComponents[x, y]] )
                            {
                                right[connComponents[x, y]] = x;
                            }

                            if( y < top[connComponents[x, y]] )
                            {
                                top[connComponents[x, y]] = y;
                            }

                            if( y > bottom[connComponents[x, y]] )
                            {
                                bottom[connComponents[x, y]] = y;
                            }


                        }
                    }
                }

                for( int i = 0; i < nextLabel; ++i )
                {
                    componentCenters[i] = new Point(xSum[i] / componentSizes[i], ySum[i] / componentSizes[i]);
                    componentBoundingBoxes[i] = new Rectangle(left[i], top[i], right[i] - left[i] + 1, bottom[i] - top[i] + 1);
                }
            }

            return new ConnectedComponentsResults(width, height, connComponents, componentSizes, componentCenters, componentBoundingBoxes);
        }

        public FastBitmap ApplyOpeningMask(int maskWidth)
        {
            FastBitmap temp = ApplyErosionMask(maskWidth);

            FastBitmap result = temp.ApplyDilationMask(maskWidth);

            temp.Dispose();

            return result;
        }

        public FastBitmap ApplyErosionMask(int maskWidth)
        {
            FastBitmap result = new FastBitmap(this.bitmap);

            for( int x = 0; x < width - maskWidth; ++x )
            {
                for( int y = 0; y < height - maskWidth; ++y )
                {
                    bool filled = true;
                    for( int i = 0; (i < maskWidth) && filled; ++i )
                    {
                        for( int j = 0; (j < maskWidth) && filled; ++j )
                        {
                            if( GetPixel(x + i, y + j).B != 255 )
                            {
                                filled = false;
                            }
                        }
                    }
                    if( !filled )
                    {
                        result.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return result;
        }

        public FastBitmap ApplyCrossErosionMask()
        {
            FastBitmap result = new FastBitmap(this.bitmap);
            bool[,] binaryMap = new bool[width, height];
            ColorARGB* currentPosition = startingPosition;

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    binaryMap[x, y] = currentPosition->B == 255;
                    ++currentPosition;
                }
            }

            for( int x = 2; x < width - 2; ++x )
            {
                for( int y = 2; y < height - 2; ++y )
                {
                    if( !(binaryMap[x, y] && binaryMap[x, y - 1] && binaryMap[x - 1, y] && binaryMap[x + 1, y] && binaryMap[x, y + 1]
                          && binaryMap[x, y - 2] && binaryMap[x - 2, y] && binaryMap[x + 2, y] && binaryMap[x, y + 2]) )
                    {
                        result.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return result;
        }

        public FastBitmap ApplyDilationMask(int maskWidth)
        {
            FastBitmap result = new FastBitmap(this.bitmap);

            for( int x = 0; x < width - maskWidth; ++x )
            {
                for( int y = 0; y < height - maskWidth; ++y )
                {
                    if( GetPixel(x, y).B == 255 )
                    {
                        for( int i = 0; i < maskWidth; ++i )
                        {
                            for( int j = 0; j < maskWidth; ++j )
                            {
                                result.SetPixel(x + i, y + j, Color.White);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public FastBitmap GreyScale()
        {
            FastBitmap result = new FastBitmap(width, height);

            ColorARGB* currentPosition = startingPosition;
            ColorARGB* resultCurrentPosition = result.startingPosition;

            for( int i = 0; i < width * height; ++i )
            {
                byte grey = (byte)((currentPosition->R + currentPosition->G + currentPosition->B) / 3);
                resultCurrentPosition->A = 255;
                resultCurrentPosition->R = grey;
                resultCurrentPosition->G = grey;
                resultCurrentPosition->B = grey;
                ++currentPosition;
                ++resultCurrentPosition;
            }

            return result;
        }

        public FastBitmap SobelGradient()
        {
            FastBitmap result = new FastBitmap(width, height);
            byte[,] intensities = new byte[width, height];
            ColorARGB* currentPosition = startingPosition;
            ColorARGB* resultCurrentPosition = result.startingPosition;

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    intensities[x, y] = (byte)((currentPosition->R + currentPosition->G + currentPosition->B) / 3);
                    ++currentPosition;
                }
            }

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    if( (x == 0) || (y == 0) || (x == width - 1) || (y == height - 1) )
                    {
                        resultCurrentPosition->R = 0;
                        resultCurrentPosition->G = 0;
                        resultCurrentPosition->B = 0;
                    }
                    else
                    {
                        int xGradient = intensities[x - 1, y - 1] + 2 * intensities[x - 1, y] + intensities[x - 1, y + 1] - intensities[x + 1, y - 1] - 2 * intensities[x + 1, y] - intensities[x + 1, y + 1];
                        int yGradient = intensities[x - 1, y - 1] + 2 * intensities[x, y - 1] + intensities[x + 1, y - 1] - intensities[x - 1, y + 1] - 2 * intensities[x, y + 1] - intensities[x + 1, y + 1];
                        byte grey = (byte)Math.Min(Math.Sqrt(xGradient * xGradient + yGradient * yGradient), 255);
                        resultCurrentPosition->R = grey;
                        resultCurrentPosition->G = grey;
                        resultCurrentPosition->B = grey;

                    }
                    resultCurrentPosition->A = 255;
                    ++resultCurrentPosition;
                }
            }

            return result;
        }

        public unsafe FastBitmap SobelGradient(byte threshold)
        {
            FastBitmap result = new FastBitmap(width, height);
            byte[,] intensities = new byte[width, height];
            ColorARGB* currentPosition = startingPosition;
            ColorARGB* resultCurrentPosition = result.startingPosition;

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    intensities[x, y] = (byte)((currentPosition->R + currentPosition->G + currentPosition->B) / 3);
                    ++currentPosition;
                }
            }

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    if( (x == 0) || (y == 0) || (x == width - 1) || (y == height - 1) )
                    {
                        resultCurrentPosition->R = 0;
                        resultCurrentPosition->G = 0;
                        resultCurrentPosition->B = 0;
                    }
                    else
                    {
                        int xGradient = intensities[x - 1, y - 1] + 2 * intensities[x - 1, y] + intensities[x - 1, y + 1] - intensities[x + 1, y - 1] - 2 * intensities[x + 1, y] - intensities[x + 1, y + 1];
                        int yGradient = intensities[x - 1, y - 1] + 2 * intensities[x, y - 1] + intensities[x + 1, y - 1] - intensities[x - 1, y + 1] - 2 * intensities[x, y + 1] - intensities[x + 1, y + 1];

                        if( Math.Sqrt(xGradient * xGradient + yGradient * yGradient) > threshold )
                        {
                            resultCurrentPosition->R = 255;
                            resultCurrentPosition->G = 255;
                            resultCurrentPosition->B = 255;
                        }
                        else
                        {
                            resultCurrentPosition->R = 0;
                            resultCurrentPosition->G = 0;
                            resultCurrentPosition->B = 0;
                        }

                    }
                    resultCurrentPosition->A = 255;
                    ++resultCurrentPosition;
                }
            }

            return result;
        }

        public FastBitmap BinaryThresholdImage(byte threshold)
        {
            FastBitmap result = new FastBitmap(width, height);

            ColorARGB* currentPosition = startingPosition;
            ColorARGB* resultCurrentPosition = result.startingPosition;

            for( int i = 0; i < width * height; ++i )
            {
                byte grey = (byte)((currentPosition->R + currentPosition->G + currentPosition->B) / 3);
                resultCurrentPosition->A = 255;
                if( grey > threshold )
                {
                    resultCurrentPosition->R = 255;
                    resultCurrentPosition->G = 255;
                    resultCurrentPosition->B = 255;
                }
                else
                {
                    resultCurrentPosition->R = 0;
                    resultCurrentPosition->G = 0;
                    resultCurrentPosition->B = 0;
                }

                ++currentPosition;
                ++resultCurrentPosition;
            }

            return result;
        }

        public FastBitmap Expand(int expansationRatio)
        {
            FastBitmap result = new FastBitmap(width * expansationRatio, height * expansationRatio);

            for( int x = 0; x < result.width; ++x )
            {
                for( int y = 0; y < result.height; ++y )
                {
                    result.SetPixel(x, y, GetPixel(x / expansationRatio, y / expansationRatio));
                }
            }

            return result;
        }

        public FastBitmap GetImageSubset(int x, int y, int width, int height)
        {
            FastBitmap result = new FastBitmap(width, height);

            for( int i = 0; i < width; ++i )
            {
                for( int j = 0; j < height; ++j )
                {
                    result.SetPixel(i, j, GetPixel(x + i, y + j));
                }
            }

            return result;
        }

        public FastBitmap MedianIntensitySmoothSubimage(int maskWidth, int x, int y, int width, int height)
        {
            if( (maskWidth % 2) == 0 )
            {
                throw new ArgumentException("MedianIntensitySmoothSubimage: maskWidth must be an odd integer");
            }
            FastBitmap result = new FastBitmap(width, height);

            for( int i = 0; i < width; ++i )
            {
                for( int j = 0; j < height; ++j )
                {
                    int intensity = GetIntensityMedian(x + i, y + j, maskWidth);
                    result.SetPixel(i, j, Color.FromArgb(intensity, intensity, intensity));
                }
            }

            return result;
        }

        public FastBitmap MedianIntensitySmooth(int maskWidth)
        {
            return MedianIntensitySmoothSubimage(maskWidth, 0, 0, width, height);
        }

        private int GetIntensityMedian(int x, int y, int maskWidth)
        {
            int halfImageWidth = (maskWidth - 1) / 2;
            ArrayList intensityList = new ArrayList();
            for( int i = Math.Max(0, x - halfImageWidth); i <= Math.Min(x + halfImageWidth, width - 1); ++i )
            {
                for( int j = Math.Max(0, y - halfImageWidth); j <= Math.Min(y + halfImageWidth, height - 1); ++j )
                {
                    intensityList.Add(GetIntensity(i, j));
                }
            }

            intensityList.Sort();

            if( (intensityList.Count % 2) == 0 )
            {
                return (int)((float)intensityList[intensityList.Count / 2] + (float)intensityList[(intensityList.Count / 2) - 1]) / 2;
            }
            else
            {
                return (int)(float)intensityList[intensityList.Count / 2];
            }
        }

        public FastBitmap GetImageSubset(Rectangle rect)
        {
            return GetImageSubset(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public FastBitmap GetImageSubset(Point p, int width, int height)
        {
            return GetImageSubset(p.X, p.Y, width, height);
        }

        /* Incomplete Canny Edge Detection

        public FastBitmap CannyEdgeDetection(int gaussianSmooth, byte highThreshold, byte lowThreshold)
        {
            return CannyEdgeDetection(gaussianSmooth, highThreshold, lowThreshold, 0);
        }

        public FastBitmap CannyEdgeDetection(int gaussianSmooth, byte highThreshold, byte lowThreshold, byte nonMaximumSuppressionGive)
        {
            FastBitmap smoothedImage          = GaussianSmooth(gaussianSmooth);
            byte[,]    intensities            = new byte[width, height];
            ColorARGB* currentPosition        = smoothedImage.startingPosition;
            byte[,]    sobelGradientIntensity = new byte[width, height];
            byte[,]    sobelGradientAngle     = new byte[width, height];
            byte[,]    cannyEdge              = new byte[width, height];
            bool[,]    edgeMap                = new bool[width, height];
            bool[,]    processedPixel         = new bool[width, height];

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    intensities[x, y] = (byte)((currentPosition->R + currentPosition->G + currentPosition->B) / 3);
                    ++currentPosition;
                }
            }

            for( int y = 1; y < height - 1; ++y )
            {
                for( int x = 1; x < width - 1; ++x )
                {
                    int xGradient                = intensities[x - 1, y - 1] + 2 * intensities[x - 1, y] + intensities[x - 1, y + 1] - intensities[x + 1, y - 1] - 2 * intensities[x + 1, y] - intensities[x + 1, y + 1];
                    int yGradient                = intensities[x - 1, y - 1] + 2 * intensities[x, y - 1] + intensities[x + 1, y - 1] - intensities[x - 1, y + 1] - 2 * intensities[x, y + 1] - intensities[x + 1, y + 1];
                    sobelGradientIntensity[x, y] = (byte)Math.Sqrt(xGradient * xGradient + yGradient * yGradient);
                    sobelGradientAngle[x, y]     = (byte)(Math.Atan2(yGradient, xGradient) * 180.0 / Math.PI);
                    sobelGradientAngle[x, y]     = (byte)((((byte)Math.Round(sobelGradientAngle[x, y] / 45.0)) * 45) % 180);
                }
            }

            for( int x = 1; x < width - 1; ++x )
            {
                for( int y = 1; y < height - 1; ++y )
                {
                    switch( sobelGradientAngle[x, y] )
                    {
                        case 0:
                            {
                                if( (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x, y + 1] - nonMaximumSuppressionGive)) &&

                            (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x, y - 1] - nonMaximumSuppressionGive)) )
                                {
                                    cannyEdge[x, y] = sobelGradientIntensity[x, y];
                                    edgeMap[x, y] = cannyEdge[x, y] > highThreshold;
                                }
                                else
                                {
                                    processedPixel[x, y] = true;
                                }
                            }
                            break;
                        case 45:
                            {
                                if( (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x - 1, y - 1] - nonMaximumSuppressionGive)) &&
                                    (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x + 1, y + 1] - nonMaximumSuppressionGive)) )
                                {
                                    cannyEdge[x, y] = sobelGradientIntensity[x, y];
                                    edgeMap[x, y] = cannyEdge[x, y] > highThreshold;
                                }
                                else
                                {
                                    processedPixel[x, y] = true;
                                }
                            }
                            break;
                        case 90:
                            {
                                if( (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x - 1, y] - nonMaximumSuppressionGive)) &&
                                    (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x + 1, y] - nonMaximumSuppressionGive)) )
                                {
                                    cannyEdge[x, y] = sobelGradientIntensity[x, y];
                                    edgeMap[x, y] = cannyEdge[x, y] > highThreshold;
                                }
                                else
                                {
                                    processedPixel[x, y] = true;
                                }
                            }
                            break;
                        case 135:
                            {
                                if( (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x + 1, y - 1] - nonMaximumSuppressionGive)) &&
                                    (sobelGradientIntensity[x, y] >= (sobelGradientIntensity[x - 1, y + 1] - nonMaximumSuppressionGive)) )
                                {
                                    cannyEdge[x, y] = sobelGradientIntensity[x, y];
                                    edgeMap[x, y] = cannyEdge[x, y] > highThreshold;
                                }
                                else
                                {
                                    processedPixel[x, y] = true;
                                }
                            }
                            break;
                        default:
                            {
                                Debug.Assert(false);
                            }
                            break;
                    }
                }
            }

            Queue<Point> pointQueue = new Queue<Point>();

            for( int x = 0; x < width; ++x )
            {
                for( int y = 0; y < height; ++y )
                {
                    if( edgeMap[x, y] )
                    {
                        pointQueue.Enqueue(new Point(x,y));

                        while(pointQueue.Count != 0)
                        {
                            Point currentPoint = pointQueue.Dequeue();
                            for(int i = -1; i <= 1; ++i)
                            {
                                if( ((currentPoint.X + i) >= width) || ((currentPoint.X + i) < 0) )
                                {
                                    continue;
                                }

                                for(int j = -1; j <= 1; ++j)
                                {
                                    if( ((currentPoint.Y + j) >= height) || ((currentPoint.Y + j) < 0) )
                                    {
                                        continue;
                                    }

                                    if( !processedPixel[currentPoint.X + i, currentPoint.Y + j] && cannyEdge[currentPoint.X + i, currentPoint.Y + j] > lowThreshold && !edgeMap[x, y] )
                                    {
                                        edgeMap[currentPoint.X + i, currentPoint.Y + j] = true;
                                        pointQueue.Enqueue(new Point(currentPoint.X + i, currentPoint.Y + j));
                                    }

                                    processedPixel[currentPoint.X + i, currentPoint.Y + j] = true;

                                }
                            }
                        }
                    }
                }
            }

            FastBitmap result = new FastBitmap(width, height);
            ColorARGB* currentResultPosition = result.startingPosition;

            for(int x = 0; x < width; ++x)
            {
                for( int y = 0; y < height; ++y )
                {
                    currentResultPosition->A = 255;

                    if( edgeMap[x, y] )
                    {
                        currentResultPosition->R = 255;
                        currentResultPosition->G = 255;
                        currentResultPosition->B = 255;
                    }
                    else
                    {
                        currentResultPosition->R = 0;
                        currentResultPosition->G = 0;
                        currentResultPosition->B = 0;
                    }

                    ++currentResultPosition;
                }
            }

            return result;

        }

        */


        #region ICloneable Members

        public object Clone()
        {
            if( startingPosition == null )
            {
                return new FastBitmap(bitmap);
            }
            else
            {
                Unlock();
                FastBitmap temp = new FastBitmap(bitmap);
                Lock();
                return temp;
            }
        }

        #endregion


        public float[,] MedianIntensitySmoothSubimageArray(int maskWidth, int x, int y, int width, int height)
        {
            if( (maskWidth % 2) == 0 )
            {
                throw new ArgumentException("MedianIntensitySmoothSubimage: maskWidth must be an odd integer");
            }
            float[,] result = new float[width, height];

            for( int i = 0; i < width; ++i )
            {
                for( int j = 0; j < height; ++j )
                {
                    result[i, j] = GetIntensityMedian(x + i, y + j, maskWidth);
                }
            }

            return result;
        }

        public static float[,] SobelGradient(float[,] intensities, int width, int height, float[,] angles)
        {
            float[,] results = new float[width, height];

            for( int y = 0; y < height; ++y )
            {
                for( int x = 0; x < width; ++x )
                {
                    if( (x == 0) || (y == 0) || (x == width - 1) || (y == height - 1) )
                    {
                        results[x, y] = 0;
                    }
                    else
                    {
                        float xGradient = intensities[x - 1, y - 1] + 2 * intensities[x - 1, y] + intensities[x - 1, y + 1] - intensities[x + 1, y - 1] - 2 * intensities[x + 1, y] - intensities[x + 1, y + 1];
                        float yGradient = intensities[x - 1, y - 1] + 2 * intensities[x, y - 1] + intensities[x + 1, y - 1] - intensities[x - 1, y + 1] - 2 * intensities[x, y + 1] - intensities[x + 1, y + 1];
                        results[x, y] = (float)Math.Min(Math.Sqrt(xGradient * xGradient + yGradient * yGradient), 255);

                        if( angles != null )
                        {
                            if( xGradient != 0 )
                            {
                                angles[x, y] = (float)(Math.Atan(yGradient / xGradient) * 180.0 / Math.PI);
                                if( xGradient > 0 )
                                {
                                    if( yGradient < 0 )
                                    {
                                        angles[x, y] += 360.0f;
                                    }
                                }
                                else
                                {
                                    angles[x, y] += 180.0f;
                                }

                                angles[x, y] = angles[x, y] % 360;
                            }
                            else
                            {
                                if( yGradient > 0 )
                                {
                                    angles[x, y] = 90;
                                }
                                else
                                {
                                    angles[x, y] = 270;
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }

        public float[,] IntensityArray(int x, int y, int width, int height)
        {
            float[,] result = new float[width, height];

            for( int i = 0; i < width; ++i )
            {
                for( int j = 0; j < height; ++j )
                {
                    result[i, j] = GetIntensity(x + i, y + j);
                }
            }

            return result;
        }

        public NccTemplate GetNccTemplate(int x, int y, int width, int height)
        {
            return new NccTemplate(this, x, y, width, height);
        }

        public NccTemplate GetNccTemplate(Rectangle templateArea)
        {
            return new NccTemplate(this, templateArea.X, templateArea.Y, templateArea.Width, templateArea.Height);
        }

        public NccTemplate GetNccTemplate(Point loc, Size size)
        {
            return new NccTemplate(this, loc.X, loc.Y, size.Width, size.Height);
        }

        public NccTemplate GetNccTemplate(Point loc, int width, int height)
        {
            return new NccTemplate(this, loc.X, loc.Y, width, height);
        }

        public float GetNccScore(NccTemplate template, Point loc)
        {
            return GetNccScore(template, loc.X, loc.Y);
        }

        public float GetNccScore(NccTemplate template, int x, int y)
        {
            if( x + template.Width >= Width || x < 0 || y + template.Height >= Height || y < 0 )
            {
                throw new ArgumentOutOfRangeException("Template dimensions excede image dimensions");
            }

            float[] intensityTemplate = new float[template.Width * template.Height];


            float squaredMean = 0;
            float mean = 0;
            int index = 0;

            for( int j = 0; j < template.Height; ++j )
            {
                for( int i = 0; i < template.Width; ++i )
                {
                    float intensity = GetIntensity(x + i, y + j);
                    intensityTemplate[index++] = intensity;
                    mean += intensity;
                    squaredMean += (intensity * intensity);
                }
            }

            mean = mean / template.Area;

            return template.GetNcc(intensityTemplate, mean, (float)Math.Sqrt((squaredMean / template.Area) - (mean * mean)));
        }

        public float GetNccScore(NccTemplate template, Rectangle searchArea, out Point bestMatchLoc)
        {
            return GetNccScore(template, searchArea, 1, out bestMatchLoc);
        }

        public float GetNccScore(NccTemplate template, Rectangle searchArea, int stepAmount, out Point bestMatchLoc)
        {
            Point[] pointArray = new Point[1];
            float[] nccs = GetNccScore(template, searchArea, stepAmount, pointArray);
            bestMatchLoc = pointArray[0];
            return nccs[0];
        }

        static void UpdateNccAndPointArray(float[] nccs, Point[] points, float newNcc, Point newPoint)
        {
            int newLoc = 0;

            while( newLoc < nccs.Length )
            {
                if( newNcc > nccs[newLoc] )
                {
                    break;
                }
                ++newLoc;
            }

            if( newLoc < nccs.Length )
            {
                for( int i = nccs.Length - 1; i > newLoc; --i )
                {
                    nccs[i] = nccs[i - 1];
                    points[i] = points[i - 1];
                }

                nccs[newLoc] = newNcc;
                points[newLoc] = newPoint;
            }
        }

        public float[] GetNccScore(NccTemplate template, Rectangle searchArea, int stepAmount, Point[] bestPoints)
        {
            if( searchArea.Width == 0 || searchArea.Height == 0 )
            {
                throw new ArgumentException("Search area width and height must be greater than 0");
            }


            if( searchArea.Width == 1 && searchArea.Height == 1 )
            {
                bestPoints[0] = searchArea.Location;

                return new float[] { GetNccScore(template, searchArea.X, searchArea.Y) };
            }

            if( searchArea.X < 0 )
            {
                searchArea.Width += searchArea.X;
                searchArea.X = 0;
            }

            if( searchArea.Y < 0 )
            {
                searchArea.Height += searchArea.Y;
                searchArea.Y = 0;
            }

            if( searchArea.Width >= Width - template.Width - searchArea.X )
            {
                searchArea.Width = Width - template.Width - searchArea.X - 1;
            }

            if( searchArea.Height >= Height - template.Height - searchArea.Y )
            {
                searchArea.Height = Height - template.Height - searchArea.Y - 1;
            }

            float tempNcc;

            float[] bestNccs = new float[bestPoints.Length];

            for( int i = 0; i < bestPoints.Length; ++i )
            {
                bestNccs[i] = float.NegativeInfinity;
            }


            float topMostMeanSum = 0;
            float currentMeanSum = 0;

            float topMostMeanSquaredSum = 0;
            float currentMeanSquaredSum = 0;

            int intensityMapWidth = searchArea.Width - 1 + template.Width;
            int intensityMapHeight = searchArea.Height - 1 + template.Height;
            float[,] intensityMap = new float[intensityMapWidth, intensityMapHeight];

            for( int x = 0; x < intensityMapWidth; ++x )
            {
                for( int y = 0; y < intensityMapHeight; ++y )
                {
                    intensityMap[x, y] = GetIntensity(searchArea.X + x, searchArea.Y + y);
                }
            }

            for( int x = 0; x < searchArea.Width; ++x )
            {
                if( x == 0 )
                {
                    for( int i = x; i < template.Width; ++i )
                    {
                        for( int j = 0; j < template.Height; ++j )
                        {
                            float intensity = intensityMap[i, j];
                            topMostMeanSum += intensity;
                            topMostMeanSquaredSum += (intensity * intensity);
                        }
                    }
                }
                else
                {
                    int xRemoved = x - 1;
                    for( int j = 0; j < template.Height; ++j )
                    {
                        float intensitySubtract = intensityMap[xRemoved, j];
                        float intensityAdd = intensityMap[xRemoved + template.Width, j];

                        topMostMeanSum -= intensitySubtract;
                        topMostMeanSum += intensityAdd;

                        topMostMeanSquaredSum -= (intensitySubtract * intensitySubtract);
                        topMostMeanSquaredSum += (intensityAdd * intensityAdd);
                    }
                }

                if( x % stepAmount == 0 )
                {
                    float topMostMean = topMostMeanSum / template.Area;

                    tempNcc = template.GetNcc(intensityMap, x, 0, topMostMean, (float)Math.Sqrt((topMostMeanSquaredSum / template.Area) - (topMostMean * topMostMean)));

                    UpdateNccAndPointArray(bestNccs, bestPoints, tempNcc, new Point(x, 0));

                    if( searchArea.Height > 1 )
                    {
                        currentMeanSum = topMostMeanSum;
                        currentMeanSquaredSum = topMostMeanSquaredSum;

                        for( int y = 1; y < searchArea.Height; ++y )
                        {
                            int yRemoved = y - 1;
                            for( int i = x; i < template.Width; ++i )
                            {
                                float intensitySubtract = intensityMap[i, yRemoved];
                                float intensityAdd = intensityMap[i, yRemoved + template.Height];

                                currentMeanSum -= intensitySubtract;
                                currentMeanSum += intensityAdd;

                                currentMeanSquaredSum -= (intensitySubtract * intensitySubtract);
                                currentMeanSquaredSum += (intensityAdd * intensityAdd);
                            }

                            if( y % stepAmount == 0 )
                            {
                                float mean = currentMeanSum / template.Area;

                                tempNcc = template.GetNcc(intensityMap, x, y, mean, (float)Math.Sqrt((currentMeanSquaredSum / template.Area) - (mean * mean)));

                                UpdateNccAndPointArray(bestNccs, bestPoints, tempNcc, new Point(x, y));
                            }
                        }
                    }
                }

            }


            for( int i = 0; i < bestPoints.Length; ++i )
            {
                bestPoints[i].X = bestPoints[i].X + searchArea.X;
                bestPoints[i].Y = bestPoints[i].Y + searchArea.Y;
            }

            return bestNccs;
        }

        public NccTemplate[] GetNccTemplates(Size templateSize, Rectangle searchArea, int stepAmount)
        {
            if( searchArea.Width == 0 || searchArea.Height == 0 )
            {
                throw new ArgumentException("Search area width and height must be greater than 0");
            }

            if( searchArea.X < 0 || searchArea.Y < 0 || searchArea.Left + templateSize.Width >= Width
                || searchArea.Bottom + templateSize.Height >= Height )
            {
                throw new ArgumentOutOfRangeException("Template dimensions excede image dimensions"
                    + "\nsearchArea.X: " + searchArea.X
                    + "\nsearchArea.Y: " + searchArea.Y
                    + "\nsearchArea.Left: " + searchArea.Left
                    + "\ntemplate.Width: " + templateSize.Width
                    + "\nWidth: " + Width
                    + "\nsearchArea.Bottom: " + searchArea.Bottom
                    + "\ntemplate.Height: " + templateSize.Height
                    + "\nHeight: " + Height);
            }

            int xSteps = searchArea.Width / stepAmount;
            int ySteps = searchArea.Height / stepAmount;

            if( searchArea.Width % stepAmount != 0 )
            {
                xSteps++;
            }

            if( searchArea.Height % stepAmount != 0 )
            {
                ySteps++;
            }

            NccTemplate[] templates = new NccTemplate[(xSteps * ySteps)];

            int xEnd = searchArea.Right;
            int yEnd = searchArea.Bottom;
            int templateWidth = templateSize.Width;
            int templateHeight = templateSize.Height;
            int index = 0;

            for( int i = searchArea.X; i < xEnd; i += stepAmount )
            {
                for( int j = searchArea.Y; j < yEnd; j += stepAmount )
                {
                    templates[index++] = new NccTemplate(this, i, j, templateWidth, templateHeight);
                }
            }

            return templates;
        }
    }
}