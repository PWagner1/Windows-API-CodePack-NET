using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerBrowserPropertyGridDemo
{
    public partial class MainForm : Form
    {
        private ExplorerBrowser explorerBrowser;
        private PropertyGrid propertyGrid;
        private SplitContainer splitContainer;

        public MainForm()
        {
            InitializeComponent();
            SetupExplorerBrowser();
            SetupPropertyGrid();
            SetupLayout();
        }

        private void InitializeComponent()
        {
            this.Text = "ExplorerBrowser PropertyGrid Demo - ExpandableObjectConverter";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void SetupExplorerBrowser()
        {
            explorerBrowser = new ExplorerBrowser();
            explorerBrowser.Dock = DockStyle.Fill;
            
            // Navigate to a default location
            try
            {
                var desktop = KnownFolders.Desktop;
                explorerBrowser.Navigate(desktop);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to navigate to Desktop: {ex.Message}", "Navigation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupPropertyGrid()
        {
            propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.ToolbarVisible = true;
            propertyGrid.HelpVisible = true;
            
            // Set the selected object to the ExplorerBrowser to show all its properties
            propertyGrid.SelectedObject = explorerBrowser;
        }

        private void SetupLayout()
        {
            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.SplitterDistance = 800; // Give more space to ExplorerBrowser

            // Add controls to split container
            splitContainer.Panel1.Controls.Add(explorerBrowser);
            splitContainer.Panel2.Controls.Add(propertyGrid);

            // Add split container to form
            this.Controls.Add(splitContainer);

            // Add a label above the property grid
            var label = new Label
            {
                Text = "ExplorerBrowser Properties (including SearchState and SearchOptions)",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font(this.Font.FontFamily, 10, System.Drawing.FontStyle.Bold)
            };
            splitContainer.Panel2.Controls.Add(label);
            splitContainer.Panel2.Controls.SetChildIndex(label, 0);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            explorerBrowser?.Dispose();
        }
    }
}
