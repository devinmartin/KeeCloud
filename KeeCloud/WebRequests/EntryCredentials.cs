using KeePassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KeeCloud.WebRequests
{
    /// <summary>
    /// Network credentials created from a KeePass entry
    /// </summary>
    public class EntryCredentials : ICredentials
    {
        private readonly NetworkCredential credential;

        public EntryCredentials(PwEntry entry)
        {
            this.credential = new NetworkCredential(GetEntry(entry, StandardProtectedStrings.Username),
                GetEntry(entry, StandardProtectedStrings.Password));
        }

        private static string GetEntry(PwEntry entry, string key)
        {
            return entry.Strings.Get(key).ReadString();
        }

        NetworkCredential ICredentials.GetCredential(Uri uri, string authType)
        {
            return this.credential;
        }
    }
}
