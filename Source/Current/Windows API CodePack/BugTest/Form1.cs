using Microsoft.WindowsAPICodePack.Shell;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShellObject Shell = ShellObject.FromParsingName("Gif.gif");

            ShellThumbnail Thumbnail = Shell.Thumbnail;
            Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;

            pictureBox1.Image = Thumbnail.ExtraLargeBitmap;
        }
    }
}
