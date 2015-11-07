﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Interop
{
    public static class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition,
           [In] ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        internal static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", SetLastError = true)] // SETLAST by us
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        internal static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        internal static extern bool DestroyWindow(IntPtr hostWindowHandle);
        
        [DllImport("user32.dll")]
        internal static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        /// <summary>
        /// Converts an item identifier list to a file system path. (Note: SHGetPathFromIDList calls the ANSI version, must call SHGetPathFromIDListW for .NET)
        /// </summary>
        /// <param name="pidl">Address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
        /// <param name="pszPath">Address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.</param>
        /// <returns>Returns TRUE if successful, or FALSE otherwise. </returns>
        [DllImport("shell32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

        /// <summary>
        /// Retrieves a handle to the specified window's parent or owner.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose parent window handle is to be retrieved.</param>
        /// <returns>If the window is a child window, the return value is a handle to the parent window. If the window is a top-level window with the WS_POPUP style, the return value is a handle to the owner window. If the function fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetParent(IntPtr hWnd);


        [DllImport("user32.dll")]
        internal static extern IntPtr CreateIconIndirect([In] ref ICONINFO iconInfo);

        public static int GWL_STYLE = -16;
        public static int WS_CHILD = 0x40000000; 


        public static int HighWord(int number)
        {
            return ((number & 0x80000000) == 0x80000000) ?
                (number >> 16) : ((number >> 16) & 0xffff);
        }

        public static int LowWord(int number)
        {
            return number & 0xffff;
        }

    }
}
