namespace MedLinkApp.Helpers;

internal class MedLinkStatic
{
    internal static byte[] StreamTyByte(Stream input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
