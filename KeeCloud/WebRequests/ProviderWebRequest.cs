using KeeCloud.Providers;
using KeePassLib.Serialization;
using System;
using System.IO;
using System.Net;

namespace KeeCloud.WebRequests
{
    public class ProviderWebRequest : WebRequest
    {
        private readonly Uri requestUri;
        private readonly IProvider provider;

        RequestStream requestStream = new RequestStream();

        public ProviderWebRequest(Uri uri, IProvider provider)
        {
            this.Headers = new WebHeaderCollection();

            this.requestUri = uri;
            this.provider = provider;
        }

        public override Uri RequestUri
        {
            get { return this.requestUri; }
        }

        public override string Method { get; set; }

        public override WebHeaderCollection Headers { get; set; }

        public override long ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override ICredentials Credentials { get; set; }

        public override bool PreAuthenticate { get; set; }

        public override Stream GetRequestStream()
        {
            return this.requestStream;
        }

        public override WebResponse GetResponse()
        {
            if (this.Method == IOConnection.WrmDeleteFile)
            {
                this.provider.Delete(this.Credentials);
                return new SuccessWebResponse();
            }
            else if (this.Method == IOConnection.WrmMoveFile)
            {
                var destination = Headers[IOConnection.WrhMoveFileTo];
                this.provider.Move(new Uri(destination), this.Credentials);
                return new SuccessWebResponse();
            }
            else if ((this.Method ?? string.Empty).ToLowerInvariant() == "post")
            {
                this.provider.Put(this.requestStream.GetReadableStream(), this.Credentials);
                return new SuccessWebResponse();
            }
            else
            {
                Stream stream = this.provider.Get(this.Credentials);
                return new GetWebResponse(stream);
            }
        }
    }
}
