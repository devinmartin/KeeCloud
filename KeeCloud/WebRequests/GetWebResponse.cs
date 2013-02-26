using System.IO;
using System.Net;

namespace KeeCloud.WebRequests
{
    public class GetWebResponse : WebResponse
    {
        Stream stream;
        public GetWebResponse(Stream stream)
        {
            this.stream = stream;
        }

        public override Stream GetResponseStream()
        {
            return this.stream;
        }
    }
}
