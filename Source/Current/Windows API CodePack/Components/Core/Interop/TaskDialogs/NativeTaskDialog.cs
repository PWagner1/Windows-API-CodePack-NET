//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable SuggestVarOrType_BuiltInTypes
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable StringLiteralTypo

namespace Microsoft.WindowsAPICodePack.Dialogs;

/// <summary>
/// Encapsulates the native logic required to create, 
/// configure, and show a TaskDialog, 
/// via the TaskDialogIndirect() Win32 function.
/// </summary>
/// <remarks>A new instance of this class should 
/// be created for each messagebox show, as
/// the HWNDs for TaskDialogs do not remain constant 
/// across calls to TaskDialogIndirect.
/// </remarks>
internal class NativeTaskDialog : IDisposable
{
    private readonly TaskDialogNativeMethods.TaskDialogConfiguration _nativeDialogConfig;
    private readonly NativeTaskDialogSettings _settings;
    private IntPtr _hWndDialog;
    private readonly TaskDialog _outerDialog;

    private readonly IntPtr[]? _updatedStrings = new IntPtr[Enum.GetNames(typeof(TaskDialogNativeMethods.TaskDialogElements)).Length];
    private IntPtr _buttonArray, _radioButtonArray;

    // Flag tracks whether our first radio 
    // button click event has come through.
    private bool _firstRadioButtonClicked = true;

    #region Constructors

    // Configuration is applied at dialog creation time.
    internal NativeTaskDialog(NativeTaskDialogSettings settings, TaskDialog outerDialog)
    {
        _nativeDialogConfig = settings.NativeConfiguration;
        _settings = settings;

        // Wireup dialog proc message loop for this instance.
        _nativeDialogConfig.callback = DialogProc;

        ShowState = DialogShowState.PreShow;

        // Keep a reference to the outer shell, so we can notify.
        _outerDialog = outerDialog;
    }

    #endregion

    #region Public Properties

    public DialogShowState ShowState { get; private set; }

    public int SelectedButtonId { get; private set; }

    public int SelectedRadioButtonId { get; private set; }

    public bool CheckBoxChecked { get; private set; }

    #endregion

    internal void NativeShow()
    {
        // Applies config struct and other settings, then
        // calls main Win32 function.
        if (_settings == null)
        {
            throw new InvalidOperationException(LocalizedMessages.NativeTaskDialogConfigurationError);
        }

        // Do a last-minute parse of the various dialog control lists,  
        // and only allocate the memory at the last minute.

        MarshalDialogControlStructs();

        // Make the call and show the dialog.
        // NOTE: this call is BLOCKING, though the thread 
        // WILL re-enter via the DialogProc.
        try
        {
            ShowState = DialogShowState.Showing;

            int selectedButtonId;
            int selectedRadioButtonId;
            bool checkBoxChecked;

            // Here is the way we use "vanilla" P/Invoke to call TaskDialogIndirect().  
            HResult hresult = TaskDialogNativeMethods.TaskDialogIndirect(
                _nativeDialogConfig,
                out selectedButtonId,
                out selectedRadioButtonId,
                out checkBoxChecked);

            if (CoreErrorHelper.Failed(hresult))
            {
                string msg;
                switch (hresult)
                {
                    case HResult.InvalidArguments:
                        msg = LocalizedMessages.NativeTaskDialogInternalErrorArgs;
                        break;
                    case HResult.OutOfMemory:
                        msg = LocalizedMessages.NativeTaskDialogInternalErrorComplex;
                        break;
                    default:
                        msg = string.Format(CultureInfo.InvariantCulture,
                            LocalizedMessages.NativeTaskDialogInternalErrorUnexpected,
                            hresult);
                        break;
                }
                Exception? e = Marshal.GetExceptionForHR((int)hresult);
                throw new Win32Exception(msg, e);
            }

            SelectedButtonId = selectedButtonId;
            SelectedRadioButtonId = selectedRadioButtonId;
            CheckBoxChecked = checkBoxChecked;
        }
        catch (EntryPointNotFoundException exc)
        {
            throw new NotSupportedException(LocalizedMessages.NativeTaskDialogVersionError, exc);
        }
        finally
        {
            ShowState = DialogShowState.Closed;
        }
    }

    // The new task dialog does not support the existing 
    // Win32 functions for closing (e.g. EndDialog()); instead,
    // a "click button" message is sent. In this case, we're 
    // abstracting out to say that the TaskDialog consumer can
    // simply call "Close" and we'll "click" the cancel button. 
    // Note that the cancel button doesn't actually
    // have to exist for this to work.
    internal void NativeClose(TaskDialogResult result)
    {
        ShowState = DialogShowState.Closing;

        int id;
        switch (result)
        {
            case TaskDialogResult.Close:
                id = (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close;
                break;
            case TaskDialogResult.CustomButtonClicked:
                id = DialogsDefaults.MinimumDialogControlId; // custom buttons
                break;
            case TaskDialogResult.No:
                id = (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.No;
                break;
            case TaskDialogResult.Ok:
                id = (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ok;
                break;
            case TaskDialogResult.Retry:
                id = (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Retry;
                break;
            case TaskDialogResult.Yes:
                id = (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Yes;
                break;
            default:
                id = (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Cancel;
                break;
        }

        SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickButton, id, 0);
    }

    #region Main Dialog Proc

    private int DialogProc(
        IntPtr windowHandle,
        uint message,
        IntPtr wparam,
        IntPtr lparam,
        IntPtr referenceData)
    {
        // Fetch the HWND - it may be the first time we're getting it.
        _hWndDialog = windowHandle;

        // Big switch on the various notifications the 
        // dialog proc can get.
        switch ((TaskDialogNativeMethods.TaskDialogNotifications)message)
        {
            case TaskDialogNativeMethods.TaskDialogNotifications.Created:
                int result = PerformDialogInitialization();
                _outerDialog.RaiseOpenedEvent();
                return result;
            case TaskDialogNativeMethods.TaskDialogNotifications.ButtonClicked:
                return HandleButtonClick((int)wparam);
            case TaskDialogNativeMethods.TaskDialogNotifications.RadioButtonClicked:
                return HandleRadioButtonClick((int)wparam);
            case TaskDialogNativeMethods.TaskDialogNotifications.HyperlinkClicked:
                return HandleHyperlinkClick(lparam);
            case TaskDialogNativeMethods.TaskDialogNotifications.Help:
                return HandleHelpInvocation();
            case TaskDialogNativeMethods.TaskDialogNotifications.Timer:
                return HandleTick((int)wparam);
            case TaskDialogNativeMethods.TaskDialogNotifications.Destroyed:
                return PerformDialogCleanup();
        }
        return (int)HResult.Ok;
    }

    // Once the task dialog HWND is open, we need to send 
    // additional messages to configure it.
    private int PerformDialogInitialization()
    {
        // Initialize Progress or Marquee Bar.
        if (IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar))
        {
            UpdateProgressBarRange();

            // The order of the following is important - 
            // state is more important than value, 
            // and non-normal states turn off the bar value change 
            // animation, which is likely the intended
            // and preferable behavior.
            UpdateProgressBarState(_settings.ProgressBarState);
            UpdateProgressBarValue(_settings.ProgressBarValue);

            // Due to a bug that wasn't fixed in time for RTM of Vista,
            // second SendMessage is required if the state is non-Normal.
            UpdateProgressBarValue(_settings.ProgressBarValue);
        }
        else if (IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar))
        {
            // TDM_SET_PROGRESS_BAR_MARQUEE is necessary 
            // to cause the marquee to start animating.
            // Note that this internal task dialog setting is 
            // round-tripped when the marquee is
            // is set to different states, so it never has to 
            // be touched/sent again.
            SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarMarquee, 1, 0);
            UpdateProgressBarState(_settings.ProgressBarState);
        }

        if (_settings.ElevatedButtons != null && _settings.ElevatedButtons.Count > 0)
        {
            foreach (int id in _settings.ElevatedButtons)
            {
                UpdateElevationIcon(id, true);
            }
        }

        return CoreErrorHelper.Ignored;
    }

    private int HandleButtonClick(int id)
    {
        // First we raise a Click event, if there is a custom button
        // However, we implement Close() by sending a cancel button, so 
        // we don't want to raise a click event in response to that.
        if (ShowState != DialogShowState.Closing)
        {
            _outerDialog.RaiseButtonClickEvent(id);
        }

        // Once that returns, we raise a Closing event for the dialog
        // The Win32 API handles button clicking-and-closing 
        // as an atomic action,
        // but it is more .NET friendly to split them up.
        // Unfortunately, we do NOT have the return values at this stage.
        if (id < DialogsDefaults.MinimumDialogControlId)
        {
            return _outerDialog.RaiseClosingEvent(id);
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb760542(v=vs.85).aspx
        // The return value is specific to the notification being processed.
        // When responding to a button click, your implementation should return S_FALSE
        // if the Task Dialog is not to close. Otherwise return S_OK.
        return ShowState == DialogShowState.Closing ? (int)HResult.Ok : (int)HResult.False;
    }

    private int HandleRadioButtonClick(int id)
    {
        // When the dialog sets the radio button to default, 
        // it (somewhat confusingly)issues a radio button clicked event
        //  - we mask that out - though ONLY if
        // we do have a default radio button
        if (_firstRadioButtonClicked
            && !IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton))
        {
            _firstRadioButtonClicked = false;
        }
        else
        {
            _outerDialog.RaiseButtonClickEvent(id);
        }

        // Note: we don't raise Closing, as radio 
        // buttons are non-committing buttons
        return CoreErrorHelper.Ignored;
    }

    private int HandleHyperlinkClick(IntPtr href)
    {
        string link = Marshal.PtrToStringUni(href) ?? string.Empty;
        _outerDialog.RaiseHyperlinkClickEvent(link);

        return CoreErrorHelper.Ignored;
    }


    private int HandleTick(int ticks)
    {
        _outerDialog.RaiseTickEvent(ticks);
        return CoreErrorHelper.Ignored;
    }

    private int HandleHelpInvocation()
    {
        _outerDialog.RaiseHelpInvokedEvent();
        return CoreErrorHelper.Ignored;
    }

    // There should be little we need to do here, 
    // as the use of the NativeTaskDialog is
    // that it is instantiated for a single show, then disposed of.
    private int PerformDialogCleanup()
    {
        _firstRadioButtonClicked = true;

        return CoreErrorHelper.Ignored;
    }

    #endregion

    #region Update members

    internal void UpdateProgressBarValue(int i)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarPosition, i, 0);
    }

    internal void UpdateProgressBarRange(int? min = null, int? max = null)
    {
        AssertCurrentlyShowing();

        if (min.HasValue)
        {
            _settings.ProgressBarMinimum = min.Value;
        }

        if (max.HasValue)
        {
            _settings.ProgressBarMaximum = max.Value;
        }

        // Build range LPARAM - note it is in REVERSE intuitive order.
        long range = MakeLongLParam(
            _settings.ProgressBarMaximum,
            _settings.ProgressBarMinimum);

        SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarRange, 0, range);
    }

    internal void UpdateProgressBarState(TaskDialogProgressBarState state)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarState, (int)state, 0);
    }

    internal void UpdateText(string? text)
    {
        UpdateTextCore(text, TaskDialogNativeMethods.TaskDialogElements.Content);
    }

    internal void UpdateInstruction(string? instruction)
    {
        UpdateTextCore(instruction, TaskDialogNativeMethods.TaskDialogElements.MainInstruction);
    }

    internal void UpdateFooterText(string? footerText)
    {
        UpdateTextCore(footerText, TaskDialogNativeMethods.TaskDialogElements.Footer);
    }

    internal void UpdateExpandedText(string? expandedText)
    {
        UpdateTextCore(expandedText, TaskDialogNativeMethods.TaskDialogElements.ExpandedInformation);
    }

    private void UpdateTextCore(string? s, TaskDialogNativeMethods.TaskDialogElements element)
    {
        AssertCurrentlyShowing();

        FreeOldString(element);
        SendMessageHelper(
            TaskDialogNativeMethods.TaskDialogMessages.SetElementText,
            (int)element,
            (long)MakeNewString(s, element));
    }

    internal void UpdateMainIcon(TaskDialogStandardIcon mainIcon)
    {
        UpdateIconCore(mainIcon, TaskDialogNativeMethods.TaskDialogIconElement.Main);
    }

    internal void UpdateFooterIcon(TaskDialogStandardIcon footerIcon)
    {
        UpdateIconCore(footerIcon, TaskDialogNativeMethods.TaskDialogIconElement.Footer);
    }

    private void UpdateIconCore(TaskDialogStandardIcon icon, TaskDialogNativeMethods.TaskDialogIconElement element)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(
            TaskDialogNativeMethods.TaskDialogMessages.UpdateIcon,
            (int)element,
            (long)icon);
    }

    internal void UpdateCheckBoxChecked(bool cbc)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(
            TaskDialogNativeMethods.TaskDialogMessages.ClickVerification,
            (cbc ? 1 : 0),
            1);
    }

    internal void UpdateElevationIcon(int buttonId, bool showIcon)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(
            TaskDialogNativeMethods.TaskDialogMessages.SetButtonElevationRequiredState,
            buttonId,
            Convert.ToInt32(showIcon));
    }

    internal void UpdateButtonEnabled(int buttonId, bool enabled)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(
            TaskDialogNativeMethods.TaskDialogMessages.EnableButton, buttonId, enabled ? 1 : 0);
    }

    internal void UpdateRadioButtonEnabled(int buttonId, bool enabled)
    {
        AssertCurrentlyShowing();
        SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableRadioButton,
            buttonId, enabled ? 1 : 0);
    }

    internal void AssertCurrentlyShowing()
    {
        Debug.Assert(ShowState == DialogShowState.Showing,
            "Update*() methods should only be called while native dialog is showing");
    }

    #endregion

    #region Helpers

    private int SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages message, int wparam, long lparam)
    {
        // Be sure to at least assert here - 
        // messages to invalid handles often just disappear silently
        Debug.Assert(_hWndDialog != IntPtr.Zero, "HWND for dialog is null during SendMessage");

        return (int)CoreNativeMethods.SendMessage(
            _hWndDialog,
            (uint)message,
            (IntPtr)wparam,
            new IntPtr(lparam));
    }

    private bool IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions flag) => ((_nativeDialogConfig.taskDialogFlags & flag) == flag);

    // Allocates a new string on the unmanaged heap, 
    // and stores the pointer so we can free it later.

    private IntPtr MakeNewString(string? text, TaskDialogNativeMethods.TaskDialogElements element)
    {
        IntPtr newStringPtr = Marshal.StringToHGlobalUni(text);
        _updatedStrings![(int)element] = newStringPtr;
        return newStringPtr;
    }

    // Checks to see if the given element already has an 
    // updated string, and if so, 
    // frees it. This is done in preparation for a call to 
    // MakeNewString(), to prevent
    // leaks from multiple updates calls on the same element 
    // within a single native dialog lifetime.
    private void FreeOldString(TaskDialogNativeMethods.TaskDialogElements element)
    {
        int elementIndex = (int)element;
        if (_updatedStrings![elementIndex] != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_updatedStrings[elementIndex]);
            _updatedStrings[elementIndex] = IntPtr.Zero;
        }
    }

    // Based on the following defines in WinDef.h and WinUser.h:
    // #define MAKELPARAM(l, h) ((LPARAM)(DWORD)MAKELONG(l, h))
    // #define MAKELONG(a, b) ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
    private static long MakeLongLParam(int a, int b)
    {
        return (a << 16) + b;
    }

    // Builds the actual configuration that the 
    // NativeTaskDialog (and underlying Win32 API)
    // expects, by parsing the various control lists, 
    // marshaling to the unmanaged heap, etc.
    private void MarshalDialogControlStructs()
    {
        if (_settings.Buttons != null && _settings.Buttons.Length > 0)
        {
            _buttonArray = AllocateAndMarshalButtons(_settings.Buttons);
            _settings.NativeConfiguration.buttons = _buttonArray;
            _settings.NativeConfiguration.buttonCount = (uint)_settings.Buttons.Length;
        }

        if (_settings.RadioButtons != null && _settings.RadioButtons.Length > 0)
        {
            _radioButtonArray = AllocateAndMarshalButtons(_settings.RadioButtons);
            _settings.NativeConfiguration.radioButtons = _radioButtonArray;
            _settings.NativeConfiguration.radioButtonCount = (uint)_settings.RadioButtons.Length;
        }
    }

    private static IntPtr AllocateAndMarshalButtons(TaskDialogNativeMethods.TaskDialogButton[]? buttons)
    {
        int sizeOfButton = Marshal.SizeOf(typeof(TaskDialogNativeMethods.TaskDialogButton));
        IntPtr initialPtr = Marshal.AllocHGlobal(sizeOfButton * buttons!.Length);
        IntPtr currentPtr = initialPtr;

        foreach (TaskDialogNativeMethods.TaskDialogButton button in buttons)
        {
            Marshal.StructureToPtr(button, currentPtr, false);
            currentPtr = new IntPtr(currentPtr.ToInt64() + sizeOfButton);
        }

        return initialPtr;
    }

    #endregion

    #region IDispose Pattern

    private bool _disposed;

    // Finalizer and IDisposable implementation.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~NativeTaskDialog()
    {
        Dispose(false);
    }

    // Core disposing logic.
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;

            // Single biggest resource - make sure the dialog 
            // itself has been instructed to close.

            if (ShowState == DialogShowState.Showing)
            {
                NativeClose(TaskDialogResult.Cancel);
            }

            // Clean up custom allocated strings that were updated
            // while the dialog was showing. Note that the strings
            // passed in the initial TaskDialogIndirect call will
            // be cleaned up automagically by the default 
            // marshalling logic.

            if (_updatedStrings != null)
            {
                for (int i = 0; i < _updatedStrings.Length; i++)
                {
                    if (_updatedStrings[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(_updatedStrings[i]);
                        _updatedStrings[i] = IntPtr.Zero;
                    }
                }
            }

            // Clean up the button and radio button arrays, if any.
            if (_buttonArray != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_buttonArray);
                _buttonArray = IntPtr.Zero;
            }
            if (_radioButtonArray != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_radioButtonArray);
                _radioButtonArray = IntPtr.Zero;
            }

            if (disposing)
            {
                // Clean up managed resources - currently there are none
                // that are interesting.
            }
        }
    }

    #endregion
}