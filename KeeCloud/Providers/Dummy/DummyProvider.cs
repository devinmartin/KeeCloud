using System;
using System.IO;

namespace KeeCloud.Providers.Dummy
{
    public class DummyProvider : IProvider
    {
        public Uri Uri { get; set; }

        public System.IO.Stream Get(System.Net.ICredentials credentials)
        {
            return new FileStream(this.Uri.LocalPath, FileMode.Open);
        }

        public void Put(System.IO.Stream stream, System.Net.ICredentials credentials)
        {
            byte[] buffer = new byte[32000];
            using (var fs = new FileStream(this.Uri.LocalPath, FileMode.Create))
            {
                int read;
                do
                {
                    read = stream.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        fs.Write(buffer, 0, read);
                        fs.Flush();
                    }
                } while (read > 0);
                fs.Flush();
            }
        }

        public void Delete(System.Net.ICredentials credentials)
        {
            if (File.Exists(this.Uri.LocalPath))
                File.Delete(this.Uri.LocalPath);
        }

        public void Move(Uri destination, System.Net.ICredentials credentials)
        {
            File.Move(this.Uri.LocalPath, destination.LocalPath);
        }

        public bool CanConfigureCredentials
        {
            get { return true; }
        }

        public ICredentialConfigurationProvider CredentialConfigurationProvider
        {
            get { return new DummyConfiguration(this.Uri); }
        }

        string IProvider.FriendlyName { get { return "Dummy (for testing)"; } }
    }
}
