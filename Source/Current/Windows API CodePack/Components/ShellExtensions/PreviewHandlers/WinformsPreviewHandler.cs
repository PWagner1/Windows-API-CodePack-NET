using Color = System.Drawing.Color;
using TextBox = System.Windows.Forms.TextBox;
using UserControl = System.Windows.Forms.UserControl;
#pragma warning disable CS8602

namespace Microsoft.WindowsAPICodePack.ShellExtensions;

/// <summary>
/// This is the base class for all WinForms-based preview handlers and provides their basic functionality.
/// To create a custom preview handler that contains a WinForms user control,
/// a class must derive from this, use the <see cref="PreviewHandlerAttribute"/>,
/// and implement 1 or more of the following interfaces: 
/// <see cref="IPreviewFromStream"/>, 
/// <see cref="IPreviewFromShellObject"/>, 
/// <see cref="IPreviewFromFile"/>.   
/// </summary>
public abstract class WinFormsPreviewHandler : PreviewHandler, IDisposable
{
    /// <summary>
    /// This control must be populated by the deriving class before the preview is shown.
    /// </summary>
    public UserControl? Control { get; protected set; }

    /// <summary>
    /// Throws an exception if the control has not been initialized.
    /// </summary>
    protected void ThrowIfNoControl()
    {
        if (Control == null)
        {
            throw new InvalidOperationException(LocalizedMessages.PreviewHandlerControlNotInitialized);
        }
    }

    /// <summary>
    /// Called when an exception is thrown during itialization of the preview control.
    /// </summary>
    /// <param name="caughtException"></param>
    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
        Justification = "The object remains reachable through the Controls collection which can be disposed at a later time.")]
    protected override void HandleInitializeException(Exception caughtException)
    {
        if (caughtException == null) { throw new ArgumentNullException(nameof(caughtException)); }

        Control = new UserControl();
        Control.Controls.Add(new TextBox
        {
            ReadOnly = true,
            Multiline = true,
            Dock = DockStyle.Fill,
            Text = caughtException.ToString(),
            BackColor = Color.OrangeRed
        });
    }

    /// <summary>
    /// Updates the bounds of the preview control.
    /// </summary>
    /// <param name="bounds">The new bounds for the preview control.</param>
    protected override void UpdateBounds(NativeRect bounds)
    {
        if (Control != null)
        {
            Control.Bounds = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
            Control.Visible = true;
        }
    }

    /// <summary>
    /// Sets focus to the preview control.
    /// </summary>
    protected override void SetFocus()
    {
        Control?.Focus();
    }

    /// <summary>
    /// Sets the background color of the preview control.
    /// </summary>
    /// <param name="argb">The ARGB color value.</param>
    protected override void SetBackground(int argb)
    {
        Control.BackColor = Color.FromArgb(argb);
    }

    /// <summary>
    /// Sets the foreground color of the preview control.
    /// </summary>
    /// <param name="argb">The ARGB color value.</param>
    protected override void SetForeground(int argb)
    {
        Control.ForeColor = Color.FromArgb(argb);
    }

    /// <summary>
    /// Sets the font of the preview control.
    /// </summary>
    /// <param name="font">The log font to apply.</param>
    protected override void SetFont(LogFont font)
    {
        Control.Font = Font.FromLogFont(font);
    }

    /// <summary>
    /// Gets the handle of the preview control.
    /// </summary>
    protected override IntPtr Handle => Control.Handle;

    /// <summary>
    /// Sets the parent window handle for the preview control.
    /// </summary>
    /// <param name="handle">The parent window handle.</param>
    protected override void SetParentHandle(IntPtr handle)
    {
        HandlerNativeMethods.SetParent(Control.Handle, handle);
    }

    #region IDisposable Members

    /// <summary>
    /// Finalizes the preview handler instance.
    /// </summary>
    ~WinFormsPreviewHandler()
    {
        Dispose(false);
    }

    /// <summary>
    /// Disposes the preview handler and releases all resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the preview handler and releases resources.
    /// </summary>
    /// <param name="disposing">True if called from Dispose(), false if called from finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && Control != null)
        {
            Control.Dispose();
        }
    }

    #endregion
}