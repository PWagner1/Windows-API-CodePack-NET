using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.WindowsAPICodePack.ShellExtensions;

using ShellObject = Microsoft.WindowsAPICodePack.Shell.ShellObject;

namespace HandlerSamples
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("HandlerSamples.XYZThumbnailer2")]
    [Guid("AB21A65F-E3E1-4D08-9422-29C7AB1BE0A5")]
    [ThumbnailProvider("XYZThumbnailer2", ".xyz2", ThumbnailAdornment = ThumbnailAdornment.PhotoBorder, DisableProcessIsolation = true)]
    public class ThumbnailProviderDemo2 : ThumbnailProvider, IThumbnailFromFile, IThumbnailFromShellObject
    {
        #region IThumbnailFromFile Members

        public Bitmap ConstructBitmap(FileInfo info, int sideSize)
        {
            using (var fs = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
            {
                XyzFileDefinition file = new XyzFileDefinition(fs);

                using (var imageStream = new MemoryStream(Convert.FromBase64String(file.EncodedImage)))
                {
                    return new Bitmap(imageStream);
                }
            }
        }

        #endregion

        #region IThumbnailFromShellObject Members

        public Bitmap ConstructBitmap(ShellObject shellObject, int sideSize)
        {
            using (FileStream stream = new FileStream(shellObject.ParsingName, FileMode.Open, FileAccess.Read))
            {
                XyzFileDefinition file = new XyzFileDefinition(stream);

                using (MemoryStream imageStream = new MemoryStream(Convert.FromBase64String(file.EncodedImage)))
                {
                    return (Bitmap)Image.FromStream(imageStream);
                }
            }
        }

        #endregion
    }
}
