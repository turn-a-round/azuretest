using Contoso.Events.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Contoso.Events.ViewModels
{
    public class DownloadViewModel
    {
        private readonly CloudStorageAccount _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["Microsoft.WindowsAzure.Storage.ConnectionString"]);
        private readonly string _blobId;

        public DownloadViewModel(string blobId)
        {
            _blobId = blobId;
        }

        
        public async Task<DownloadPayload> GetStream()
        {
            //return await Task.FromResult<DownloadPayload>(null);
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("signin");
            container.CreateIfNotExists();

            CloudBlockBlob blob = container.GetBlockBlobReference(_blobId);
            Stream blobStream = await blob.OpenReadAsync();

            return new DownloadPayload { Stream = blobStream, ContentType = blob.Properties.ContentType };
        }

        
        public async Task<string> GetSecureUrl()
        {
            return await Task.FromResult<string>(String.Empty);
        }
    }
}