using System.IO;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public class AttachmentData
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}