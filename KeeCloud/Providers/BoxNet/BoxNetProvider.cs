using System;
using System.IO;
using System.Net;
using BoxSync.Core.ServiceReference;

namespace KeeCloud.Providers.BoxNet
{
    public class BoxNetProvider : IProvider
    {
        private Uri uri;

        Uri IProvider.Uri { get { return this.uri; } set { this.uri = value; } }

        Stream IProvider.Get(ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        void IProvider.Put(Stream stream, ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        void IProvider.Delete(ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        void IProvider.Move(Uri destination, ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        bool IProvider.CanConfigureCredentials
        {
            get { return true; }
        }

        ICredentialConfigurationProvider IProvider.CredentialConfigurationProvider
        {
            get { return new BoxNetCredentialConfigurationProvider(); }
        }

        string IProvider.FriendlyName
        {
            get { return "Box.net"; }
        }

        private NetworkCredential GetCredential(ICredentials credentials)
        {
            return credentials.GetCredential(this.uri, "basic");
        }
    }
}
