//Copyright (c) Microsoft Corporation.  All rights reserved.

using UserControl = System.Windows.Controls.UserControl;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable UseNameofForDependencyProperty


namespace Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation
{
    /// <summary>
    /// Interaction logic for ExplorerBrowser.xaml
    /// </summary>
    public partial class ExplorerBrowser : UserControl, IDisposable
    {
        /// <summary>
        /// The underlying WinForms control
        /// </summary>
        public Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser ExplorerBrowserControl
        {
            get;
            set;
        }

        private readonly ObservableCollection<ShellObject> _selectedItems;
        private readonly ObservableCollection<ShellObject> _items;
        private readonly ObservableCollection<ShellObject?> _navigationLog;
        private readonly DispatcherTimer _dtClrUpdater = new();

        private ShellObject? _initialNavigationTarget;
        private ExplorerBrowserViewMode? _initialViewMode;

        private readonly AutoResetEvent _itemsChanged = new(false);
        private readonly AutoResetEvent _selectionChanged = new(false);
        private int _selectionChangeWaitCount;

        /// <summary>
        /// Hosts the ExplorerBrowser WinForms wrapper in this control
        /// </summary>
        public ExplorerBrowser()
        {
            InitializeComponent();

            // the ExplorerBrowser WinForms control
            ExplorerBrowserControl = new();

            // back the dependency collection properties with instances
            SelectedItems = _selectedItems = new();
            Items = _items = new();
            NavigationLog = _navigationLog = new();

            // hook up events for collection synchronization
            ExplorerBrowserControl.ItemsChanged += ItemsChanged;
            ExplorerBrowserControl.SelectionChanged += SelectionChanged;
            ExplorerBrowserControl.ViewEnumerationComplete += ExplorerBrowserControl_ViewEnumerationComplete;
            ExplorerBrowserControl.ViewSelectedItemChanged += ExplorerBrowserControl_ViewSelectedItemChanged;
            ExplorerBrowserControl.NavigationLog.NavigationLogChanged += NavigationLogChanged;

            // host the control           
            WindowsFormsHost host = new();
            try
            {
                host.Child = ExplorerBrowserControl;
                root.Children.Clear();
                root.Children.Add(host);
            }
            catch
            {
                host.Dispose();
                throw;
            }

            Loaded += ExplorerBrowser_Loaded;
        }

        void ExplorerBrowserControl_ViewSelectedItemChanged(object sender, EventArgs e)
        {
        }

        void ExplorerBrowserControl_ViewEnumerationComplete(object sender, EventArgs e)
        {
            _itemsChanged.Set();
            _selectionChanged.Set();
        }

        /// <summary>
        /// To avoid the 'Dispatcher processing has been suspended' InvalidOperationException on Win7,
        /// the ExplorerBorwser native control is initialized after this control is fully loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExplorerBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            // setup timer to update dependency properties from CLR properties of WinForms ExplorerBrowser object
            _dtClrUpdater.Tick += new(UpdateDependencyPropertiesFromClrpRoperties);
            _dtClrUpdater.Interval = new(100 * 10000); // 100ms
            _dtClrUpdater.Start();

            if (_initialNavigationTarget != null)
            {
                ExplorerBrowserControl.Navigate(_initialNavigationTarget);
                _initialNavigationTarget = null;
            }

            if (_initialViewMode != null)
            {
                ExplorerBrowserControl.ContentOptions.ViewMode = (ExplorerBrowserViewMode)_initialViewMode;
                _initialViewMode = null;
            }
        }

        /// <summary>
        /// Map changes to the CLR flags to the dependency properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UpdateDependencyPropertiesFromClrpRoperties(object sender, EventArgs e)
        {
            AlignLeft = ExplorerBrowserControl.ContentOptions.AlignLeft;
            AutoArrange = ExplorerBrowserControl.ContentOptions.AutoArrange;
            CheckSelect = ExplorerBrowserControl.ContentOptions.CheckSelect;
            ExtendedTiles = ExplorerBrowserControl.ContentOptions.ExtendedTiles;
            FullRowSelect = ExplorerBrowserControl.ContentOptions.FullRowSelect;
            HideFileNames = ExplorerBrowserControl.ContentOptions.HideFileNames;
            NoBrowserViewState = ExplorerBrowserControl.ContentOptions.NoBrowserViewState;
            NoColumnHeader = ExplorerBrowserControl.ContentOptions.NoColumnHeader;
            NoHeaderInAllViews = ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews;
            NoIcons = ExplorerBrowserControl.ContentOptions.NoIcons;
            NoSubfolders = ExplorerBrowserControl.ContentOptions.NoSubfolders;
            SingleClickActivate = ExplorerBrowserControl.ContentOptions.SingleClickActivate;
            SingleSelection = ExplorerBrowserControl.ContentOptions.SingleSelection;
            ThumbnailSize = ExplorerBrowserControl.ContentOptions.ThumbnailSize;
            ViewMode = ExplorerBrowserControl.ContentOptions.ViewMode;
            AlwaysNavigate = ExplorerBrowserControl.NavigationOptions.AlwaysNavigate;
            NavigateOnce = ExplorerBrowserControl.NavigationOptions.NavigateOnce;
            AdvancedQueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery;
            CommandsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands;
            CommandsOrganizePane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize;
            CommandsViewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView;
            DetailsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details;
            NavigationPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation;
            PreviewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview;
            QueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query;
            NavigationLogIndex = ExplorerBrowserControl.NavigationLog.CurrentLocationIndex;

            if (_itemsChanged.WaitOne(1, false))
            {
                _items.Clear();
                foreach (ShellObject obj in ExplorerBrowserControl.Items)
                {
                    _items.Add(obj);
                }
            }

            if (_selectionChanged.WaitOne(1, false))
            {
                _selectionChangeWaitCount = 4;
            }
            else if (_selectionChangeWaitCount > 0)
            {
                _selectionChangeWaitCount--;

                if (_selectionChangeWaitCount == 0)
                {
                    _selectedItems.Clear();
                    foreach (ShellObject obj in ExplorerBrowserControl.SelectedItems)
                    {
                        _selectedItems.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Synchronize NavigationLog collection to dependency collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void NavigationLogChanged(object sender, NavigationLogEventArgs args)
        {
            _navigationLog.Clear();
            foreach (ShellObject? obj in ExplorerBrowserControl.NavigationLog.Locations)
            {
                _navigationLog.Add(obj);
            }
        }

        /// <summary>
        /// Synchronize SelectedItems collection to dependency collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectionChanged(object sender, EventArgs e)
        {
            _selectionChanged.Set();
        }

        // Synchronize ItemsCollection to dependency collection
        void ItemsChanged(object sender, EventArgs e)
        {
            _itemsChanged.Set();
        }

        /// <summary>
        /// The items in the ExplorerBrowser window
        /// </summary>
        public ObservableCollection<ShellObject> Items
        {
            get => (ObservableCollection<ShellObject>)GetValue(ItemsProperty);
            set => SetValue(ItemsPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ItemsPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                        "Items", typeof(ObservableCollection<ShellObject>),
                        typeof(ExplorerBrowser),
                        new(null));

        /// <summary>
        /// The items in the ExplorerBrowser window
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// The selected items in the ExplorerBrowser window
        /// </summary>
        public ObservableCollection<ShellObject> SelectedItems
        {
            get => (ObservableCollection<ShellObject>)GetValue(SelectedItemsProperty);
            internal set => SetValue(SelectedItemsPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                        "SelectedItems", typeof(ObservableCollection<ShellObject>),
                        typeof(ExplorerBrowser),
                        new(null));

        /// <summary>
        /// The selected items in the ExplorerBrowser window
        /// </summary>
        public ObservableCollection<ShellObject?> NavigationLog
        {
            get => (ObservableCollection<ShellObject>)GetValue(NavigationLogProperty);
            internal set => SetValue(NavigationLogPropertyKey, value);
        }

        private static readonly DependencyPropertyKey NavigationLogPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                        "NavigationLog", typeof(ObservableCollection<ShellObject>),
                        typeof(ExplorerBrowser),
                        new(null));

        /// <summary>
        /// The NavigationLog
        /// </summary>
        public static readonly DependencyProperty NavigationLogProperty = NavigationLogPropertyKey.DependencyProperty;

        /// <summary>
        /// The selected items in the ExplorerBrowser window
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;


        /// <summary>
        /// The location the explorer browser is navigating to
        /// </summary>
        public ShellObject NavigationTarget
        {
            get => (ShellObject)GetValue(NavigationTargetProperty);
            set => SetValue(NavigationTargetProperty, value);
        }

        /// <summary>
        /// The DependencyProperty for the NavigationTarget property
        /// </summary>
        public static readonly DependencyProperty NavigationTargetProperty =
                    DependencyProperty.Register(
                        @"NavigationTarget", typeof(ShellObject),
                        typeof(ExplorerBrowser),
                        new(null, NavigationTargetChanged));

        private static void NavigationTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser? instance = d as ExplorerBrowser;

            if (instance != null && instance.ExplorerBrowserControl.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.Navigate((ShellObject)e.NewValue);
            }
            else
            {
                instance._initialNavigationTarget = (ShellObject)e.NewValue;
            }
        }

        /// <summary>
        /// The view should be left-aligned. 
        /// </summary>
        public bool AlignLeft
        {
            get => (bool)GetValue(AlignLeftProperty);
            set => SetValue(AlignLeftProperty, value);
        }

        internal static DependencyProperty AlignLeftProperty =
                    DependencyProperty.Register(
                        "AlignLeft", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnAlignLeftChanged));

        private static void OnAlignLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
                instance.ExplorerBrowserControl.ContentOptions.AlignLeft = (bool)e.NewValue;
        }


        /// <summary>
        /// Automatically arrange the elements in the view. 
        /// </summary>
        public bool AutoArrange
        {
            get => (bool)GetValue(AutoArrangeProperty);
            set => SetValue(AutoArrangeProperty, value);
        }

        internal static DependencyProperty AutoArrangeProperty =
                    DependencyProperty.Register(
                        "AutoArrange", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnAutoArrangeChanged));

        private static void OnAutoArrangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
                instance.ExplorerBrowserControl.ContentOptions.AutoArrange = (bool)e.NewValue;
        }

        /// <summary>
        /// Turns on check mode for the view
        /// </summary>
        public bool CheckSelect
        {
            get => (bool)GetValue(CheckSelectProperty);
            set => SetValue(CheckSelectProperty, value);
        }

        internal static DependencyProperty CheckSelectProperty =
                    DependencyProperty.Register(
                        "CheckSelect", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnCheckSelectChanged));

        private static void OnCheckSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.CheckSelect = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.
        /// </summary>
        public bool ExtendedTiles
        {
            get => (bool)GetValue(ExtendedTilesProperty);
            set => SetValue(ExtendedTilesProperty, value);
        }

        internal static DependencyProperty ExtendedTilesProperty =
                    DependencyProperty.Register(
                        "ExtendedTiles", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnExtendedTilesChanged));

        private static void OnExtendedTilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
                instance.ExplorerBrowserControl.ContentOptions.ExtendedTiles = (bool)e.NewValue;
        }

        /// <summary>
        /// When an item is selected, the item and all its sub-items are highlighted.
        /// </summary>
        public bool FullRowSelect
        {
            get => (bool)GetValue(FullRowSelectProperty);
            set => SetValue(FullRowSelectProperty, value);
        }

        internal static DependencyProperty FullRowSelectProperty =
                    DependencyProperty.Register(
                        "FullRowSelect", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnFullRowSelectChanged));

        private static void OnFullRowSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.FullRowSelect = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// The view should not display file names
        /// </summary>
        public bool HideFileNames
        {
            get => (bool)GetValue(HideFileNamesProperty);
            set => SetValue(HideFileNamesProperty, value);
        }

        internal static DependencyProperty HideFileNamesProperty =
                    DependencyProperty.Register(
                        "HideFileNames", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnHideFileNamesChanged));

        private static void OnHideFileNamesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.HideFileNames = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// The view should not save view state in the browser.
        /// </summary>
        public bool NoBrowserViewState
        {
            get => (bool)GetValue(NoBrowserViewStateProperty);
            set => SetValue(NoBrowserViewStateProperty, value);
        }

        internal static DependencyProperty NoBrowserViewStateProperty =
                    DependencyProperty.Register(
                        "NoBrowserViewState", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnNoBrowserViewStateChanged));

        private static void OnNoBrowserViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.NoBrowserViewState = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Do not display a column header in the view in any view mode.
        /// </summary>
        public bool NoColumnHeader
        {
            get => (bool)GetValue(NoColumnHeaderProperty);
            set => SetValue(NoColumnHeaderProperty, value);
        }

        internal static DependencyProperty NoColumnHeaderProperty =
                    DependencyProperty.Register(
                        "NoColumnHeader", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnNoColumnHeaderChanged));

        private static void OnNoColumnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
                instance.ExplorerBrowserControl.ContentOptions.NoColumnHeader = (bool)e.NewValue;
        }

        /// <summary>
        /// Only show the column header in details view mode.
        /// </summary>
        public bool NoHeaderInAllViews
        {
            get => (bool)GetValue(NoHeaderInAllViewsProperty);
            set => SetValue(NoHeaderInAllViewsProperty, value);
        }

        internal static DependencyProperty NoHeaderInAllViewsProperty =
                    DependencyProperty.Register(
                        "NoHeaderInAllViews", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnNoHeaderInAllViewsChanged));

        private static void OnNoHeaderInAllViewsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// The view should not display icons. 
        /// </summary>
        public bool NoIcons
        {
            get => (bool)GetValue(NoIconsProperty);
            set => SetValue(NoIconsProperty, value);
        }

        internal static DependencyProperty NoIconsProperty =
                    DependencyProperty.Register(
                        "NoIcons", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnNoIconsChanged));

        private static void OnNoIconsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.NoIcons = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Do not show subfolders. 
        /// </summary>
        public bool NoSubfolders
        {
            get => (bool)GetValue(NoSubfoldersProperty);
            set => SetValue(NoSubfoldersProperty, value);
        }

        internal static DependencyProperty NoSubfoldersProperty =
                    DependencyProperty.Register(
                        "NoSubfolders", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnNoSubfoldersChanged));

        private static void OnNoSubfoldersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.NoSubfolders = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Navigate with a single click
        /// </summary>
        public bool SingleClickActivate
        {
            get => (bool)GetValue(SingleClickActivateProperty);
            set => SetValue(SingleClickActivateProperty, value);
        }

        internal static DependencyProperty SingleClickActivateProperty =
                    DependencyProperty.Register(
                        "SingleClickActivate", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnSingleClickActivateChanged));

        private static void OnSingleClickActivateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.SingleClickActivate = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Do not allow more than a single item to be selected.
        /// </summary>
        public bool SingleSelection
        {
            get => (bool)GetValue(SingleSelectionProperty);
            set => SetValue(SingleSelectionProperty, value);
        }

        internal static DependencyProperty SingleSelectionProperty =
                    DependencyProperty.Register(
                        "SingleSelection", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnSingleSelectionChanged));

        private static void OnSingleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.SingleSelection = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// The size of the thumbnails in the explorer browser
        /// </summary>
        public int ThumbnailSize
        {
            get => (int)GetValue(ThumbnailSizeProperty);
            set => SetValue(ThumbnailSizeProperty, value);
        }

        internal static DependencyProperty ThumbnailSizeProperty =
                    DependencyProperty.Register(
                        "ThumbnailSize", typeof(int),
                        typeof(ExplorerBrowser),
                        new(32, OnThumbnailSizeChanged));

        private static void OnThumbnailSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.ContentOptions.ThumbnailSize = (int)e.NewValue;
            }
        }



        /// <summary>
        /// The various view modes of the explorer browser control
        /// </summary>
        public ExplorerBrowserViewMode ViewMode
        {
            get => (ExplorerBrowserViewMode)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }

        internal static DependencyProperty ViewModeProperty =
                    DependencyProperty.Register(
                        "ViewMode", typeof(ExplorerBrowserViewMode),
                        typeof(ExplorerBrowser),
                        new(ExplorerBrowserViewMode.Auto, OnViewModeChanged));

        private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;

            if (instance.ExplorerBrowserControl != null)
            {
                if (instance.ExplorerBrowserControl.ExplorerBrowserControl == null)
                {
                    instance._initialViewMode = (ExplorerBrowserViewMode)e.NewValue;
                }
                else
                {
                    instance.ExplorerBrowserControl.ContentOptions.ViewMode = (ExplorerBrowserViewMode)e.NewValue;
                }
            }
        }


        /// <summary>
        /// Always navigate, even if you are attempting to navigate to the current folder.
        /// </summary>
        public bool AlwaysNavigate
        {
            get => (bool)GetValue(AlwaysNavigateProperty);
            set => SetValue(AlwaysNavigateProperty, value);
        }

        internal static DependencyProperty AlwaysNavigateProperty =
                    DependencyProperty.Register(
                        "AlwaysNavigate", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnAlwaysNavigateChanged));

        private static void OnAlwaysNavigateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.AlwaysNavigate = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Do not navigate further than the initial navigation.
        /// </summary>
        public bool NavigateOnce
        {
            get => (bool)GetValue(NavigateOnceProperty);
            set => SetValue(NavigateOnceProperty, value);
        }

        internal static DependencyProperty NavigateOnceProperty =
                    DependencyProperty.Register(
                        "NavigateOnce", typeof(bool),
                        typeof(ExplorerBrowser),
                        new(false, OnNavigateOnceChanged));

        private static void OnNavigateOnceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.NavigateOnce = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the AdvancedQuery pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState AdvancedQueryPane
        {
            get => (PaneVisibilityState)GetValue(AdvancedQueryPaneProperty);
            set => SetValue(AdvancedQueryPaneProperty, value);
        }

        internal static DependencyProperty AdvancedQueryPaneProperty =
                    DependencyProperty.Register(
                        "AdvancedQueryPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnAdvancedQueryPaneChanged));

        private static void OnAdvancedQueryPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery = (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the Commands pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState CommandsPane
        {
            get => (PaneVisibilityState)GetValue(CommandsPaneProperty);
            set => SetValue(CommandsPaneProperty, value);
        }

        internal static DependencyProperty CommandsPaneProperty =
                    DependencyProperty.Register(
                        "CommandsPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnCommandsPaneChanged));

        private static void OnCommandsPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands =
                    (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the Organize menu in the Commands pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState CommandsOrganizePane
        {
            get => (PaneVisibilityState)GetValue(CommandsOrganizePaneProperty);
            set => SetValue(CommandsOrganizePaneProperty, value);
        }

        internal static DependencyProperty CommandsOrganizePaneProperty =
                    DependencyProperty.Register(
                        "CommandsOrganizePane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnCommandsOrganizePaneChanged));

        private static void OnCommandsOrganizePaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize =
                    (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the View menu in the Commands pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState CommandsViewPane
        {
            get => (PaneVisibilityState)GetValue(CommandsViewPaneProperty);
            set => SetValue(CommandsViewPaneProperty, value);
        }

        internal static DependencyProperty CommandsViewPaneProperty =
                    DependencyProperty.Register(
                        "CommandsViewPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnCommandsViewPaneChanged));

        private static void OnCommandsViewPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView = (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the Details pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState DetailsPane
        {
            get => (PaneVisibilityState)GetValue(DetailsPaneProperty);
            set => SetValue(DetailsPaneProperty, value);
        }

        internal static DependencyProperty DetailsPaneProperty =
                    DependencyProperty.Register(
                        "DetailsPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnDetailsPaneChanged));

        private static void OnDetailsPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details = (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the Navigation pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState NavigationPane
        {
            get => (PaneVisibilityState)GetValue(NavigationPaneProperty);
            set => SetValue(NavigationPaneProperty, value);
        }

        internal static DependencyProperty NavigationPaneProperty =
                    DependencyProperty.Register(
                        "NavigationPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnNavigationPaneChanged));

        private static void OnNavigationPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation = (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the Preview pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState PreviewPane
        {
            get => (PaneVisibilityState)GetValue(PreviewPaneProperty);
            set => SetValue(PreviewPaneProperty, value);
        }

        internal static DependencyProperty PreviewPaneProperty =
                    DependencyProperty.Register(
                        "PreviewPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnPreviewPaneChanged));

        private static void OnPreviewPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview = (PaneVisibilityState)e.NewValue;
            }
        }

        /// <summary>
        /// Show/Hide the Query pane on subsequent navigation
        /// </summary>
        public PaneVisibilityState QueryPane
        {
            get => (PaneVisibilityState)GetValue(QueryPaneProperty);
            set => SetValue(QueryPaneProperty, value);
        }

        internal static DependencyProperty QueryPaneProperty =
                    DependencyProperty.Register(
                        "QueryPane", typeof(PaneVisibilityState),
                        typeof(ExplorerBrowser),
                        new(PaneVisibilityState.DoNotCare, OnQueryPaneChanged));

        private static void OnQueryPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
            {
                instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query = (PaneVisibilityState)e.NewValue;
            }
        }


        /// <summary>
        /// Navigation log index
        /// </summary>
        public int NavigationLogIndex
        {
            get => (int)GetValue(NavigationLogIndexProperty);
            set => SetValue(NavigationLogIndexProperty, value);
        }

        internal static DependencyProperty NavigationLogIndexProperty =
                    DependencyProperty.Register(
                        "NavigationLogIndex", typeof(int),
                        typeof(ExplorerBrowser),
                        new(0, OnNavigationLogIndexChanged));

        private static void OnNavigationLogIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExplorerBrowser instance = d as ExplorerBrowser;
            if (instance.ExplorerBrowserControl != null)
                instance.ExplorerBrowserControl.NavigationLog.NavigateLog((int)e.NewValue);
        }


        #region IDisposable Members

        /// <summary>
        /// Disposes the class
        /// </summary>        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the browser.
        /// </summary>
        /// <param name="disposed"></param>
        protected virtual void Dispose(bool disposed)
        {
            if (disposed)
            {
                if (_itemsChanged != null)
                {
                    _itemsChanged.Close();
                }

                if (_selectionChanged != null)
                {
                    _selectionChanged.Close();
                }
            }
        }

        #endregion
    }
}
