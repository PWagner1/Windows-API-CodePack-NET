//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConvertToAutoProperty
// ReSharper disable InconsistentNaming
namespace Microsoft.WindowsAPICodePack.ApplicationServices;

/// <summary>
/// Provides methods for interacting with the Windows Application Restart and Recovery (ARR) APIs. These APIs allow
/// applications to register for automatic restart and recovery in the event of an unexpected termination.
/// </summary>
/// <remarks>This class contains P/Invoke declarations for native Windows functions related to application restart
/// and recovery. It is intended for internal use and provides low-level access to the ARR functionality. Developers can
/// use these methods to register recovery callbacks, manage restart behavior, and handle recovery progress.</remarks>
public static class AppRestartRecoveryNativeMethods
{
    #region Application Restart and Recovery Definitions

    internal delegate uint InternalRecoveryCallback(IntPtr state);

    private static readonly InternalRecoveryCallback _internalRecoveryCallback = new(InternalRecoveryHandler);
    internal static InternalRecoveryCallback InternalCallback => _internalRecoveryCallback;

    private static uint InternalRecoveryHandler(IntPtr parameter)
    {
        ApplicationRecoveryInProgress(out _);

        GCHandle handle = GCHandle.FromIntPtr(parameter);
        if (handle.Target is RecoveryData data)
        {
            data.Invoke();
        }

        handle.Free();

        return (0);
    }



    [DllImport("kernel32.dll")]
    internal static extern void ApplicationRecoveryFinished(
        [MarshalAs(UnmanagedType.Bool)] bool success);

    [DllImport("kernel32.dll")]
    [PreserveSig]
    internal static extern HResult ApplicationRecoveryInProgress(
        [Out, MarshalAs(UnmanagedType.Bool)] out bool canceled);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    [PreserveSig]
    internal static extern HResult RegisterApplicationRecoveryCallback(
        InternalRecoveryCallback callback, IntPtr param,
        uint pingInterval,
        uint flags); // Unused.

    [DllImport("kernel32.dll")]
    [PreserveSig]
    internal static extern HResult RegisterApplicationRestart(
        [MarshalAs(UnmanagedType.BStr)] string commandLineArgs,
        RestartRestrictions flags);

    [DllImport("kernel32.dll")]
    [PreserveSig]
    internal static extern HResult UnregisterApplicationRecoveryCallback();

    [DllImport("kernel32.dll")]
    [PreserveSig]
    internal static extern HResult UnregisterApplicationRestart();

    #endregion
}