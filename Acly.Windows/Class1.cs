using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Acly.Windows
{
    public class WindowsSimplePlayer
    {
        public WindowsSimplePlayer(string fileName)
        {
            this.fileName = fileName;
            this.alias = Guid.NewGuid().ToString(); // Generate a unique alias for each audio file

            string command = $"open \"{fileName}\" type mpegvideo alias {alias}";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        private string fileName;
        private string alias;

        public enum PlayerState { Stopped, Playing, Paused }

        private PlayerState state;

        

        public void Play()
        {
            string command = $"play {alias}";
            mciSendString(command, null, 0, IntPtr.Zero);
            UpdateState();
        }

        public void Pause()
        {
            string command = $"pause {alias}";
            mciSendString(command, null, 0, IntPtr.Zero);
            UpdateState();
        }

        public void Stop()
        {
            string command = $"stop {alias}";
            mciSendString(command, null, 0, IntPtr.Zero);
            UpdateState();
        }

        public void SetVolume(int volume)
        {
            string command = $"setaudio {alias} volume to {volume}";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        private void UpdateState()
        {
            StringBuilder returnValue = new StringBuilder(128);
            string command = $"status {alias} mode";
            mciSendString(command, returnValue, 128, IntPtr.Zero);

            string mode = returnValue.ToString();
            if (mode.Contains("playing"))
            {
                state = PlayerState.Playing;
            }
            else if (mode.Contains("paused"))
            {
                state = PlayerState.Paused;
            }
            else
            {
                state = PlayerState.Stopped;
            }
        }

        public PlayerState GetState()
        {
            return state;
        }
    }
}
