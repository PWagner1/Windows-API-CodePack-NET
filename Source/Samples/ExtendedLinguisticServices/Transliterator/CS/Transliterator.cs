// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.ExtendedLinguisticServices;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Dialogs;

using TD = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace Transliterator
{
    public partial class Transliterator : Form
    {
        public const int SB_LINEUP = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_ENDSCROLL = 8;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;

        private const string CategoryTransliteration = "Transliteration";
        private readonly MappingService[] _transliterationServices;
        private Guid? _guidService = null;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public Transliterator()
        {
            InitializeComponent();

            MinimumSize = this.Size;

            string fileName = Path.GetFullPath("sample.txt");
            if (File.Exists(fileName))
            {
                textBoxSourceFile.Text = fileName;
                textBoxSource.Text = File.ReadAllText(fileName);
            }

            _transliterationServices = GetSpecifiedMappingServices(CategoryTransliteration);
            if ((_transliterationServices != null) && (_transliterationServices.Any()))
            {
                foreach (MappingService ms in _transliterationServices)
                {
                    comboBoxServices.Items.Add(new DataItem() { Name = ms.Description, Guid = ms.Guid });
                }
                comboBoxServices.SelectedIndex = 0;
            }

        }

        private MappingService[] GetSpecifiedMappingServices(string categoryName)
        {
            MappingService[] transliterationServices = null;
            try
            {
                MappingEnumOptions enumOptions = new MappingEnumOptions() { Category = categoryName };
                transliterationServices = MappingService.GetServices(enumOptions);
            }
            catch (LinguisticException exc)
            {
                ShowErrorMessage(String.Format("Error calling ELS: {0}, HResult: {1}",
                    exc.ResultState.ErrorMessage, exc.ResultState.HResult));
            }
            return transliterationServices;
        }

        private string LanguageConverter(Guid serviceGuid, string sourceContent)
        {
            string transliterated = null;
            if ((sourceContent != null) && (sourceContent.Length > 0))
            {
                try
                {
                    MappingService mapService = new MappingService(serviceGuid);
                    using (MappingPropertyBag bag = mapService.RecognizeText(sourceContent, null))
                    {
                        transliterated = bag.GetResultRanges()[0].FormatData(new StringFormatter());
                    }
                }
                catch (LinguisticException exc)
                {
                    ShowErrorMessage(
                        $"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
                }
            }
            return transliterated;
        }

        private void ShowErrorMessage(string msg)
        {
            TD td = new TD()
            {
                StandardButtons = TaskDialogStandardButtons.Close,
                Caption = "Error",
                InstructionText = msg,
                Icon = TaskDialogStandardIcon.Error
            };

            TaskDialogResult res = td.Show();
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.AllowNonFileSystemItems = true;
            openFileDialog.Title = "Select source file";
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filters.Add(new CommonFileDialogFilter("Text files (*.txt)", "*.txt"));
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != CommonFileDialogResult.Cancel)
            {
                try
                {
                    textBoxSourceFile.Text = openFileDialog.FileAsShellObject?.ParsingName;
                    textBoxSource.Text = File.ReadAllText(textBoxSourceFile.Text);
                    textBoxResult.Text = "";
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.Assert(_guidService != null);
                string result = LanguageConverter(_guidService.GetValueOrDefault(), textBoxSource.Text);
                if (!string.IsNullOrEmpty(result))
                {
                    textBoxResult.Text = result;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            TD taskHelp = new TD();
            taskHelp.Caption = "Help";
            taskHelp.Text = "Steps to use the tool:\n\n";
            taskHelp.Text += "1) Use the Browse button to load the unicode text from a ";
            taskHelp.Text += "text file or copy and paste text ";
            taskHelp.Text += "directly to the text box under the 'Text for conversion:' label,\n";
            taskHelp.Text += "2) Choose a tranliteration service from the drop down list,\n";
            taskHelp.Text += "3) Click the Convert button.\n\n";
            taskHelp.Text += "This demo uses the Extended Linguistic Services API in the Windows API Code ";
            taskHelp.Text += "Pack for Microsoft .NET Framework and .NET.";
            taskHelp.DetailsExpandedText = "<a href=\"https://github.com/Wagnerp/Windows-API-CodePack-NET\">Windows API Code Pack for .NET Framework and .NET</a>";

            // Enable the hyperlinks
            taskHelp.HyperlinksEnabled = true;
            taskHelp.HyperlinkClick += taskHelp_HyperlinkClick;

            taskHelp.Cancelable = true;

            taskHelp.Icon = TaskDialogStandardIcon.Information;
            taskHelp.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        static void taskHelp_HyperlinkClick(object sender, TaskDialogHyperlinkClickedEventArgs e)
        {
            // Launch the application associated with http links
            Process.Start(e.LinkText);
        }

        private void textBoxSource_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBoxSource.Capture)
            {
                textBoxResult.SelectionLength = 0;
            }
        }

        private void textBoxSource_MouseMove(object sender, MouseEventArgs e)
        {
            if ((textBoxSource.Capture) && (textBoxSource.SelectionLength > 0))
            {
                textBoxResult.SelectionStart = textBoxSource.SelectionStart;
                textBoxResult.SelectionLength = textBoxSource.SelectionLength;
            }
        }

        private void textBoxResult_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBoxResult.Capture)
            {
                textBoxSource.SelectionLength = 0;
            }
        }

        private void textBoxResult_MouseMove(object sender, MouseEventArgs e)
        {
            if ((textBoxResult.Capture) && (textBoxResult.SelectionLength > 0))
            {
                textBoxSource.SelectionStart = textBoxResult.SelectionStart;
                textBoxSource.SelectionLength = textBoxResult.SelectionLength;
            }
        }

        private void textBoxSource_TextChanged(object sender, EventArgs e)
        {
            // Enable the "Convert" button only when source text is not empty 
            btnConvert.Enabled = textBoxSource.Text.Length > 0;
        }

        private void textBoxSourceFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    textBoxSource.Text = File.ReadAllText(textBoxSourceFile.Text);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private void comboBoxServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            DataItem di = (DataItem)comboBoxServices.Items[cb.SelectedIndex];
            _guidService = di.Guid;
        }

        private void textBoxSource_OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            //Look all the Type you want to control. I've put some here :
            //This type is when you click on the scrollbar buttons up/down.
            if (textBoxSource.Capture)
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                        SendMessage(textBoxResult.Handle, WM_VSCROLL, SB_LINEDOWN, 0);
                        break;
                    case ScrollEventType.SmallDecrement:
                        SendMessage(textBoxResult.Handle, WM_VSCROLL, SB_LINEUP, 0);
                        break;
                    case ScrollEventType.ThumbTrack:
                        SendMessage(textBoxResult.Handle, WM_VSCROLL, (SB_THUMBTRACK | (e.NewValue << 16)), 0);
                        break;
                }
            }
        }

        private void textBoxResult_OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            //Look all the Type you want to control. I've put some here :
            //This type is when you click on the scrollbar buttons up/down.
            if (textBoxResult.Capture)
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                        SendMessage(textBoxSource.Handle, WM_VSCROLL, SB_LINEDOWN, 0);
                        break;
                    case ScrollEventType.SmallDecrement:
                        SendMessage(textBoxSource.Handle, WM_VSCROLL, SB_LINEUP, 0);
                        break;
                    case ScrollEventType.ThumbTrack:
                        SendMessage(textBoxSource.Handle, WM_VSCROLL, (SB_THUMBTRACK | (e.NewValue << 16)), 0);
                        break;
                }
            }
        }

    }

    internal class DataItem : object
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }

}
