using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using Dropbox.Api.Files;

namespace KeeCloud.Providers.Dropbox
{
    public class DropboxProvider : IProvider
    {
        public Uri Uri { get; set; }

        Stream IProvider.Get(ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));
            var task1 = client.Files.DownloadAsync(GetPath(this.Uri));
            var task2 = task1.Result.GetContentAsStreamAsync();
            return task2.Result;
        }

        void IProvider.Put(Stream stream, ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));

            var path = GetPath(this.Uri);
            var commit = new CommitInfo(path, mode: WriteMode.Overwrite.Instance, mute: true);
            var task = client.Files.UploadAsync(commit, stream);
            task.Wait();
        }

        void IProvider.Delete(ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));

            var path = GetPath(this.Uri);
            var task = client.Files.DeleteV2Async(path);
            task.Wait();
        }

        void IProvider.Move(Uri destination, ICredentials credentials)
        {
            var client = Api.AuthenticatedClient(this.GetNetworkCredential(credentials));

            var sourcePath = GetPath(this.Uri);
            var destinationPath = GetPath(destination);
            var task = client.Files.MoveV2Async(new RelocationArg(sourcePath, destinationPath));
            task.Wait();
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
            var path = "/" + uri.OriginalString.Substring(uri.Scheme.Length + 3);
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
