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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Media;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    public class NccBlinkDetector : IBlinkDetector
    {
        private        const    int    InitializationImageCount         = 20;
        private        const    int    RefinedSearchCount               = 3;
        private        const    float  ClassDifferenceThreshold         = 0.8f;
        private        const    float  XEyeDistToNccTemplateHeightRatio = 0.25f;
        private        const    float  XEyeDistToNccTemplateWidthRatio  = 0.55f;
        private        const    float  XEyeDistToSearchHeightRatio      = 0.25f;
        private        const    float  XEyeDistToSearchWidthRatio       = 0.15f;
        private        const    float  MinimumDifferenceForUpdate       = 0.02f;
        private        const    int    CloseEyeMessageStartTime         = 2500;
        private        const    string CloseEyesMessage                 = "Please close your eyes";
        private static readonly Random Rand                             = new Random();

        private FastBitmap.NccTemplate   openEyeTemplate;
        private FastBitmap.NccTemplate   closedEyeTemplate;
        private float                    closeEyeMessageTimer;
        private float                    inputOpenEyeNcc;
        private float                    inputClosedEyeNcc;
        private int                      nextInitializationImageIndex;
        private FastBitmap.NccTemplate[] initialSearchTemplates;
        private FastBitmap.NccTemplate[] refinedSearchTemplates;
        private FastBitmap.NccTemplate[] initializationTemplates;
        private float[,]                 initializationNccScores;
        private Point                    eyeOpenLoc;
        private Point                    eyeClosedLoc;
        private bool                     leftEye;
        private bool                     xDistanceBetweenEyesSet;
        private int                      stepAmount;
        private int                      xDistBetweenEyes;
        private int[]                    refinedSearchIndices;
        private float[,]                 initialSearchIntensityMap;
        private object                   templateMutex;
        private CMSTrackingSuiteAdapter  cmsTrackingSuiteAdapter;

        private int nccTemplateWidth;
        private int halfNccTemplateWidth;
        private int nccTemplateHeight;
        private int halfNccTemplateHeight;
        private int nccSearchAreaHeight;
        private int halfNccSearchAreaHeight;
        private int nccSearchAreaWidth;
        private int halfNccSearchAreaWdith;
        private int findingTemplateNccSearchAreaWidth;
        private int halfFindingTemplateNccSearchAreaWidth;
        private int findingTemplateNccSearchAreaHeight;
        private int halfFindingTemplateNccSearchAreaHeight;
        private int initialSearchintensityMapWidth;
        private int initialSearchintensityMapHeight;
        private Pen boxPen;


        public NccBlinkDetector(bool leftEye, CMSTrackingSuiteAdapter cmsTrackingSuiteAdapter)
        {
            templateMutex           = new object();
            boxPen                  = new Pen(Color.Red, 2);
            initializationNccScores = new float[InitializationImageCount, InitializationImageCount];
            initializationTemplates = new FastBitmap.NccTemplate[InitializationImageCount];
            this.leftEye            = leftEye;
            xDistanceBetweenEyesSet = false;
            refinedSearchIndices    = new int[RefinedSearchCount];
            initialSearchTemplates  = null;
            refinedSearchTemplates  = null;
            this.cmsTrackingSuiteAdapter = cmsTrackingSuiteAdapter;
            SetStepAmount();
            Restart();
        }

        #region Properties

        public bool EyeOpen
        {
            get
            {
                return inputOpenEyeNcc >= inputClosedEyeNcc;
            }
        }

        public bool EyeClosed
        {
            get
            {
                return !EyeOpen;
            }
        }

        public float MaximumVoluntaryBlinkTime
        {
            get
            {
                return 3000;
            }
        }

        public float MinimumVoluntaryBlinkTime
        {
            get
            {
                return 1500;
            }
        }

        public bool IsReady
        {
            get
            {
                return openEyeTemplate != null && closedEyeTemplate != null;
            }
        }

        private Point EyeLocation
        {
            get
            {
                Point p;

                if( inputOpenEyeNcc >= inputClosedEyeNcc )
                {
                    p = eyeOpenLoc;
                }
                else
                {
                    p = eyeClosedLoc;
                }

                return new Point(p.X + halfNccTemplateWidth, p.Y + halfNccTemplateHeight);
            }
        }

        #endregion

        private void SetStepAmount()
        {
            stepAmount = 3;
        }
        private void CalculateNccDimensions(int xDistBetweenEyes)
        {
            float heightSearchMultipler = 1f;
            float widthSearchMultiplier = 1f;

            heightSearchMultipler = 1.5f;
            widthSearchMultiplier = 2.5f;

            nccTemplateWidth = (int)(xDistBetweenEyes * XEyeDistToNccTemplateWidthRatio);

            if( nccTemplateWidth % 2 == 0 )
            {
                ++nccTemplateWidth;
            }

            nccTemplateHeight = (int)(xDistBetweenEyes * XEyeDistToNccTemplateHeightRatio);

            if( nccTemplateHeight % 2 == 0 )
            {
                ++nccTemplateHeight;
            }

            nccSearchAreaHeight = (int)(xDistBetweenEyes * XEyeDistToSearchHeightRatio * heightSearchMultipler);

            if( nccSearchAreaHeight % 2 == 0 )
            {
                ++nccSearchAreaHeight;
            }

            nccSearchAreaWidth = (int)(xDistBetweenEyes * XEyeDistToSearchWidthRatio * widthSearchMultiplier);

            if( nccSearchAreaWidth % 2 == 0 )
            {
                ++nccSearchAreaWidth;
            }

            findingTemplateNccSearchAreaWidth = (int)(xDistBetweenEyes * XEyeDistToSearchWidthRatio);

            if( findingTemplateNccSearchAreaWidth % 2 == 0 )
            {
                ++findingTemplateNccSearchAreaWidth;
            }

            findingTemplateNccSearchAreaHeight = (int)(xDistBetweenEyes * XEyeDistToSearchHeightRatio);

            if( findingTemplateNccSearchAreaHeight % 2 == 0 )
            {
                ++findingTemplateNccSearchAreaHeight;
            }


            halfNccTemplateWidth = (nccTemplateWidth - 1) / 2;
            halfNccTemplateHeight = (nccTemplateHeight - 1) / 2;
            halfNccSearchAreaHeight = (nccSearchAreaHeight - 1) / 2;
            halfNccSearchAreaWdith = (nccSearchAreaWidth - 1) / 2;
            halfFindingTemplateNccSearchAreaHeight = (findingTemplateNccSearchAreaHeight - 1) / 2;
            halfFindingTemplateNccSearchAreaWidth = (findingTemplateNccSearchAreaWidth - 1) / 2;

            int xSteps = nccSearchAreaWidth / stepAmount;
            int ySteps = nccSearchAreaHeight / stepAmount;

            if( nccSearchAreaWidth % stepAmount != 0 )
            {
                xSteps++;
            }

            if( nccSearchAreaHeight % stepAmount != 0 )
            {
                ySteps++;
            }

            initialSearchTemplates = new FastBitmap.NccTemplate[xSteps * ySteps];

            for( int i = 0; i < initialSearchTemplates.Length; ++i )
            {
                initialSearchTemplates[i] = new FastBitmap.NccTemplate(nccTemplateWidth, nccTemplateHeight);
            }

            int refinedSearchWidth = (2 * stepAmount) - 1;

            refinedSearchTemplates = new FastBitmap.NccTemplate[refinedSearchWidth * refinedSearchWidth];

            for( int i = 0; i < refinedSearchTemplates.Length; ++i )
            {
                refinedSearchTemplates[i] = new FastBitmap.NccTemplate(nccTemplateWidth, nccTemplateHeight);
            }



            initialSearchintensityMapWidth = nccTemplateWidth + nccSearchAreaWidth;
            initialSearchintensityMapHeight = nccTemplateHeight + nccSearchAreaHeight;
            initialSearchIntensityMap = new float[initialSearchintensityMapWidth, initialSearchintensityMapHeight];
        }
        private Point GetSearchAreaStartPoint(Point eyeLocation)
        {
            return new Point(eyeLocation.X - halfNccTemplateWidth - halfNccSearchAreaWdith, eyeLocation.Y - halfNccTemplateHeight - halfNccSearchAreaHeight);
        }
        private Rectangle GetTemplateRectange(Point p)
        {
            return new Rectangle(p.X - halfNccTemplateWidth, p.Y - halfNccTemplateHeight, nccTemplateWidth, nccTemplateHeight);
        }
        public void LabelEye(Bitmap img, double ratioInputToOutput)
        {
            if( openEyeTemplate != null )
            {
                Point eyeLoc = EyeLocation;

                if( EyeOpen )
                {
                    boxPen.Color = Color.Red;
                }
                else
                {
                    boxPen.Color = Color.Blue;
                }

                int w = openEyeTemplate.Width,
                    h = openEyeTemplate.Height;
                
                boxPen.Width = 2f * (float)ratioInputToOutput;
                Graphics tempG = Graphics.FromImage(img);
                tempG.DrawRectangle(boxPen, eyeLoc.X - w / 2, eyeLoc.Y - h / 2, w, h);
                tempG.Dispose();
            }
        }
        public void LabelTemplateSearchArea(Bitmap img, double ratioInputToOutput)
        {
            if( openEyeTemplate != null )
            {
                boxPen.Color = Color.Orange;
                Graphics tempG = Graphics.FromImage(img);
                tempG.DrawRectangle(boxPen, new Rectangle(GetSearchAreaStartPoint(EyeLocation), new Size(nccSearchAreaWidth, nccTemplateHeight)));
                tempG.Dispose();
            }
        }
        public void LabelImage(Bitmap img, double ratioInputToOutput)
        {
            LabelEye(img, ratioInputToOutput);
            //LabelTemplateSearchArea(img, ratioInputToOutput);
        }
        private void UpdateTemplates(FastBitmap img)
        {
            float templateDiff = Math.Abs(inputOpenEyeNcc - inputClosedEyeNcc);

            if( templateDiff >= MinimumDifferenceForUpdate )
            {
                string executableName = Application.ExecutablePath;
                FileInfo executableFileInfo = new FileInfo(executableName);
                FastBitmap.NccTemplate currentImageTemplate = img.GetNccTemplate(GetTemplateRectange(EyeLocation));

                if( inputOpenEyeNcc > inputClosedEyeNcc )
                {
                    openEyeTemplate.MergeTemplates(currentImageTemplate, templateDiff);
                }
                else
                {
                    closedEyeTemplate.MergeTemplates(currentImageTemplate, templateDiff);
                }
            }
        }
        public void Update(FastBitmap img, Point eyeLocation, float timeElapsed)
        {
            lock( templateMutex )
            {
                if( IsReady )
                {
                    Point searchAreaStartPoint = GetSearchAreaStartPoint(eyeLocation);

                    try
                    {

                        img.GetIntensityMap(searchAreaStartPoint.X, searchAreaStartPoint.Y,
                            initialSearchintensityMapWidth, initialSearchintensityMapHeight, initialSearchIntensityMap);

                        Point tempPoint;
                        float[] refinedSearchNccScores;

                        SetInitialNccSearchTemplates(eyeLocation);

                        refinedSearchNccScores = openEyeTemplate.GetTopNccScores(initialSearchTemplates, refinedSearchIndices);


                        inputOpenEyeNcc = float.NegativeInfinity;

                        for( int i = 0; i < refinedSearchIndices.Length; ++i )
                        {
                            FastBitmap.NccTemplate tempTemplate = initialSearchTemplates[refinedSearchIndices[i]];
                            float tempNcc = img.GetNccScore(openEyeTemplate, new Rectangle(tempTemplate.X - (stepAmount - 1),
                                                tempTemplate.Y - (stepAmount - 1), (2 * stepAmount) - 1, (2 * stepAmount) - 1), out tempPoint);

                            if( tempNcc > inputOpenEyeNcc )
                            {
                                inputOpenEyeNcc = tempNcc;
                                eyeOpenLoc = tempPoint;
                            }
                        }

                        refinedSearchNccScores = closedEyeTemplate.GetTopNccScores(initialSearchTemplates, refinedSearchIndices);

                        inputClosedEyeNcc = float.NegativeInfinity;

                        for( int i = 0; i < refinedSearchIndices.Length; ++i )
                        {
                            FastBitmap.NccTemplate tempTemplate = initialSearchTemplates[refinedSearchIndices[i]];
                            float tempNcc = img.GetNccScore(closedEyeTemplate, new Rectangle(tempTemplate.X - (stepAmount - 1),
                                                tempTemplate.Y - (stepAmount - 1), (2 * stepAmount) - 1, (2 * stepAmount) - 1), out tempPoint);

                            if( tempNcc > inputClosedEyeNcc )
                            {
                                inputClosedEyeNcc = tempNcc;
                                eyeClosedLoc = tempPoint;
                            }
                        }
                    }
                    catch( ArgumentOutOfRangeException )
                    {
                        // Set as eye open since this performs no action, set open eye location as estimated location
                        inputOpenEyeNcc = 1;
                        eyeOpenLoc = eyeLocation;
                        inputClosedEyeNcc = 0;
                    }
                }
                else
                {
                    if( !xDistanceBetweenEyesSet )
                    {
                        throw new InvalidOperationException("Must call SetXDistanceBetweenEyes before calling Update");
                    }
                    try
                    {
                        initializationTemplates[nextInitializationImageIndex] = img.GetNccTemplate(GetTemplateRectange(eyeLocation));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return;
                    }

                    if( closeEyeMessageTimer > 0 )
                    {
                        closeEyeMessageTimer -= timeElapsed;
                        inputOpenEyeNcc = img.GetNccScore(openEyeTemplate, GetFindingTemplateSearchArea(eyeLocation), 1, out eyeOpenLoc);
                    }
                    else
                    {
                        if( openEyeTemplate == null )
                        {
                            for( int i = 0; i < nextInitializationImageIndex; ++i )
                            {
                                initializationNccScores[i, nextInitializationImageIndex]
                                    = initializationNccScores[nextInitializationImageIndex, i]
                                    = initializationTemplates[i].GetNcc(initializationTemplates[nextInitializationImageIndex]);
                            }
                        }
                        else
                        {
                            inputOpenEyeNcc = img.GetNccScore(openEyeTemplate, GetFindingTemplateSearchArea(eyeLocation), 1, out eyeOpenLoc);

                            initializationNccScores[nextInitializationImageIndex, 0]
                                = openEyeTemplate.GetNcc(initializationTemplates[nextInitializationImageIndex]);
                        }

                        initializationNccScores[nextInitializationImageIndex, nextInitializationImageIndex] = 1;

                        nextInitializationImageIndex++;

                        if( InitializationImageCount == nextInitializationImageIndex )
                        {
                            if( openEyeTemplate == null )
                            {
                                int bestIndex = GetBestRepresentativeTemplateIndex();
                                openEyeTemplate = initializationTemplates[bestIndex];
                                cmsTrackingSuiteAdapter.SendMessage(CloseEyesMessage);
                                closeEyeMessageTimer = CloseEyeMessageStartTime;

                                if(CMSLogger.CanCreateLogEvent(false,true,false,"BlinkLinkLogTemplatesEvent"))
                                {
                                    BlinkLinkLogTemplatesEvent logEvent = new BlinkLinkLogTemplatesEvent();
                                    logEvent.IsOpenTemplates = true;
                                    logEvent.SelectedTemplate = bestIndex;
                                    logEvent.SetTemplates(initializationTemplates);
                                    CMSLogger.SendLogEvent(logEvent);
                                }
                            }
                            else
                            {
                                float nccScore;
                                int bestIndex = GetFurtherestFromEyeOpen(out nccScore);
                                FastBitmap.NccTemplate representativeTemplate = initializationTemplates[bestIndex];

                                if( nccScore <= ClassDifferenceThreshold )
                                {
                                    closedEyeTemplate = representativeTemplate;

                                    if (CMSLogger.CanCreateLogEvent(false, true, false, "BlinkLinkLogTemplatesEvent"))
                                    {
                                        BlinkLinkLogTemplatesEvent logEvent = new BlinkLinkLogTemplatesEvent();
                                        logEvent.IsOpenTemplates = false;
                                        logEvent.SelectedTemplate = bestIndex;
                                        logEvent.SetTemplates(initializationTemplates);
                                        CMSLogger.SendLogEvent(logEvent);
                                    }
                                }
                            }

                            nextInitializationImageIndex = 0;
                        }
                    }
                }
            }
        }
        private void SetInitialNccSearchTemplates(Point eyeLocation)
        {
            int index = 0;
            Rectangle intensityMapRect = new Rectangle(this.GetSearchAreaStartPoint(eyeLocation),
                                                            new Size(initialSearchintensityMapWidth, initialSearchintensityMapHeight));

            for( int i = 0; i < nccSearchAreaWidth; i += stepAmount )
            {
                for( int j = 0; j < nccSearchAreaHeight; j += stepAmount )
                {
                    initialSearchTemplates[index++].SetNccTemplate(initialSearchIntensityMap, intensityMapRect, i, j);
                }
            }
        }
        private Rectangle GetFindingTemplateSearchArea(Point eyeLocation)
        {
            return new Rectangle(eyeLocation.X - halfNccTemplateWidth - halfFindingTemplateNccSearchAreaWidth, 
                eyeLocation.Y - halfNccTemplateHeight - halfFindingTemplateNccSearchAreaHeight, 
                findingTemplateNccSearchAreaWidth, findingTemplateNccSearchAreaHeight);
        }
        private int GetFurtherestFromEyeOpen(out float bestTemplateScore)
        {
            int bestTemplateIndex = 0;
            bestTemplateScore     = float.MaxValue;

            for( int i = 0; i < InitializationImageCount; ++i )
            {
                if( initializationNccScores[i, 0] < bestTemplateScore )
                {
                    bestTemplateScore = initializationNccScores[i, 0];
                    bestTemplateIndex = i;
                }
            }

            return bestTemplateIndex;
        }
        private int GetBestRepresentativeTemplateIndex()
        {
            int   bestTemplate      = 0;
            float bestTemplateScore = float.MinValue;
            
            for( int i = 0; i < InitializationImageCount; ++i )
            {
                float tempScore = 0;

                for( int j = 0; j < InitializationImageCount; ++j )
                {
                    tempScore += initializationNccScores[i, j];
                }

                if( tempScore > bestTemplateScore )
                {
                    bestTemplate = i;
                    bestTemplateScore = tempScore;
                }
            }

            return bestTemplate;
        }
        public void Restart()
        {
            lock( templateMutex )
            {
                openEyeTemplate = null;
                closedEyeTemplate = null;
                nextInitializationImageIndex = 0;
                closeEyeMessageTimer = -1;
            }
        }
        public void SetXDistanceBetweenEyes(int dist)
        {
            this.xDistBetweenEyes = dist;
            CalculateNccDimensions(dist);
            xDistanceBetweenEyesSet = true;
        }

        #region IDisposable Members

        public void Dispose() { }

        #endregion
    }
}