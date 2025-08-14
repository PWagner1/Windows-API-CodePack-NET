using Microsoft.WindowsAPICodePack.Dialogs;

namespace ScratchProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();

            cofd.Filters.Add(new CommonFileDialogFilter("xmltxt", ".xml.txt"));


            cofd.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
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
                    string wrongFileName = dlg.FileName; // value is "test.docx", WRONG
                }
            }
        }
    }
}