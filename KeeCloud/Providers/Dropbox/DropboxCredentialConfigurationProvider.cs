using System;

namespace KeeCloud.Providers.Dropbox
{
    public class DropboxCredentialConfigurationProvider : IOAuth2CredentialConfigurationProvider
    {
        private string oauth2State;

        public InitializeResult Initialize()
        {
            this.oauth2State = Guid.NewGuid().ToString("N");
            return InitializeResult.Ok;
        }

        public Uri GetExternalAuthorizationUrl()
        {
            return Api.GetAuthorizeUri(this.oauth2State);
        }

        public bool IsManual { get { return true; } }

        public CredentialClaimResult ExchangeCode(string code)
        {
            return new CredentialClaimResult("", Api.ProcessCodeFlow(code));
        }
    }
}
