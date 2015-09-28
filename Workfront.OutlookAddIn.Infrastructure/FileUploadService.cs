using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.Utilities;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public sealed class FileUploadService : IUploadService
    {
        public FileUploadService()
        {
        }

        public ConnectionSettings Settings { get; set; }

        public async Task<HttpResponseMessage> UploadFileAsync(string path, CancellationToken token, IProgress<double> progress)
        {
            int bufferSize = ResolveBufferSize(path);

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous))
            using (var stream = progress == null ? new ReportableStream(fileStream) : new ReportableStream(fileStream, progress))
            using (var streamContent = new StreamContent(stream, bufferSize))
            {
                content.Add(streamContent);

                var mediaType = IOUtil.GetMimeType(path);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                var disposition = CreateContentDispositionHeader(path);

                streamContent.Headers.ContentDisposition = disposition;

                client.DefaultRequestHeaders.UserAgent.ParseAdd(Settings.UserAgent);
                string uriString = string.Format(QueryString, Settings.Host, Settings.SessionId);

                var uri = new Uri(uriString);

                return await client.PostAsync(uri, content, token).ConfigureAwait(false);
            }
        }

        public async Task<HttpResponseMessage> UploadFileAsync(AttachmentData attachmentData, CancellationToken token, IProgress<double> progress)
        {
            int bufferSize = ResolveBufferSize(attachmentData.Stream.Length);

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            using (var stream = progress == null ? new ReportableStream(attachmentData.Stream) : new ReportableStream(attachmentData.Stream, progress))
            using (var streamContent = new StreamContent(stream, bufferSize))
            {
                content.Add(streamContent);

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(attachmentData.ContentType);
                var disposition = CreateContentDispositionHeader(attachmentData.FileName);

                streamContent.Headers.ContentDisposition = disposition;

                client.DefaultRequestHeaders.UserAgent.ParseAdd(Settings.UserAgent);
                string uriString = string.Format(QueryString, Settings.Host, Settings.SessionId);

                var uri = new Uri(uriString);

                return await client.PostAsync(uri, content, token).ConfigureAwait(false);
            }
        }

        public Task<HttpResponseMessage> UploadFileAsync(AttachmentData attachmentData, CancellationToken token)
        {
            return UploadFileAsync(attachmentData, token, null);
        }

        public Task<HttpResponseMessage> UploadFileAsync(AttachmentData attachmentData)
        {
            return UploadFileAsync(attachmentData, CancellationToken.None);
        }

        public Task<HttpResponseMessage> UploadFileAsync(string path, CancellationToken token)
        {
            return UploadFileAsync(path, token, null);
        }

        public Task<HttpResponseMessage> UploadFileAsync(string path)
        {
            return UploadFileAsync(path, CancellationToken.None);
        }

        private static int ResolveBufferSize(string path)
        {
            var fileInfo = new FileInfo(path);
            var length = fileInfo.Length;

            return ResolveBufferSize(length);
        }

        private static int ResolveBufferSize(long length)
        {
            if (length > 0x3FFFFFFF)
                return 0xFFFF;
            if (length > 0x0FFFFFFF)
                return 0x7FFF;
            if (length > 0x07FFFFFF)
                return 0x3FFF;
            if (length > 0x03FFFFFF)
                return 0x1FFF;
            return 0x0FFF;
        }

        private static ContentDispositionHeaderValue CreateContentDispositionHeader(string path)
        {
            var disposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"uploadedFile\"",
                FileName = "\"" + Path.GetFileName(path) + "\""
            };
            return disposition;
        }

        const string QueryString = "{0}/attask/api/upload?sessionID={1}";
    }
}