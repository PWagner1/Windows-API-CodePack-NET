
namespace Microsoft.WindowsAPICodePack.Shell.Taskbar
{
    /// <summary>
    /// Event args for the TabbedThumbnailClosing event. The application should set
    /// the Cancel property to true if the thumbnail should not be removed.
    /// </summary>
    public class TabbedThumbnailClosingEventArgs : TabbedThumbnailEventArgs
    {
        /// <summary>
        /// Creates a Event Args for a TabbedThumbnailClosing event.
        /// </summary>
        /// <param name="windowHandle">Window handle for the control/window related to the event</param>
        public TabbedThumbnailClosingEventArgs(IntPtr windowHandle)
            : base(windowHandle)
        {
        }

        /// <summary>
        /// Creates a Event Args for a TabbedThumbnailClosing event.
        /// </summary>
        /// <param name="windowsControl">WPF Control (UIElement) related to the event</param>
        public TabbedThumbnailClosingEventArgs(UIElement windowsControl)
            : base(windowsControl)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the thumbnail should not be removed.
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
    }
}