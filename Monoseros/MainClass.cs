using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.IoC;
using Dalamud.Plugin;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

#pragma warning disable CA1416
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
        [PluginService] public static ChatGui ChatGui { get; private set; } = null!;

        public MainClass()
        {
            ChatGui.PrintChat(new XivChatEntry() { Message = "Monoseros Init", Type = XivChatType.Echo });
            CurrentProcess = Process.GetCurrentProcess();

            PostMessage(CurrentProcess.MainWindowHandle, 256U, new IntPtr(32), IntPtr.Zero);
            Thread.Sleep(10);
            PostMessage(CurrentProcess.MainWindowHandle, 257U, new IntPtr(32), IntPtr.Zero);
            IsRunning = true;
            Thread = new(new ThreadStart(() =>
            {
                while(IsRunning)
                {
                    ChatGui.PrintChat(new XivChatEntry() { Message = "Input Action ", Type = XivChatType.Echo });

                    PostMessage(CurrentProcess.MainWindowHandle, 256U, new IntPtr(18), IntPtr.Zero);
                    Thread.Sleep(10);
                    PostMessage(CurrentProcess.MainWindowHandle, 257U, new IntPtr(18), IntPtr.Zero);

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
#pragma warning restore CA1416
