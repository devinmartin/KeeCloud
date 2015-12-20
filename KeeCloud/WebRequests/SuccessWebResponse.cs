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

		public override void Close()
		{
			// WebReponse.Close() must not be called
			// System.NotSupportedException: Specified method is not supported.
			// at System.Net.WebResponse.Close () [0x00000] in /.../mono-4.2.1/mcs/class/System/System.Net/WebResponse.cs:100 
			try
			{
				this.stream.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

    }
}