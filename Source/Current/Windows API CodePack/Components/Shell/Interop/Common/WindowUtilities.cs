//Copyright (c) Microsoft Corporation.  All rights reserved.

using Size = System.Drawing.Size;

namespace Microsoft.WindowsAPICodePack.Shell
{

    internal static class WindowUtilities
    {
        internal static System.Drawing.Point GetParentOffsetOfChild(IntPtr hwnd, IntPtr hwndParent)
        {
            var childScreenCoord = new NativePoint();

            TabbedThumbnailNativeMethods.ClientToScreen(hwnd, ref childScreenCoord);

            var parentScreenCoord = new NativePoint();

            TabbedThumbnailNativeMethods.ClientToScreen(hwndParent, ref parentScreenCoord);

            System.Drawing.Point offset = new(
                childScreenCoord.X - parentScreenCoord.X,
                childScreenCoord.Y - parentScreenCoord.Y);

            return offset;
        }

        internal static System.Drawing.Size GetNonClientArea(IntPtr hwnd)
        {
            var c = new NativePoint();

            TabbedThumbnailNativeMethods.ClientToScreen(hwnd, ref c);

            var r = new NativeRect();

            TabbedThumbnailNativeMethods.GetWindowRect(hwnd, ref r);

            return new Size(c.X - r.Left, c.Y - r.Top);
        }
    }
}
