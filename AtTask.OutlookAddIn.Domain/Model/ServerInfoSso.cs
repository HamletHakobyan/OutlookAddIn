namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum SAMLType
    {
        NONE,
        SAML1,
        SAML2
    }

    public class ServerInfoSso
    {
        public int? ProviderPort { get; set; }

        public string SignoutURL { get; set; }

        public string SsoType { get; set; }

        public SAMLType SAMLType
        {
            get
            {
                switch (SsoType)
                {
                    case "saml1":
                        return Model.SAMLType.SAML1;
                    case "saml2":
                        return Model.SAMLType.SAML2;
                    default:
                        return Model.SAMLType.NONE;
                }
            }
        }
        
        public string ProviderURL { get; set; }
    }
}