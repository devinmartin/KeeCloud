using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace KeeCloud.Providers.Dropbox
{
    public class DropboxProvider : IProvider
    {
        public Uri Uri { get; set; }

        Stream IProvider.Get(ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));
            var data = client.GetFile(GetPath(this.Uri));
            return new MemoryStream(data);
        }

        void IProvider.Put(Stream stream, ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));

            var path = GetPath(this.Uri);
            client.UploadFile(Path.GetDirectoryName(path), Path.GetFileName(path), this.ReadAll(stream));
        }

        void IProvider.Delete(ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));

            var path = GetPath(this.Uri);
            client.Delete(path);
        }

        void IProvider.Move(Uri destination, ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));

            var sourcePath = GetPath(this.Uri);
            var destinationPath = GetPath(destination);
            client.Move(sourcePath, destinationPath);
        }

        bool IProvider.CanConfigureCredentials { get { return true; } }

        ICredentialConfigurationProvider IProvider.CredentialConfigurationProvider
        {
            get { return new DropboxCredentialConfigurationProvider(); }
        }

        string IProvider.FriendlyName { get { return "Dropbox"; } }

        private NetworkCredential GetNetworkCredential(ICredentials credentials)
        {
            return credentials.GetCredential(this.Uri, "basic");
        }

        private static string GetPath(Uri uri)
        {
            var path = uri.OriginalString.Substring(uri.Scheme.Length + 3);
            if (path.EndsWith("/"))
                return path.TrimEnd('/');
            else
                return path;
        }

        private byte[] ReadAll(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int read = 0;
            byte[] buffer = new byte[32000];
            do
            {
                read = stream.Read(buffer, 0, buffer.Length);
                bytes.AddRange(buffer.Take(read));
            } while (read > 0);

            return bytes.ToArray();
        }
    }
}
