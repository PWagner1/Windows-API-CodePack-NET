using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable PossibleNullReferenceException
#pragma warning disable CS8602

namespace Microsoft.WindowsAPICodePack.ShellExtensions;

/// <summary>
/// This is the base class for all WPF-based preview handlers and provides their basic functionality.
/// To create a custom preview handler that contains a WPF user control,
/// a class must derive from this, use the <see cref="PreviewHandlerAttribute"/>,
/// and implement 1 or more of the following interfaces: 
/// <see cref="IPreviewFromStream"/>, 
/// <see cref="IPreviewFromShellObject"/>, 
/// <see cref="IPreviewFromFile"/>.   
/// </summary>
public abstract class WpfPreviewHandler : PreviewHandler, IDisposable
{
    HwndSource? _source = null;
    private IntPtr _parentHandle = IntPtr.Zero;
    private NativeRect _bounds;

    /// <summary>
    /// This control must be populated by the deriving class before the preview is shown.
    /// </summary>
    public UserControl? Control { get; protected set; }

    /// <summary>
    /// Throws an exception if the Control property has not been populated.
    /// </summary>
    protected void ThrowIfNoControl()
    {
        if (Control == null)
        {
            throw new InvalidOperationException(LocalizedMessages.PreviewHandlerControlNotInitialized);
        }
    }

    /// <summary>
    /// Updates the placement of the Control.
    /// </summary>
    protected void UpdatePlacement()
    {
        if (_source != null)
        {
            HandlerNativeMethods.SetParent(_source.Handle, _parentHandle);

            HandlerNativeMethods.SetWindowPos(_source.Handle, new IntPtr((int)SetWindowPositionInsertAfter.Top),
                0, 0, Math.Abs(_bounds.Left - _bounds.Right), Math.Abs(_bounds.Top - _bounds.Bottom), SetWindowPositionOptions.ShowWindow);
        }
    }

    /// <summary>
    /// Sets the parent window handle for the WPF preview control and updates its placement.
    /// </summary>
    /// <param name="handle">The handle of the parent window.</param>
    protected override void SetParentHandle(IntPtr handle)
    {
        _parentHandle = handle;
        UpdatePlacement();
    }

    /// <summary>
    /// Initializes the WPF preview handler and creates the hosting window for the WPF control.
    /// </summary>
    protected override void Initialize()
    {
        if (_source == null)
        {
            ThrowIfNoControl();

            HwndSourceParameters p = new()
            {
                WindowStyle = (int)(WindowStyles.Child | WindowStyles.Visible | WindowStyles.ClipSiblings),
                ParentWindow = _parentHandle,
                Width = Math.Abs(_bounds.Left - _bounds.Right),
                Height = Math.Abs(_bounds.Top - _bounds.Bottom)
            };

            _source = new HwndSource(p);
            _source.CompositionTarget.BackgroundColor = Brushes.WhiteSmoke.Color;
            _source.RootVisual = (Visual)Control.Content;
        }
        UpdatePlacement();
    }

    /// <summary>
    /// Gets the window handle for the WPF preview control.
    /// </summary>
    protected override IntPtr Handle
    {
        get
        {
            {
                if (_source == null)
                {
                    throw new InvalidOperationException(LocalizedMessages.WpfPreviewHandlerNoHandle);
                }
                return _source.Handle;
            }
        }
    }

    /// <summary>
    /// Updates the bounding rectangle of the WPF preview control.
    /// </summary>
    /// <param name="bounds">The new bounds in screen coordinates.</param>
    protected override void UpdateBounds(NativeRect bounds)
    {
        _bounds = bounds;
        UpdatePlacement();
    }

    /// <summary>
    /// Handles exceptions that occur during initialization of the WPF preview control.
    /// </summary>
    /// <param name="caughtException">The exception that was thrown.</param>
    protected override void HandleInitializeException(Exception? caughtException)
    {
        if (caughtException == null) { return; }

        TextBox text = new()
        {
            IsReadOnly = true,
            MaxLines = 20,
            Text = caughtException.ToString()
        };
        Control = new UserControl { Content = text };
    }

    /// <summary>
    /// Sets keyboard focus to the WPF preview control.
    /// </summary>
    protected override void SetFocus()
    {
        Control.Focus();
    }

    /// <summary>
    /// Sets the background color of the WPF preview control.
    /// </summary>
    /// <param name="argb">The ARGB color value.</param>
    protected override void SetBackground(int argb)
    {
        Control.Background = new SolidColorBrush(Color.FromArgb(
            (byte)((argb >> 24) & 0xFF), //a         
            (byte)((argb >> 16) & 0xFF), //r
            (byte)((argb >> 8) & 0xFF), //g
            (byte)(argb & 0xFF))); //b
    }

    /// <summary>
    /// Sets the foreground (text) color of the WPF preview control.
    /// </summary>
    /// <param name="argb">The ARGB color value.</param>
    protected override void SetForeground(int argb)
    {
        Control.Foreground = new SolidColorBrush(Color.FromArgb(
            (byte)((argb >> 24) & 0xFF), //a                
            (byte)((argb >> 16) & 0xFF), //r
            (byte)((argb >> 8) & 0xFF), //g
            (byte)(argb & 0xFF))); //b                 
    }

    /// <summary>
    /// Sets the font of the WPF preview control based on the specified log font.
    /// </summary>
    /// <param name="font">The log font information to apply.</param>
    protected override void SetFont(LogFont font)
    {
        if (font == null) { throw new ArgumentNullException(nameof(font)); }

        Control.FontFamily = new FontFamily(font.FaceName);
        Control.FontSize = font.Height;
        Control.FontWeight = font.Weight is > 0 and < 1000 ?
            System.Windows.FontWeight.FromOpenTypeWeight(font.Weight) :
            System.Windows.FontWeights.Normal;
    }

    #region IDisposable Members

    /// <summary>
    /// Preview handler control finalizer
    /// </summary>
    ~WpfPreviewHandler()
    {
        Dispose(false);
    }

    /// <summary>
    /// Disposes the control
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Provides means to dispose the object.
    /// When overriden, it is imperative that base.Dispose(true) is called within the implementation.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _source != null)
        {
            _source.Dispose();
        }
    }

    #endregion

}