namespace AtTask.OutlookAddIn.StreamApi.Connector.Service
{
    /// <summary>
    /// A helper utility class for Name-Value string pair used in IStreamApiConnector methods for criteria list.
    /// </summary>
    public class StringPair
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public StringPair(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}