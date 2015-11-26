using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Runtime.InteropServices;


namespace camera
{
    public class myConsoleWindow
    {
        public static IntPtr CreateConsole()
        {
            var console = new myConsoleWindow();
            return console.Hwnd;
        }

        public IntPtr Hwnd { get; private set; }

        public myConsoleWindow()
        {
            Initialize();
        }

        public void Initialize()
        {
            Hwnd = GetConsoleWindow();

            // Console app
            if (Hwnd != IntPtr.Zero)
            {
                return;
            }

            // Windows app
            AllocConsole();
            Hwnd = GetConsoleWindow();
        }

        #region Win32

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32")]
        static extern bool AllocConsole();


        #endregion
    }
}
