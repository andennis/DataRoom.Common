using System.IO;

namespace Common.Extensions
{
    public static class FileExtensions
    {
        public static void SaveToFile(this Stream stream, string fileName)
        {
            using (Stream fs = new FileStream(fileName, FileMode.Create))
            {
                stream.CopyTo(fs);
            }
        }
    }
}
