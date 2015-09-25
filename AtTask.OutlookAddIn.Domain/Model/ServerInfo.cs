using Newtonsoft.Json;
namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ServerInfo
    {
        public string[] Versions { get; set; }
                
        public ServerInfoSso Sso { get; set; }

    }
}