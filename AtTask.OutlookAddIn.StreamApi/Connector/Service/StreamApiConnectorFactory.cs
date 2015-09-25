using AtTask.OutlookAddIn.StreamApi.Connector.Impl;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Service
{
    /// <summary>
    /// Represents the factory of IStreamApiConnector implementations.
    /// By default uses StreamApiConnector class as an implementation.
    /// </summary>
    public class StreamApiConnectorFactory
    {
        //TODO: Hrant: Add method for creating instances of other implementations

        public static IStreamApiConnector NewInstance()
        {
            return new StreamApiConnector();
        }
    }
}