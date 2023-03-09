using System.Drawing;
using DevExpress.Persistent.BaseImpl;

namespace Domain.Utils;

public static class FileDataHelper
{
    private const int LängsteSeite = 100;

    public static Image ErstelleBildAusDatei(FileData datei)
    {
        if (datei == null || datei.IsEmpty || datei.Content == null)
            return null;

        var memoryStream = new MemoryStream();
        datei.SaveToStream(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var retVal = Image.FromStream(memoryStream);

        return retVal;
    }

    public static Image ErstelleThumbnailAusBild(Lazy<Image> bild)
    {
        if (bild?.Value == null)
            return null;

        var breite = bild.Value.Width >= bild.Value.Height ? LängsteSeite : (int)(LängsteSeite * (bild.Value.Width / (double)bild.Value.Height));
        var höhe   = bild.Value.Height >= bild.Value.Width ? LängsteSeite : (int)(LängsteSeite * (bild.Value.Height / (double)bild.Value.Width));

        return bild.Value.GetThumbnailImage(breite, höhe, () => false, IntPtr.Zero);
    }
}