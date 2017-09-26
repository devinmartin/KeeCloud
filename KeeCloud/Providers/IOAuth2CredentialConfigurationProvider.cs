using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeeCloud.Providers
{
    public interface IOAuth2CredentialConfigurationProvider: ICredentialConfigurationProvider
    {
        /// <summary>
        /// Exchanges an authorization code for an access token.
        /// </summary>
        /// <param name="code">The authorization code</param>
        CredentialClaimResult ExchangeCode(string code);
    }
}
