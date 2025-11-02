namespace MS.WindowsAPICodePack.Internal;

/// <summary>
/// Defines Windows message codes used in window procedures.
/// </summary>
public enum WindowMessage
{
    /// <summary>
    /// Null message. Used for internal purposes.
    /// </summary>
    Null = 0x00,
    
    /// <summary>
    /// Sent when an application requests that a window be created.
    /// </summary>
    Create = 0x01,
    
    /// <summary>
    /// Sent when a window is being destroyed.
    /// </summary>
    Destroy = 0x02,
    
    /// <summary>
    /// Sent after a window has been moved.
    /// </summary>
    Move = 0x03,
    
    /// <summary>
    /// Sent to a window after its size has changed.
    /// </summary>
    Size = 0x05,
    
    /// <summary>
    /// Sent when a window is being activated or deactivated.
    /// </summary>
    Activate = 0x06,
    
    /// <summary>
    /// Sent to a window after it has gained keyboard focus.
    /// </summary>
    SetFocus = 0x07,
    
    /// <summary>
    /// Sent to a window immediately before it loses keyboard focus.
    /// </summary>
    KillFocus = 0x08,
    
    /// <summary>
    /// Sent when an application changes the enabled state of a window.
    /// </summary>
    Enable = 0x0A,
    
    /// <summary>
    /// Sent to a window to allow changes in its appearance to be redrawn or to prevent changes.
    /// </summary>
    SetRedraw = 0x0B,
    
    /// <summary>
    /// Sets the text of a window.
    /// </summary>
    SetText = 0x0C,
    
    /// <summary>
    /// Copies the text that corresponds to a window into a buffer provided by the caller.
    /// </summary>
    GetText = 0x0D,
    
    /// <summary>
    /// Determines the length of the text associated with a window.
    /// </summary>
    GetTextLength = 0x0E,
    
    /// <summary>
    /// Sent when the system or another application makes a request to paint a portion of a window.
    /// </summary>
    Paint = 0x0F,
    
    /// <summary>
    /// Sent as a signal that a window or application should terminate.
    /// </summary>
    Close = 0x10,
    
    /// <summary>
    /// Sent when the user chooses to end the session or when an application calls ExitWindows.
    /// </summary>
    QueryEndSession = 0x11,
    
    /// <summary>
    /// Indicates a request to terminate an application.
    /// </summary>
    Quit = 0x12,
    
    /// <summary>
    /// Sent to an icon when the user requests that the window be restored to its previous size and position.
    /// </summary>
    QueryOpen = 0x13,
    
    /// <summary>
    /// Sent when the window background must be erased.
    /// </summary>
    EraseBackground = 0x14,
    
    /// <summary>
    /// Sent to all top-level windows when a change is made to system color settings.
    /// </summary>
    SystemColorChange = 0x15,
    
    /// <summary>
    /// Sent to an application after the system ends the session.
    /// </summary>
    EndSession = 0x16,
    
    /// <summary>
    /// Sent when a system error occurs.
    /// </summary>
    SystemError = 0x17,
    
    /// <summary>
    /// Sent to a window when the window is about to be hidden or shown.
    /// </summary>
    ShowWindow = 0x18,
    
    /// <summary>
    /// Sent to the parent window of an owner-drawn control when the control is about to be drawn.
    /// </summary>
    ControlColor = 0x19,
    
    /// <summary>
    /// Sent to all top-level windows when the WIN.INI file is changed.
    /// </summary>
    WinIniChange = 0x1A,
    
    /// <summary>
    /// Sent to all top-level windows when a change is made to a system parameter setting.
    /// </summary>
    SettingChange = 0x1A,
    
    /// <summary>
    /// Sent to all top-level windows when the default device mode has changed.
    /// </summary>
    DevModeChange = 0x1B,
    
    /// <summary>
    /// Sent when an application is going to become the active application.
    /// </summary>
    ActivateApplication = 0x1C,
    
    /// <summary>
    /// Sent to all top-level windows when the pool of font resources has changed.
    /// </summary>
    FontChange = 0x1D,
    
    /// <summary>
    /// Sent to all top-level windows when the system time changes.
    /// </summary>
    TimeChange = 0x1E,
    
    /// <summary>
    /// Sent to cancel certain modes, such as mouse capture.
    /// </summary>
    CancelMode = 0x1F,
    
    /// <summary>
    /// Sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
    /// </summary>
    SetCursor = 0x20,
    
    /// <summary>
    /// Sent when the cursor is in an inactive window and the user presses a mouse button.
    /// </summary>
    MouseActivate = 0x21,
    
    /// <summary>
    /// Sent to a child window when the user clicks the window's title bar or when the window is activated.
    /// </summary>
    ChildActivate = 0x22,
    
    /// <summary>
    /// Sent by a computer-based training (CBT) application to separate user-input messages.
    /// </summary>
    QueueSync = 0x23,
    
    /// <summary>
    /// Sent to a window when its size or position is about to change.
    /// </summary>
    GetMinMaxInfo = 0x24,
    
    /// <summary>
    /// Sent to a minimized window when the icon is to be painted.
    /// </summary>
    PaintIcon = 0x26,
    
    /// <summary>
    /// Sent to a minimized window when the background of the icon must be filled.
    /// </summary>
    IconEraseBackground = 0x27,
    
    /// <summary>
    /// Sent to a dialog box procedure to set the keyboard focus to a different control.
    /// </summary>
    NextDialogControl = 0x28,
    
    /// <summary>
    /// Sent from Print Manager whenever a job is added to or removed from the queue.
    /// </summary>
    SpoolerStatus = 0x2A,
    
    /// <summary>
    /// Sent to the parent window of an owner-drawn control when the control needs to be drawn.
    /// </summary>
    DrawItem = 0x2B,
    
    /// <summary>
    /// Sent to the owner window of a combo box, list box, or menu item when the control is created.
    /// </summary>
    MeasureItem = 0x2C,
    
    /// <summary>
    /// Sent to the owner of a list box or combo box when the control is destroyed.
    /// </summary>
    DeleteItem = 0x2D,
    
    /// <summary>
    /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.
    /// </summary>
    VKeyToItem = 0x2E,
    
    /// <summary>
    /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.
    /// </summary>
    CharToItem = 0x2F,

    /// <summary>
    /// Sets the font that a control is to use when drawing text.
    /// </summary>
    SetFont = 0x30,
    
    /// <summary>
    /// Retrieves the font that a control is currently using to draw its text.
    /// </summary>
    GetFont = 0x31,
    
    /// <summary>
    /// Associates a hot key with a window.
    /// </summary>
    SetHotkey = 0x32,
    
    /// <summary>
    /// Determines the hot key associated with a window.
    /// </summary>
    GetHotkey = 0x33,
    
    /// <summary>
    /// Sent to a minimized window when the user drags the icon.
    /// </summary>
    QueryDragIcon = 0x37,
    
    /// <summary>
    /// Sent to determine the relative position of a new item in the sorted list of an owner-drawn combo or list box.
    /// </summary>
    CompareItem = 0x39,
    
    /// <summary>
    /// Sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory.
    /// </summary>
    Compacting = 0x41,
    
    /// <summary>
    /// Sent to a window whose size, position, or place in the Z order is about to change.
    /// </summary>
    WindowPositionChanging = 0x46,
    
    /// <summary>
    /// Sent to a window whose size, position, or place in the Z order has changed.
    /// </summary>
    WindowPositionChanged = 0x47,
    
    /// <summary>
    /// Notifies applications that the system is entering or leaving suspend mode.
    /// </summary>
    Power = 0x48,
    
    /// <summary>
    /// Sent when data is to be copied between applications.
    /// </summary>
    CopyData = 0x4A,
    
    /// <summary>
    /// Notifies applications that a system journal record is being canceled.
    /// </summary>
    CancelJournal = 0x4B,
    
    /// <summary>
    /// Sent by a common control to its parent window when an event has occurred or the control requires some information.
    /// </summary>
    Notify = 0x4E,
    
    /// <summary>
    /// Posted to the window with the focus when the user chooses a new input language.
    /// </summary>
    InputLanguageChangeRequest = 0x50,
    
    /// <summary>
    /// Sent to the topmost affected window after an application's input language has been changed.
    /// </summary>
    InputLanguageChange = 0x51,
    
    /// <summary>
    /// Sent to an application that has initiated a training card with the system.
    /// </summary>
    Card = 0x52,
    
    /// <summary>
    /// Indicates that the user pressed the F1 key.
    /// </summary>
    Help = 0x53,
    
    /// <summary>
    /// Sent to all windows after the user has logged on or off.
    /// </summary>
    UserChanged = 0x54,
    
    /// <summary>
    /// Sent by a common control to its parent window to determine whether the control should display tooltips in Unicode or ANSI.
    /// </summary>
    NotifyFormat = 0x55,
    
    /// <summary>
    /// Notifies a window that the user clicked the right mouse button in the window.
    /// </summary>
    ContextMenu = 0x7B,
    
    /// <summary>
    /// Sent to a window when the style or extended style is about to change.
    /// </summary>
    StyleChanging = 0x7C,
    
    /// <summary>
    /// Sent to a window after the style or extended style has changed.
    /// </summary>
    StyleChanged = 0x7D,
    
    /// <summary>
    /// Sent to all windows when the display resolution has changed.
    /// </summary>
    DisplayChange = 0x7E,
    
    /// <summary>
    /// Sent to a window to retrieve a handle to the large or small icon associated with the window.
    /// </summary>
    GetIcon = 0x7F,
    
    /// <summary>
    /// Associates a new large or small icon with a window.
    /// </summary>
    SetIcon = 0x80,

    /// <summary>
    /// Sent prior to the Create message when a window is first created in the nonclient area.
    /// </summary>
    NcCreate = 0x81,
    
    /// <summary>
    /// Notifies a window that its nonclient area is being destroyed.
    /// </summary>
    NcDestroy = 0x82,
    
    /// <summary>
    /// Sent when the size and position of a window's client area must be calculated.
    /// </summary>
    NcCalculateSize = 0x83,
    
    /// <summary>
    /// Sent to a window to determine what part of the window corresponds to a particular screen coordinate.
    /// </summary>
    NcHitTest = 0x84,
    
    /// <summary>
    /// Sent to a window when its frame must be painted.
    /// </summary>
    NcPaint = 0x85,
    
    /// <summary>
    /// Sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
    /// </summary>
    NcActivate = 0x86,
    
    /// <summary>
    /// Sent to a control when a dialog box is created or when a message arrives that requires the control to process.
    /// </summary>
    GetDialogCode = 0x87,
    
    /// <summary>
    /// Posted to a window when the cursor moves within the nonclient area of the window.
    /// </summary>
    NcMouseMove = 0xA0,
    
    /// <summary>
    /// Posted when the user presses the left mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcLeftButtonDown = 0xA1,
    
    /// <summary>
    /// Posted when the user releases the left mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcLeftButtonUp = 0xA2,
    
    /// <summary>
    /// Posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcLeftButtonDoubleClick = 0xA3,
    
    /// <summary>
    /// Posted when the user presses the right mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcRightButtonDown = 0xA4,
    
    /// <summary>
    /// Posted when the user releases the right mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcRightButtonUp = 0xA5,
    
    /// <summary>
    /// Posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcRightButtonDoubleClick = 0xA6,
    
    /// <summary>
    /// Posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcMiddleButtonDown = 0xA7,
    
    /// <summary>
    /// Posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcMiddleButtonUp = 0xA8,
    
    /// <summary>
    /// Posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window.
    /// </summary>
    NcMiddleButtonDoubleClick = 0xA9,

    /// <summary>
    /// First keyboard message value in the range of keyboard messages.
    /// </summary>
    KeyFirst = 0x100,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when a nonsystem key is pressed.
    /// </summary>
    KeyDown = 0x100,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when a nonsystem key is released.
    /// </summary>
    KeyUp = 0x101,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when a WM_KEYDOWN message is translated.
    /// </summary>
    Char = 0x102,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when a WM_KEYUP message is translated.
    /// </summary>
    DeadChar = 0x103,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when the user presses the F10 key or holds down the ALT key and then presses another key.
    /// </summary>
    SystemKeyDown = 0x104,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down.
    /// </summary>
    SystemKeyUp = 0x105,
    
    /// <summary>
    /// Posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated.
    /// </summary>
    SystemChar = 0x106,
    
    /// <summary>
    /// Sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function.
    /// </summary>
    SystemDeadChar = 0x107,
    
    /// <summary>
    /// Last keyboard message value in the range of keyboard messages.
    /// </summary>
    KeyLast = 0x108,

    /// <summary>
    /// Sent to an application when the IME window finds no space to extend the area for the composition window.
    /// </summary>
    ImeStartComposition = 0x10D,
    
    /// <summary>
    /// Sent to an application when the IME ends composition.
    /// </summary>
    ImeEndComposition = 0x10E,
    
    /// <summary>
    /// Sent to an application when the IME changes composition status as a result of a keystroke.
    /// </summary>
    ImeComposition = 0x10F,
    
    /// <summary>
    /// Last IME keyboard message.
    /// </summary>
    ImeKeyLast = 0x10F,

    /// <summary>
    /// Sent to a dialog box procedure immediately before a dialog box is displayed.
    /// </summary>
    InitializeDialog = 0x110,
    
    /// <summary>
    /// Sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
    /// </summary>
    Command = 0x111,
    
    /// <summary>
    /// Sent when the user selects a command from the Window menu or when the user selects the maximize button, minimize button, restore button, or close button.
    /// </summary>
    SystemCommand = 0x112,
    
    /// <summary>
    /// Posted to the installing thread's message queue when a timer expires.
    /// </summary>
    Timer = 0x113,
    
    /// <summary>
    /// Sent to a window when a scroll event occurs in the window's standard horizontal scroll bar.
    /// </summary>
    HorizontalScroll = 0x114,
    
    /// <summary>
    /// Sent to a window when a scroll event occurs in the window's standard vertical scroll bar.
    /// </summary>
    VerticalScroll = 0x115,
    
    /// <summary>
    /// Sent when a menu is about to become active.
    /// </summary>
    InitializeMenu = 0x116,
    
    /// <summary>
    /// Sent when a drop-down menu or submenu is about to become active.
    /// </summary>
    InitializeMenuPopup = 0x117,
    
    /// <summary>
    /// Sent when the user selects a menu item.
    /// </summary>
    MenuSelect = 0x11F,
    
    /// <summary>
    /// Sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key.
    /// </summary>
    MenuChar = 0x120,
    
    /// <summary>
    /// Sent to the owner window of a modal dialog box or menu that is entering an idle state.
    /// </summary>
    EnterIdle = 0x121,

    /// <summary>
    /// Sent to the owner window of a message box before the system draws the message box.
    /// </summary>
    CtlColorMessageBox = 0x132,
    
    /// <summary>
    /// Sent to the parent window of an edit control when the control is about to be drawn.
    /// </summary>
    CtlColorEdit = 0x133,
    
    /// <summary>
    /// Sent to the parent window of a list box before the system draws the list box.
    /// </summary>
    CtlColorListbox = 0x134,
    
    /// <summary>
    /// Sent to the parent window of a button before drawing the button.
    /// </summary>
    CtlColorButton = 0x135,
    
    /// <summary>
    /// Sent to a dialog box before the system draws the dialog box.
    /// </summary>
    CtlColorDialog = 0x136,
    
    /// <summary>
    /// Sent to the parent window of a scroll bar control when the control is about to be drawn.
    /// </summary>
    CtlColorScrollBar = 0x137,
    
    /// <summary>
    /// Sent to the parent window of a static control before the system draws the control.
    /// </summary>
    CtlColorStatic = 0x138,

    /// <summary>
    /// First mouse message value in the range of mouse messages.
    /// </summary>
    MouseFirst = 0x200,
    
    /// <summary>
    /// Posted to a window when the cursor moves.
    /// </summary>
    MouseMove = 0x200,
    
    /// <summary>
    /// Posted when the user presses the left mouse button while the cursor is in the client area of a window.
    /// </summary>
    LeftButtonDown = 0x201,
    
    /// <summary>
    /// Posted when the user releases the left mouse button while the cursor is in the client area of a window.
    /// </summary>
    LeftButtonUp = 0x202,
    
    /// <summary>
    /// Posted when the user double-clicks the left mouse button while the cursor is in the client area of a window.
    /// </summary>
    LeftButtonDoubleClick = 0x203,
    
    /// <summary>
    /// Posted when the user presses the right mouse button while the cursor is in the client area of a window.
    /// </summary>
    RightButtonDown = 0x204,
    
    /// <summary>
    /// Posted when the user releases the right mouse button while the cursor is in the client area of a window.
    /// </summary>
    RightButtonUp = 0x205,
    
    /// <summary>
    /// Posted when the user double-clicks the right mouse button while the cursor is in the client area of a window.
    /// </summary>
    RightButtonDoubleClick = 0x206,
    
    /// <summary>
    /// Posted when the user presses the middle mouse button while the cursor is in the client area of a window.
    /// </summary>
    MiddleButtonDown = 0x207,
    
    /// <summary>
    /// Posted when the user releases the middle mouse button while the cursor is in the client area of a window.
    /// </summary>
    MiddleButtonUp = 0x208,
    
    /// <summary>
    /// Posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window.
    /// </summary>
    MiddleButtonDoubleClick = 0x209,
    
    /// <summary>
    /// Sent to the focus window when the mouse wheel is rotated.
    /// </summary>
    MouseWheel = 0x20A,
    
    /// <summary>
    /// Sent to the active window when the mouse's horizontal scroll wheel is tilted or rotated.
    /// </summary>
    MouseHorizontalWheel = 0x20E,

    /// <summary>
    /// Sent to the parent window of a control when an event occurs in the control.
    /// </summary>
    ParentNotify = 0x210,
    
    /// <summary>
    /// Notifies an application that a menu modal loop has been entered.
    /// </summary>
    EnterMenuLoop = 0x211,
    
    /// <summary>
    /// Notifies an application that a menu modal loop has been exited.
    /// </summary>
    ExitMenuLoop = 0x212,
    
    /// <summary>
    /// Sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.
    /// </summary>
    NextMenu = 0x213,
    
    /// <summary>
    /// Sent to a window that the user is resizing.
    /// </summary>
    Sizing = 0x214,
    
    /// <summary>
    /// Sent to the window that is losing the mouse capture.
    /// </summary>
    CaptureChanged = 0x215,
    
    /// <summary>
    /// Sent to a window that the user is moving.
    /// </summary>
    Moving = 0x216,
    
    /// <summary>
    /// Notifies applications that a power-management event has occurred.
    /// </summary>
    PowerBroadcast = 0x218,
    
    /// <summary>
    /// Notifies an application of a change to the hardware configuration of a device or the computer.
    /// </summary>
    DeviceChange = 0x219,

    /// <summary>
    /// Sent to create an MDI child window.
    /// </summary>
    MdiCreate = 0x220,
    
    /// <summary>
    /// Sent to close an MDI child window.
    /// </summary>
    MdiDestroy = 0x221,
    
    /// <summary>
    /// Sent to activate an MDI child window.
    /// </summary>
    MdiActivate = 0x222,
    
    /// <summary>
    /// Sent to restore an MDI child window from maximized or minimized size.
    /// </summary>
    MdiRestore = 0x223,
    
    /// <summary>
    /// Sent to activate the next or previous MDI child window.
    /// </summary>
    MdiNext = 0x224,
    
    /// <summary>
    /// Sent to maximize an MDI child window.
    /// </summary>
    MdiMaximize = 0x225,
    
    /// <summary>
    /// Sent to arrange all MDI child windows in a tiled format.
    /// </summary>
    MdiTile = 0x226,
    
    /// <summary>
    /// Sent to arrange all MDI child windows in a cascade format.
    /// </summary>
    MdiCascade = 0x227,
    
    /// <summary>
    /// Sent to arrange all minimized MDI child windows.
    /// </summary>
    MdiIconArrange = 0x228,
    
    /// <summary>
    /// Sent to retrieve the handle to the active MDI child window.
    /// </summary>
    MdiGetActive = 0x229,
    
    /// <summary>
    /// Sent to replace the entire menu of an MDI frame window.
    /// </summary>
    MdiSetMenu = 0x230,
    
    /// <summary>
    /// Sent once to a window after it enters the moving or sizing modal loop.
    /// </summary>
    EnterSizeMove = 0x231,
    
    /// <summary>
    /// Sent once to a window after it has exited moving or sizing modal loop.
    /// </summary>
    ExitSizeMove = 0x232,
    
    /// <summary>
    /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
    /// </summary>
    DropFiles = 0x233,
    
    /// <summary>
    /// Sent to refresh the menu of an MDI frame window.
    /// </summary>
    MdiRefreshMenu = 0x234,

    /// <summary>
    /// Sent to an application when a window is activated.
    /// </summary>
    ImeSetContext = 0x281,
    
    /// <summary>
    /// Sent to an application to notify it of changes to the IME window.
    /// </summary>
    ImeNotify = 0x282,
    
    /// <summary>
    /// Sent by an application to direct the IME window to carry out the requested command.
    /// </summary>
    ImeControl = 0x283,
    
    /// <summary>
    /// Sent to an application when the IME window finds that no more room is available to extend the area for the composition window.
    /// </summary>
    ImeCompositionFull = 0x284,
    
    /// <summary>
    /// Sent to an application when the operating system is about to change the current IME.
    /// </summary>
    ImeSelect = 0x285,
    
    /// <summary>
    /// Sent to an application when the IME gets a character of the conversion result.
    /// </summary>
    ImeChar = 0x286,
    
    /// <summary>
    /// Sent to an application by the IME to notify the application of a key press.
    /// </summary>
    ImeKeyDown = 0x290,
    
    /// <summary>
    /// Sent to an application by the IME to notify the application of a key release.
    /// </summary>
    ImeKeyUp = 0x291,

    /// <summary>
    /// Posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
    /// </summary>
    MouseHover = 0x2A1,
    
    /// <summary>
    /// Posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
    /// </summary>
    NcMouseLeave = 0x2A2,
    
    /// <summary>
    /// Posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
    /// </summary>
    MouseLeave = 0x2A3,

    /// <summary>
    /// Sent to a window when the user chooses Cut from the Edit menu.
    /// </summary>
    Cut = 0x300,
    
    /// <summary>
    /// Sent to a window when the user chooses Copy from the Edit menu.
    /// </summary>
    Copy = 0x301,
    
    /// <summary>
    /// Sent to a window when the user chooses Paste from the Edit menu.
    /// </summary>
    Paste = 0x302,
    
    /// <summary>
    /// Sent to a window when the user chooses Clear from the Edit menu.
    /// </summary>
    Clear = 0x303,
    
    /// <summary>
    /// Sent to a window when the user chooses Undo from the Edit menu.
    /// </summary>
    Undo = 0x304,

    /// <summary>
    /// Sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format.
    /// </summary>
    RenderFormat = 0x305,
    
    /// <summary>
    /// Sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats.
    /// </summary>
    RenderAllFormats = 0x306,
    
    /// <summary>
    /// Sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.
    /// </summary>
    DestroyClipboard = 0x307,
    
    /// <summary>
    /// Sent to the first window in the clipboard viewer chain when the content of the clipboard changes.
    /// </summary>
    DrawClipbard = 0x308,
    
    /// <summary>
    /// Sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format.
    /// </summary>
    PaintClipbard = 0x309,
    
    /// <summary>
    /// Sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar.
    /// </summary>
    VerticalScrollClipBoard = 0x30A,
    
    /// <summary>
    /// Sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.
    /// </summary>
    SizeClipbard = 0x30B,
    
    /// <summary>
    /// Sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
    /// </summary>
    AskClipboardFormatname = 0x30C,
    
    /// <summary>
    /// Sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
    /// </summary>
    ChangeClipboardChain = 0x30D,
    
    /// <summary>
    /// Sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar.
    /// </summary>
    HorizontalScrollClipboard = 0x30E,
    
    /// <summary>
    /// Sent to a window that is about to receive the keyboard focus, allowing the window to realize its logical palette when it receives the focus.
    /// </summary>
    QueryNewPalette = 0x30F,
    
    /// <summary>
    /// Sent to all windows that have a palette, informing them that an application is going to realize its logical palette.
    /// </summary>
    PaletteIsChanging = 0x310,
    
    /// <summary>
    /// Sent to all top-level and overlapped windows after the window with the keyboard focus has realized its logical palette.
    /// </summary>
    PaletteChanged = 0x311,

    /// <summary>
    /// Posted when the user presses a hot key registered by the RegisterHotKey function.
    /// </summary>
    Hotkey = 0x312,
    
    /// <summary>
    /// Sent to a window to request that it draw itself in the specified device context.
    /// </summary>
    Print = 0x317,
    
    /// <summary>
    /// Sent to a window to request that it draw its client area in the specified device context.
    /// </summary>
    PrintClient = 0x318,

    /// <summary>
    /// First handheld-specific message.
    /// </summary>
    HandHeldFirst = 0x358,
    
    /// <summary>
    /// Last handheld-specific message.
    /// </summary>
    HandHeldlast = 0x35F,
    
    /// <summary>
    /// First PenWindows message.
    /// </summary>
    PenWinFirst = 0x380,
    
    /// <summary>
    /// Last PenWindows message.
    /// </summary>
    PenWinLast = 0x38F,
    
    /// <summary>
    /// First coalesced paint message.
    /// </summary>
    CoalesceFirst = 0x390,
    
    /// <summary>
    /// Last coalesced paint message.
    /// </summary>
    CoalesceLast = 0x39F,
    
    /// <summary>
    /// First DDE message value in the range of DDE messages.
    /// </summary>
    DdeFirst = 0x3E0,
    
    /// <summary>
    /// Sent by a client application to initiate a conversation with a server application.
    /// </summary>
    DdeInitiate = 0x3E0,
    
    /// <summary>
    /// Sent to terminate a DDE conversation.
    /// </summary>
    DdeTerminate = 0x3E1,
    
    /// <summary>
    /// Sent to establish a data link between a client and server for a particular data item.
    /// </summary>
    DdeAdvise = 0x3E2,
    
    /// <summary>
    /// Sent to terminate a data link previously established.
    /// </summary>
    DdeUnadvise = 0x3E3,
    
    /// <summary>
    /// Sent to acknowledge receipt of a DDE message.
    /// </summary>
    DdeAck = 0x3E4,
    
    /// <summary>
    /// Sent to provide data to a client application.
    /// </summary>
    DdeData = 0x3E5,
    
    /// <summary>
    /// Sent to request data from a server application.
    /// </summary>
    DdeRequest = 0x3E6,
    
    /// <summary>
    /// Sent to provide unsolicited data to a server application.
    /// </summary>
    DdePoke = 0x3E7,
    
    /// <summary>
    /// Sent to execute a series of commands.
    /// </summary>
    DdeExecute = 0x3E8,
    
    /// <summary>
    /// Last DDE message value in the range of DDE messages.
    /// </summary>
    DdeLast = 0x3E8,

    /// <summary>
    /// Starting value for private user-defined window messages.
    /// </summary>
    User = 0x400,
    
    /// <summary>
    /// Starting value for application-specific messages.
    /// </summary>
    App = 0x8000,
}
