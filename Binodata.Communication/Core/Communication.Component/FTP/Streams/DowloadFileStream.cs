using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.FTP.Streams
{
    public class DowloadFileStream : BaseStream
    {
        protected override Stream ftpStream { get; set; }

        public DowloadFileStream(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            ftpStream = request.GetResponse().GetResponseStream();
        }
    }
}
