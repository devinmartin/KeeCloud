using System;
using KeeCloud.Providers;

namespace KeeCloud.Providers.Google
{
    public class GoogleProvider : IProvider
    {
        public GoogleProvider()
        {
        }

        #region IProvider implementation

        System.IO.Stream IProvider.Get(System.Net.ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        void IProvider.Put(System.IO.Stream stream, System.Net.ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        void IProvider.Delete(System.Net.ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        void IProvider.Move(Uri destination, System.Net.ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        Uri IProvider.Uri
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool IProvider.CanConfigureCredentials
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ICredentialConfigurationProvider IProvider.CredentialConfigurationProvider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IProvider.FriendlyName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}