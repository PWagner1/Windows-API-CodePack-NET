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
    }
}