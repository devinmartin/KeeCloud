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
            // this method is called by the .net framework when it wants a specialized
            // web request for a registered non-standard protocol.  KeeCloud has only one
            // specialized WebRequest type, but we pass a provider into the web reqeust.
            var supported = ProviderRegistry.GetProviderForUri(uri);
            return new ProviderWebRequest(uri, supported.Create(uri), this.host);
        }
    }
}