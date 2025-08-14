//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem;

#region Property System Enumerations

/// <summary>
/// Property store cache state
/// </summary>
public enum PropertyStoreCacheState
{
    /// <summary>
    /// Contained in file, not updated.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Not contained in file.
    /// </summary>
    NotInSource = 1,

    /// <summary>
    /// Contained in file, has been updated since file was consumed.
    /// </summary>
    Dirty = 2
}

#endregion