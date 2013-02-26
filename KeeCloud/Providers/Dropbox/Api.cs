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

        /// <summary>
        /// This is the App key provided by Dropbox
        /// </summary>
        const string appKey = "lmtap1z523irkms";
        /// <summary>
        /// This is the App secret provided by Dropbox
        /// </summary>
        const string appSecret = "2v9k5xze6f9r0ua";

        public static DropNetClient Client
        {
            get
            {
                return new DropNetClient(appKey, appSecret);
            }
        }

        public static DropNetClient AuthenticatedClient(NetworkCredential credential)
        {
            return new DropNetClient(appKey, appSecret, credential.UserName, credential.Password);
        }
    }
}
