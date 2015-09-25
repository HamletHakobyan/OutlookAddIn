using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public interface IUploadService
    {
        Task<HttpResponseMessage> UploadFileAsync(string path, CancellationToken token, IProgress<double> progress);
        Task<HttpResponseMessage> UploadFileAsync(string path, CancellationToken token);
        Task<HttpResponseMessage> UploadFileAsync(string path);
    }
}