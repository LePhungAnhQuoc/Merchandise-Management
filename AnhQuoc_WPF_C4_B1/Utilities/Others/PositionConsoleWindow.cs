using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; // To enable P/Invoke signatures.

namespace AnhQuoc_WPF_C4_B1
{
    class PositionConsoleWindow
    {
        #region P/Invoke declarations
        const int MONITOR_DEFAULTTOPRIMARY = 1;
        const uint SW_RESTORE = 9;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int x, y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
            public static MONITORINFO Default
            {
                get { var inst = new MONITORINFO(); inst.cbSize = (uint)Marshal.SizeOf(inst); return inst; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct WINDOWPLACEMENT
        {
            public uint Length;
            public uint Flags;
            public uint ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    var instance = new WINDOWPLACEMENT();
                    instance.Length = (uint)Marshal.SizeOf(instance);
                    return instance;
                }
            }
        }
        #endregion

        public static void Set(int left, int top)
        {
            IntPtr hWnd = GetConsoleWindow();

            var mi = MONITORINFO.Default;
            GetMonitorInfo(MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY), ref mi);
            var wp = WINDOWPLACEMENT.Default;
            GetWindowPlacement(hWnd, ref wp);

            int fudgeOffset = 7;

            int sizeHeight = wp.NormalPosition.Bottom - wp.NormalPosition.Top;
            int sizeWidth = wp.NormalPosition.Right - wp.NormalPosition.Left;

            wp.NormalPosition = new RECT();
            wp.NormalPosition.Left = -fudgeOffset + left;
            wp.NormalPosition.Right = sizeWidth + left;
            wp.NormalPosition.Top = top;
            wp.NormalPosition.Bottom
                = fudgeOffset + sizeHeight + top;

            try
            {
                SetWindowPlacement(hWnd, ref wp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey(true);

                Console.Clear();
                return;
            }
        }
    }
}
