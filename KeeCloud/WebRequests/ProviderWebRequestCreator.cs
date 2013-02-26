using KeePass.Plugins;
using System;
using System.Net;

namespace KeeCloud.WebRequests
{
    public class ProviderWebRequestCreator : IWebRequestCreate
    {
        private readonly IPluginHost host;

        public ProviderWebRequestCreator(IPluginHost host)
        {
            this.host = host;
        }

        public WebRequest Create(Uri uri)
        {
            var supported = ProviderRegistry.GetProviderForUri(uri);
            return new ProviderWebRequest(uri, supported.Create(uri), this.host);
        }
    }
}