using DropNet;
using System.Net;

namespace KeeCloud.Providers.Dropbox
{
    class Api
    {
        /*
        The consumer key and the secret key included here are dummy keys.
        You should go to http://dropbox.com/developers to create your own application
        and get your own keys.

        This is done to prevent bots from scraping the keys from the source code posted on the web.
        */
        public const string consumerKey = "dummy";
        public const string consumerSecret = "dummy";

        public static DropNetClient Client
        {
            get
            {
                return new DropNetClient(consumerKey, consumerSecret);
            }
        }

        public static DropNetClient AuthenticatedClient(NetworkCredential credential)
        {
            return new DropNetClient(consumerKey, consumerSecret, credential.UserName, credential.Password);
        }
    }
}
