using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace KeeCloud.Providers.Amazon
{
    public class AmazonS3Provider : IProvider
    {
        public Uri Uri { get; set; }

        Stream IProvider.Get(ICredentials credentials)
        {
            string bucket;
            string key;
            RegionEndpoint region;
            this.GetBucketAndKey(out bucket, out key, out region);

            using (var client = this.GetClient(credentials, region))
            {
                var request = new GetObjectRequest {
                    BucketName = bucket,
                    Key = key
                };

                var response = client.GetObject(request);

                MemoryStream returnStream = new MemoryStream();
                this.CopyStream(response.ResponseStream, returnStream);
                returnStream.Position = 0;
                return returnStream;
            }
        }

        void IProvider.Put(Stream stream, ICredentials credentials)
        {
            string bucket;
            string key;
            RegionEndpoint region;
            this.GetBucketAndKey(out bucket, out key, out region);

            using (var client = this.GetClient(credentials, region))
            {
                var request = new PutObjectRequest {
                    BucketName = bucket,
                    Key = key
                };

                request.InputStream = stream;
                client.PutObject(request);
            }
        }

        public void Delete(ICredentials credentials)
        {
            string bucket;
            string key;
            RegionEndpoint region;
            this.GetBucketAndKey(out bucket, out key, out region);

            using (var client = this.GetClient(credentials, region))
            {
                var request = new DeleteObjectRequest {
                    BucketName = bucket,
                    Key = key
                };

                client.DeleteObject(request);
            }
        }

        void IProvider.Move(Uri destination, ICredentials credentials)
        {
            string bucket;
            string sourceKey;
            RegionEndpoint region;
            this.GetBucketAndKey(out bucket, out sourceKey, out region);
            using (var client = this.GetClient(credentials, region))
            {
                string destinationKey;
                AmazonS3Provider.GetBucketAndKey(destination, out bucket, out destinationKey, out region);

                var request = new CopyObjectRequest {
                    SourceBucket = bucket,
                    DestinationBucket = bucket,
                    SourceKey = sourceKey,
                    DestinationKey = destinationKey
                };

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

        private AmazonS3Client GetClient(ICredentials credentials, RegionEndpoint region)
        {
            var basicCredential = credentials.GetCredential(this.Uri, "basic");
            var client = new AmazonS3Client(new BasicAWSCredentials(basicCredential.UserName, basicCredential.Password), region);
            return client;
        }

        private const string DEFAULT_S3_REGION_SYSTEM_NAME = "us-east-1";

        private static void GetBucketAndKey(Uri uri, out string bucket, out string key, out RegionEndpoint region)
        {
            bucket = uri.OriginalString.Substring(uri.Scheme.Length + 3, uri.Authority.Length);
            key = uri.AbsolutePath.TrimStart('/');
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var regionName = queryParams.Get("region");
            if (regionName == null)
            {
                region = RegionEndpoint.GetBySystemName(DEFAULT_S3_REGION_SYSTEM_NAME);
            }
            else
            {
                // KeePass appends ".tmp" to the uri during sync, remove it from the region and append to key
                if (regionName.EndsWith(".tmp")) 
                { 
                    regionName = regionName.Remove(regionName.Length - 4);
                    key = key + ".tmp";
                }
                region = RegionEndpoint.GetBySystemName(regionName);
            }
        }

        private void GetBucketAndKey(out string bucket, out string key, out RegionEndpoint region)
        {
            AmazonS3Provider.GetBucketAndKey(this.Uri, out bucket, out key, out region);
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
