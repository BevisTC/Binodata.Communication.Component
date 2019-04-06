using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.FTP.Streams
{
    public class UploadFileStream : BaseStream
    {
        protected override Stream ftpStream { get; set; }

        public UploadFileStream(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.UploadFile;

            ftpStream = request.GetRequestStream();
        }
    }
}
