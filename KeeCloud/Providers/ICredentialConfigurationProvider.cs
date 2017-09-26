using System;

namespace KeeCloud.Providers
{
    /// <summary>
    /// This is intended to follow the OAuth model, but almost anything can be addapted to it if needed.
    /// </summary>
    public interface ICredentialConfigurationProvider
    {
        /// <summary>
        /// Begin the authorization process
        /// </summary>
        /// <param name="uri">The URI to be initialized</param>
        InitializeResult Initialize();
        /// <summary>
        /// Launch the external authorization process
        /// </summary>
        Uri GetExternalAuthorizationUrl();
    }
}
