using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace KeeCloud.Providers.Azure
{
    public class AzureBlobProvider : IProvider
    {
        public Uri Uri { get; set; }

        Stream IProvider.Get(ICredentials credentials)
        {
            string container;
            string identifier;
            GetContainerAndId(this.Uri, out container, out identifier);

            var client = this.GetClient(credentials);
            var containerReference = client.GetContainerReference(container);
            var blob = containerReference.GetBlockBlobReference(identifier);

            var returnStream = new MemoryStream();
            blob.DownloadToStream(returnStream);
            returnStream.Position = 0;
            return returnStream;
        }

        void IProvider.Put(Stream stream, ICredentials credentials)
        {
            string container;
            string identifier;
            GetContainerAndId(this.Uri, out container, out identifier);

            var client = this.GetClient(credentials);
            var containerReference = client.GetContainerReference(container);
            var blob = containerReference.GetBlockBlobReference(identifier);

            blob.UploadFromStream(stream);
        }

        public void Delete(ICredentials credentials)
        {
            string container;
            string identifier;
            GetContainerAndId(this.Uri, out container, out identifier);

            var client = this.GetClient(credentials);
            var containerReference = client.GetContainerReference(container);
            var blob = containerReference.GetBlockBlobReference(identifier);

            blob.Delete();
        }

        void IProvider.Move(Uri destination, ICredentials credentials)
        {
            string container;
            string identifier;
            GetContainerAndId(this.Uri, out container, out identifier);

            var client = this.GetClient(credentials);
            var containerReference = client.GetContainerReference(container);
            var blob = containerReference.GetBlockBlobReference(identifier);

            string destinationId;
            GetContainerAndId(destination, out container, out destinationId);
            var destinationBlob = containerReference.GetBlockBlobReference(destinationId);

            var asyncResult = destinationBlob.BeginStartCopyFromBlob(blob, null, null);
            destinationBlob.EndStartCopyFromBlob(asyncResult);
            blob.Delete();
        }

        public bool CanConfigureCredentials { get { return false; } }

        public ICredentialConfigurationProvider CredentialConfigurationProvider
        {
            get { throw new NotSupportedException("This provider doesn't support credential configuration"); }
        }

        string IProvider.FriendlyName { get { return "Azure Blob Storage"; } }

        private CloudBlobClient GetClient(ICredentials credentials)
        {
            var basicCredential = credentials.GetCredential(this.Uri, "basic");
            var connection = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", basicCredential.UserName, basicCredential.Password);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);
            return storageAccount.CreateCloudBlobClient();
        }

        private static void GetContainerAndId(Uri uri, out string container, out string identifier)
        {
            container = uri.OriginalString.Substring(uri.Scheme.Length + 3, uri.Authority.Length);
            identifier = uri.AbsolutePath.TrimStart('/');
        }
    }
}
