using System;
using System.Net;

namespace KeeCloud.WebRequests
{
    public class ProviderWebRequestCreator : IWebRequestCreate
    {
        public static readonly ProviderWebRequestCreator Instance = new ProviderWebRequestCreator();
        private ProviderWebRequestCreator()
        {
        }

        public WebRequest Create(Uri uri)
        {
            var supported = ProviderRegistry.GetProviderForUri(uri);
            return new ProviderWebRequest(uri, supported.Create(uri));
        }
    }
}