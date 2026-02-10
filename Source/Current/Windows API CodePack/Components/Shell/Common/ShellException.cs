namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// An exception thrown when an error occurs while dealing with ShellObjects.
/// </summary>
[Serializable]
public class ShellException : ExternalException
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public ShellException() { }

    /// <summary>
    /// Initializes a new exception using an HResult
    /// </summary>
    /// <param name="result">HResult error</param>
    internal ShellException(HResult result) : this((int)result) { }

    /// <summary>
    /// Initializes an excpetion with a custom message.
    /// </summary>
    /// <param name="message">Custom message</param>
    public ShellException(string message) : base(message) { }

    /// <summary>
    /// Initializes an exception with custom message and inner exception.
    /// </summary>
    /// <param name="message">Custom message</param>
    /// <param name="innerException">The original exception that preceded this exception</param>
    public ShellException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes an exception with custom message and error code.
    /// </summary>
    /// <param name="message">Custom message</param>
    /// <param name="errorCode">HResult error code</param>
    public ShellException(string message, int errorCode) : base(message, errorCode) { }

    /// <summary>
    /// Initializes an exception with custom message and error code.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errorCode"></param>
    internal ShellException(string message, HResult errorCode) : this(message, (int)errorCode) { }

    /// <summary>
    /// Initializes an exception with custom message and inner exception.
    /// </summary>
    /// <param name="errorCode">HRESULT of an operation</param>        
    public ShellException(int errorCode)
        : base(LocalizedMessages.ShellExceptionDefaultText, errorCode)
    {
    }

    /// <summary>
    /// Initializes an exception from serialization info and a context.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
#pragma warning disable SYSLIB0051 // Type or member is obsolete - required for serialization compatibility
    protected ShellException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
#pragma warning restore SYSLIB0051

}