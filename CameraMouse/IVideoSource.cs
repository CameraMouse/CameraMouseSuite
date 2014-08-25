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

namespace CameraMouseSuite
{

    /// <summary>
    /// IVideoSource interface
    /// </summary>
    public interface IVideoSource
    {
        /// <summary>
        /// New frame event - notify client about the new frame
        /// </summary>
        event CameraEventHandler NewFrame;

        /// <summary>
        /// Video source property
        /// </summary>
        string VideoSource { get; set; }

        /// <summary>
        /// Login property
        /// </summary>
        string Login { get; set; }

        /// <summary>
        /// Password property
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// FramesReceived property
        /// get number of frames the video source received from the last
        /// access to the property
        /// </summary>
        int FramesReceived { get; }

        /// <summary>
        /// BytesReceived property
        /// get number of bytes the video source received from the last
        /// access to the property
        /// </summary>
        int BytesReceived { get; }

        /// <summary>
        /// UserData property
        /// allows to associate user data with an object
        /// </summary>
        object UserData { get; set; }

        /// <summary>
        /// Get state of video source
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// Start receiving video frames
        /// </summary>
        void Start();

        /// <summary>
        /// Stop receiving video frames
        /// </summary>
        void SignalToStop();

        /// <summary>
        /// Wait for stop
        /// </summary>
        void WaitForStop();

        /// <summary>
        /// Stop work
        /// </summary>
        void Stop();
    }
}
