using KeeCloud.Providers;
using KeeCloud.Utilities;
using KeePass.Plugins;
using KeePassLib.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace KeeCloud.WebRequests
{
    /// <summary>
    /// Custom web request that uses an IProvider to perform its web operations
    /// </summary>
    public class ProviderWebRequest : WebRequest
    {
        private readonly Uri requestUri;
        private readonly IProvider provider;
        private IPluginHost host;

        RequestStream requestStream;

        public ProviderWebRequest(Uri uri, IProvider provider, IPluginHost host)
        {
            this.Headers = new WebHeaderCollection();

            this.requestStream = new RequestStream(this);
            this.requestUri = uri;
            this.provider = provider;
            this.host = host;
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

        public override string ConnectionGroupName { get; set; }

        public override int Timeout { get; set; }

        public override IWebProxy Proxy { get; set; }

        public override Stream GetRequestStream()
        {
            return this.requestStream;
        }

        public override WebResponse GetResponse()
        {
            if (this.Method == IOConnection.WrmDeleteFile)
            {
                this.provider.Delete(this.GetCredentials());
                return new SuccessWebResponse();
            }
            else if (this.Method == IOConnection.WrmMoveFile)
            {
                var destination = Headers[IOConnection.WrhMoveFileTo];
                this.provider.Move(new Uri(destination), this.GetCredentials());
                return new SuccessWebResponse();
            }
            else if ((this.Method ?? string.Empty).ToLowerInvariant() == "post")
            {
                using (var stream = this.requestStream.GetReadableStream())
                {
                    this.provider.Put(stream, this.GetCredentials());
                    return new SuccessWebResponse();
                }
            }
            else
            {
                Stream stream = this.provider.Get(this.GetCredentials());
                return new GetWebResponse(stream);
            }
        }

        /// <summary>
        /// There are 2 ways that credentials can be provided. They can be entered directly into the form
        /// or they can put the title of an entry in the form. If a DB is open, and the username matches the title,
        /// and no password is provided, and the URL has a protocol prefix that matches the service, we'll use that instead
        /// </summary>
        /// <returns></returns>
        private ICredentials GetCredentials()
        {
            var basicCredential = this.Credentials.GetCredential(this.requestUri, "basic");
            if (basicCredential != null &&
                !string.IsNullOrEmpty(basicCredential.UserName) &&
                this.host != null &&
                this.host.Database.IsOpen)
            {
                var pwQuery = from pe in this.host.Database.GetAllPasswords()
                              where pe.EntryStringEquals(StandardProtectedStrings.Title, basicCredential.UserName) &&
                                    pe.EntryStringNotNullOrEmpty(StandardProtectedStrings.Password) &&
                                    pe.EntryStringExistsAndContidtionMet(StandardProtectedStrings.Url,
                                        value => ProtocolEqualsExpectedProtocol(value))
                              select pe;

                var entry = pwQuery.FirstOrDefault();
                if (entry == null)
                    return this.Credentials;
                else
                    return new EntryCredentials(entry);
            }
            else
                return this.Credentials;
        }

        private bool ProtocolEqualsExpectedProtocol(string url)
        {
            try
            {
                var entryUri = new Uri(url);

                return !string.IsNullOrEmpty(this.requestUri.Scheme) &&
                    !string.IsNullOrEmpty(entryUri.Scheme) &&
                    entryUri.Scheme.ToLowerInvariant() == this.requestUri.Scheme.ToLowerInvariant();
            }
            catch
            {
                return false;
            }
        }
    }
}
