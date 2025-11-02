namespace Microsoft.WindowsAPICodePack.Dialogs;

/// <summary>
/// Specifies the default button for a task dialog.
/// </summary>
/// <remarks>This enumeration is used to indicate which button in a task dialog is pre-selected  when the dialog
/// is displayed. The default button is highlighted and can be activated  by pressing the Enter key. If no default
/// button is specified, the dialog will not  have a pre-selected button.</remarks>
public enum TaskDialogDefaultButton
{
    /// <summary>No default button.</summary>
    None = 0,

    /// <summary>An "OK" button.</summary>
    Ok = 1,

    /// <summary>A "Yes" button.</summary>
    Yes = 6,

    /// <summary>A "No" button.</summary>
    No = 7,

    /// <summary>A "Cancel" button.</summary>
    Cancel = 2,

    /// <summary>A "Retry" button.</summary>
    Retry = 4,

    /// <summary>A "Close" button.</summary>
    Close = 8
}