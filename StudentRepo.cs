using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storagesasdemo
{
    public class StudentRepo
    {
        private StorageService storageService;
        private CloudTable studentTable;

        public StudentRepo(StorageService azureStorageService)
        {
            storageService = azureStorageService;
            var tableClient = storageService.GetCloudTableClient().GetAwaiter().GetResult();
            studentTable = tableClient.GetTableReference("students");
            studentTable.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        private CloudTable GetSasTable()
        {
            var sasToken = studentTable.GetSharedAccessSignature(storageService.GetServiceSasTokenPolicy());
            var sasCredentials = new StorageCredentials(sasToken);
            return new CloudTable(studentTable.Uri, sasCredentials);
        }

        public async Task<IQueryable<Student>> GetAllstudents()
        {
            TableContinuationToken token = null;

            var entities = new List<Student>();
            do
            {
                var queryResult = await GetSasTable().ExecuteQuerySegmentedAsync(new TableQuery<Student>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities.AsQueryable();
        }

        public async Task InsertOrUpdateStudentAsync(Student student)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(student);
            await GetSasTable().ExecuteAsync(insertOrReplaceOperation);
        }
    }
}