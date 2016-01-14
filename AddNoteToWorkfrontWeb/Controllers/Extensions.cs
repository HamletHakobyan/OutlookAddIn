using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.StreamApi;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using Workfront.OutlookAddIn.Infrastructure;

namespace AddNoteToWorkfrontWeb.Controllers
{
    internal static class Extensions
    {
        public static async System.Threading.Tasks.Task<Note> CreateNoteAsync(this IStreamApiConnector streamApiConnector, Note note, CancellationToken token)
        {
            try
            {
                return await streamApiConnector.CreateAsync(note, null, token);
            }
            catch (StreamApiException ex)
            {
                throw new AtTaskException(ex);
            }
        }

        public static async System.Threading.Tasks.Task UploadDocsAsync<T>(this IStreamApiConnector connector, T entity, IEnumerable<FileHandle> fileHandles, CancellationToken token) where T : EntityBase
        {
            var tasks = new List<Task<Document>>();
            foreach (var fileHande in fileHandles)
            {
                Document document = PrepareDocument(entity, fileHande.Handle, fileHande.FileName);
                document.ObjID = entity.ID;
                document.DocObjCode = entity.ObjCode;
                tasks.Add(connector.CreateAsync(document, null, token));
            }

            await System.Threading.Tasks.Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private static Document PrepareDocument(EntityBase entity, string handle, string fileName)
        {
            Document document = new Document();
            document.Name = fileName;
            document.Handle = handle;
            document.DocObjCode = entity.ObjCode;
            document.ObjID = entity.ID;

            DocumentVersion docVersion = new DocumentVersion();
            docVersion.Version = "1";
            docVersion.FileName = fileName;
            document.CurrentVersion = docVersion;

            return document;
        }
    }
}