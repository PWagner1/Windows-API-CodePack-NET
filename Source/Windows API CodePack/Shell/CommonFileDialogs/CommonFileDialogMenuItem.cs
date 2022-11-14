namespace Microsoft.WindowsAPICodePack.Dialogs.Controls;

/// <summary>
/// Creates the CommonFileDialogMenuItem items for the Common File Dialog.
/// </summary>
public class CommonFileDialogMenuItem : CommonFileDialogControl
{
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    public CommonFileDialogMenuItem() : base(string.Empty) { }

    /// <summary>
    /// Creates a new instance of this class with the specified text.
    /// </summary>
    /// <param name="text">The text to display for this control.</param>
    public CommonFileDialogMenuItem(string? text) : base(text) { }

    /// <summary>
    /// Occurs when a user clicks a menu item.
    /// </summary>
    public event EventHandler Click = delegate { };
    internal void RaiseClickEvent()
    {
        // Make sure that this control is enabled and has a specified delegate
        if (Enabled) { Click(this, EventArgs.Empty); }
    }

    /// <summary>
    /// Attach this control to the dialog object
    /// </summary>
    /// <param name="dialog">Target dialog</param>
    internal override void Attach(IFileDialogCustomize? dialog)
    {
        // Items are added via the menu itself
    }
}