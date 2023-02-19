//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs
{
    /// <summary>
    /// Data associated with <see cref="TaskDialog.Closing"/> event.
    /// </summary>
    public class TaskDialogClosingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets or sets the standard button that was clicked.
        /// </summary>
        public TaskDialogResult TaskDialogResult { get; set; }

        /// <summary>
        /// Gets or sets the text of the custom button that was clicked.
        /// </summary>
        public string? CustomButton { get; set; }
    }
}
