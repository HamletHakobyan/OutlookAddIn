using System;
using System.IO;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public class AttachmentData
    {
        public string Id { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}