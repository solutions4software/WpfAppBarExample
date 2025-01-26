using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace WpfAppBarExample.Utilities
{
    public static class DesktopExtension
    {
        #region P/Invoke

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        // Delegate to filter which windows to include 
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        const uint WM_USER = 0x0400; //http://msdn.microsoft.com/en-us/library/windows/desktop/ms644931(v=vs.85).aspx

        #endregion

        public enum DesktopWindow
        {
            ProgMan,
            SHELLDLL_DefViewParent,
            SHELLDLL_DefView,
            SysListView32
        }

        public static IntPtr GetDesktopWindow(DesktopWindow desktopWindow)
        {
            IntPtr _ProgMan = GetShellWindow();
            IntPtr _SHELLDLL_DefViewParent = _ProgMan;
            IntPtr _SHELLDLL_DefView = FindWindowEx(_ProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
            IntPtr _SysListView32 = FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");

            if (_SHELLDLL_DefView == IntPtr.Zero)
            {
                EnumWindows((hwnd, lParam) =>
                {
                    var sb = new StringBuilder(256);
                    GetClassName(hwnd, sb, sb.Capacity);

                    if (sb.ToString() == "WorkerW")
                    {
                        IntPtr child = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                        if (child != IntPtr.Zero)
                        {
                            _SHELLDLL_DefViewParent = hwnd;
                            _SHELLDLL_DefView = child;
                            _SysListView32 = FindWindowEx(child, IntPtr.Zero, "SysListView32", "FolderView"); ;
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            }

            switch (desktopWindow)
            {
                case DesktopWindow.ProgMan:
                    return _ProgMan;
                case DesktopWindow.SHELLDLL_DefViewParent:
                    return _SHELLDLL_DefViewParent;
                case DesktopWindow.SHELLDLL_DefView:
                    return _SHELLDLL_DefView;
                case DesktopWindow.SysListView32:
                    return _SysListView32;
                default:
                    return IntPtr.Zero;
            }
        }

        public static bool FindDesktop()
        {
            try
            {
                IntPtr _desktopHandle = GetDesktopWindow(DesktopWindow.SHELLDLL_DefViewParent);

                //Logger.Info("INIT PTR: {0}", _desktopHandle.ToInt32());
                PostMessage(_desktopHandle, WM_USER + 436, (IntPtr)0, (IntPtr)0);

                do
                {
                    _desktopHandle = GetDesktopWindow(DesktopWindow.SysListView32);
                    //Logger.Info("PTR: {0}", _desktopHandle.ToInt32());

                    if (_desktopHandle.ToInt32() != 0)
                    {
                        //Logger.Info("Success. Breaking out of loop.");
                        return true;
                    }

                    Thread.Sleep(100); // default time is 1000
                } while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public static void SendWpfWindowBack(Window window)
        {
            try
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
