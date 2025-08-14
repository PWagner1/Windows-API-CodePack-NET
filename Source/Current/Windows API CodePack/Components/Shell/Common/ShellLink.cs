//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable InvertIf
// ReSharper disable UseNameofExpression

namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// Represents a link to existing FileSystem or Virtual item.
/// </summary>
public class ShellLink : ShellObject
{
    /// <summary>
    /// Path for this file e.g. c:\Windows\file.txt,
    /// </summary>
    private string? _internalPath;

    #region Internal Constructors

    internal ShellLink(IShellItem2? shellItem)
    {
        nativeShellItem = shellItem;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// The path for this link
    /// </summary>
    public virtual string? Path
    {
        get
        {
            if (_internalPath == null && NativeShellItem != null)
            {
                _internalPath = base.ParsingName;
            }
            return _internalPath;
        }
        protected set => _internalPath = value;
    }

    private string? _internalTargetLocation;
    /// <summary>
    /// Gets the location to which this link points to.
    /// </summary>
    public string? TargetLocation
    {
        get
        {
            if (string.IsNullOrEmpty(_internalTargetLocation) && NativeShellItem2 != null)
            {
                if (Properties!.System != null)
                {
                    _internalTargetLocation = Properties.System.Link.TargetParsingPath?.Value;
                }
            }
            return _internalTargetLocation;
        }
        set
        {
            if (value == null) { return; }

            _internalTargetLocation = value;

            if (NativeShellItem2 != null)
            {
                if (Properties!.System != null)
                {
                    if (Properties.System.Link.TargetParsingPath != null)
                    {
                        Properties.System.Link.TargetParsingPath.Value = _internalTargetLocation;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the ShellObject to which this link points to.
    /// </summary>
    public ShellObject? TargetShellObject => ShellObjectFactory.Create(TargetLocation);

    /// <summary>
    /// Gets or sets the link's title
    /// </summary>
    public string? Title
    {
        get
        {
            return NativeShellItem2 != null ? Properties!.System!.Title?.Value : null;
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(@"value");
            }

            if (NativeShellItem2 != null)
            {
                ShellProperty<string>? shellProperty = Properties!.System!.Title;
                if (shellProperty != null)
                {
                    shellProperty.Value = value;
                }
            }
        }
    }

    private string? _internalArguments;
    /// <summary>
    /// Gets the arguments associated with this link.
    /// </summary>
    public string? Arguments
    {
        get
        {
            if (string.IsNullOrEmpty(_internalArguments) && NativeShellItem2 != null)
            {
                _internalArguments = Properties!.System!.Link.Arguments?.Value;
            }

            return _internalArguments;
        }
    }

    private string? _internalComments;
    /// <summary>
    /// Gets the comments associated with this link.
    /// </summary>
    public string? Comments
    {
        get
        {
            if (string.IsNullOrEmpty(_internalComments) && NativeShellItem2 != null)
            {
                _internalComments = Properties!.System!.Comment?.Value;
            }

            return _internalComments;
        }
    }


    #endregion
}