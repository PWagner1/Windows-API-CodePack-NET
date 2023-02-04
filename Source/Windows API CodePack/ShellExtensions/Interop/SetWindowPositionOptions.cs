namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop;

[Flags]
internal enum SetWindowPositionOptions
{
    AsyncWindowPos = 0x4000,
    DeferErase = 0x2000,
    DrawFrame = FrameChanged,
    FrameChanged = 0x0020,  // The frame changed: send WM_NCCALCSIZE
    HideWindow = 0x0080,
    NoActivate = 0x0010,
    CoCopyBits = 0x0100,
    NoMove = 0x0002,
    NoOwnerZOrder = 0x0200,  // Don't do owner Z ordering
    NoRedraw = 0x0008,
    NoResposition = NoOwnerZOrder,
    NoSendChanging = 0x0400,  // Don't send WM_WINDOWPOSCHANGING
    NoSize = 0x0001,
    NoZOrder = 0x0004,
    ShowWindow = 0x0040
}