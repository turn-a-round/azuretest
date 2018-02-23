using Contoso.Events.Data;
using Contoso.Events.Models;
using Contoso.Events.ViewModels.Dynamic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Contoso.Events.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        { }

        
        public RegisterViewModel(string eventKey)
        {
            using (EventsContext context = new EventsContext())
            {
                this.Event = context.Events.SingleOrDefault(e => e.EventKey == eventKey);
            }

            string connectionString = ConfigurationManager.AppSettings["Microsoft.WindowsAzure.Storage.ConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("EventRegistrations");

            string partitionKey = String.Format("Stub_{0}", eventKey);
            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            TableQuery query = new TableQuery().Where(filter);
            DynamicTableEntity tableEntity = table.ExecuteQuery(query).SingleOrDefault();
            this.RegistrationStub = DynamicEntity.GenerateDynamicItem(tableEntity);

        }

        public Event Event { get; set; }

        public Registration RegistrationStub { get; set; }
    }
}