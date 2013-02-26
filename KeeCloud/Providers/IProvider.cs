using System;
using System.IO;
using System.Net;

namespace KeeCloud.Providers
{
    public interface IProvider
    {
        /// <summary>
        /// Uri of the DB file in question
        /// </summary>
        Uri Uri { get; set; }
        /// <summary>
        /// Get the stream representing the database file for a URI
        /// </summary>
        Stream Get(ICredentials credentials);
        /// <summary>
        /// Put a DB stream into a specific URI
        /// </summary>
        void Put(Stream stream, ICredentials credentials);
        /// <summary>
        /// Delete a DB file from a specific URI
        /// </summary>
        void Delete(ICredentials credentials);
        /// <summary>
        /// Move a URI to another URI.
        /// </summary>
        /// <reremarks>
        /// For some providers this may be a delete add operation if the provider doesn't support it directly.
        /// </reremarks>
        void Move(Uri destination, ICredentials credentials);
        /// <summary>
        /// Indicates that this provider can make use of the credential wizard
        /// </summary>
        bool CanConfigureCredentials { get; }
        /// <summary>
        /// Gets a credential configuration provider for this particular protocol provider
        /// </summary>
        /// <exception cref="NotSupportedException">Should be thrown if the CanConfigureCredentials property returns false</exception>
        ICredentialConfigurationProvider CredentialConfigurationProvider { get; }
        /// <summary>
        /// This is a name that will be displayed to end users to describe the protocol
        /// </summary>
        string FriendlyName { get; }
    }
}
