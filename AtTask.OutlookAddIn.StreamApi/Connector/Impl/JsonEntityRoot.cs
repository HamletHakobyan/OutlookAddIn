using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class JsonEntityRoot<T>
    {
        public JsonEntityRoot()
        {
        }

        public T Data { get; set; }

        public StreamApiError Error { get; set; }
    }
}