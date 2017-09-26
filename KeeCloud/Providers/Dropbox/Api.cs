using System;
using System.Net;
using System.Net.Http;

using Dropbox.Api;

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

        private static HttpClient httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
        {
            // Specify request level timeout which decides maximum time that can be spent on
            // download/upload files.
            Timeout = TimeSpan.FromMinutes(1)
        };

        public static Uri GetAuthorizeUri(string state)
        {
            return DropboxOAuth2Helper.GetAuthorizeUri(Api.appKey);
        }

        public static string ProcessCodeFlow(string code)
        {
            var task = DropboxOAuth2Helper.ProcessCodeFlowAsync(code, appKey, appSecret, client: httpClient);
            var resp = task.Result;
            return resp.AccessToken;
        }

        public static DropboxClient AuthenticatedClient(NetworkCredential credential)
        {
            var config = new DropboxClientConfig("KeeCloud")
            {
                HttpClient = httpClient
            };
            return new DropboxClient(credential.Password, config);
        }
    }
}
