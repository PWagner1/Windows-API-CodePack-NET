// ReSharper disable InconsistentNaming
namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop;

internal static class HandlerNativeMethods
{
    [DllImport("user32.dll")]
    internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetFocus();

    [DllImport("user32.dll")]
    internal static extern void SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        SetWindowPositionOptions flags);

    internal static readonly Guid IPreviewHandlerGuid = new("8895b1c6-b41f-4c1c-a562-0d564250836f");
    internal static readonly Guid IThumbnailProviderGuid = new("e357fccd-a995-4576-b01f-234630154e96");

    internal static readonly Guid IInitializeWithFileGuid = new("b7d14566-0509-4cce-a71f-0a554233bd9b");
    internal static readonly Guid IInitializeWithStreamGuid = new("b824b49d-22ac-4161-ac8a-9916e8fa3f7f");
    internal static readonly Guid IInitializeWithItemGuid = new("7f73be3f-fb79-493c-a6c7-7ee14e245841");

    internal static readonly Guid IMarshalGuid = new("00000003-0000-0000-C000-000000000046");
}

#region Interfaces

#endregion

#region Structs

#endregion