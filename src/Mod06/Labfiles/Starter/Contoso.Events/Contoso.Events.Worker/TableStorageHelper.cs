using Contoso.Events.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contoso.Events.Worker
{
    public sealed class TableStorageHelper : StorageHelper
    {
        private readonly CloudTableClient _tableClient;

        public TableStorageHelper()
            : base()
        {
            _tableClient = base.StorageAccount.CreateCloudTableClient();
        }

        
        public IEnumerable<string> GetRegistrantNames(string eventKey)
        {
            //return Enumerable.Empty<string>();
            CloudTable table = this._tableClient.GetTableReference("EventRegistrations");

            string partitionKey = eventKey;
            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            TableQuery<Registration> query = new TableQuery<Registration>().Where(filter);

            return table.ExecuteQuery<Registration>(query).OrderBy(r => r.LastName).ThenBy(r => r.FirstName).Select(r=> string.Format("{0}, {1}", r.LastName, r.FirstName));
        }
    }
}