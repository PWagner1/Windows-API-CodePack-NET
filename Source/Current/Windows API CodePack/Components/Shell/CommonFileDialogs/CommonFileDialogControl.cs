//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls;

/// <summary>
/// Defines an abstract class that supports shared functionality for the 
/// common file dialog controls.
/// </summary>
public abstract class CommonFileDialogControl : DialogControl
{
    /// <summary>
    /// Holds the text that is displayed for this control.
    /// </summary>
    private string? _textValue;

    /// <summary>
    /// Gets or sets the text string that is displayed on the control.
    /// </summary>
    public virtual string? Text
    {
        get => _textValue;
        set
        {
            // Don't update this property if it hasn't changed
            if (value != _textValue)
            {
                _textValue = value;
                ApplyPropertyChange("Text");
            }
        }
    }

    private bool _enabled = true;
    /// <summary>
    /// Gets or sets a value that determines if this control is enabled.  
    /// </summary>
    public bool Enabled
    {
        get => _enabled;
        set
        {
            // Don't update this property if it hasn't changed
            if (value == _enabled) { return; }

            _enabled = value;
            ApplyPropertyChange("Enabled");
        }
    }

    private bool _visible = true;
    /// <summary>
    /// Gets or sets a boolean value that indicates whether  
    /// this control is visible.
    /// </summary>
    public bool Visible
    {
        get => _visible;
        set
        {
            // Don't update this property if it hasn't changed
            if (value == _visible) { return; }

            _visible = value;
            ApplyPropertyChange("Visible");
        }
    }

    private bool _isAdded;
    /// <summary>
    /// Has this control been added to the dialog
    /// </summary>
    internal bool IsAdded
    {
        get => _isAdded;
        set => _isAdded = value;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    protected CommonFileDialogControl() { }

    /// <summary>
    /// Creates a new instance of this class with the text.
    /// </summary>
    /// <param name="text">The text of the common file dialog control.</param>
    protected CommonFileDialogControl(string? text)
        : base()
    {
        _textValue = text;
    }

    /// <summary>
    /// Creates a new instance of this class with the specified name and text.
    /// </summary>
    /// <param name="name">The name of the common file dialog control.</param>
    /// <param name="text">The text of the common file dialog control.</param>
    protected CommonFileDialogControl(string? name, string? text)
        : base(name)
    {
        _textValue = text;
    }

    /// <summary>
    /// Attach the custom control itself to the specified dialog
    /// </summary>
    /// <param name="dialog">the target dialog</param>
    internal abstract void Attach(IFileDialogCustomize? dialog);

    internal virtual void SyncUnmanagedProperties()
    {
        ApplyPropertyChange("Enabled");
        ApplyPropertyChange("Visible");
    }
}