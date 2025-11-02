using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace ScratchProject;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object? sender, EventArgs e)
    {
        CommonOpenFileDialog cofd = new CommonOpenFileDialog();

        cofd.Filters.Add(new CommonFileDialogFilter("xmltxt", ".xml.txt"));


        cofd.ShowDialog();
    }

    private void button2_Click(object? sender, EventArgs e)
    {
        using (var dlg = new CommonSaveFileDialog())
        {
            dlg.Filters.Add(new CommonFileDialogFilter("Word Document", "docx"));
            dlg.Filters.Add(new CommonFileDialogFilter("Adobe PDF", "pdf"));
            dlg.DefaultExtension = "docx";

            // when dialog opens, user enters "test.docx" and then changes file type to "pdf"
            // dialog correctly updates filename to "test.pdf"

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string? wrongFileName = dlg.FileName; // value is "test.docx", WRONG
            }
        }
    }

    private void button3_Click(object? sender, EventArgs e)
    {
        //TestShellThumbnailMarshaling();

        ShellObject? objectS = ShellObject.FromParsingName("GIF.gif");

        ShellThumbnail? thumbnail = objectS.Thumbnail;

        thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;

        pictureBox1.Image = thumbnail.LargeBitmap;
    }

    /// <summary>
    /// Test method to verify that the ShellThumbnail marshaling issue has been fixed.
    /// This method reproduces the original issue described in GitHub issue #30.
    /// </summary>
    private void TestShellThumbnailMarshaling()
    {
        try
        {
            // Test with a common folder path
            string testPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            MessageBox.Show($"Test path: {testPath}\nExists: {System.IO.Directory.Exists(testPath)}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Create a ShellObject from the path
            ShellObject shellObject = ShellObject.FromParsingName(testPath);
            
            // Get the thumbnail
            ShellThumbnail thumbnail = shellObject.Thumbnail;
            // Use Default format option which will fall back to icon if no thumbnail exists
            // This is appropriate since folders typically only have icons, not thumbnails
            thumbnail.FormatOption = ShellThumbnailFormatOption.Default;

            // Test various thumbnail operations that previously caused marshaling exceptions
            MessageBox.Show("Testing ExtraLargeBitmap...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var extraLargeBitmap = thumbnail.ExtraLargeBitmap;
            
            MessageBox.Show("Testing LargeBitmap...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var largeBitmap = thumbnail.LargeBitmap;
            
            MessageBox.Show("Testing MediumBitmap...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var mediumBitmap = thumbnail.MediumBitmap;
            
            MessageBox.Show("Testing SmallBitmap...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var smallBitmap = thumbnail.SmallBitmap;

            MessageBox.Show("Testing ExtraLargeBitmapSource...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var extraLargeBitmapSource = thumbnail.ExtraLargeBitmapSource;
            
            MessageBox.Show("Testing LargeBitmapSource...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var largeBitmapSource = thumbnail.LargeBitmapSource;
            
            MessageBox.Show("Testing MediumBitmapSource...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var mediumBitmapSource = thumbnail.MediumBitmapSource;
            
            MessageBox.Show("Testing SmallBitmapSource...", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var smallBitmapSource = thumbnail.SmallBitmapSource;

            // If we get here without exceptions, the marshaling fix worked!
            MessageBox.Show(
                "SUCCESS! All thumbnail operations completed without marshaling exceptions.\n" +
                "The fix for the DeleteObject marshaling issue is working correctly.",
                "Test Results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"FAILED: Marshaling exception still occurs:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                "Test Results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}