//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Controls
{

    /// <summary>
    /// These options control the results subsequent navigations of the ExplorerBrowser
    /// </summary>
    public class ExplorerBrowserNavigationOptions
    {
        #region construction

        readonly ExplorerBrowser _eb;
        internal ExplorerBrowserNavigationOptions(ExplorerBrowser eb)
        {
            this._eb = eb;
            PaneVisibility = new();
        }
        #endregion

        #region Flags property
        /// <summary>
        /// The binary flags that are passed to the explorer browser control's GetOptions/SetOptions methods
        /// </summary>
        public ExplorerBrowserNavigateOptions Flags
        {
            get
            {
                ExplorerBrowserOptions ebo = new();
                if (_eb.ExplorerBrowserControl != null)
                {
                    _eb.ExplorerBrowserControl.GetOptions(out ebo);
                    return (ExplorerBrowserNavigateOptions)ebo;
                }
                return (ExplorerBrowserNavigateOptions)ebo;
            }
            set
            {
                ExplorerBrowserOptions ebo = (ExplorerBrowserOptions)value;
                if (_eb.ExplorerBrowserControl != null)
                {
                    // Always forcing SHOWFRAMES because we handle IExplorerPaneVisibility
                    _eb.ExplorerBrowserControl.SetOptions(ebo | ExplorerBrowserOptions.ShowFrames);
                }
            }
        }
        #endregion

        #region control flags to properties mapping
        /// <summary>
        /// Do not navigate further than the initial navigation.
        /// </summary>
        public bool NavigateOnce
        {
            get => IsFlagSet(ExplorerBrowserNavigateOptions.NavigateOnce);
            set => SetFlag(ExplorerBrowserNavigateOptions.NavigateOnce, value);
        }
        /// <summary>
        /// Always navigate, even if you are attempting to navigate to the current folder.
        /// </summary>
        public bool AlwaysNavigate
        {
            get => IsFlagSet(ExplorerBrowserNavigateOptions.AlwaysNavigate);
            set => SetFlag(ExplorerBrowserNavigateOptions.AlwaysNavigate, value);
        }

        private bool IsFlagSet(ExplorerBrowserNavigateOptions flag)
        {
            return (Flags & flag) != 0;
        }

        private void SetFlag(ExplorerBrowserNavigateOptions flag, bool value)
        {
            if (value)
            {
                Flags |= flag;
            }
            else
            {
                Flags = Flags & ~flag;
            }
        }
        #endregion

        #region ExplorerBrowser pane visibility
        /// <summary>
        /// Controls the visibility of the various ExplorerBrowser panes on subsequent navigation
        /// </summary>
        public ExplorerBrowserPaneVisibility PaneVisibility { get; private set; }

        #endregion
    }
}
