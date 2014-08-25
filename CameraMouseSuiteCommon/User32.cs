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

	/// <summary>

	/// Summary description for User32.

	/// </summary>

	public class User32

	{
		#region DLL IMPORT DECLARATIONS


		public static int CX_SCREEN = 0;

		public static int CY_SCREEN = 1;

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int x , int y);

		[DllImport("user32.dll")]
		public static extern short GetKeyState(int vKEY);

		[DllImport("user32.dll")]
		public static extern void mouse_event(int dwFlags, 
			int dx, int dy, int dwData, long dwExtraInfo);

		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(int id);


		#endregion


	}



	



	class Keyboard

	{

		public enum VirtualKeys: byte

		{

			VK_NUMLOCK = 0x90,

			VK_SCROLL = 0x91,

            VK_CAPITAL = 0x14,
            
            VK_CONTROL = 0x11
		}





		const uint KEYEVENTF_EXTENDEDKEY = 0x1;

		const uint KEYEVENTF_KEYUP = 0x2;

	

		[DllImport("user32.dll")]

		static extern short GetKeyState(int nVirtKey);

		

		[DllImport("user32.dll")]

		static extern void keybd_event(

			byte bVk,

			byte bScan,

			uint dwFlags,

			uint dwExtraInfo

			);

		

		public static bool GetState(VirtualKeys Key)

		{

			return (GetKeyState((int)Key)==1);

		}

		

		public static void SetState(VirtualKeys Key, bool State)

		{

			if(State!=GetState(Key))

			{

				keybd_event((byte)Key, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);

				keybd_event((byte)Key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

			}

		} 



	}

}

