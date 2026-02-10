//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable PossibleInvalidCastExceptionInForeachLoop
namespace Microsoft.WindowsAPICodePack.Dialogs;

/// <summary>
/// Encapsulates a new-to-Vista Win32 TaskDialog window 
/// - a powerful successor to the MessageBox available
/// in previous versions of Windows.
/// </summary>
public sealed class TaskDialog : IDialogControlHost, IDisposable
{
    // Global instance of TaskDialog, to be used by static Show() method.
    // As most parameters of a dialog created via static Show() will have
    // identical parameters, we'll create one TaskDialog and treat it
    // as a NativeTaskDialog generator for all static Show() calls.
    private static TaskDialog? _staticDialog;

    // Main current native dialog.
    private NativeTaskDialog? _nativeDialog;

    private List<TaskDialogButtonBase?>? _buttons = new();
    private List<TaskDialogButtonBase?>? _radioButtons = new();
    private List<TaskDialogButtonBase?>? _commandLinks = new();
    private IntPtr _ownerWindow;

    #region Public Properties
    /// <summary>
    /// Occurs when a progress bar changes.
    /// </summary>
    public event EventHandler<TaskDialogTickEventArgs>? Tick;

    /// <summary>
    /// Occurs when a user clicks a hyperlink.
    /// </summary>
    public event EventHandler<TaskDialogHyperlinkClickedEventArgs>? HyperlinkClick;

    /// <summary>
    /// Occurs when the TaskDialog is closing.
    /// </summary>
    public event EventHandler<TaskDialogClosingEventArgs>? Closing;

    /// <summary>
    /// Occurs when a user clicks on Help.
    /// </summary>
    public event EventHandler? HelpInvoked;

    /// <summary>
    /// Occurs when the TaskDialog is opened.
    /// </summary>
    public event EventHandler? Opened;

    /// <summary>
    /// Gets or sets a value that contains the owner window's handle.
    /// </summary>
    public IntPtr OwnerWindowHandle
    {
        get => _ownerWindow;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.OwnerCannotBeChanged);
            _ownerWindow = value;
        }
    }

    // Main content (maps to MessageBox's "message"). 
    private string? _text;
    /// <summary>
    /// Gets or sets a value that contains the message text.
    /// </summary>
    public string? Text
    {
        get => _text;
        set
        {
            // Set local value, then update native dialog if showing.
            _text = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateText(_text);
            }
        }
    }

    private string? _instructionText;
    /// <summary>
    /// Gets or sets a value that contains the instruction text.
    /// </summary>
    public string? InstructionText
    {
        get => _instructionText;
        set
        {
            // Set local value, then update native dialog if showing.
            _instructionText = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateInstruction(_instructionText);
            }
        }
    }

    private string? _caption;
    /// <summary>
    /// Gets or sets a value that contains the caption text.
    /// </summary>
    public string? Caption
    {
        get => _caption;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.CaptionCannotBeChanged);
            _caption = value;
        }
    }

    private string? _footerText;
    /// <summary>
    /// Gets or sets a value that contains the footer text.
    /// </summary>
    public string? FooterText
    {
        get => _footerText;
        set
        {
            // Set local value, then update native dialog if showing.
            _footerText = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateFooterText(_footerText);
            }
        }
    }

    private string? _checkBoxText;
    /// <summary>
    /// Gets or sets a value that contains the footer check box text.
    /// </summary>
    public string? FooterCheckBoxText
    {
        get => _checkBoxText;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.CheckBoxCannotBeChanged);
            _checkBoxText = value;
        }
    }

    private string? _detailsExpandedText;
    /// <summary>
    /// Gets or sets a value that contains the expanded text in the details section.
    /// </summary>
    public string? DetailsExpandedText
    {
        get => _detailsExpandedText;
        set
        {
            // Set local value, then update native dialog if showing.
            _detailsExpandedText = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateExpandedText(_detailsExpandedText);
            }
        }
    }

    private bool _detailsExpanded;
    /// <summary>
    /// Gets or sets a value that determines if the details section is expanded.
    /// </summary>
    public bool DetailsExpanded
    {
        get => _detailsExpanded;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.ExpandingStateCannotBeChanged);
            _detailsExpanded = value;
        }
    }

    private string? _detailsExpandedLabel;
    /// <summary>
    /// Gets or sets a value that contains the expanded control text.
    /// </summary>
    public string? DetailsExpandedLabel
    {
        get => _detailsExpandedLabel;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.ExpandedLabelCannotBeChanged);
            _detailsExpandedLabel = value;
        }
    }

    private string? _detailsCollapsedLabel;
    /// <summary>
    /// Gets or sets a value that contains the collapsed control text.
    /// </summary>
    public string? DetailsCollapsedLabel
    {
        get => _detailsCollapsedLabel;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.CollapsedTextCannotBeChanged);
            _detailsCollapsedLabel = value;
        }
    }

    private bool _cancelable;
    /// <summary>
    /// Gets or sets a value that determines if Cancelable is set.
    /// </summary>
    public bool Cancelable
    {
        get => _cancelable;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.CancelableCannotBeChanged);
            _cancelable = value;
        }
    }

    private TaskDialogStandardIcon _icon;
    /// <summary>
    /// Gets or sets a value that contains the TaskDialog main icon.
    /// </summary>
    public TaskDialogStandardIcon Icon
    {
        get => _icon;
        set
        {
            // Set local value, then update native dialog if showing.
            _icon = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateMainIcon(_icon);
            }
        }
    }

    private TaskDialogStandardIcon _footerIcon;
    /// <summary>
    /// Gets or sets a value that contains the footer icon.
    /// </summary>
    public TaskDialogStandardIcon FooterIcon
    {
        get => _footerIcon;
        set
        {
            // Set local value, then update native dialog if showing.
            _footerIcon = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateFooterIcon(_footerIcon);
            }
        }
    }

    private TaskDialogStandardButtons _standardButtons = TaskDialogStandardButtons.None;
    /// <summary>
    /// Gets or sets a value that contains the standard buttons.
    /// </summary>
    public TaskDialogStandardButtons StandardButtons
    {
        get => _standardButtons;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.StandardButtonsCannotBeChanged);
            _standardButtons = value;
        }
    }

    private TaskDialogDefaultButton _defaultButton = TaskDialogDefaultButton.None;
    /// <summary>
    /// Gets or sets a value that contains the default button.
    /// </summary>
    public TaskDialogDefaultButton DefaultButton
    {
        get => _defaultButton;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.DefaultButtonCannotBeChanged);
            _defaultButton = value;
        }
    }


    private DialogControlCollection<TaskDialogControl> _controls;
    /// <summary>
    /// Gets a value that contains the TaskDialog controls.
    /// </summary>
    public DialogControlCollection<TaskDialogControl> Controls =>
        // "Show protection" provided by collection itself, 
        // as well as individual controls.
        _controls;

    private bool _hyperlinksEnabled;
    /// <summary>
    /// Gets or sets a value that determines if hyperlinks are enabled.
    /// </summary>
    public bool HyperlinksEnabled
    {
        get => _hyperlinksEnabled;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.HyperlinksCannotBetSet);
            _hyperlinksEnabled = value;
        }
    }

    private bool? _footerCheckBoxChecked;
    /// <summary>
    /// Gets or sets a value that indicates if the footer checkbox is checked.
    /// </summary>
    public bool? FooterCheckBoxChecked
    {
        get => _footerCheckBoxChecked.GetValueOrDefault(false);
        set
        {
            // Set local value, then update native dialog if showing.
            _footerCheckBoxChecked = value;
            if (NativeDialogShowing)
            {
                _nativeDialog?.UpdateCheckBoxChecked(_footerCheckBoxChecked != null &&
                                                     _footerCheckBoxChecked.Value);
            }
        }
    }

    private TaskDialogExpandedDetailsLocation _expansionMode;
    /// <summary>
    /// Gets or sets a value that contains the expansion mode for this dialog.
    /// </summary>
    public TaskDialogExpandedDetailsLocation ExpansionMode
    {
        get => _expansionMode;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.ExpandedDetailsCannotBeChanged);
            _expansionMode = value;
        }
    }

    private TaskDialogStartupLocation _startupLocation;
    /// <summary>
    /// Gets or sets a value that contains the startup location.
    /// </summary>
    public TaskDialogStartupLocation StartupLocation
    {
        get => _startupLocation;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.StartupLocationCannotBeChanged);
            _startupLocation = value;
        }
    }

    private TaskDialogProgressBar? _progressBar;
    /// <summary>
    /// Gets or sets the progress bar on the taskdialog. ProgressBar a visual representation 
    /// of the progress of a long running operation.
    /// </summary>
    public TaskDialogProgressBar? ProgressBar
    {
        get => _progressBar;
        set
        {
            ThrowIfDialogShowing(LocalizedMessages.ProgressBarCannotBeChanged);
            if (value != null)
            {
                if (value.HostingDialog != null)
                {
                    throw new InvalidOperationException(LocalizedMessages.ProgressBarCannotBeHostedInMultipleDialogs);
                }

                value.HostingDialog = this;
            }
            _progressBar = value;
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a basic TaskDialog window 
    /// </summary>
    public TaskDialog()
    {
        CoreHelpers.ThrowIfNotVista();

        // Initialize various data structs.
        _controls = new DialogControlCollection<TaskDialogControl>(this);
    }

    #endregion

    #region Static Show Methods

    /// <summary>
    /// Creates and shows a task dialog with the specified message text.
    /// </summary>
    /// <param name="text">The text to display.</param>
    /// <returns>The dialog result.</returns>
    public static TaskDialogResult Show(string? text)
    {
        return ShowCoreStatic(
            text,
            TaskDialogDefaults.MainInstruction,
            TaskDialogDefaults.Caption, TaskDialogStandardIcon.None, IntPtr.Zero);
    }

    /// <summary>
    /// Creates and shows a task dialog with the specified supporting text and main instruction.
    /// </summary>
    /// <param name="text">The supporting text to display.</param>
    /// <param name="instructionText">The main instruction text to display.</param>
    /// <returns>The dialog result.</returns>
    public static TaskDialogResult Show(string? text, string? instructionText)
    {
        return ShowCoreStatic(
            text,
            instructionText,
            TaskDialogDefaults.Caption, TaskDialogStandardIcon.None, IntPtr.Zero);
    }

    /// <summary>
    /// Creates and shows a task dialog with the specified supporting text, main instruction, and dialog caption.
    /// </summary>
    /// <param name="text">The supporting text to display.</param>
    /// <param name="instructionText">The main instruction text to display.</param>
    /// <param name="caption">The caption for the dialog.</param>
    /// <returns>The dialog result.</returns>
    public static TaskDialogResult Show(string? text, string? instructionText, string? caption)
    {
        return ShowCoreStatic(text, instructionText, caption, TaskDialogStandardIcon.None, IntPtr.Zero);
    }

    /// <summary>
    /// Creates and shows a task dialog with the specified supporting text, main instruction, and dialog caption.
    /// </summary>
    /// <param name="text">The supporting text to display.</param>
    /// <param name="instructionText">The main instruction text to display.</param>
    /// <param name="caption">The caption for the dialog.</param>
    /// <param name="icon">The icon for the dialog.</param>
    /// <returns>The dialog result.</returns>
    public static TaskDialogResult Show(string? text, string? instructionText, string? caption, TaskDialogStandardIcon icon)
    {
        return ShowCoreStatic(text, instructionText, caption, icon, IntPtr.Zero);
    }

    /// <summary>
    /// Creates and shows a task dialog with the specified supporting text, main instruction, and dialog caption.
    /// </summary>
    /// <param name="text">The supporting text to display.</param>
    /// <param name="instructionText">The main instruction text to display.</param>
    /// <param name="caption">The caption for the dialog.</param>
    /// <param name="icon">The icon for the dialog.</param>
    /// <param name="parent">The parent for the dialog.</param>
    /// <returns>The dialog result.</returns>
    public static TaskDialogResult Show(string? text, string? instructionText, string? caption, TaskDialogStandardIcon icon, IWin32Window parent)
    {
        return ShowCoreStatic(text, instructionText, caption, icon, parent.Handle);
    }
    #endregion

    #region Instance Show Methods

    /// <summary>
    /// Creates and shows a task dialog.
    /// </summary>
    /// <returns>The dialog result.</returns>
    public TaskDialogResult Show()
    {
        return ShowCore();
    }
    #endregion

    #region Core Show Logic

    // CORE SHOW METHODS:
    // All static Show() calls forward here - 
    // it is responsible for retrieving
    // or creating our cached TaskDialog instance, getting it configured,
    // and in turn calling the appropriate instance Show.

    private static TaskDialogResult ShowCoreStatic(
        string? text,
        string? instructionText,
        string? caption, TaskDialogStandardIcon icon, IntPtr hwnd)
    {
        CoreHelpers.ThrowIfNotVista();

        // If no instance cached yet, create it.
        if (_staticDialog == null)
        {
            // New TaskDialog will automatically pick up defaults when 
            // a new config structure is created as part of ShowCore().
            _staticDialog = new TaskDialog();
        }

        // Set the few relevant properties, 
        // and go with the defaults for the others.
        _staticDialog._text = text;
        _staticDialog._instructionText = instructionText;
        _staticDialog._caption = caption;
        _staticDialog.Icon = icon;
        _staticDialog.OwnerWindowHandle = hwnd;

        return _staticDialog.Show();
    }

    private TaskDialogResult ShowCore()
    {
        TaskDialogResult result;

        try
        {
            // Populate control lists, based on current 
            // contents - note we are somewhat late-bound 
            // on our control lists, to support XAML scenarios.
            SortDialogControls();

            // First, let's make sure it even makes 
            // sense to try a show.
            ValidateCurrentDialogSettings();

            // Create settings object for new dialog, 
            // based on current state.
            NativeTaskDialogSettings settings = new();
            ApplyCoreSettings(settings);
            ApplySupplementalSettings(settings);

            // Show the dialog.
            // NOTE: this is a BLOCKING call; the dialog proc callbacks
            // will be executed by the same thread as the 
            // Show() call before the thread of execution 
            // contines to the end of this method.
            _nativeDialog = new NativeTaskDialog(settings, this);
            _nativeDialog.NativeShow();

            // Build and return dialog result to public API - leaving it
            // null after an exception is thrown is fine in this case
            result = ConstructDialogResult(_nativeDialog);
            _footerCheckBoxChecked = _nativeDialog.CheckBoxChecked;
        }
        finally
        {
            CleanUp();
            _nativeDialog = null;
        }

        return result;
    }

    // Helper that looks at the current state of the TaskDialog and verifies
    // that there aren't any abberant combinations of properties.
    // NOTE that this method is designed to throw 
    // rather than return a bool.
    private void ValidateCurrentDialogSettings()
    {
        if (_footerCheckBoxChecked.HasValue &&
            _footerCheckBoxChecked.Value &&
            string.IsNullOrEmpty(_checkBoxText))
        {
            throw new InvalidOperationException(LocalizedMessages.TaskDialogCheckBoxTextRequiredToEnableCheckBox);
        }

        // Progress bar validation.
        // Make sure the progress bar values are valid.
        // the Win32 API will valiantly try to rationalize 
        // bizarre min/max/value combinations, but we'll save
        // it the trouble by validating.
        if (_progressBar is { HasValidValues: false })
        {
            throw new InvalidOperationException(LocalizedMessages.TaskDialogProgressBarValueInRange);
        }

        // Validate Buttons collection.
        // Make sure we don't have buttons AND 
        // command-links - the Win32 API treats them as different
        // flavors of a single button struct.
        if (_buttons!.Count > 0 && _commandLinks!.Count > 0)
        {
            throw new NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndLinks);
        }
        if (_buttons.Count > 0 && _standardButtons != TaskDialogStandardButtons.None)
        {
            throw new NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndButtons);
        }
    }

    // Analyzes the final state of the NativeTaskDialog instance and creates the 
    // final TaskDialogResult that will be returned from the public API
    private static TaskDialogResult ConstructDialogResult(NativeTaskDialog? native)
    {
        Debug.Assert(native is { ShowState: DialogShowState.Closed }, "dialog result being constructed for unshown dialog.");

        TaskDialogResult result = TaskDialogResult.Cancel;

        // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (native != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        {
            TaskDialogStandardButtons standardButton = MapButtonIdToStandardButton(native.SelectedButtonId);

            // If returned ID isn't a standard button, let's fetch 
            if (standardButton == TaskDialogStandardButtons.None)
            {
                result = TaskDialogResult.CustomButtonClicked;
            }
            else { result = (TaskDialogResult)standardButton; }
        }

        return result;
    }

    /// <summary>
    /// Close TaskDialog
    /// </summary>
    /// <exception cref="InvalidOperationException">if TaskDialog is not showing.</exception>
    public void Close()
    {
        if (!NativeDialogShowing)
        {
            throw new InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing);
        }

        _nativeDialog?.NativeClose(TaskDialogResult.Cancel);
        // TaskDialog's own cleanup code - 
        // which runs post show - will handle disposal of native dialog.
    }

    /// <summary>
    /// Close TaskDialog with a given TaskDialogResult
    /// </summary>
    /// <param name="closingResult">TaskDialogResult to return from the TaskDialog.Show() method</param>
    /// <exception cref="InvalidOperationException">if TaskDialog is not showing.</exception>
    public void Close(TaskDialogResult closingResult)
    {
        if (!NativeDialogShowing)
        {
            throw new InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing);
        }

        _nativeDialog?.NativeClose(closingResult);
        // TaskDialog's own cleanup code - 
        // which runs post show - will handle disposal of native dialog.
    }

    #endregion

    #region Configuration Construction

    private void ApplyCoreSettings(NativeTaskDialogSettings settings)
    {
        ApplyGeneralNativeConfiguration(settings.NativeConfiguration);
        ApplyTextConfiguration(settings.NativeConfiguration);
        ApplyOptionConfiguration(settings.NativeConfiguration);
        ApplyControlConfiguration(settings);
    }

    private void ApplyGeneralNativeConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
    {
        // If an owner wasn't specifically specified, 
        // we'll use the app's main window.
        if (_ownerWindow != IntPtr.Zero)
        {
            dialogConfig.parentHandle = _ownerWindow;
        }

        // Other miscellaneous sets.
        dialogConfig.mainIcon = new TaskDialogNativeMethods.IconUnion((IntPtr)_icon);
        dialogConfig.footerIcon = new TaskDialogNativeMethods.IconUnion((IntPtr)_footerIcon);
        dialogConfig.commonButtons = (TaskDialogNativeMethods.TaskDialogCommonButtons)_standardButtons;
    }

    /// <summary>
    /// Sets important text properties.
    /// </summary>
    /// <param name="dialogConfig">An instance of a <see cref="TaskDialogNativeMethods.TaskDialogConfiguration"/> object.</param>
    private void ApplyTextConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
    {
        // note that nulls or empty strings are fine here.
        dialogConfig.content = _text;
        dialogConfig.windowTitle = _caption;
        dialogConfig.mainInstruction = _instructionText;
        dialogConfig.expandedInformation = _detailsExpandedText;
        dialogConfig.expandedControlText = _detailsExpandedLabel;
        dialogConfig.collapsedControlText = _detailsCollapsedLabel;
        dialogConfig.footerText = _footerText;
        dialogConfig.verificationText = _checkBoxText;
    }

    private void ApplyOptionConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
    {
        // Handle options - start with no options set.
        TaskDialogNativeMethods.TaskDialogOptions options = TaskDialogNativeMethods.TaskDialogOptions.None;
        if (_cancelable)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.AllowCancel;
        }
        if (_footerCheckBoxChecked.HasValue && _footerCheckBoxChecked.Value)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.CheckVerificationFlag;
        }
        if (_hyperlinksEnabled)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.EnableHyperlinks;
        }
        if (_detailsExpanded)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.ExpandedByDefault;
        }
        if (Tick != null)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.UseCallbackTimer;
        }
        if (_startupLocation == TaskDialogStartupLocation.CenterOwner)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.PositionRelativeToWindow;
        }

        // Note: no validation required, as we allow this to 
        // be set even if there is no expanded information 
        // text because that could be added later.
        // Default for Win32 API is to expand into (and after) 
        // the content area.
        if (_expansionMode == TaskDialogExpandedDetailsLocation.ExpandFooter)
        {
            options |= TaskDialogNativeMethods.TaskDialogOptions.ExpandFooterArea;
        }

        // Finally, apply options to config.
        dialogConfig.taskDialogFlags = options;
    }

    // Builds the actual configuration 
    // that the NativeTaskDialog (and underlying Win32 API)
    // expects, by parsing the various control 
    // lists, marshalling to the unmanaged heap, etc.

    private void ApplyControlConfiguration(NativeTaskDialogSettings settings)
    {
        // Deal with progress bars/marquees.
        if (_progressBar != null)
        {
            if (_progressBar.State == TaskDialogProgressBarState.Marquee)
            {
                settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar;
            }
            else
            {
                settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar;
            }
        }

        // Build the native struct arrays that NativeTaskDialog 
        // needs - though NTD will handle
        // the heavy lifting marshalling to make sure 
        // all the cleanup is centralized there.
        if (_buttons!.Count > 0 || _commandLinks!.Count > 0)
        {
            // These are the actual arrays/lists of 
            // the structs that we'll copy to the 
            // unmanaged heap.
            List<TaskDialogButtonBase?>? sourceList = (_buttons.Count > 0 ? _buttons : _commandLinks);
            settings.Buttons = BuildButtonStructArray(sourceList);

            // Apply option flag that forces all 
            // custom buttons to render as command links.
            if (_commandLinks!.Count > 0)
            {
                settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.UseCommandLinks;
            }

            // Set default button and add elevation icons 
            // to appropriate buttons.
            settings.NativeConfiguration.defaultButtonIndex = FindDefaultButtonId(sourceList);

            ApplyElevatedIcons(settings, sourceList);
        }

        if (_radioButtons!.Count > 0)
        {
            settings.RadioButtons = BuildButtonStructArray(_radioButtons);

            // Set default radio button - radio buttons don't support.
            int defaultRadioButton = FindDefaultButtonId(_radioButtons);
            settings.NativeConfiguration.defaultRadioButtonIndex = defaultRadioButton;

            if (defaultRadioButton == TaskDialogNativeMethods.NoDefaultButtonSpecified)
            {
                settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton;
            }
        }
    }

    private static TaskDialogNativeMethods.TaskDialogButton[] BuildButtonStructArray(List<TaskDialogButtonBase?>? controls)
    {
        TaskDialogNativeMethods.TaskDialogButton[]? buttonStructs;
        TaskDialogButtonBase? button;

        int totalButtons = controls!.Count;
        buttonStructs = new TaskDialogNativeMethods.TaskDialogButton[totalButtons];
        for (int i = 0; i < totalButtons; i++)
        {
            button = controls[i];
            buttonStructs[i] = new TaskDialogNativeMethods.TaskDialogButton(button!.Id, button.ToString());
        }
        return buttonStructs;
    }

    // Searches list of controls and returns the ID of 
    // the default control, or 0 if no default was specified.
    private static int FindDefaultButtonId(List<TaskDialogButtonBase?>? controls)
    {
        var defaults = controls?.FindAll(control => control!.Default);

        return defaults!.Count switch
        {
            1 => defaults[0]!.Id,
            > 1 => throw new InvalidOperationException(LocalizedMessages.TaskDialogOnlyOneDefaultControl),
            _ => TaskDialogNativeMethods.NoDefaultButtonSpecified
        };
    }

    private static void ApplyElevatedIcons(NativeTaskDialogSettings settings, List<TaskDialogButtonBase?>? controls)
    {
        foreach (TaskDialogButton? control in controls!)
        {
            if (control!.UseElevationIcon)
            {
                if (settings.ElevatedButtons == null) { settings.ElevatedButtons = new List<int>(); }
                settings.ElevatedButtons.Add(control.Id);
            }
        }
    }

    private void ApplySupplementalSettings(NativeTaskDialogSettings settings)
    {
        if (_progressBar != null)
        {
            if (_progressBar.State != TaskDialogProgressBarState.Marquee)
            {
                settings.ProgressBarMinimum = _progressBar.Minimum;
                settings.ProgressBarMaximum = _progressBar.Maximum;
                settings.ProgressBarValue = _progressBar.Value;
                settings.ProgressBarState = _progressBar.State;
            }
        }

        if (HelpInvoked != null) { settings.InvokeHelp = true; }
    }

    // Here we walk our controls collection and 
    // sort the various controls by type.         
    private void SortDialogControls()
    {
        foreach (TaskDialogControl control in _controls)
        {
            TaskDialogButtonBase? buttonBase = control as TaskDialogButtonBase;
            TaskDialogCommandLink? commandLink = control as TaskDialogCommandLink;

            if (buttonBase != null && string.IsNullOrEmpty(buttonBase.Text) &&
                commandLink != null && string.IsNullOrEmpty(commandLink.Instruction))
            {
                throw new InvalidOperationException(LocalizedMessages.TaskDialogButtonTextEmpty);
            }

            TaskDialogRadioButton? radButton;
            TaskDialogProgressBar? progBar;

            // Loop through child controls 
            // and sort the controls based on type.
            if (commandLink != null)
            {
                _commandLinks?.Add(commandLink);
            }
            else if ((radButton = control as TaskDialogRadioButton) != null)
            {
                if (_radioButtons == null) { _radioButtons = new List<TaskDialogButtonBase?>(); }
                _radioButtons.Add(radButton);
            }
            else if (buttonBase != null)
            {
                if (_buttons == null) { _buttons = new List<TaskDialogButtonBase?>(); }
                _buttons.Add(buttonBase);
            }
            else if ((progBar = control as TaskDialogProgressBar) != null)
            {
                _progressBar = progBar;
            }
            else
            {
                throw new InvalidOperationException(LocalizedMessages.TaskDialogUnkownControl);
            }
        }
    }

    #endregion

    #region Helpers

    // Helper to map the standard button IDs returned by 
    // TaskDialogIndirect to the standard button ID enum - 
    // note that we can't just cast, as the Win32
    // typedefs differ incoming and outgoing.

    private static TaskDialogStandardButtons MapButtonIdToStandardButton(int id)
    {
        switch ((TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds)id)
        {
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ok:
                return TaskDialogStandardButtons.Ok;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Cancel:
                return TaskDialogStandardButtons.Cancel;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Abort:
                // Included for completeness in API - 
                // we can't pass in an Abort standard button.
                return TaskDialogStandardButtons.None;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Retry:
                return TaskDialogStandardButtons.Retry;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ignore:
                // Included for completeness in API - 
                // we can't pass in an Ignore standard button.
                return TaskDialogStandardButtons.None;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Yes:
                return TaskDialogStandardButtons.Yes;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.No:
                return TaskDialogStandardButtons.No;
            case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close:
                return TaskDialogStandardButtons.Close;
            default:
                return TaskDialogStandardButtons.None;
        }
    }

    private void ThrowIfDialogShowing(string message)
    {
        if (NativeDialogShowing) { throw new NotSupportedException(message); }
    }

    private bool NativeDialogShowing =>
        (_nativeDialog != null)
        && (_nativeDialog.ShowState == DialogShowState.Showing
            || _nativeDialog.ShowState == DialogShowState.Closing);

    // NOTE: we are going to require names be unique 
    // across both buttons and radio buttons,
    // even though the Win32 API allows them to be separate.
    private TaskDialogButtonBase? GetButtonForId(int id) => _controls.GetControlbyId(id) as TaskDialogButtonBase;

    #endregion

    #region IDialogControlHost Members

    // We're explicitly implementing this interface 
    // as the user will never need to know about it
    // or use it directly - it is only for the internal 
    // implementation of "pseudo controls" within 
    // the dialogs.

    // Called whenever controls are being added 
    // to or removed from the dialog control collection.
    bool IDialogControlHost.IsCollectionChangeAllowed()
    {
        // Only allow additions to collection if dialog is NOT showing.
        return !NativeDialogShowing;
    }

    // Called whenever controls have been added or removed.
    void IDialogControlHost.ApplyCollectionChanged()
    {
        // If we're showing, we should never get here - 
        // the changing notification would have thrown and the 
        // property would not have been changed.
        Debug.Assert(!NativeDialogShowing,
            "Collection changed notification received despite show state of dialog");
    }

    // Called when a control currently in the collection 
    // has a property changing - this is 
    // basically to screen out property changes that 
    // cannot occur while the dialog is showing
    // because the Win32 API has no way for us to 
    // propagate the changes until we re-invoke the Win32 call.
    bool IDialogControlHost.IsControlPropertyChangeAllowed(string propertyName, DialogControl control)
    {
        Debug.Assert(control is TaskDialogControl,
            "Property changing for a control that is not a TaskDialogControl-derived type");
        Debug.Assert(propertyName != "Name",
            "Name changes at any time are not supported - public API should have blocked this");

        bool canChange = false;

        if (!NativeDialogShowing)
        {
            // Certain properties can't be changed if the dialog is not showing
            // we need a handle created before we can set these...
            switch (propertyName)
            {
                case "Enabled":
                    canChange = false;
                    break;
                default:
                    canChange = true;
                    break;
            }
        }
        else
        {
            // If the dialog is showing, we can only 
            // allow some properties to change.
            switch (propertyName)
            {
                // Properties that CAN'T be changed while dialog is showing.
                case "Text":
                case "Default":
                    canChange = false;
                    break;

                // Properties that CAN be changed while dialog is showing.
                case "ShowElevationIcon":
                case "Enabled":
                    canChange = true;
                    break;
                default:
                    Debug.Assert(true, "Unknown property name coming through property changing handler");
                    break;
            }
        }
        return canChange;
    }

    // Called when a control currently in the collection 
    // has a property changed - this handles propagating
    // the new property values to the Win32 API. 
    // If there isn't a way to change the Win32 value, then we
    // should have already screened out the property set 
    // in NotifyControlPropertyChanging.        
    void IDialogControlHost.ApplyControlPropertyChange(string propertyName, DialogControl control)
    {
        // We only need to apply changes to the 
        // native dialog when it actually exists.
        if (NativeDialogShowing)
        {
            TaskDialogButton? button;
            TaskDialogRadioButton? radioButton;
            if (control is TaskDialogProgressBar)
            {
                if (_progressBar is { HasValidValues: false })
                {
                    throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange);
                }

                switch (propertyName)
                {
                    case "State":
                        if (_progressBar != null)
                        {
                            _nativeDialog?.UpdateProgressBarState(_progressBar.State);
                        }
                        break;
                    case "Value":
                        if (_progressBar != null)
                        {
                            _nativeDialog?.UpdateProgressBarValue(_progressBar.Value);
                        }
                        break;
                    case "Minimum":
                    case "Maximum":
                        if (_progressBar != null)
                        {
                            _nativeDialog?.UpdateProgressBarRange(_progressBar.Minimum, _progressBar.Maximum);
                        }
                        break;
                    default:
                        Debug.Assert(true, "Unknown property being set");
                        break;
                }
            }
            else if ((button = control as TaskDialogButton) != null)
            {
                switch (propertyName)
                {
                    case "ShowElevationIcon":
                        _nativeDialog?.UpdateElevationIcon(button.Id, button.UseElevationIcon);
                        break;
                    case "Enabled":
                        _nativeDialog?.UpdateButtonEnabled(button.Id, button.Enabled);
                        break;
                    default:
                        Debug.Assert(true, "Unknown property being set");
                        break;
                }
            }
            else if ((radioButton = control as TaskDialogRadioButton) != null)
            {
                switch (propertyName)
                {
                    case "Enabled":
                        _nativeDialog?.UpdateRadioButtonEnabled(radioButton.Id, radioButton.Enabled);
                        break;
                    default:
                        Debug.Assert(true, "Unknown property being set");
                        break;
                }
            }
            else
            {
                // Do nothing with property change - 
                // note that this shouldn't ever happen, we should have
                // either thrown on the changing event, or we handle above.
                Debug.Assert(true, "Control property changed notification not handled properly - being ignored");
            }
        }
    }

    #endregion

    #region Event Percolation Methods

    // All Raise*() methods are called by the 
    // NativeTaskDialog when various pseudo-controls
    // are triggered.
    internal void RaiseButtonClickEvent(int id)
    {
        // First check to see if the ID matches a custom button.
        TaskDialogButtonBase? button = GetButtonForId(id);

        // If a custom button was found, 
        // raise the event - if not, it's a standard button, and
        // we don't support custom event handling for the standard buttons
        button?.RaiseClickEvent();
    }

    internal void RaiseHyperlinkClickEvent(string link)
    {
        EventHandler<TaskDialogHyperlinkClickedEventArgs>? handler = HyperlinkClick;
        if (handler != null)
        {
            handler(this, new TaskDialogHyperlinkClickedEventArgs(link));
        }
    }

    // Gives event subscriber a chance to prevent 
    // the dialog from closing, based on 
    // the current state of the app and the button 
    // used to commit. Note that we don't 
    // have full access at this stage to 
    // the full dialog state.
    internal int RaiseClosingEvent(int id)
    {
        EventHandler<TaskDialogClosingEventArgs>? handler = Closing;
        if (handler != null)
        {
            TaskDialogButtonBase? customButton;
            TaskDialogClosingEventArgs e = new TaskDialogClosingEventArgs();

            // Try to identify the button - is it a standard one?
            TaskDialogStandardButtons buttonClicked = MapButtonIdToStandardButton(id);

            // If not, it had better be a custom button...
            if (buttonClicked == TaskDialogStandardButtons.None)
            {
                customButton = GetButtonForId(id);

                // ... or we have a problem.
                if (customButton == null)
                {
                    throw new InvalidOperationException(LocalizedMessages.TaskDialogBadButtonId);
                }

                e.CustomButton = customButton.Name;
                e.TaskDialogResult = TaskDialogResult.CustomButtonClicked;
            }
            else
            {
                e.TaskDialogResult = (TaskDialogResult)buttonClicked;
            }

            // Raise the event and determine how to proceed.
            handler(this, e);
            if (e.Cancel) { return (int)HResult.False; }
        }

        // It's okay to let the dialog close.
        return (int)HResult.Ok;
    }

    internal void RaiseHelpInvokedEvent()
    {
        if (HelpInvoked != null) { HelpInvoked(this, EventArgs.Empty); }
    }

    internal void RaiseOpenedEvent()
    {
        if (Opened != null) { Opened(this, EventArgs.Empty); }
    }

    internal void RaiseTickEvent(int ticks)
    {
        if (Tick != null) { Tick(this, new TaskDialogTickEventArgs(ticks)); }
    }

    #endregion

    #region Cleanup Code

    // Cleans up data and structs from a single 
    // native dialog Show() invocation.
    private void CleanUp()
    {
        // Reset values that would be considered 
        // 'volatile' in a given instance.
        _progressBar?.Reset();

        // Clean out sorted control lists - 
        // though we don't of course clear the main controls collection,
        // so the controls are still around; we'll 
        // resort on next show, since the collection may have changed.
        _buttons?.Clear();
        _commandLinks?.Clear();
        _radioButtons?.Clear();
        _progressBar = null;

        // Have the native dialog clean up the rest.
        _nativeDialog?.Dispose();
    }


    // Dispose pattern - cleans up data and structs for 
    // a) any native dialog currently showing, and
    // b) anything else that the outer TaskDialog has.
    private bool _disposed;

    /// <summary>
    /// Dispose TaskDialog Resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// TaskDialog Finalizer
    /// </summary>
    ~TaskDialog()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose TaskDialog Resources
    /// </summary>
    /// <param name="disposing">If true, indicates that this is being called via Dispose rather than via the finalizer.</param>
    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;

            if (disposing)
            {
                // Clean up managed resources.
                if (_nativeDialog is { ShowState: DialogShowState.Showing })
                {
                    _nativeDialog.NativeClose(TaskDialogResult.Cancel);
                }

                _buttons = null;
                _radioButtons = null;
                _commandLinks = null;
            }

            // Clean up unmanaged resources SECOND, NTD counts on 
            // being closed before being disposed.
            _nativeDialog?.Dispose();
            _nativeDialog = null;
            _staticDialog?.Dispose();
            _staticDialog = null;


        }
    }

    #endregion

    /// <summary>
    /// Indicates whether this feature is supported on the current platform.
    /// </summary>
    public static bool IsPlatformSupported =>
        // We need Windows Vista onwards ...
        CoreHelpers.RunningOnVista;
}