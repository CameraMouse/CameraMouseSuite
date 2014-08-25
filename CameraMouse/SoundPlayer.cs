using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace CameraMouseSuite
{

    public class SoundPlayer
    {

        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int PlaySound(string szSound, IntPtr hMod, int flags);

        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int PlaySound(byte[] szSound, IntPtr hMod, int flags);


        private enum Flags
        {
            SND_SYNC = 0x0000,  /* play synchronously (default) */
            SND_ASYNC = 0x0001,  /* play asynchronously */
            SND_NODEFAULT = 0x0002,  /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004,  /* pszSound points to a memory file */
            SND_LOOP = 0x0008,  /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010,  /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004  /* name is resource name or atom */
        }


        private byte[] click_bytes;
        private byte[] state_change_bytes;


        public SoundPlayer()
        {
            //Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("CameraMouse.clickerx.wav");
            Stream s = Properties.Resources.clickerx;

            if (s != null)
            {
                click_bytes = new byte[s.Length];
                s.Read(click_bytes, 0, (int)s.Length);
                s.Close();
            }

            //Properties.Resources.notify.g
            //s = Assembly.GetExecutingAssembly().GetManifestResourceStream("CameraMouse.notify.wav");
            s = Properties.Resources.notify;

            if (s != null)
            {
                state_change_bytes = new byte[s.Length];
                s.Read(state_change_bytes, 0, (int)s.Length);
                s.Close();
            }
        }

        public void PlayClick()
        {
            if (click_bytes != null)
                PlaySound(click_bytes, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_MEMORY));
        }


        public void PlayChangeState()
        {
            if (state_change_bytes != null)
                PlaySound(state_change_bytes, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_MEMORY));
        }


    }
}
