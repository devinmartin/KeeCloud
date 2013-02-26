using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Net;

namespace KeeCloud.Providers.Amazon
{
    public class AmazonS3Provider : IProvider
    {
        public Uri Uri { get; set; }

        Stream IProvider.Get(ICredentials credentials)
        {
            using (var client = this.GetClient(credentials))
            {
                string bucket;
                string key;
                this.GetBucketAndKey(out bucket, out key);

                var request = new GetObjectRequest().WithBucketName(bucket).WithKey(key);

                var response = client.GetObject(request);

                MemoryStream returnStream = new MemoryStream();
                this.CopyStream(response.ResponseStream, returnStream);
                returnStream.Position = 0;
                return returnStream;
            }
        }

        void IProvider.Put(Stream stream, ICredentials credentials)
        {
            using (var client = this.GetClient(credentials))
            {
                string bucket;
                string key;
                this.GetBucketAndKey(out bucket, out key);

                var request = new PutObjectRequest().WithBucketName(bucket).WithKey(key);

                request.InputStream = stream;
                client.PutObject(request);
            }
        }

        public void Delete(ICredentials credentials)
        {
            using (var client = this.GetClient(credentials))
            {
                string bucket;
                string key;
                this.GetBucketAndKey(out bucket, out key);

                var request = new DeleteObjectRequest().WithBucketName(bucket).WithKey(key);

                client.DeleteObject(request);
            }
        }

        void IProvider.Move(Uri destination, ICredentials credentials)
        {
            using (var client = this.GetClient(credentials))
            {
                string bucket;
                string sourceKey;
                string destinationKey;
                this.GetBucketAndKey(out bucket, out sourceKey);
                AmazonS3Provider.GetBucketAndKey(destination, out bucket, out destinationKey);

                var request = new CopyObjectRequest().WithSourceBucket(bucket)
                    .WithDestinationBucket(bucket)
                    .WithSourceKey(sourceKey)
                    .WithDestinationKey(destinationKey);

                client.CopyObject(request);
                this.Delete(credentials);
            }
        }

        public bool CanConfigureCredentials { get { return false; } }

        public ICredentialConfigurationProvider CredentialConfigurationProvider
        {
            get { throw new NotSupportedException("This provider doesn't support credential configuration"); }
        }

        string IProvider.FriendlyName { get { return "Amazon S3"; } }

        private AmazonS3Client GetClient(ICredentials credentials)
        {
            var basicCredential = credentials.GetCredential(this.Uri, "basic");
            var client = new AmazonS3Client(new BasicAWSCredentials(basicCredential.UserName, basicCredential.Password));
            return client;
        }

        private static void GetBucketAndKey(Uri uri, out string bucket, out string key)
        {
            bucket = uri.OriginalString.Substring(uri.Scheme.Length + 3, uri.Authority.Length);
            key = uri.PathAndQuery.TrimStart('/');
        }

        private void GetBucketAndKey(out string bucket, out string key)
        {
            AmazonS3Provider.GetBucketAndKey(this.Uri, out bucket, out key);
        }

        private void CopyStream(Stream input, Stream output)
        {
            int read = 0;
            byte[] buffer = new byte[32000];
            do
            {
                read = input.Read(buffer, 0, buffer.Length);

                if (read > 0)
                {
                    output.Write(buffer, 0, read);
                    output.Flush();
                }
            } while (read > 0);
        }
    }
}
