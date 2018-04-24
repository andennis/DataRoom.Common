using System;
using System.IO;

namespace Common.Utils
{
    public class FileContentInfo : IDisposable, ICloneable
    {
        public string FileName { get; set; }
        public Stream ContentStream { get; set; }
        public string ContentType { get; set; }

        public void Dispose()
        {
            if (ContentStream != null)
            {
                ContentStream.Dispose();
                ContentStream = null;
            }
        }
        public object Clone()
        {
            return new FileContentInfo()
                   {
                       FileName = this.FileName,
                       ContentType = this.ContentType,
                       ContentStream = this.ContentStream
                   };
        }
    }
}
