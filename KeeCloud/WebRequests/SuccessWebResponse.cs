using System;
using System.IO;
using System.Net;

namespace KeeCloud.WebRequests
{
    class SuccessWebResponse : WebResponse, IDisposable
    {
        Stream stream = new MemoryStream();
        public override Stream GetResponseStream()
        {
            return this.stream;
        }

        void IDisposable.Dispose()
        {
            this.stream.Dispose();
        }
    }
}