﻿
// ReSharper disable InconsistentNaming
// ReSharper disable SuspiciousTypeConversion.Global
namespace Microsoft.WindowsAPICodePack.Taskbar
{
    /// <summary>
    /// Provides internal access to the functions provided by the ITaskbarList4 interface,
    /// without being forced to refer to it through another singleton.
    /// </summary>
    internal static class TaskbarList
    {
        private static readonly object _syncLock = new();

        private static ITaskbarList4? _taskBarList;
        internal static ITaskbarList4? Instance
        {
            get
            {
                if (_taskBarList == null)
                {
                    lock (_syncLock)
                    {
                        if (_taskBarList == null)
                        {
                            _taskBarList = (ITaskbarList4)new CTaskbarList();
                            _taskBarList.HrInit();
                        }
                    }
                }

                return _taskBarList;
            }
        }
    }
}
