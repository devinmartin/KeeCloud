using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BoxSync.Core;

namespace KeeCloud.Providers.BoxNet
{
    class Api
    {
        /*
        The consumer key and the secret key included here are dummy keys.
        You should go to http://Box.net/developers to create your own application
        and get your own keys.

        This is done to prevent bots from scraping the keys from the source code posted on the web.
         
        Every now and then an accidental checkin of keys may occur, but these are all dummy applications
        created specifically for development that are deleted frequently.
        */

        public const string applicationKey = "dummy";

        public const string BaseAuthorizationUrl = "";

        private const string serviceUrl = "";

        public static BoxManager Client
        {
            get
            {
                return new BoxSync.Core.BoxManager(Api.applicationKey, Api.serviceUrl, null);
            }
        }

        public static BoxManager AuthenticatedClient(NetworkCredential credential)
        {
            return new BoxManager(Api.applicationKey, Api.serviceUrl, null, credential.Password);
        }
    }
}
