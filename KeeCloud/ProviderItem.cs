using KeeCloud.Providers;
using System;

namespace KeeCloud
{
    public class ProviderItem
    {
        private readonly string protocolPrefix;
        private readonly Func<IProvider> providerFactory;

        public ProviderItem(string protocol, Func<IProvider> providerFactory)
        {
            this.protocolPrefix = protocol;
            this.providerFactory = providerFactory;
        }

        public string Protocol { get { return this.protocolPrefix; } }
        public IProvider Create(Uri uri)
        {
            IProvider provider = providerFactory();
            provider.Uri = uri;
            return provider;
        }
    }
}
