using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace ExplorerBrowserSearchDemo
{
    public partial class ExplorerBrowserSearchForm : Form
    {
        private ExplorerBrowser explorerBrowser;
        private TextBox addressBar;
        private TextBox searchBox;
        private Button searchButton;
        private Button goButton;
        private ComboBox scopeComboBox;
        private Label statusLabel;

        public ExplorerBrowserSearchForm()
        {
            InitializeComponent();
            InitializeExplorerBrowser();
            InitializeSearchControls();
            SetupEventHandlers();
            
            // Navigate to Desktop initially
            NavigateToLocation(KnownFolders.Desktop);
        }

        private void InitializeExplorerBrowser()
        {
            explorerBrowser = new ExplorerBrowser();
            explorerBrowser.Dock = DockStyle.Fill;
            explorerBrowser.NavigationComplete += ExplorerBrowser_NavigationComplete;
            explorerBrowser.NavigationFailed += ExplorerBrowser_NavigationFailed;
            
            // Enable search folders support
            explorerBrowser.ContentOptions.Flags |= ExplorerBrowserContentSectionOptions.UseSearchFolders;
        }

        private void InitializeSearchControls()
        {
            // Create toolbar panel
            var toolbarPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = SystemColors.Control
            };

            // Address bar
            var addressLabel = new Label
            {
                Text = "Address:",
                Location = new Point(10, 15),
                AutoSize = true
            };

            addressBar = new TextBox
            {
                Location = new Point(70, 12),
                Width = 400,
                ReadOnly = true
            };

            goButton = new Button
            {
                Text = "Go",
                Location = new Point(480, 10),
                Width = 50
            };

            // Search controls
            var searchLabel = new Label
            {
                Text = "Search:",
                Location = new Point(10, 45),
                AutoSize = true
            };

            searchBox = new TextBox
            {
                Location = new Point(70, 42),
                Width = 300,
                PlaceholderText = "Enter search terms..."
            };

            searchButton = new Button
            {
                Text = "Search",
                Location = new Point(380, 40),
                Width = 60
            };

            // Scope selector
            var scopeLabel = new Label
            {
                Text = "Scope:",
                Location = new Point(450, 45),
                AutoSize = true
            };

            scopeComboBox = new ComboBox
            {
                Location = new Point(500, 42),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Status label
            statusLabel = new Label
            {
                Location = new Point(10, 70),
                AutoSize = true,
                ForeColor = SystemColors.GrayText
            };

            // Add controls to toolbar
            toolbarPanel.Controls.AddRange(new Control[]
            {
                addressLabel, addressBar, goButton,
                searchLabel, searchBox, searchButton,
                scopeLabel, scopeComboBox, statusLabel
            });

            // Add controls to form
            Controls.Add(toolbarPanel);
            Controls.Add(explorerBrowser);

            // Populate scope combo box
            PopulateScopeComboBox();
        }

        private void PopulateScopeComboBox()
        {
            scopeComboBox.Items.Clear();
            
            // Add common search scopes
            var scopes = new[]
            {
                new { Name = "This PC", Folder = KnownFolders.ComputerFolder },
                new { Name = "Desktop", Folder = KnownFolders.Desktop },
                new { Name = "Documents", Folder = KnownFolders.Documents },
                new { Name = "Downloads", Folder = KnownFolders.Downloads },
                new { Name = "Pictures", Folder = KnownFolders.Pictures },
                new { Name = "Music", Folder = KnownFolders.Music },
                new { Name = "Videos", Folder = KnownFolders.Videos },
                new { Name = "User Files", Folder = KnownFolders.UsersFiles }
            };

            foreach (var scope in scopes)
            {
                scopeComboBox.Items.Add(scope);
            }

            scopeComboBox.SelectedIndex = 0; // Default to "This PC"
        }

        private void SetupEventHandlers()
        {
            goButton.Click += GoButton_Click;
            searchButton.Click += SearchButton_Click;
            searchBox.KeyPress += SearchBox_KeyPress;
            scopeComboBox.SelectedIndexChanged += ScopeComboBox_SelectedIndexChanged;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(addressBar.Text))
                return;

            try
            {
                // Try to navigate to the entered path
                if (System.IO.Directory.Exists(addressBar.Text))
                {
                    var folder = ShellFileSystemFolder.FromFolderPath(addressBar.Text);
                    NavigateToLocation(folder);
                }
                else if (System.IO.File.Exists(addressBar.Text))
                {
                    var file = ShellFile.FromFilePath(addressBar.Text);
                    NavigateToLocation(file);
                }
                else
                {
                    MessageBox.Show("The specified path does not exist.", "Navigation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to path: {ex.Message}", "Navigation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void SearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch();
                e.Handled = true;
            }
        }

        private void ScopeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update search scope when changed
            if (searchBox.Text.Length > 0)
            {
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                // Clear search - navigate to current scope
                var selectedScope = GetSelectedScope();
                if (selectedScope != null)
                {
                    NavigateToLocation(selectedScope);
                }
                return;
            }

            try
            {
                statusLabel.Text = "Performing search...";
                Application.DoEvents();

                var selectedScope = GetSelectedScope();
                if (selectedScope == null)
                {
                    MessageBox.Show("Please select a valid search scope.", "Search Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create search condition
                var searchCondition = SearchConditionFactory.CreateLeafCondition(
                    "System.FileName", 
                    searchBox.Text, 
                    SearchConditionOperation.Contains
                );

                // Create search folder
                using (var searchFolder = new ShellSearchFolder(searchCondition, selectedScope))
                {
                    // Navigate to the search results
                    NavigateToLocation(searchFolder);
                    
                    statusLabel.Text = $"Search completed. Found {searchFolder.Count} items.";
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Search failed.";
                MessageBox.Show($"Search error: {ex.Message}", "Search Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ShellContainer GetSelectedScope()
        {
            if (scopeComboBox.SelectedItem is dynamic selectedItem)
            {
                return selectedItem.Folder as ShellContainer;
            }
            return null;
        }

        private void NavigateToLocation(ShellObject location)
        {
            try
            {
                explorerBrowser.Navigate(location);
                
                // Update address bar
                if (location is ShellSearchFolder searchFolder)
                {
                    addressBar.Text = $"Search Results: {searchBox.Text}";
                }
                else
                {
                    addressBar.Text = location.ParsingName;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Navigation failed.";
                MessageBox.Show($"Navigation error: {ex.Message}", "Navigation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExplorerBrowser_NavigationComplete(object sender, NavigationCompleteEventArgs e)
        {
            statusLabel.Text = "Navigation completed.";
        }

        private void ExplorerBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            statusLabel.Text = "Navigation failed.";
            MessageBox.Show($"Failed to navigate to: {e.FailedLocation?.Name}", "Navigation Failed", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            explorerBrowser?.Dispose();
            base.OnFormClosing(e);
        }
    }

    // Extension method to enable search folders support
    public static class ExplorerBrowserExtensions
    {
        public static void NavigateToSearch(this ExplorerBrowser browser, string searchQuery, ShellContainer searchScope)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                throw new ArgumentException("Search query must not be empty.", nameof(searchQuery));

            if (searchScope == null)
                throw new ArgumentNullException(nameof(searchScope));

            try
            {
                // Create search condition for filename search
                var searchCondition = SearchConditionFactory.CreateLeafCondition(
                    "System.FileName", 
                    searchQuery, 
                    SearchConditionOperation.Contains
                );

                // Create search folder
                using (var searchFolder = new ShellSearchFolder(searchCondition, searchScope))
                {
                    // Navigate to search results
                    browser.Navigate(searchFolder);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to perform search: {ex.Message}", ex);
            }
        }
    }

    // Main entry point
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ExplorerBrowserSearchForm());
        }
    }
}
