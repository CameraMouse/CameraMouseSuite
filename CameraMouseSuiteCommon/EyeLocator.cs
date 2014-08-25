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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CameraMouseSuite
{

    public enum AutoStartMode
    {
        None,
        LeftEye,
        RightEye,
        NoseMouth
    }

    public class EyeLocator : IDisposable
    {
        private class EyeData
        {
            public Point loc;
            public Rectangle boundingBox;

            public EyeData(Point l, Rectangle bb)
            {
                loc = l;
                boundingBox = bb;
            }
        }

        private class BlinkVector
        {
            private float[] elements;
            private int leftEyeLabel;
            private int rightEyeLabel;

            public int LeftEyeLabel
            {
                get
                {
                    return leftEyeLabel;
                }
            }

            public int RightEyeLabel
            {
                get
                {
                    return rightEyeLabel;
                }
            }

            public BlinkVector(int leftEyeLabel, int rightEyeLabel, FastBitmap.ConnectedComponentsResults ccResults)
            {
                this.leftEyeLabel = leftEyeLabel;
                this.rightEyeLabel = rightEyeLabel;
                elements = new float[10];

                Point leftEyeLoc = ccResults.Centers[leftEyeLabel];
                Point rightEyeLoc = ccResults.Centers[rightEyeLabel];

                Point DiffVec = new Point(leftEyeLoc.X - rightEyeLoc.X, leftEyeLoc.Y - rightEyeLoc.Y);

                float distance = (float)Math.Sqrt((DiffVec.X * DiffVec.X) + (DiffVec.Y * DiffVec.Y));

                /*
                elements[0] = leftArea / distance;
                elements[1] = rightArea / distance;
                elements[2] = yDisplacement / xDisplacement;
                elements[3] = distance;
                elements[4] = (leftYMax - leftYMin) / distance;
                elements[5] = (leftXMax - leftXMin) / distance;
                elements[6] = (rightYMax - rightYMin) / distance;
                elements[7] = (rightXMax - rightXMin) / distance;
                elements[8] = elements[4] / elements[5];
                elements[9] = elements[6] / elements[7];
                */


                elements[0] = ccResults.Sizes[leftEyeLabel] / distance;
                elements[1] = ccResults.Sizes[rightEyeLabel] / distance;

                if( DiffVec.X == 0 )
                {
                    elements[2] = float.PositiveInfinity;
                }
                else
                {
                    elements[2] = ((float)Math.Abs(DiffVec.Y)) / ((float)Math.Abs(DiffVec.X));
                }
                elements[3] = distance;
                elements[4] = (ccResults.BoundingBoxes[leftEyeLabel].Height) / distance;
                elements[5] = (ccResults.BoundingBoxes[leftEyeLabel].Width) / distance;
                elements[6] = (ccResults.BoundingBoxes[rightEyeLabel].Height) / distance;
                elements[7] = (ccResults.BoundingBoxes[rightEyeLabel].Width) / distance;
                elements[8] = elements[4] / elements[5];
                elements[9] = elements[6] / elements[7];
            }

            public float GetMahalanobisDistanceSquared(float[] averageVector, float[,] inverseCovarianceMatrix)
            {
                int length = elements.Length;
                float[] diffVec = new float[length];
                float[] tempMat = new float[length];
                float resultSquared = 0;

                for( int i = 0; i < elements.Length; ++i )
                {
                    diffVec[i] = elements[i] - averageVector[i];
                }

                for( int i = 0; i < length; ++i )
                {
                    for( int j = 0; j < length; ++j )
                    {
                        tempMat[i] += (diffVec[j] * inverseCovarianceMatrix[i, j]);
                    }
                }

                for( int i = 0; i < length; ++i )
                {
                    resultSquared += (tempMat[i] * diffVec[i]);
                }

                return resultSquared;
            }
        }

        private static float[,] InverseCovarianceMatrix = 
                {
                    {47.8188170006076f,	-31.8863580398749f,	18.8295958029369f,	-0.141928474553974f,	-377.621354443724f,	-84.7669973073686f,	199.879668036092f,	35.5669380347106f,	22.1018942528971f,	-12.2580482893774f},
                    {-31.8863580398749f,	65.3850810736188f,	5.41737934548177f,	-0.324012413868203f,	181.723287166709f,	61.0251744744206f,	-490.263927284560f,	-96.4713124766619f,	-10.8701162625795f,	25.6233807821722f},
                    {18.8295958029369f,	5.41737934548177f,	795.546582634168f,	-0.248470092905520f,	-239.520863931407f,	-56.6495251376594f,	-43.0282137263588f,	1.40729551529020f,	3.52061711204502f,	-8.30632383979022f},
                    {-0.141928474553974f,	-0.324012413868203f,	-0.248470092905520f,	0.0216839461430628f,	2.52819347453318f,	-0.472270238829971f,	5.00498219989456f,	-0.391485327965681f,	-0.192737288027323f,	-0.249285285530788f},
                    {-377.621354443724f,	181.723287166709f,	-239.520863931407f,	2.52819347453318f,	10110.2996292900f,	-2298.38935360313f,	-1734.90527266992f,	167.398716827179f,	-1439.55442105198f,	173.906124058967f},
                    {-84.7669973073686f,	61.0251744744206f,	-56.6495251376594f,	-0.472270238829971f,	-2298.38935360313f,	1751.17063065129f,	-156.600690530943f,	-376.516480277290f,	524.532068283182f,	-32.5644694096612f},
                    {199.879668036092f,	-490.263927284560f,	-43.0282137263588f,	5.00498219989456f,	-1734.90527266992f,	-156.600690530943f,	12441.2428394632f,	-2793.73826312386f,	173.696796617739f,	-1588.35113727332f},
                    {35.5669380347106f,	-96.4713124766619f,	1.40729551529020f,	-0.391485327965681f,	167.398716827179f,	-376.516480277290f,	-2793.73826312386f,	1977.73181107894f,	-61.7491584814378f,	564.079918572846f},
                    {22.1018942528971f,	-10.8701162625795f,	3.52061711204502f,	-0.192737288027323f,	-1439.55442105198f,	524.532068283182f,	173.696796617739f,	-61.7491584814378f,	271.058150516767f,	-27.4554705900014f},
                    {-12.2580482893774f,	25.6233807821722f,	-8.30632383979022f,	-0.249285285530788f,	173.906124058967f,	-32.5644694096612f,	-1588.35113727332f,	564.079918572846f,	-27.4554705900014f,	272.690753987982f}
                };

        private static float[] AverageVector = { 0.754271f, 0.683835f, 0.03645018f, 63.87263f, 0.08013105f, 0.1989242f, 0.07420406f, 0.1854712f, 0.4185668f, 0.4166945f };

        private const int   TrackingPointWidth                = 11;
        private const int   MaxNumberOfConnectedComponents    = 5;
        private const float SizeRatioThreadshold              = 3f;
        private const float DisplacementRatioThreshold        = 1.2f;
        private const float StartMinimumNcc                   = 0.85f;
        private const float EndMinimumNcc                     = 0.75f;
        private const float StartMahalanobisDistanceThreshold = 10f;
        private const float EndMahalanobisDistanceThreshold   = 25f;
        private const float ThresholdDecreaseStartTime        = 5f;
        private const float ThresholdDecreaseTime             = 10f;

        private volatile FastBitmap[]    imageArray;
        private volatile FastBitmap[]    workerImageArray;
        private          EventWaitHandle imageArrayWaitHandler;
        private          object          imageArrayMutex;
        private volatile int             nextIndex;
        private          Thread          eyeFinderThread;
        private volatile bool            trackingPointsFound;
        private          CvPoint2D32f    leftEyePoint;
        private          CvPoint2D32f    rightEyePoint;
        private          CvPoint2D32f    leftEyeTrackingPoint;
        private          CvPoint2D32f    rightEyeTrackingPoint;
        private          CvPoint2D32f    mouseTrackingPoint;
        private          float           thresholdElapsedTime;
        private          Stopwatch       stopwatch;

        public EyeLocator(int imageArraySize)
        {
            imageArray = new FastBitmap[imageArraySize];
            workerImageArray = new FastBitmap[imageArraySize];
            imageArrayWaitHandler = new AutoResetEvent(false);
            nextIndex = 0;
            imageArrayMutex = new object();
            thresholdElapsedTime = -1;
            stopwatch = new Stopwatch();
            //Reset();
        }

        #region Properties

        public bool TrackingPointsFound
        {
            get
            {
                return trackingPointsFound;
            }
        }

        public CvPoint2D32f LeftEyePoint
        {
            get
            {
                return leftEyePoint;
            }
        }

        public CvPoint2D32f RightEyePoint
        {
            get
            {
                return rightEyePoint;
            }
        }

        public CvPoint2D32f LeftEyeTrackingPoint
        {
            get
            {
                return leftEyeTrackingPoint;
            }
        }

        public CvPoint2D32f RightEyeTrackingPoint
        {
            get
            {
                return rightEyeTrackingPoint;
            }
        }

        public CvPoint2D32f MouseTrackingPoint
        {
            get
            {
                return mouseTrackingPoint;
            }
        }

        private float ThresholdAlpha
        {
            get
            {
                float result = 1 - ((thresholdElapsedTime - ThresholdDecreaseStartTime) / ThresholdDecreaseTime);

                if( result < 0 )
                {
                    result = 0;
                }
                else if( result > 1 )
                {
                    result = 1;
                }

                return result;
            }
        }

        private float MinimumNcc
        {
            get
            {
                float alpha = ThresholdAlpha;
                return (alpha * StartMinimumNcc) + ((1 - alpha) * EndMinimumNcc);
            }
        }

        private float MahalanobisDistanceSquaredThreshold
        {
            get
            {
                float threshold = MahalanobisDistanceThreshold;
                return threshold * threshold;
            }
        }

        private float MahalanobisDistanceThreshold
        {
            get
            {
                float alpha = ThresholdAlpha;
                return (alpha * StartMahalanobisDistanceThreshold) + ((1 - alpha) * EndMahalanobisDistanceThreshold);
            }
        }

        #endregion

        private void eyeFinderThreadMain()
        {
            while( !trackingPointsFound )
            {
                int firstImageIndex;
                FastBitmap[] tempWorkerImageArray;
                int threadid = Thread.CurrentThread.ManagedThreadId;
                imageArrayWaitHandler.WaitOne();

                lock( imageArrayMutex )
                {
                    firstImageIndex = nextIndex;
                    nextIndex = 0;
                    tempWorkerImageArray = imageArray;
                    imageArray = new FastBitmap[workerImageArray.Length];
                }

                for( int i = 0; i < workerImageArray.Length; ++i )
                {
                    workerImageArray[i] = tempWorkerImageArray[(firstImageIndex + i) % workerImageArray.Length];
                }

                bool arrayFull = true;

                for( int i = 0; i < workerImageArray.Length; ++i )
                {
                    if( workerImageArray[i] == null )
                    {
                        arrayFull = false;
                        break;
                    }
                }

                if( arrayFull )
                {
                    trackingPointsFound = FindTrackingPoints();
                }

                for( int i = 0; i < workerImageArray.Length; ++i )
                {
                    if( workerImageArray[i] != null )
                    {
                        workerImageArray[i].Dispose();
                    }
                }
            }
        }
        public void AddImage(Bitmap img)
        {
            if( !TrackingPointsFound )
            {
                lock( imageArrayMutex )
                {
                    if( imageArray[nextIndex] != null )
                    {
                        imageArray[nextIndex].Dispose();
                    }

                    imageArray[nextIndex++] = new FastBitmap(img);
                    if( nextIndex == imageArray.Length )
                    {
                        imageArrayWaitHandler.Set();
                    }
                    nextIndex = nextIndex % imageArray.Length;
                }
            }
        }
        private bool FindTrackingPoints()
        {
            EyeData leftEyeData, rightEyeData;

            if( !FindEyes(out leftEyeData, out rightEyeData) )
            {
                return false;
            }

            leftEyePoint = new CvPoint2D32f(leftEyeData.loc.X, leftEyeData.loc.Y);
            rightEyePoint = new CvPoint2D32f(rightEyeData.loc.X, rightEyeData.loc.Y);
            try
            {
                GetTrackingPoints(workerImageArray[workerImageArray.Length - 1], leftEyeData, rightEyeData);
            }
            catch( Exception )
            {
                return false;
            }

            return true;
        }
        private void GetTrackingPoints(FastBitmap img, EyeData leftEye, EyeData rightEye)
        {
            const int   YDownShift = 30;
            const int   XExpand    = 5;
            const float eyebrowMult = 0.6f;

            int eyebrowWidth = (int)Math.Abs((leftEye.loc.X - rightEye.loc.X) * eyebrowMult);
            Rectangle  searchArea     = new Rectangle();
            FastBitmap smoothed       = img.GaussianSmooth(5);
            FastBitmap sobel          = smoothed.SobelGradient();

            smoothed.Dispose();

            searchArea.Y = Math.Max(leftEye.loc.Y, rightEye.loc.Y) + YDownShift;
            searchArea.X = leftEye.loc.X - XExpand;
            searchArea.Width = rightEye.loc.X - leftEye.loc.X + (2 * XExpand);
            searchArea.Height = (int)((rightEye.loc.X - leftEye.loc.X) * 0.6f);

            float[,] trackingPointScores = new float[searchArea.Width, searchArea.Height];

            for( int x = searchArea.Left; x < searchArea.Right; ++x )
            {
                for( int y = searchArea.Top; y < searchArea.Bottom; ++y )
                {
                    float score = 0;

                    for( int i = 0; i < TrackingPointWidth; ++i )
                    {
                        for( int j = 0; j < TrackingPointWidth; ++j )
                        {
                            score += sobel.GetIntensity(x + i, y + j);
                        }
                    }
                    trackingPointScores[x - searchArea.Left, y - searchArea.Top] = score;
                }
            }

            mouseTrackingPoint = GetBestTrackingPointWithinColumn(trackingPointScores, searchArea, (leftEye.loc.X + rightEye.loc.X) / 2);
            leftEyeTrackingPoint = GetBestTrackingForEye(sobel, leftEye, eyebrowWidth);
            rightEyeTrackingPoint = GetBestTrackingForEye(sobel, rightEye, eyebrowWidth);

            sobel.Dispose();
        }
        private CvPoint2D32f GetBestTrackingPointWithinColumn(float[,] scores, Rectangle searchArea, int x)
        {
            float  bestScore = float.NegativeInfinity;
            CvPoint2D32f  bestPoint = new CvPoint2D32f();

            for( int i = x - 2; i <= x + 2; ++i )
            {
                for( int j = 0; j < searchArea.Height; ++j )
                {
                    if( scores[i - searchArea.Left, j] > bestScore )
                    {
                        bestScore = scores[i - searchArea.Left, j];
                        bestPoint.x = i;
                        bestPoint.y = j + searchArea.Top;
                    }
                }
            }

            return bestPoint;
        }
        private unsafe CvPoint2D32f GetBestTrackingForEye(FastBitmap sobel, EyeData eyeData, int eyebrowWidth)
        {
            const float HeightMultiplier = 0.5f;
            const float YIncrease   = 0.7f;
            const float WidthMultiplier = 0.5f;

            float bestScore = float.NegativeInfinity;
            CvPoint2D32f bestPoint = new CvPoint2D32f();

            Rectangle searchArea = new Rectangle(eyeData.boundingBox.X + (int)((eyeData.boundingBox.Width / 2) - (WidthMultiplier * eyebrowWidth / 2)), (int)(eyeData.boundingBox.Y - (eyebrowWidth * YIncrease)), (int)(eyebrowWidth * WidthMultiplier), (int)(eyebrowWidth * HeightMultiplier));

            for( int x = searchArea.Left; x < searchArea.Right; ++x )
            {
                for( int y = searchArea.Top; y < searchArea.Bottom; ++y )
                {
                    float score = 0;

                    for( int i = 0; i < TrackingPointWidth; ++i )
                    {
                        for( int j = 0; j < TrackingPointWidth; ++j )
                        {
                            score += sobel.GetIntensity(x + i, y + j);
                        }
                    }

                    if( score > bestScore )
                    {
                        bestScore = score;
                        bestPoint = new CvPoint2D32f(x, y);
                    }
                }
            }

            return bestPoint;
        }
        private unsafe bool FindEyesFromDifferenceImage(FastBitmap img, FastBitmap differenceImage, out EyeData leftEye, out EyeData rightEye)
        {
            leftEye = null;
            rightEye = null;

            if( thresholdElapsedTime < 0 )
            {
                thresholdElapsedTime = 0;
                stopwatch.Reset();
                stopwatch.Start();
            }
            else
            {
                thresholdElapsedTime += stopwatch.ElapsedMilliseconds / 1000f;
                stopwatch.Reset();
                stopwatch.Start();
            }

            FastBitmap.ConnectedComponentsResults ccResults = differenceImage.FindConnectedComponents(100, 2, 10);


            if( ccResults.NumberOfComponents >= 2 && ccResults.NumberOfComponents <= MaxNumberOfConnectedComponents )
            {
                LinkedList<BlinkVector> vectors = GetBlinkVectors(ccResults);

                if( vectors.Count > 0 )
                {
                    float minDistanceSquared = float.PositiveInfinity;
                    BlinkVector bestVector = null;
                    float mahalanobisDistanceSquaredThreshold = MahalanobisDistanceSquaredThreshold;
                    float minimumNcc = MinimumNcc;

                    foreach( BlinkVector v in vectors )
                    {
                        float tempDistSquared = v.GetMahalanobisDistanceSquared(AverageVector, InverseCovarianceMatrix);
                        if( tempDistSquared < minDistanceSquared )
                        {
                            bestVector = v;
                            minDistanceSquared = tempDistSquared;
                        }
                    }

                    if( minDistanceSquared <= mahalanobisDistanceSquaredThreshold )
                    {
                        try
                        {
                            float ncc = HorizontalFlipNccTemplateMatch(img, ccResults.Centers[bestVector.LeftEyeLabel], ccResults.Centers[bestVector.RightEyeLabel]);

                            if( ncc > minimumNcc )
                            {
                                leftEye = new EyeData(ccResults.Centers[bestVector.LeftEyeLabel], ccResults.BoundingBoxes[bestVector.LeftEyeLabel]);
                                rightEye = new EyeData(ccResults.Centers[bestVector.RightEyeLabel], ccResults.BoundingBoxes[bestVector.RightEyeLabel]);
                                return true;
                            }
                            return false;
                        }
                        catch( ArgumentOutOfRangeException ) { }
                    }
                }
            }
            return false;
        }

        private static LinkedList<BlinkVector> GetBlinkVectors(FastBitmap.ConnectedComponentsResults ccResults)
        {
            LinkedList<BlinkVector> vectors = new LinkedList<BlinkVector>();

            for( int i = 0; i < ccResults.NumberOfComponents; ++i )
            {
                for( int j = i + 1; j < ccResults.NumberOfComponents; ++j )
                {
                    int leftEyeLabel = ccResults.Centers[i].X < ccResults.Centers[j].X ? i : j;
                    int rightEyeLabel = leftEyeLabel == i ? j : i;

                    if( PossibleEyeVector(leftEyeLabel, rightEyeLabel, ccResults) )
                    {
                        vectors.AddLast(new BlinkVector(leftEyeLabel, rightEyeLabel, ccResults));
                    }
                }
            }

            return vectors;
        }

        private static bool PossibleEyeVector(int leftEyeLabel, int rightEyeLabel, FastBitmap.ConnectedComponentsResults ccResults)
        {
            float sizeRatio = ((float)ccResults.Sizes[leftEyeLabel]) / ((float)ccResults.Sizes[rightEyeLabel]);

            if( sizeRatio > SizeRatioThreadshold || sizeRatio < (1f / SizeRatioThreadshold) )
            {
                return false;
            }

            Point leftEyeLoc = ccResults.Centers[leftEyeLabel];
            Point rightEyeLoc = ccResults.Centers[rightEyeLabel];

            Point DiffVec = new Point(leftEyeLoc.X - rightEyeLoc.X, leftEyeLoc.Y - rightEyeLoc.Y);

            if( DiffVec.X == 0 )
            {
                return false;
            }

            if( Math.Abs(DiffVec.Y) / Math.Abs(DiffVec.X) > DisplacementRatioThreshold )
            {
                return false;
            }

            return true;
        }

        /* private static unsafe bool FindEyesFromDifferenceImage(FastBitmap img, FastBitmap differenceImage, out EyeData leftEye, out EyeData rightEye)
        {
            leftEye = null;
            rightEye = null;

            try
            {
                const int DistanceBetweenLabels   = 40;
                const float MinimumNcc            = 0.7f;
                FastBitmap.ConnectedComponentsResults ccResults = differenceImage.FindConnectedComponents(100, 2, 10);


                if( ccResults.NumberOfComponents >= 2 )
                {
                    int   largestLabel           = -1;
                    float largestLabelSize       = -1;
                    int   secondLargestLabel     = -1;
                    float secondLargestLabelSize = -1;

                    for( int i = 0; i < ccResults.NumberOfComponents; ++i )
                    {
                        if( ccResults.Sizes[i] > largestLabelSize )
                        {
                            largestLabelSize = ccResults.Sizes[i];
                            largestLabel = i;
                        }
                    }


                    if( largestLabelSize > 600 )
                    {
                        return false;
                    }

                    for( int i = 0; i < ccResults.NumberOfComponents; ++i )
                    {
                        if( ccResults.Sizes[i] > secondLargestLabelSize )
                        {
                            int xDiff = ccResults.Centers[largestLabel].X - ccResults.Centers[i].X;
                            int yDiff = ccResults.Centers[largestLabel].Y - ccResults.Centers[i].Y;

                            float distance = (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

                            if( distance > DistanceBetweenLabels )
                            {
                                secondLargestLabelSize = ccResults.Sizes[i];
                                secondLargestLabel = i;
                            }
                        }
                    }

                    if( secondLargestLabel == -1 )
                    {
                        return false;
                    }


                    if( ccResults.BoundingBoxes[largestLabel].Height > ccResults.BoundingBoxes[largestLabel].Width )
                    {
                        return false;
                    }

                    if( ccResults.BoundingBoxes[secondLargestLabel].Height > ccResults.BoundingBoxes[secondLargestLabel].Width )
                    {
                        return false;
                    }

                    float FinalXDiff = Math.Abs(ccResults.Centers[largestLabel].X - ccResults.Centers[secondLargestLabel].X);
                    float FinalYDiff = Math.Abs(ccResults.Centers[largestLabel].Y - ccResults.Centers[secondLargestLabel].Y);

                    if( FinalYDiff > 40 )
                    {
                        return false;
                    }
                    if( (FinalYDiff / FinalXDiff) > 0.5 )
                    {
                        return false;
                    }

                    float ncc = HorizontalFlipNccTemplateMatch(img, ccResults.Centers[largestLabel], ccResults.Centers[secondLargestLabel]);

                    if( ncc < MinimumNcc )
                    {
                        return false;
                    }

                    EyeData tempOne = new EyeData(ccResults.Centers[largestLabel], 
                        ccResults.BoundingBoxes[largestLabel]);

                    EyeData tempTwo = new EyeData(ccResults.Centers[secondLargestLabel], 
                        ccResults.BoundingBoxes[secondLargestLabel]);

                    if( tempOne.loc.X < tempTwo.loc.X )
                    {
                        leftEye  = tempOne;
                        rightEye = tempTwo;
                    }
                    else
                    {
                        leftEye  = tempTwo;
                        rightEye = tempOne;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch( ArgumentOutOfRangeException )
            {
                return false;
            }
        }
        */
        // Makes the template from pOne tries to match it to pTwo
        private static float HorizontalFlipNccTemplateMatch(FastBitmap img, Point pOne, Point pTwo)
        {
            const int TemplateWidth = 31;
            const int HalfTemplateWidth = (TemplateWidth - 1) / 2;
            const int TemplateHeight = 21;
            const int HalfTemplateHeight = (TemplateHeight - 1) / 2;
            const int SearchWidth = 31;
            const int HalfSearchWidth = (SearchWidth - 1) / 2;

            float[,] template      = new float[TemplateWidth, TemplateHeight];
            float    templateMean  = 0;
            float    templateSigma = 0;
            float    bestNcc       = 0;


            for( int x = 0; x < TemplateWidth; ++x )
            {
                for( int y = 0; y < TemplateHeight; ++y )
                {
                    template[TemplateWidth - x - 1, y] = img.GetIntensity(pOne.X - HalfTemplateWidth + x, pOne.Y - HalfTemplateHeight + y);
                    templateMean += template[TemplateWidth - x - 1, y];
                }
            }

            templateMean /= (TemplateWidth * TemplateHeight);

            for( int x = 0; x < TemplateWidth; ++x )
            {
                for( int y = 0; y < TemplateHeight; ++y )
                {
                    float diff = template[x, y] - templateMean;
                    templateSigma += (diff * diff);
                }
            }

            templateSigma = (float)Math.Sqrt(templateSigma / (TemplateWidth * TemplateHeight));

            for( int x = pTwo.X - HalfSearchWidth; x <= pTwo.X + HalfSearchWidth; ++x )
            {
                for( int y = pTwo.Y - HalfSearchWidth; y <= pTwo.Y + HalfSearchWidth; ++y )
                {
                    float mean = 0;
                    float sigma = 0;
                    float ncc = 0;

                    for( int i = 0; i < TemplateWidth; ++i )
                    {
                        for( int j = 0; j < TemplateHeight; ++j )
                        {
                            mean += img.GetIntensity(x - HalfTemplateWidth + i, y - HalfTemplateHeight + j);
                        }
                    }

                    mean /= (TemplateWidth * TemplateHeight);

                    for( int i = 0; i < TemplateWidth; ++i )
                    {
                        for( int j = 0; j < TemplateHeight; ++j )
                        {
                            float diff = img.GetIntensity(x - HalfTemplateWidth + i, y - HalfTemplateHeight + j) - mean;
                            sigma += (diff * diff);
                        }
                    }

                    sigma = (float)Math.Sqrt(sigma / (TemplateWidth * TemplateHeight));

                    // r = 1/n Σ_i (((s_i - mean(s)) * (m_i- mean(m))) / (σ_s σ_m)

                    for( int i = 0; i < TemplateWidth; ++i )
                    {
                        for( int j = 0; j < TemplateHeight; ++j )
                        {
                            ncc += (mean - img.GetIntensity(x - HalfTemplateWidth + i, y - HalfTemplateHeight + j)) * (templateMean - template[i, j]);
                        }
                    }

                    ncc = Math.Abs(ncc / (TemplateWidth * TemplateHeight * sigma * templateSigma));

                    if( ncc > bestNcc )
                    {
                        bestNcc = ncc;
                    }
                }
            }

            return bestNcc;
        }
        private unsafe bool FindEyes(out EyeData leftEye, out EyeData rightEye)
        {
            const byte DifferenceThreshold = 25;
            int[,] changeCount = new int[workerImageArray[0].Width, workerImageArray[0].Height];

            for( int i = 0; i < workerImageArray.Length - 1; ++i )
            {
                FastBitmap differenceImage = workerImageArray[i].GetDifferenceImage(workerImageArray[i + 1], DifferenceThreshold);
                FastBitmap erosionImage = differenceImage.ApplyCrossErosionMask();

                ColorARGB* currentPosition = erosionImage.StartingPosition;
                for( int y = 0; y < erosionImage.Height; ++y )
                {
                    for( int x = 0; x < erosionImage.Width; ++x )
                    {
                        if( currentPosition->B == 255 )
                        {
                            ++changeCount[x, y];
                        }
                        ++currentPosition;
                    }
                }
                differenceImage.Dispose();
                erosionImage.Dispose();
            }

            FastBitmap finalDifferenceImage = new FastBitmap(workerImageArray[0].Width, workerImageArray[0].Height);

            ColorARGB* currentPositionResult = finalDifferenceImage.StartingPosition;
            for( int y = 0; y < finalDifferenceImage.Height; ++y )
            {
                for( int x = 0; x < finalDifferenceImage.Width; ++x )
                {
                    byte color = 0;

                    if( changeCount[x, y] > 0 )
                    {
                        color = 255;
                    }

                    currentPositionResult->A = 255;
                    currentPositionResult->R = color;
                    currentPositionResult->G = color;
                    currentPositionResult->B = color;

                    ++currentPositionResult;
                }
            }

            return FindEyesFromDifferenceImage(workerImageArray[workerImageArray.Length - 1], finalDifferenceImage, out leftEye, out rightEye);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if( eyeFinderThread != null )
            {
                try
                {
                    eyeFinderThread.Abort();
                }
                catch( Exception )
                {
                    eyeFinderThread.Resume();
                }
                eyeFinderThread.Join();
                eyeFinderThread = null;
            }
            imageArrayWaitHandler.Close();
            for( int i = 0; i < imageArray.Length; ++i )
            {
                if( imageArray[i] != null )
                {
                    imageArray[i].Dispose();
                }
            }

            for( int i = 0; i < workerImageArray.Length; ++i )
            {
                if( workerImageArray[i] != null )
                {
                    workerImageArray[i].Dispose();
                }
            }
        }

        #endregion

        public void Reset()
        {
            thresholdElapsedTime = -1;
            if( eyeFinderThread != null )
            {
                try
                {
                    eyeFinderThread.Abort();
                }
                catch( Exception )
                {
                    eyeFinderThread.Resume();
                }
                eyeFinderThread.Join();
            }

            trackingPointsFound = false;
            imageArrayWaitHandler.Reset();
            eyeFinderThread = new Thread(new ThreadStart(eyeFinderThreadMain));
            eyeFinderThread.Start();
        }
    }
}
