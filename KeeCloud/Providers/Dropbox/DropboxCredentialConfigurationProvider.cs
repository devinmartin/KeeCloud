using DropNet;
using System;

namespace KeeCloud.Providers.Dropbox
{
    public class DropboxCredentialConfigurationProvider : ICredentialConfigurationProvider
    {
        private DropNetClient authenticationClient = Api.Client;

        public InitializeResult Initialize()
        {
            try
            {
                this.authenticationClient.GetToken();
                return InitializeResult.Ok;
            }
            catch
            {
                return InitializeResult.Error;
            }
        }

        public Uri GetExternalAuthorizationUrl()
        {
            return new Uri(this.authenticationClient.BuildAuthorizeUrl());
        }

        public CredentialClaimResult Claim()
        {
            try
            {
                var result = this.authenticationClient.GetAccessToken();
                return new CredentialClaimResult(result.Token, result.Secret);
            }
            catch
            {
                return new CredentialClaimResult();
            }
        }
    }
}
