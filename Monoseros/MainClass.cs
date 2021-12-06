using Dalamud.Plugin;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Monoseros
{
    public class MainClass : IDalamudPlugin
    {
        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        internal static Thread Thread;
        internal static Process CurrentProcess;
        internal static bool IsRunning;
        public string Name => "Monoseros";

        public MainClass()
        {
            CurrentProcess = Process.GetCurrentProcess();
            IsRunning = true;

            Thread = new(new ThreadStart(() =>
            {
                while(IsRunning)
                {
                    PostMessage(CurrentProcess.Handle, 0x0100U, new IntPtr(0x0012), new IntPtr(0x001F0001));
                    Thread.Sleep(10);
                    PostMessage(CurrentProcess.Handle, 0x0101U, new IntPtr(0x0012), new IntPtr(0x001F0001));
                    if (IsRunning) Thread.Sleep(60 * 5 * 1000);
                }
            }));

            Thread.IsBackground = true;
            Thread.Start();
        }

        public void Dispose()
        {
            IsRunning = false;
            //GC.SuppressFinalize(this);
        }
    }
}
