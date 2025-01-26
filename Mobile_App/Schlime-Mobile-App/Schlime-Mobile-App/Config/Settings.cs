using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Config
{
    public class Settings
    {
        public string FireBaseApiKey { get; set; }
        public string FireBaseAuthDomain { get; set; }
        
        public string EventHubCompatibleEndpoint { get; set; }

        public string EventHubName { get; set; }

        public string SharedAccessKey { get; set; }

        public string StorageConnectionString { get; set; }

        public string BlobContainerName { get; set; }
        public string IotHubServicePrimaryString { get; set; }

        public string GetEventHubConnectionString()
        {
            const string iotHubSharedAccessKeyName = "service";
            return $"Endpoint={EventHubCompatibleEndpoint};SharedAccessKeyName={iotHubSharedAccessKeyName};SharedAccessKey={SharedAccessKey}";
        }
    }
}
