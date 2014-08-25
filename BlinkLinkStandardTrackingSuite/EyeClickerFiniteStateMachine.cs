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
using System.ComponentModel;
using System.Windows.Forms;
using System.Media;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using CameraMouseSuite;

namespace BlinkLinkStandardTrackingSuite
{
    public enum ClickAction
    {
        [DescriptionAttribute("None")]
        None,

        [DescriptionAttribute("Left Click")]
        LeftClick,

        [DescriptionAttribute("Right Click")]
        RightClick,

        [DescriptionAttribute("Double Click")]
        DoubleClick,

        [DescriptionAttribute("Left Drag")]
        LeftDrag
    }

    class EyeClickerFiniteStateMachine
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int FramesToLookBack = 5;
        private const int MaximumAllowedEyesOpen = 2;
        private const float TimeToShowMouseEventImage = 1500;

        #region Private Enumeration Data Types

        private enum EyeClickState
        {
            EyeOpen,
            DeterminingBlinkType,
            VoluntaryBlink,
            LongVoluntaryBlink,
            DoubleClickPause,
            DoubleClickPauseWaitForOtherEye
        }

        private enum EyeStatus
        {
            Open,
            Close
        }

        private enum BlinkType
        {
            Unknown,
            SmallVoluntaryNotDone,
            SmallVoluntaryDone,
            LargeVoluntaryNotDone,
            LargeVoluntaryDone,
            Involuntary
        }

        private enum Eye
        {
            Left,
            Right
        }

        #endregion

        private List<EyeStatus> leftEyeStatusList;
        private List<EyeStatus> rightEyeStatusList;
        private volatile EyeClickState leftEyeState;
        private volatile EyeClickState rightEyeState;
        private float leftEyeClosedTime;
        private float rightEyeClosedTime;
        private BlinkLinkEyeClickData blinkLinkEyeClickData;
        private bool noActionImageSet;
        private bool isDragging;
        private float mouseEventImageTimer;
        private EyeStatusWindow eyeStatusWindow;

        public EyeClickerFiniteStateMachine(BlinkLinkEyeClickData blinkLinkEyeClickData, EyeStatusWindow eyeStatusWindow)
        {
            this.blinkLinkEyeClickData = blinkLinkEyeClickData;
            this.eyeStatusWindow = eyeStatusWindow;
            leftEyeStatusList = new List<EyeStatus>();
            rightEyeStatusList = new List<EyeStatus>();
            leftEyeClosedTime = 0;
            rightEyeClosedTime = 0;
            leftEyeState = EyeClickState.EyeOpen;
            rightEyeState = EyeClickState.EyeOpen;
            noActionImageSet = true;
            mouseEventImageTimer = -1;
            isDragging = false;
        }

        public float EyeClosedTime
        {
            get
            {
                return Math.Max(leftEyeClosedTime, rightEyeClosedTime);
            }
        }

        public bool IsDragging
        {
            get
            {
                return isDragging;
            }
        }

        private void SetEyeState(Eye eye, EyeClickState value)
        {

            EyeClickState oldEyeState;
            EyeClickState otherEyeState;

            if( eye == Eye.Left )
            {
                otherEyeState = rightEyeState;
                oldEyeState = leftEyeState;
                leftEyeState = value;
            }
            else
            {
                otherEyeState = leftEyeState;
                oldEyeState = rightEyeState;
                rightEyeState = value;
            }

            switch( value )
            {
                case EyeClickState.DeterminingBlinkType:
                    {
                        leftEyeStatusList.Add(EyeStatus.Open);
                    }
                    break;

                case EyeClickState.DoubleClickPause:
                    {
                        PerformBlinkAction();
                    }
                    break;

                case EyeClickState.DoubleClickPauseWaitForOtherEye:
                    {
                        if( rightEyeState == EyeClickState.DoubleClickPauseWaitForOtherEye )
                        {
                            SetEyeState(Eye.Left, EyeClickState.EyeOpen);
                            SetEyeState(Eye.Right, EyeClickState.EyeOpen);
                        }
                    }
                    break;

                case EyeClickState.LongVoluntaryBlink:
                    {
                        if( eye == Eye.Left )
                        {
                            StartLongLeftWinkAction();
                        }
                        else
                        {
                            StartLongRightWinkAction();
                        }
                    }
                    break;

                case EyeClickState.EyeOpen:
                    {
                        if( eye == Eye.Left )
                        {
                            leftEyeClosedTime = 0;
                            leftEyeStatusList.Clear();
                        }
                        else
                        {
                            rightEyeClosedTime = 0;
                            rightEyeStatusList.Clear();
                        }

                        if( oldEyeState == EyeClickState.VoluntaryBlink )
                        {
                            if( eye == Eye.Left )
                            {
                                PerformShortLeftWinkAction();
                            }
                            else
                            {
                                PerformShortRightWinkAction();
                            }
                        }
                        else if( oldEyeState == EyeClickState.LongVoluntaryBlink )
                        {
                            if( eye == Eye.Left )
                            {
                                StopLongLeftWinkAction();
                            }
                            else
                            {
                                StopLongRightWinkAction();
                            }
                        }
                    }
                    break;

                case EyeClickState.VoluntaryBlink:
                    {
                        if( otherEyeState == EyeClickState.VoluntaryBlink )
                        {
                            SetEyeState(Eye.Left, EyeClickState.DoubleClickPause);
                            SetEyeState(Eye.Right, EyeClickState.DoubleClickPause);
                        }
                        else
                        {
                            if( eye == Eye.Left )
                            {
                                if( blinkLinkEyeClickData.ShortLeftWinkAction == ClickAction.LeftClick ||
                                    blinkLinkEyeClickData.ShortLeftWinkAction == ClickAction.RightClick ||
                                    blinkLinkEyeClickData.ShortLeftWinkAction == ClickAction.DoubleClick )
                                {
                                    SetMouseImage(blinkLinkEyeClickData.ShortLeftWinkAction, true);
                                }
                            }
                            else
                            {
                                if( blinkLinkEyeClickData.ShortRightWinkAction == ClickAction.LeftClick ||
                                    blinkLinkEyeClickData.ShortRightWinkAction == ClickAction.RightClick ||
                                    blinkLinkEyeClickData.ShortRightWinkAction == ClickAction.DoubleClick )
                                {
                                    SetMouseImage(blinkLinkEyeClickData.ShortRightWinkAction, true);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public void Start()
        {
            leftEyeStatusList.Clear();
            rightEyeStatusList.Clear();
            leftEyeState = EyeClickState.EyeOpen;
            rightEyeState = EyeClickState.EyeOpen;
            leftEyeClosedTime = 0;
            rightEyeClosedTime = 0;
        }

        public void Update(bool leftEyeOpen, bool rightEyeOpen, float elapsedTime)
        {
            if( mouseEventImageTimer > 0 )
            {
                mouseEventImageTimer -= elapsedTime;
            }

            if( leftEyeState == EyeClickState.EyeOpen && rightEyeState == EyeClickState.EyeOpen
                                    && !noActionImageSet && mouseEventImageTimer < 0 )
            {
                noActionImageSet = true;
                SetMouseImage(ClickAction.None);
            }

            if( leftEyeState == EyeClickState.DeterminingBlinkType
                || leftEyeState == EyeClickState.VoluntaryBlink || leftEyeState == EyeClickState.LongVoluntaryBlink
                || leftEyeState == EyeClickState.DoubleClickPause )
            {
                leftEyeClosedTime += elapsedTime;
                leftEyeStatusList.Add(leftEyeOpen ? EyeStatus.Open : EyeStatus.Close);
            }

            if( rightEyeState == EyeClickState.DeterminingBlinkType
                || rightEyeState == EyeClickState.VoluntaryBlink || rightEyeState == EyeClickState.LongVoluntaryBlink
                || rightEyeState == EyeClickState.DoubleClickPause )
            {
                rightEyeClosedTime += elapsedTime;
                rightEyeStatusList.Add(rightEyeOpen ? EyeStatus.Open : EyeStatus.Close);
            }

            UpdateEyeState(Eye.Left, leftEyeOpen);
            UpdateEyeState(Eye.Right, rightEyeOpen);

        }

        private void UpdateEyeState(Eye eye, bool eyeOpen)
        {
            switch( eye == Eye.Left ? leftEyeState : rightEyeState )
            {
                case EyeClickState.DeterminingBlinkType:
                    {
                        switch( DetermineBlinkType(eye) )
                        {
                            case BlinkType.Involuntary:
                                {
                                    SetEyeState(eye, EyeClickState.EyeOpen);
                                }
                                break;

                            case BlinkType.LargeVoluntaryNotDone:
                            case BlinkType.LargeVoluntaryDone:
                            case BlinkType.SmallVoluntaryNotDone:
                            case BlinkType.SmallVoluntaryDone:
                                {
                                    SetEyeState(eye, EyeClickState.VoluntaryBlink);
                                }
                                break;
                        }
                    }
                    break;

                case EyeClickState.DoubleClickPause:
                    {
                        BlinkType tempBlinkType = DetermineBlinkType(eye);
                        if( tempBlinkType == BlinkType.LargeVoluntaryDone || tempBlinkType == BlinkType.SmallVoluntaryDone )
                        {
                            SetEyeState(eye, EyeClickState.DoubleClickPauseWaitForOtherEye);
                        }
                    }
                    break;

                case EyeClickState.DoubleClickPauseWaitForOtherEye:
                    {
                    }
                    break;

                case EyeClickState.LongVoluntaryBlink:
                    {
                        if( DetermineBlinkType(eye) == BlinkType.LargeVoluntaryDone )
                        {
                            SetEyeState(eye, EyeClickState.EyeOpen);
                        }
                    }
                    break;

                case EyeClickState.EyeOpen:
                    {
                        if( !eyeOpen )
                        {
                            SetEyeState(eye, EyeClickState.DeterminingBlinkType);
                        }
                    }
                    break;

                case EyeClickState.VoluntaryBlink:
                    {

                        switch( DetermineBlinkType(eye) )
                        {
                            case BlinkType.SmallVoluntaryDone:
                                {
                                    SetEyeState(eye, EyeClickState.EyeOpen);
                                }
                                break;
                            case BlinkType.LargeVoluntaryNotDone:
                            case BlinkType.LargeVoluntaryDone:
                                {
                                    SetEyeState(eye, EyeClickState.LongVoluntaryBlink);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private BlinkType DetermineBlinkType(Eye eye)
        {
            if( eye == Eye.Left )
            {
                return DetermineBlinkType(leftEyeStatusList, leftEyeClosedTime);
            }
            else
            {
                return DetermineBlinkType(rightEyeStatusList, rightEyeClosedTime);
            }
        }

        private BlinkType DetermineBlinkType(List<EyeStatus> blinkList, float eyeClosedTime)
        {
            if( blinkList.Count < FramesToLookBack )
            {
                return BlinkType.Unknown;
            }
            else
            {
                int numberOfEyesOpen = 0;
                bool eyeTruelyOpen;

                for( int i = blinkList.Count - FramesToLookBack; i < blinkList.Count; ++i )
                {
                    if( blinkList[i] == EyeStatus.Open )
                    {
                        ++numberOfEyesOpen;
                    }
                }

                eyeTruelyOpen = numberOfEyesOpen > MaximumAllowedEyesOpen;

                if( eyeClosedTime >= blinkLinkEyeClickData.LongWinkTime * 1000 )
                {
                    if( eyeTruelyOpen )
                    {
                        return BlinkType.LargeVoluntaryDone;
                    }
                    else
                    {
                        return BlinkType.LargeVoluntaryNotDone;
                    }
                }
                else if( eyeClosedTime >= blinkLinkEyeClickData.ShortWinkTime * 1000 )
                {
                    if( eyeTruelyOpen )
                    {
                        return BlinkType.SmallVoluntaryDone;
                    }
                    else
                    {
                        return BlinkType.SmallVoluntaryNotDone;
                    }
                }
                else
                {
                    if( eyeTruelyOpen )
                    {
                        return BlinkType.Involuntary;
                    }
                    else
                    {
                        return BlinkType.Unknown;
                    }
                }
            }
        }

        public void Reset()
        {
            leftEyeStatusList.Clear();
            rightEyeStatusList.Clear();
            leftEyeState = EyeClickState.EyeOpen;
            rightEyeState = EyeClickState.EyeOpen;
            leftEyeClosedTime = 0;
            rightEyeClosedTime = 0;
            if( !noActionImageSet )
            {
                noActionImageSet = true;
                SetMouseImage(ClickAction.None);
            }
            mouseEventImageTimer = -1;
            isDragging = false;
        }

        private void SetMouseImage(ClickAction clickAction, bool startOrWaiting)
        {
            eyeStatusWindow.SetMouseImage(GetCorrespondingMouseState(clickAction, startOrWaiting));
            if( ClickAction.None != clickAction )
            {
                mouseEventImageTimer = TimeToShowMouseEventImage;
                noActionImageSet = false;
            }
        }

        private EyeStatusWindow.MouseState GetCorrespondingMouseState(ClickAction clickAction, bool startOrWaiting)
        {
            switch( clickAction )
            {
                case ClickAction.DoubleClick:
                    {
                        return EyeStatusWindow.MouseState.DoubleClick;
                    }

                case ClickAction.LeftDrag:
                    {
                        if( startOrWaiting )
                        {
                            return EyeStatusWindow.MouseState.DragStart;
                        }
                        else
                        {
                            return EyeStatusWindow.MouseState.DragEnd;
                        }
                    }

                case ClickAction.RightClick:
                    {
                        if( startOrWaiting )
                        {
                            return EyeStatusWindow.MouseState.RightClickWaiting;
                        }
                        else
                        {
                            return EyeStatusWindow.MouseState.RightClick;
                        }
                    }

                case ClickAction.LeftClick:
                    {
                        if( startOrWaiting )
                        {
                            return EyeStatusWindow.MouseState.LeftClickWaiting;
                        }
                        else
                        {
                            return EyeStatusWindow.MouseState.LeftClick;
                        }
                    }
            }

            return EyeStatusWindow.MouseState.NoAction;
        }

        private void SetMouseImage(ClickAction clickAction)
        {
            SetMouseImage(clickAction, false);
        }

        #region Mouse Control Functions

        private void PerformShortAction(ClickAction clickAction)
        {
            Point CursorPos = Cursor.Position;

            switch( clickAction )
            {
                case ClickAction.DoubleClick:
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, CursorPos.X, CursorPos.Y, 0, 0);
                        Thread.Sleep(10);
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, CursorPos.X, CursorPos.Y, 0, 0);
                    }
                    break;

                case ClickAction.LeftClick:
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, CursorPos.X, CursorPos.Y, 0, 0);
                    }
                    break;

                case ClickAction.RightClick:
                    {
                        mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, CursorPos.X, CursorPos.Y, 0, 0);
                    }
                    break;
                default:
                    {
                        return;
                    }
            }

            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogClickEvent"))
            {
                CMSLogClickEvent clickEvent = new CMSLogClickEvent();
                if (clickEvent != null)
                {
                    Point c = Cursor.Position;
                    clickEvent.X = c.X;
                    clickEvent.Y = c.Y;
                    clickEvent.Width = CMSConstants.SCREEN_WIDTH;
                    clickEvent.Height = CMSConstants.SCREEN_HEIGHT;
                    clickEvent.ClickType
                        = GetXmlClickType(clickAction, false);
                    CMSLogger.SendLogEvent(clickEvent);
                }
            }
        }

        private void StartLongAction(ClickAction clickAction)
        {
            switch( clickAction )
            {

                case ClickAction.LeftDrag:
                    {
                        isDragging = true;
                        Point CursorPos = Cursor.Position;
                        mouse_event(MOUSEEVENTF_LEFTDOWN, CursorPos.X, CursorPos.Y, 0, 0);

                        if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogClickEvent"))
                        {
                            CMSLogClickEvent clickEvent = new CMSLogClickEvent();
                            if (clickEvent != null)
                            {
                                Point c = Cursor.Position;
                                clickEvent.X = c.X;
                                clickEvent.Y = c.Y;
                                clickEvent.Width = CMSConstants.SCREEN_WIDTH;
                                clickEvent.Height = CMSConstants.SCREEN_HEIGHT;
                                clickEvent.ClickType
                                    = GetXmlClickType(clickAction, true);
                                CMSLogger.SendLogEvent(clickEvent);
                            }
                        }
                    }
                    break;
                default:
                    {
                        return;
                    }
            }
        }

        private void StopLongAction(ClickAction clickAction)
        {
            switch( clickAction )
            {
                case ClickAction.DoubleClick:
                case ClickAction.LeftClick:
                case ClickAction.RightClick:
                    {
                        PerformShortAction(clickAction);
                    }
                    break;
                case ClickAction.LeftDrag:
                    {
                        isDragging = false;
                        Point CursorPos = Cursor.Position;
                        mouse_event(MOUSEEVENTF_LEFTUP, CursorPos.X, CursorPos.Y, 0, 0);
                    }
                    break;
                default:
                    {
                        return;
                    }
            }

            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogClickEvent"))
            {
                CMSLogClickEvent clickEvent = new CMSLogClickEvent();
                if (clickEvent != null)
                {
                    Point c = Cursor.Position;
                    clickEvent.X = c.X;
                    clickEvent.Y = c.Y;
                    clickEvent.Width = CMSConstants.SCREEN_WIDTH;
                    clickEvent.Height = CMSConstants.SCREEN_HEIGHT;
                    clickEvent.ClickType
                        = GetXmlClickType(clickAction, false);
                    CMSLogger.SendLogEvent(clickEvent);
                }
            }
        }

        private void PerformBlinkAction()
        {
            PerformShortAction(blinkLinkEyeClickData.BlinkAction);
            SetMouseImage(blinkLinkEyeClickData.BlinkAction);

            if( (blinkLinkEyeClickData.PlaySoundForAllClicks || blinkLinkEyeClickData.PlaySoundForBlinkOnlyClicks)
                && (blinkLinkEyeClickData.BlinkAction != ClickAction.None) )
            {
                SystemSounds.Beep.Play();
            }
            
            
        }

        private ClickType GetXmlClickType(ClickAction clickAction, bool start)
        {
            switch (clickAction)
            {
                case ClickAction.DoubleClick:
                    {
                        return ClickType.DlClk;
                    }

                case ClickAction.LeftClick:
                    {
                        return ClickType.LClk;
                    }

                case ClickAction.LeftDrag:
                    {
                        if (start)
                        {
                            return ClickType.LDn;
                        }
                        else
                        {
                            return ClickType.LUp;
                        }
                    }

                default: //case ClickAction.RightClick
                    {
                        return ClickType.RClk;
                    }
            }
        }

        private void StopLongLeftWinkAction()
        {

            StopLongAction(blinkLinkEyeClickData.LongLeftWinkAction);
            SetMouseImage(blinkLinkEyeClickData.LongLeftWinkAction, false);
        }

        private void StopLongRightWinkAction()
        {

            StopLongAction(blinkLinkEyeClickData.LongRightWinkAction);
            SetMouseImage(blinkLinkEyeClickData.LongRightWinkAction, false);
        }

        private void PerformShortLeftWinkAction()
        {
            PerformShortAction(blinkLinkEyeClickData.ShortLeftWinkAction);
            SetMouseImage(blinkLinkEyeClickData.ShortLeftWinkAction);

            if( blinkLinkEyeClickData.PlaySoundForAllClicks && (blinkLinkEyeClickData.ShortLeftWinkAction != ClickAction.None) )
            {
                SystemSounds.Beep.Play();
            }

        }

        private void PerformShortRightWinkAction()
        {
            PerformShortAction(blinkLinkEyeClickData.ShortRightWinkAction);
            SetMouseImage(blinkLinkEyeClickData.ShortRightWinkAction);

            if( blinkLinkEyeClickData.PlaySoundForAllClicks && (blinkLinkEyeClickData.ShortRightWinkAction != ClickAction.None) )
            {
                SystemSounds.Beep.Play();
            }
        }

        private void StartLongLeftWinkAction()
        {
            StartLongAction(blinkLinkEyeClickData.LongLeftWinkAction);
            SetMouseImage(blinkLinkEyeClickData.LongLeftWinkAction, true);

            if( blinkLinkEyeClickData.PlaySoundForAllClicks && (blinkLinkEyeClickData.LongLeftWinkAction != ClickAction.None) )
            {
                SystemSounds.Beep.Play();
            }
        }

        private void StartLongRightWinkAction()
        {
            StartLongAction(blinkLinkEyeClickData.LongRightWinkAction);
            SetMouseImage(blinkLinkEyeClickData.LongRightWinkAction, true);

            if( blinkLinkEyeClickData.PlaySoundForAllClicks && (blinkLinkEyeClickData.LongLeftWinkAction != ClickAction.None) )
            {
                SystemSounds.Beep.Play();
            }
        }

        #endregion
    }
}
