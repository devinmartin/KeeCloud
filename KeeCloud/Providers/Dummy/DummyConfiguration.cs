using System;
using System.Threading;

namespace KeeCloud.Providers.Dummy
{
    public class DummyConfiguration : IOAuth1CredentialConfigurationProvider
    {
        private Uri uri;

        public DummyConfiguration(Uri uri)
        {
            this.uri = uri;
        }
        public InitializeResult Initialize()
        {
            if (!string.IsNullOrEmpty(this.uri.Authority) && this.uri.Authority.ToLowerInvariant() == "delay")
                Thread.Sleep(1500);

            if (!string.IsNullOrEmpty(this.uri.Authority) && this.uri.Authority.ToLowerInvariant() == "initexception")
                throw new Exception("exception");

            if (!string.IsNullOrEmpty(this.uri.Authority) && this.uri.Authority.ToLowerInvariant() == "initerror")
                return InitializeResult.Error;
            else
                return InitializeResult.Ok;
        }

        public Uri GetExternalAuthorizationUrl()
        {
            return new Uri("https://www.google.com");
        }

        public CredentialClaimResult Claim()
        {
            if (!string.IsNullOrEmpty(this.uri.Authority) && this.uri.Authority.ToLowerInvariant() == "delay")
                Thread.Sleep(1500);

            if (!string.IsNullOrEmpty(this.uri.Authority) && this.uri.Authority.ToLowerInvariant() == "claimexception")
                throw new Exception("exception");

            if (!string.IsNullOrEmpty(this.uri.Authority) && this.uri.Authority.ToLowerInvariant() == "claimerror")
                return new CredentialClaimResult();
            else
                return new CredentialClaimResult("username", "password");
        }
    }
}
