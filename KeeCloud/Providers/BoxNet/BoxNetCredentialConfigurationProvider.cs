using System;
using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;

namespace KeeCloud.Providers.BoxNet
{
    public class BoxNetCredentialConfigurationProvider : ICredentialConfigurationProvider
    {
        string ticket;
        InitializeResult ICredentialConfigurationProvider.Initialize()
        {
            var client = Api.Client;
            
            string ticket = null;
            var status = client.GetTicket(out ticket);

            if (status == BoxSync.Core.Statuses.GetTicketStatus.Successful)
            {
                this.ticket = ticket;

                return InitializeResult.Ok;
            }
            else
                return InitializeResult.Error;
        }

        Uri ICredentialConfigurationProvider.GetExternalAuthorizationUrl()
        {
            return new Uri(string.Format(Api.BaseAuthorizationUrl, this.ticket));
        }

        CredentialClaimResult ICredentialConfigurationProvider.Claim()
        {
            var client = Api.Client;

            string oauthToken = null;
            User user = null;
            var status = client.GetAuthenticationToken(this.ticket, out oauthToken, out user);
            if (status == BoxSync.Core.Statuses.GetAuthenticationTokenStatus.Successful)
            {
                return new CredentialClaimResult(user.Email, oauthToken);
            }
            else
                return new CredentialClaimResult();
        }
    }
}
