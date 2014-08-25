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

namespace CameraMouseSuite
{
    public class CMSConstants
    {
        public static string PLEASE_CLICK_TF = "Please click on feature on face to track.";
        public static string CTRL_KEY_DESCRIPTION = "Ctrl key";
        public static string SCROLL_LOCK_KEY_DESCRIPTION = "Scroll Lock key";
        public static string TO_STOP_CONTROL = "To stop Camera Mouse control:"; 
        public static string MOVE_MOUSE = "Move mouse by hand";
        public static string TO_START_CONTROL = "To start Camera Mouse control:";

        public static string DEFAULT_LOG_SERVER_URI = "http://cameramouse.bu.edu/CameraMouseServlet/log";

        public static string STANDARD_TRACKING_SUITE_NAME = "STANDARD";
        public static string STANDARD_TRACKING_SUITE_INFORMAL_NAME = "Classic";
        public static string STANDARD_TRACKING_SUITE_DECSCRIPTION = "The original tracker used in the Camera Mouse Suites";

        public static string EMPTY_TRACKING_SUITE_MESSAGE = "No Tracker Selected";
        public static string EMPTY_TRACKING_SUITE_NAME = "EMPTY";
        public static string EMPTY_TRACKING_SUITE_INFORMAL_NAME = "No Tracker";
        public static string EMPTY_TRACKING_SUITE_DESCRIPTION = "No tracker is currently selected.";

        public static double STANDARD_BOX_LENGTH = 12;
        public static string AUTO_START = "Auto start ";

        public static string USER_LIB_DIRECTORY = "userlib";
        public static string DEFAULT_SUITE_NAME = "Original Camera Mouse";
        public static string SUITE_CONFIG_SUFFIX = "-config.xml";
        public static string SELECTED_SUITE_FILENAME = "suites-config.xml";
        public static string SUITE_CONFIG_DIR = "sconfig";
        public static string SUITE_LIB_DIR = "slib";
        public static string LOG_DIR = "log";
        public static string MAIN_CONFIG_FILE ="config/cmsconfig.xml";
        public static string MAIN_CAMERA_CONFIG_FILE = "config/cmscameraconfig.xml";
        public static string MAIN_LOG_CONFIG_FILE = "config/cmslogconfig.xml";
        public static string MAIN_ID_CONFIG_FILE = "config/cmsidconfig.xml";

        public static string CAMERA_MOUSE_MANUAL_PDF = "config\\CameraMouse2008Manual.pdf";

        public static string ADULT_INFO_PDF_FILE = "InfoForms/AdultInformationForm.pdf";
        public static string ADULT_INFO_RTF_FILE = "InfoForms/adult.rtf";

        public static string CONTROL_CAMERA_MOUSE = "Control: Camera Mouse";
        public static string CONTROL_MOUSE = "Control: Mouse";

        public static int SCREEN_WIDTH = 0;
        public static int SCREEN_HEIGHT = 0;

        public const int CV_LKFLOW_PYR_A_READY = 1;
        public const int CV_LKFLOW_PYR_B_READY = 2;
        public const int CV_LKFLOW_INITIAL_GUESSES = 4;

        public const int CV_TERMCRIT_ITER = 1;
        public const int CV_TERMCRIT_NUMBER = CV_TERMCRIT_ITER;
        public const int CV_TERMCRIT_EPS = 3;

        public const long CONTROL_TOGGLER_EVENT_SPACING = 2000000;
        public static string MINIMIZE_FORM_TOOLTIP = "Set form to minimum size";
        public static string MAXIMIZE_FORM_TOOLTIP = "Set form to maximum size";
        
        public static void Init()
        {
            SCREEN_WIDTH = User32.GetSystemMetrics(User32.CX_SCREEN);
            SCREEN_HEIGHT = User32.GetSystemMetrics(User32.CY_SCREEN);
        }

        public static int VIDEO_DISPLAY_MAX_WIDTH = 320;
        public static int VIDEO_DISPLAY_MAX_HEIGHT = 240;
        public static int VIDEO_DISPLAY_MIN_WIDTH = 160;
        public static int VIDEO_DISPLAY_MIN_HEIGHT = 120;

    }
}
