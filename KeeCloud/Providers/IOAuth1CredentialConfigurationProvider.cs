namespace KeeCloud.Providers
{
    public interface IOAuth1CredentialConfigurationProvider: ICredentialConfigurationProvider
    {
        /// <summary>
        /// Upon success of external authorization, attempt to make a claim
        /// </summary>
        CredentialClaimResult Claim();
    }
}
