using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerBrowserPropertiesDemo
{
    public partial class MainForm : Form
    {
        private ExplorerBrowser explorerBrowser;
        private TextBox searchBox;
        private ComboBox scopeComboBox;
        private CheckBox autoRefreshCheckBox;
        private NumericUpDown maxResultsNumeric;
        private CheckBox showProgressCheckBox;
        private CheckBox cacheResultsCheckBox;
        private ComboBox sortOrderComboBox;
        private CheckBox groupByTypeCheckBox;
        private CheckBox showPreviewsCheckBox;
        private NumericUpDown thumbnailSizeNumeric;
        private CheckBox incrementalSearchCheckBox;
        private NumericUpDown searchDelayNumeric;
        private CheckBox showSuggestionsCheckBox;
        private NumericUpDown maxSuggestionsNumeric;
        private CheckBox highlightTermsCheckBox;
        private Button highlightColorButton;
        private CheckBox advancedFiltersCheckBox;
        private CheckBox rememberHistoryCheckBox;
        private NumericUpDown maxHistoryNumeric;
        private CheckBox enableExportCheckBox;
        private CheckBox showStatisticsCheckBox;
        private CheckBox enableSharingCheckBox;
        private CheckBox showContextMenuCheckBox;
        private CheckBox enableDragDropCheckBox;
        private CheckBox showTooltipsCheckBox;
        private CheckBox persistSelectionCheckBox;
        private CheckBox showResultCountCheckBox;
        private CheckBox enableVirtualScrollingCheckBox;
        private NumericUpDown pageSizeNumeric;
        private Button searchButton;
        private Button clearSearchButton;
        private Label statusLabel;

        public MainForm()
        {
            InitializeComponent();
            InitializeExplorerBrowser();
            InitializeControls();
            SetupEventHandlers();
            LoadDefaultValues();
        }

        private void InitializeExplorerBrowser()
        {
            explorerBrowser = new ExplorerBrowser
            {
                Dock = DockStyle.Fill
            };

            // Navigate to Documents folder by default
            try
            {
                explorerBrowser.Navigate(KnownFolders.Documents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to navigate to Documents folder: {ex.Message}", "Navigation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Add to form
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 400
            };
            panel.Controls.Add(explorerBrowser);
            Controls.Add(panel);
        }

        private void InitializeControls()
        {
            // Create main layout
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 600,
                ColumnCount = 4,
                RowCount = 15
            };

            // Search controls
            mainPanel.Controls.Add(new Label { Text = "Search Query:", Dock = DockStyle.Fill }, 0, 0);
            searchBox = new TextBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(searchBox, 1, 0);

            mainPanel.Controls.Add(new Label { Text = "Search Scope:", Dock = DockStyle.Fill }, 2, 0);
            scopeComboBox = new ComboBox { Dock = DockStyle.Fill };
            scopeComboBox.Items.AddRange(new object[] { "Documents", "Desktop", "Pictures", "Music", "Videos" });
            scopeComboBox.SelectedIndex = 0;
            mainPanel.Controls.Add(scopeComboBox, 3, 0);

            // Search behavior controls
            mainPanel.Controls.Add(new Label { Text = "Auto Refresh:", Dock = DockStyle.Fill }, 0, 1);
            autoRefreshCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(autoRefreshCheckBox, 1, 1);

            mainPanel.Controls.Add(new Label { Text = "Max Results:", Dock = DockStyle.Fill }, 2, 1);
            maxResultsNumeric = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 10000, Value = 0 };
            mainPanel.Controls.Add(maxResultsNumeric, 3, 1);

            mainPanel.Controls.Add(new Label { Text = "Show Progress:", Dock = DockStyle.Fill }, 0, 2);
            showProgressCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showProgressCheckBox, 1, 2);

            mainPanel.Controls.Add(new Label { Text = "Cache Results:", Dock = DockStyle.Fill }, 2, 2);
            cacheResultsCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(cacheResultsCheckBox, 3, 2);

            // Display options
            mainPanel.Controls.Add(new Label { Text = "Sort Order:", Dock = DockStyle.Fill }, 0, 3);
            sortOrderComboBox = new ComboBox { Dock = DockStyle.Fill };
            sortOrderComboBox.Items.AddRange(Enum.GetValues(typeof(SearchResultSortOrder)));
            sortOrderComboBox.SelectedItem = SearchResultSortOrder.Relevance;
            mainPanel.Controls.Add(sortOrderComboBox, 1, 3);

            mainPanel.Controls.Add(new Label { Text = "Group by Type:", Dock = DockStyle.Fill }, 2, 3);
            groupByTypeCheckBox = new CheckBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(groupByTypeCheckBox, 3, 3);

            mainPanel.Controls.Add(new Label { Text = "Show Previews:", Dock = DockStyle.Fill }, 0, 4);
            showPreviewsCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showPreviewsCheckBox, 1, 4);

            mainPanel.Controls.Add(new Label { Text = "Thumbnail Size:", Dock = DockStyle.Fill }, 2, 4);
            thumbnailSizeNumeric = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 32, Maximum = 256, Value = 96 };
            mainPanel.Controls.Add(thumbnailSizeNumeric, 3, 4);

            // Advanced features
            mainPanel.Controls.Add(new Label { Text = "Incremental Search:", Dock = DockStyle.Fill }, 0, 5);
            incrementalSearchCheckBox = new CheckBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(incrementalSearchCheckBox, 1, 5);

            mainPanel.Controls.Add(new Label { Text = "Search Delay (ms):", Dock = DockStyle.Fill }, 2, 5);
            searchDelayNumeric = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 100, Maximum = 2000, Value = 500 };
            mainPanel.Controls.Add(searchDelayNumeric, 3, 5);

            mainPanel.Controls.Add(new Label { Text = "Show Suggestions:", Dock = DockStyle.Fill }, 0, 6);
            showSuggestionsCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showSuggestionsCheckBox, 1, 6);

            mainPanel.Controls.Add(new Label { Text = "Max Suggestions:", Dock = DockStyle.Fill }, 2, 6);
            maxSuggestionsNumeric = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 1, Maximum = 50, Value = 10 };
            mainPanel.Controls.Add(maxSuggestionsNumeric, 3, 6);

            mainPanel.Controls.Add(new Label { Text = "Highlight Terms:", Dock = DockStyle.Fill }, 0, 7);
            highlightTermsCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(highlightTermsCheckBox, 1, 7);

            mainPanel.Controls.Add(new Label { Text = "Highlight Color:", Dock = DockStyle.Fill }, 2, 7);
            highlightColorButton = new Button { Text = "Yellow", Dock = DockStyle.Fill, BackColor = Color.Yellow };
            mainPanel.Controls.Add(highlightColorButton, 3, 7);

            // Additional features
            mainPanel.Controls.Add(new Label { Text = "Advanced Filters:", Dock = DockStyle.Fill }, 0, 8);
            advancedFiltersCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(advancedFiltersCheckBox, 1, 8);

            mainPanel.Controls.Add(new Label { Text = "Remember History:", Dock = DockStyle.Fill }, 2, 8);
            rememberHistoryCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(rememberHistoryCheckBox, 3, 8);

            mainPanel.Controls.Add(new Label { Text = "Max History:", Dock = DockStyle.Fill }, 0, 9);
            maxHistoryNumeric = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 10, Maximum = 200, Value = 50 };
            mainPanel.Controls.Add(maxHistoryNumeric, 1, 9);

            mainPanel.Controls.Add(new Label { Text = "Enable Export:", Dock = DockStyle.Fill }, 2, 9);
            enableExportCheckBox = new CheckBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(enableExportCheckBox, 3, 9);

            mainPanel.Controls.Add(new Label { Text = "Show Statistics:", Dock = DockStyle.Fill }, 0, 10);
            showStatisticsCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showStatisticsCheckBox, 1, 10);

            mainPanel.Controls.Add(new Label { Text = "Enable Sharing:", Dock = DockStyle.Fill }, 2, 10);
            enableSharingCheckBox = new CheckBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(enableSharingCheckBox, 3, 10);

            mainPanel.Controls.Add(new Label { Text = "Show Context Menu:", Dock = DockStyle.Fill }, 0, 11);
            showContextMenuCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showContextMenuCheckBox, 1, 11);

            mainPanel.Controls.Add(new Label { Text = "Enable Drag & Drop:", Dock = DockStyle.Fill }, 2, 11);
            enableDragDropCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(enableDragDropCheckBox, 3, 11);

            mainPanel.Controls.Add(new Label { Text = "Show Tooltips:", Dock = DockStyle.Fill }, 0, 12);
            showTooltipsCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showTooltipsCheckBox, 1, 12);

            mainPanel.Controls.Add(new Label { Text = "Persist Selection:", Dock = DockStyle.Fill }, 2, 12);
            persistSelectionCheckBox = new CheckBox { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(persistSelectionCheckBox, 3, 12);

            mainPanel.Controls.Add(new Label { Text = "Show Result Count:", Dock = DockStyle.Fill }, 0, 13);
            showResultCountCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(showResultCountCheckBox, 1, 13);

            mainPanel.Controls.Add(new Label { Text = "Virtual Scrolling:", Dock = DockStyle.Fill }, 2, 13);
            enableVirtualScrollingCheckBox = new CheckBox { Dock = DockStyle.Fill, Checked = true };
            mainPanel.Controls.Add(enableVirtualScrollingCheckBox, 3, 13);

            mainPanel.Controls.Add(new Label { Text = "Page Size:", Dock = DockStyle.Fill }, 0, 14);
            pageSizeNumeric = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 10, Maximum = 1000, Value = 100 };
            mainPanel.Controls.Add(pageSizeNumeric, 1, 14);

            // Action buttons
            searchButton = new Button { Text = "Search", Dock = DockStyle.Fill };
            mainPanel.Controls.Add(searchButton, 2, 14);

            clearSearchButton = new Button { Text = "Clear Search", Dock = DockStyle.Fill };
            mainPanel.Controls.Add(clearSearchButton, 3, 14);

            // Status label
            statusLabel = new Label { Text = "Ready", Dock = DockStyle.Bottom, Height = 20 };
            mainPanel.Controls.Add(statusLabel, 0, 15);
            mainPanel.SetColumnSpan(statusLabel, 4);

            Controls.Add(mainPanel);
        }

        private void SetupEventHandlers()
        {
            searchButton.Click += SearchButton_Click;
            clearSearchButton.Click += ClearSearchButton_Click;
            highlightColorButton.Click += HighlightColorButton_Click;

            // Property change handlers
            autoRefreshCheckBox.CheckedChanged += (s, e) => explorerBrowser.AutoRefreshSearchResults = autoRefreshCheckBox.Checked;
            maxResultsNumeric.ValueChanged += (s, e) => explorerBrowser.MaxSearchResults = (int)maxResultsNumeric.Value;
            showProgressCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowSearchProgress = showProgressCheckBox.Checked;
            cacheResultsCheckBox.CheckedChanged += (s, e) => explorerBrowser.CacheSearchResults = cacheResultsCheckBox.Checked;
            sortOrderComboBox.SelectedIndexChanged += (s, e) => explorerBrowser.SearchResultSortOrder = (SearchResultSortOrder)sortOrderComboBox.SelectedItem;
            groupByTypeCheckBox.CheckedChanged += (s, e) => explorerBrowser.GroupSearchResultsByType = groupByTypeCheckBox.Checked;
            showPreviewsCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowFilePreviews = showPreviewsCheckBox.Checked;
            thumbnailSizeNumeric.ValueChanged += (s, e) => explorerBrowser.SearchResultThumbnailSize = (int)thumbnailSizeNumeric.Value;
            incrementalSearchCheckBox.CheckedChanged += (s, e) => explorerBrowser.EnableIncrementalSearch = incrementalSearchCheckBox.Checked;
            searchDelayNumeric.ValueChanged += (s, e) => explorerBrowser.IncrementalSearchDelay = (int)searchDelayNumeric.Value;
            showSuggestionsCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowSearchSuggestions = showSuggestionsCheckBox.Checked;
            maxSuggestionsNumeric.ValueChanged += (s, e) => explorerBrowser.MaxSearchSuggestions = (int)maxSuggestionsNumeric.Value;
            highlightTermsCheckBox.CheckedChanged += (s, e) => explorerBrowser.HighlightSearchTerms = highlightTermsCheckBox.Checked;
            advancedFiltersCheckBox.CheckedChanged += (s, e) => explorerBrowser.EnableAdvancedSearchFilters = advancedFiltersCheckBox.Checked;
            rememberHistoryCheckBox.CheckedChanged += (s, e) => explorerBrowser.RememberSearchHistory = rememberHistoryCheckBox.Checked;
            maxHistoryNumeric.ValueChanged += (s, e) => explorerBrowser.MaxSearchHistoryEntries = (int)maxHistoryNumeric.Value;
            enableExportCheckBox.CheckedChanged += (s, e) => explorerBrowser.EnableSearchResultExport = enableExportCheckBox.Checked;
            showStatisticsCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowSearchStatistics = showStatisticsCheckBox.Checked;
            enableSharingCheckBox.CheckedChanged += (s, e) => explorerBrowser.EnableSearchResultSharing = enableSharingCheckBox.Checked;
            showContextMenuCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowSearchResultContextMenu = showContextMenuCheckBox.Checked;
            enableDragDropCheckBox.CheckedChanged += (s, e) => explorerBrowser.EnableSearchResultDragDrop = enableDragDropCheckBox.Checked;
            showTooltipsCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowSearchResultTooltips = showTooltipsCheckBox.Checked;
            persistSelectionCheckBox.CheckedChanged += (s, e) => explorerBrowser.PersistSearchResultSelection = persistSelectionCheckBox.Checked;
            showResultCountCheckBox.CheckedChanged += (s, e) => explorerBrowser.ShowSearchResultCount = showResultCountCheckBox.Checked;
            enableVirtualScrollingCheckBox.CheckedChanged += (s, e) => explorerBrowser.EnableVirtualScrolling = enableVirtualScrollingCheckBox.Checked;
            pageSizeNumeric.ValueChanged += (s, e) => explorerBrowser.VirtualScrollingPageSize = (int)pageSizeNumeric.Value;
        }

        private void LoadDefaultValues()
        {
            // Set default values to match the ExplorerBrowser defaults
            autoRefreshCheckBox.Checked = explorerBrowser.AutoRefreshSearchResults;
            maxResultsNumeric.Value = explorerBrowser.MaxSearchResults;
            showProgressCheckBox.Checked = explorerBrowser.ShowSearchProgress;
            cacheResultsCheckBox.Checked = explorerBrowser.CacheSearchResults;
            sortOrderComboBox.SelectedItem = explorerBrowser.SearchResultSortOrder;
            groupByTypeCheckBox.Checked = explorerBrowser.GroupSearchResultsByType;
            showPreviewsCheckBox.Checked = explorerBrowser.ShowFilePreviews;
            thumbnailSizeNumeric.Value = explorerBrowser.SearchResultThumbnailSize;
            incrementalSearchCheckBox.Checked = explorerBrowser.EnableIncrementalSearch;
            searchDelayNumeric.Value = explorerBrowser.IncrementalSearchDelay;
            showSuggestionsCheckBox.Checked = explorerBrowser.ShowSearchSuggestions;
            maxSuggestionsNumeric.Value = explorerBrowser.MaxSearchSuggestions;
            highlightTermsCheckBox.Checked = explorerBrowser.HighlightSearchTerms;
            advancedFiltersCheckBox.Checked = explorerBrowser.EnableAdvancedSearchFilters;
            rememberHistoryCheckBox.Checked = explorerBrowser.RememberSearchHistory;
            maxHistoryNumeric.Value = explorerBrowser.MaxSearchHistoryEntries;
            enableExportCheckBox.Checked = explorerBrowser.EnableSearchResultExport;
            showStatisticsCheckBox.Checked = explorerBrowser.ShowSearchStatistics;
            enableSharingCheckBox.Checked = explorerBrowser.EnableSearchResultSharing;
            showContextMenuCheckBox.Checked = explorerBrowser.ShowSearchResultContextMenu;
            enableDragDropCheckBox.Checked = explorerBrowser.EnableSearchResultDragDrop;
            showTooltipsCheckBox.Checked = explorerBrowser.ShowSearchResultTooltips;
            persistSelectionCheckBox.Checked = explorerBrowser.PersistSearchResultSelection;
            showResultCountCheckBox.Checked = explorerBrowser.ShowSearchResultCount;
            enableVirtualScrollingCheckBox.Checked = explorerBrowser.EnableVirtualScrolling;
            pageSizeNumeric.Value = explorerBrowser.VirtualScrollingPageSize;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                MessageBox.Show("Please enter a search query.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                statusLabel.Text = "Searching...";
                Application.DoEvents();

                // Get the selected scope
                ShellContainer searchScope = GetSelectedScope();

                // Perform the search
                explorerBrowser.Search(searchBox.Text, searchScope);

                statusLabel.Text = $"Search completed. Found {explorerBrowser.Items?.Count ?? 0} results.";
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Search failed: {ex.Message}";
                MessageBox.Show($"Search failed: {ex.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Navigate back to Documents folder
                explorerBrowser.ClearSearch(KnownFolders.Documents);
                searchBox.Clear();
                statusLabel.Text = "Search cleared. Back to Documents folder.";
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Failed to clear search: {ex.Message}";
                MessageBox.Show($"Failed to clear search: {ex.Message}", "Clear Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HighlightColorButton_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = highlightColorButton.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    highlightColorButton.BackColor = colorDialog.Color;
                    highlightColorButton.Text = colorDialog.Color.Name;
                    explorerBrowser.SearchTermHighlightColor = colorDialog.Color;
                }
            }
        }

        private ShellContainer GetSelectedScope()
        {
            return scopeComboBox.SelectedItem.ToString() switch
            {
                "Documents" => KnownFolders.Documents,
                "Desktop" => KnownFolders.Desktop,
                "Pictures" => KnownFolders.Pictures,
                "Music" => KnownFolders.Music,
                "Videos" => KnownFolders.Videos,
                _ => KnownFolders.Documents
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            explorerBrowser?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
