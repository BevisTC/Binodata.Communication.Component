using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.FTP.Streams
{
    public class AppendFileStream : BaseStream
    {
        protected override Stream ftpStream { get; set; }

        public AppendFileStream(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.AppendFile;

            ftpStream = request.GetRequestStream();
        }
    }
}
