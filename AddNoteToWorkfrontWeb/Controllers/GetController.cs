using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using TPL = System.Threading.Tasks;
using System.Web.Http;
using AddNoteToWorkfrontWeb.Extensions;
using AddNoteToWorkfrontWeb.Utils;
using AtTask.OutlookAddIn.Assets;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.StreamApi;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Workfront.OutlookAddIn.Infrastructure;
using Task = AtTask.OutlookAddIn.Domain.Model.Task;

namespace AddNoteToWorkfrontWeb.Controllers
{
    public class GetController : ApiController
    {
        private async TPL.Task<IStreamApiConnector> GetApiConnectorAsync()
        {
            var sessionId = Request.GetCookie("workfront-session");
            var host = Request.GetCookie("workfront-host");
            var connector = StreamApiConnectorFactory.NewInstance();
            connector.Init(host, Settings.Default.StreamAPIVersion, StreamApiEndpoint.InternalApi);
            connector.UserAgent = new UserAgent();
            await connector.LoginAsync(sessionId);

            return connector;
        }

        //GET: api/Get/5
        public async TPL.Task<IEnumerable<Work>> MyWork()
        {
            try
            {
                var connector = await GetApiConnectorAsync();

                var criteria = new List<StringPair>
                {
                    new StringPair(CriteriaLimit, "10")
                };

                return await connector.SearchAsync<Work>(criteria, ApiFieldsWorkMyWorkAndRequests, CancellationToken.None, "myWork");
            }
            catch (Exception e)
            {
                var resEx = e;
                var status = HttpStatusCode.BadRequest;
                var ex = e as StreamApiException;
                if (ex != null)
                {
                    status = HttpStatusCode.BadRequest;
                    resEx = ex;
                    var wex = ex.WebException;
                    if (wex != null)
                    {
                        if (wex.Status == WebExceptionStatus.ProtocolError)
                        {
                            var response = wex.Response as HttpWebResponse;
                            if (response != null)
                            {
                                status = response.StatusCode;
                                resEx = wex;
                            }
                        }
                    }
                }

                throw new HttpResponseException(Request.CreateErrorResponse(status, resEx));
            }
        }

        private EntityBase EntityBaseFactory(string id, string objCode)
        {
            switch (objCode)
            {
                case Project.ObjCodeString:
                    return new Project { ID = id, ObjCode = objCode };
                case Task.ObjCodeString:
                    return new Task { ID = id, ObjCode = objCode };
                case OpTask.ObjCodeString:
                    return new OpTask { ID = id, ObjCode = objCode };
                default:
                    throw new NotSupportedException();
            }
        }

        public async TPL.Task<EntityBase> UpdateEntity([FromBody]UpdateData updateData)
        {
            var note = updateData.Note;
            var updateEntity = EntityBaseFactory(note.ObjID, note.NoteObjCode);

            var connector = await GetApiConnectorAsync();

            try
            {
                var newNote = await connector.CreateNoteAsync(updateData.Note, CancellationToken.None);

                var handles = await AttachmentHandles(updateData.EwsCredentials, updateData.Attachments);

                await connector.UploadDocsAsync(updateEntity, handles, CancellationToken.None);

                return (EntityBase)newNote;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async TPL.Task<IEnumerable<NamedEntityBase>> ContainsSearch([FromBody]string searchText)
        {
            var token = new CancellationToken();
            var connector = await GetApiConnectorAsync();

            var projectRecentsTask = GetProjectRecentsAsync(connector, searchText, token);
            var taskRecentsTask = GetTaskRecentsAsync(connector, searchText, token);
            var issueRecentsTask = GetIssueRecentsAsync(connector, searchText, token);

            var projectsTask = GetProjectsAsync(connector, searchText, 5, token);
            var tasksTask = GetTasksAsync(connector, searchText, 5, token);
            var issuesTask = GetIssuesAsync(connector, searchText, 5, token);

            var recentProjects = await projectRecentsTask.ConfigureAwait(false);
            var recentTasks = await taskRecentsTask.ConfigureAwait(false);
            var recentIssues = await issueRecentsTask.ConfigureAwait(false);

            var projects = await projectsTask.ConfigureAwait(false);
            var tasks = await tasksTask.ConfigureAwait(false);
            var issues = await issuesTask.ConfigureAwait(false);

            var updatableProjectViewModels =
                recentProjects.Where(p => p != null)
                    .Select(UpdatableEntitiyViewModelFactory)
                    .Union(projects.Where(p => p != null))
                    .Take(5);

            var updatableTaskViewModels =
                recentTasks.Where(t => t != null)
                    .Select(UpdatableEntitiyViewModelFactory)
                    .Union(tasks.Where(t => t != null))
                    .Take(5);

            var updatableIssueViewModels =
                recentIssues.Where(i => i != null)
                    .Select(UpdatableEntitiyViewModelFactory)
                    .Union(issues.Where(i => i != null))
                    .Take(5);

            return updatableProjectViewModels.Union(updatableTaskViewModels.Union(updatableIssueViewModels));
        }

        private FileUploadService _fileUploadService;

        private FileUploadService FileUploadService
        {
            get
            {
                return _fileUploadService ?? (_fileUploadService = new FileUploadService
                {
                    Settings = new ConnectionSettings()
                    {
                        Host = Request.GetCookie("workfront-host"),
                        SessionId = Request.GetCookie("workfront-session"),
                        UserAgent = new UserAgent().ToString()
                    }
                });
            }
        }

        private async TPL.Task<List<FileHandle>> AttachmentHandles(EwsCredentials credentials, List<AttachmentInfo> attachmentInfos)
        {
            var service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            service.Url = new Uri(credentials.EwsUrl);
            service.Credentials = new OAuthCredentials(credentials.AttachmentToken);
            var itemId = new ItemId(credentials.EwsId);
            var tasks = GetAttachmentsFromEmail(service, itemId)
                .Where(s => attachmentInfos.Any(a => a.Id == s.Id))
                .Select(UplaodAttachmentAsync);

            if (attachmentInfos.Any(i => i.Id == Guid.Empty.ToString("D")))
            {
                var fileHandle = UploadEmlAsync(service, itemId);
                tasks = tasks.Union(new []{fileHandle});
            }

            var handles = await TPL.Task.WhenAll(tasks);
            return handles.ToList();
        }

        private async TPL.Task<FileHandle> UploadEmlAsync(ExchangeService service, ItemId itemId)
        {
            var mimeContent = GetMimeContent(service, itemId);
            using (var memoryStream = new MemoryStream(mimeContent.Content))
            {
                var attachmentData = new AttachmentData
                {
                    Id = Guid.Empty.ToString("D"),
                    Stream = memoryStream,
                    FileName = "current.eml",
                    ContentType = "message/rfc822"
                };

                var uploadFileAsync = await FileUploadService.UploadFileAsync(attachmentData);
                var handle = await GetHandleFromResponse(uploadFileAsync);
                return new FileHandle { Handle = handle, FileName = attachmentData.FileName };
            }
        }

        private async TPL.Task<FileHandle> UplaodAttachmentAsync(FileAttachment fileAttachment)
        {
            using (var stream = new MemoryStream())
            {
                //Load the attachment into a file.
                //This call results in a GetAttachment call to EWS.
                fileAttachment.Load(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var attachmentData = new AttachmentData
                {
                    Id = fileAttachment.Id,
                    Stream = stream,
                    FileName = fileAttachment.FileName ?? fileAttachment.Name,
                    ContentType = fileAttachment.ContentType
                };
                var uploadFileAsync = await FileUploadService.UploadFileAsync(attachmentData);
                var handle = await GetHandleFromResponse(uploadFileAsync);
                return new FileHandle { Handle = handle, FileName = attachmentData.FileName };
            }
        }

        private static async TPL.Task<string> GetHandleFromResponse(HttpResponseMessage message)
        {
            string responseMessage = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return GetHandle(responseMessage);
            }

            throw new Exception(responseMessage);
        }

        private static string GetHandle(string responseMessage)
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseMessage);
            return (string)jsonObject["data"]["handle"];
        }

        public MimeContent GetMimeContent(ExchangeService service, ItemId itemId)
        {
            EmailMessage message = EmailMessage.Bind(service, itemId);

            // Eetrieve the mime content and attachments collection.
            // This method results in an GetItem call to EWS.
            message.Load(new PropertySet(ItemSchema.MimeContent));

            return message.MimeContent;
        }

        public static IEnumerable<FileAttachment> GetAttachmentsFromEmail(ExchangeService service, ItemId itemId)
        {
            // Bind to an existing message item

            EmailMessage message = EmailMessage.Bind(service, itemId);

            // Eetrieve the mime content and attachments collection.
            // This method results in an GetItem call to EWS.
            message.Load(new PropertySet(ItemSchema.Attachments));

            // Bind to an existing message item and retrieve the attachments collection.
            // This method results in an GetItem call to EWS.


            // Iterate through the attachments collection and load each attachment.
            foreach (Attachment attachment in message.Attachments)
            {
                if (attachment is FileAttachment)
                {
                    yield return attachment as FileAttachment;

                    //var stream = new MemoryStream();
                    // Load the attachment into a file.
                    // This call results in a GetAttachment call to EWS.
                    //fileAttachment.Load(stream);
                    //stream.Seek(0, SeekOrigin.Begin);
                    //yield return
                    //    new AttachmentData
                    //    {
                    //        Id = fileAttachment.Id,
                    //        Stream = stream,
                    //        FileName = fileAttachment.FileName ?? fileAttachment.Name,
                    //        ContentType = fileAttachment.ContentType
                    //    };

                    //var mimeContent = message.MimeContent;

                    //yield return new AttachmentData
                    //{
                    //    Id = Guid.Empty.ToString("D"),
                    //    Stream = new MemoryStream(mimeContent.Content),
                    //    FileName = "current.eml",
                    //    ContentType = "message/rfc822"
                    //};

                }
                //else // Attachment is an item attachment.
                //{
                //    ItemAttachment itemAttachment = attachment as ItemAttachment;

                //    // Load attachment into memory and write out the subject.
                //    // This does not save the file like it does with a file attachment.
                //    // This call results in a GetAttachment call to EWS.
                //    itemAttachment.Load();

                //    Console.WriteLine("Item attachment name: " + itemAttachment.Name);
                //}
            }
        }


        //public IEnumerable<string> AttachmentHandles([FromBody] EwsCredentials credentials)
        //{
        //    AutodiscoverService adAutoDiscoverService = new AutodiscoverService(new Uri(credentials.EwsUrl).Host);
        //    adAutoDiscoverService.Credentials = new OAuthCredentials(credentials.UserIdentityToken);
        //    adAutoDiscoverService.RedirectionUrlValidationCallback = url => false;
        //    try
        //    {
        //        GetUserSettingsResponse adResponse = GetUserSettings(adAutoDiscoverService, "hamlethakobyan@3hcompany.onmicrosoft.com", 4, UserSettingName.EwsSupportedSchemas);
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return new List<string> { "sdfsdf" };
        //}

        //public static GetUserSettingsResponse GetUserSettings(
        //    AutodiscoverService service,
        //    string emailAddress,
        //    int maxHops,
        //    params UserSettingName[] settings)
        //{
        //    Uri url = null;
        //    GetUserSettingsResponse response = null;

        //    for (int attempt = 0; attempt < maxHops; attempt++)
        //    {
        //        service.Url = url;
        //        service.EnableScpLookup = (attempt < 2);

        //        response = service.GetUserSettings(emailAddress, settings);

        //        if (response.ErrorCode == AutodiscoverErrorCode.RedirectAddress)
        //        {
        //            url = new Uri(response.RedirectTarget);
        //        }
        //        else if (response.ErrorCode == AutodiscoverErrorCode.RedirectUrl)
        //        {
        //            url = new Uri(response.RedirectTarget);
        //        }
        //        else
        //        {
        //            return response;
        //        }
        //    }

        //    throw new Exception("No suitable Autodiscover endpoint was found.");
        //}

        private NamedEntityBase UpdatableEntitiyViewModelFactory(Recent entity)
        {
            switch (entity.ObjObjCode)
            {
                case Project.ObjCodeString:
                    return new Project { ID = entity.ObjID, Name = entity.Name, ObjCode = entity.ObjObjCode };
                case Task.ObjCodeString:
                    return new Task { ID = entity.ObjID, Name = entity.Name, ObjCode = entity.ObjObjCode };
                case OpTask.ObjCodeString:
                    return new OpTask { ID = entity.ObjID, Name = entity.Name, ObjCode = entity.ObjObjCode };
                default:
                    throw new NotSupportedException();
            }
        }

        private async TPL.Task<List<Recent>> GetProjectRecentsAsync(IStreamApiConnector streamApiConnector, string search, CancellationToken token)
        {
            var recentCriteria = CreateCiLikeCriteria(search, StreamConstants.ParamName);
            recentCriteria.Add(new StringPair(StreamConstants.ParamLastViewedDate + StreamConstants.Sort, StreamConstants.SortOrderDesc));
            recentCriteria.Add(new StringPair(StreamConstants.CriteriaLimit, "5"));
            recentCriteria.Add(new StringPair(StreamConstants.ParamUserID, StreamConstants.UserId));
            recentCriteria.Add(new StringPair(StreamConstants.ParamObjObjCode, "PROJ"));

            try
            {
                return await streamApiConnector.SearchAsync<Recent>(recentCriteria, StreamConstants.ApiFieldsRecent, token).ConfigureAwait(false);
            }
            catch (StreamApiException e)
            {
                throw new AtTaskException(e);
            }
        }

        private async TPL.Task<List<Recent>> GetTaskRecentsAsync(IStreamApiConnector streamApiConnector, string search, CancellationToken token)
        {
            var recentCriteria = CreateCiLikeCriteria(search, StreamConstants.ParamName);
            recentCriteria.Add(new StringPair(StreamConstants.ParamLastViewedDate + StreamConstants.Sort, StreamConstants.SortOrderDesc));
            recentCriteria.Add(new StringPair(StreamConstants.CriteriaLimit, "5"));
            recentCriteria.Add(new StringPair(StreamConstants.ParamUserID, StreamConstants.UserId));
            recentCriteria.Add(new StringPair(StreamConstants.ParamObjObjCode, "TASK"));

            try
            {
                return await streamApiConnector.SearchAsync<Recent>(recentCriteria, StreamConstants.ApiFieldsRecent, token).ConfigureAwait(false);
            }
            catch (StreamApiException e)
            {
                throw new AtTaskException(e);
            }
        }

        private async TPL.Task<List<Recent>> GetIssueRecentsAsync(IStreamApiConnector streamApiConnector, string search, CancellationToken token)
        {
            var recentCriteria = CreateCiLikeCriteria(search, StreamConstants.ParamName);
            recentCriteria.Add(new StringPair(StreamConstants.ParamLastViewedDate + StreamConstants.Sort, StreamConstants.SortOrderDesc));
            recentCriteria.Add(new StringPair(StreamConstants.CriteriaLimit, "5"));
            recentCriteria.Add(new StringPair(StreamConstants.ParamUserID, StreamConstants.UserId));
            recentCriteria.Add(new StringPair(StreamConstants.ParamObjObjCode, "OPTASK"));

            try
            {
                return await streamApiConnector.SearchAsync<Recent>(recentCriteria, StreamConstants.ApiFieldsRecent, token).ConfigureAwait(false);
            }
            catch (StreamApiException e)
            {
                throw new AtTaskException(e);
            }
        }

        private async TPL.Task<List<Project>> GetProjectsAsync(IStreamApiConnector streamApiConnector, string search, int limit, CancellationToken token)
        {
            var searchBuilder = new SolrSearchBuilder<Project>
            {
                PageSize = limit,
                PageNumber = 0
            };

            string filterByRefNum = string.Empty;

            ulong dummy;
            if (ulong.TryParse(search, out dummy))
            {
                filterByRefNum = search;
            }

            searchBuilder.QueryString("")
                .WithFilter(StreamConstants.NameFullSearch, search, "*")
                .WithFilter(StreamConstants.ParamRefNumberSolr, filterByRefNum)
                .WithFields(t => t.ID)
                .WithFields(t => t.Name);

            try
            {
                return await streamApiConnector.SolrSearchAsync(searchBuilder, token).ConfigureAwait(false);
            }
            catch (StreamApiException e)
            {
                throw new AtTaskException(e);
            }
        }

        private async TPL.Task<List<Task>> GetTasksAsync(IStreamApiConnector streamApiConnector, string search, int limit, CancellationToken token)
        {
            var searchBuilder = new SolrSearchBuilder<Task>
            {
                PageSize = limit,
                PageNumber = 0
            };

            string filterByRefNum = string.Empty;

            ulong dummy;
            if (ulong.TryParse(search, out dummy))
            {
                filterByRefNum = search;
            }

            searchBuilder.QueryString("")
                .WithFilter(StreamConstants.NameFullSearch, search, "*")
                .WithFilter(StreamConstants.ParamRefNumberSolr, filterByRefNum)
                .WithFields(t => t.ID)
                .WithFields(t => t.Name);

            try
            {
                return await streamApiConnector.SolrSearchAsync(searchBuilder, token).ConfigureAwait(false);
            }
            catch (StreamApiException e)
            {
                throw new AtTaskException(e);
            }
        }

        private async TPL.Task<List<OpTask>> GetIssuesAsync(IStreamApiConnector streamApiConnector, string search, int limit, CancellationToken token)
        {
            var searchBuilder = new SolrSearchBuilder<OpTask>
            {
                PageSize = limit,
                PageNumber = 0
            };

            string filterByRefNum = string.Empty;

            ulong dummy;
            if (ulong.TryParse(search, out dummy))
            {
                filterByRefNum = search;
            }

            searchBuilder.QueryString("")
                .WithFilter(StreamConstants.NameFullSearch, search, "*")
                .WithFilter(StreamConstants.ParamRefNumberSolr, filterByRefNum)
                .WithFields(t => t.ID)
                .WithFields(t => t.Name);

            try
            {
                return await streamApiConnector.SolrSearchAsync(searchBuilder, token).ConfigureAwait(false);
            }
            catch (StreamApiException e)
            {
                throw new AtTaskException(e);
            }
        }

        private static List<StringPair> CreateCiLikeCriteria(string search, string field)
        {
            var criteria = new List<StringPair>();
            if (string.IsNullOrWhiteSpace(search))
            {
                return criteria;
            }

            criteria.Add(new StringPair(field, string.Format("%{0}%", search)));
            criteria.Add(new StringPair(field + StreamConstants.Mod, StreamConstants.ModCILike));

            return criteria;
        }

        private static readonly string[] ApiFieldsWorkMyWorkAndRequests =
        {
            "ID", "name", "description", "plannedStartDate", "dueDate", "actualCompletionDate","commitDate", "plannedCompletionDate"
        };

        private const string CriteriaLimit = "$$LIMIT";
    }

    public class UpdateData
    {
        public Note Note { get; set; }
        public List<AttachmentInfo> Attachments { get; set; }
        public EwsCredentials EwsCredentials { get; set; }
    }

    public class AttachmentInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public string ContentType { get; set; }
        public bool IsInline { get; set; }
    }

    public enum AttachmentType
    {
        file,
        item
    }


    public class FileHandle
    {
        public string Handle { get; set; }
        public string FileName { get; set; }
    }

    public class EwsCredentials
    {
        public string AttachmentToken { get; set; }
        public string EwsId { get; set; }
        public string EwsUrl { get; set; }
    }
}
