using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace storagesasdemo
{
    public class StorageService
    {
        string connectionString;
        CloudStorageAccount storageAccount;

        public StorageService(string conString)
        {
            connectionString = conString;
        }

        public async Task<CloudTableClient> GetCloudTableClient()
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tableServiceProperties = await tableClient.GetServicePropertiesAsync();

            ConfigureCors(tableServiceProperties);
            
            await tableClient.SetServicePropertiesAsync(tableServiceProperties);
            return tableClient;
        }

        private void ConfigureCors(ServiceProperties serviceProperties)
        {
            serviceProperties.Cors = new CorsProperties();
            serviceProperties.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedHeaders = new List<string>() { "*" },
                AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head,
                AllowedOrigins = new List<string>() {
                      "http://localhost:5000",
                      "https://localhost:1659",
                      "https://www.azurenotes.tech/",
                    },
                ExposedHeaders = new List<string> { "*" },
                MaxAgeInSeconds = 1800
            });
        }

        public SharedAccessTablePolicy GetServiceSasTokenPolicy()
        {
            return new SharedAccessTablePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
                Permissions = SharedAccessTablePermissions.Add
                            | SharedAccessTablePermissions.Query
                            | SharedAccessTablePermissions.Update
                            | SharedAccessTablePermissions.Delete
            };
        }
    }
}