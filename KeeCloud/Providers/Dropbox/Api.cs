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
         
        Every now and then an accidental checkin of keys may occur, but these are all dummy applications
        created specifically for development that are deleted frequently and limited to the developer,
        never the real production keys.
        */

        /// <summary>
        /// This is the App key provided by Dropbox
        /// </summary>
        const string appKey = "dummy";
        /// <summary>
        /// This is the App secret provided by Dropbox
        /// </summary>
        const string appSecret = "dummy";

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
