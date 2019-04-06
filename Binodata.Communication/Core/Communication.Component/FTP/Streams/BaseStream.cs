using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.FTP.Streams
{
    public abstract class BaseStream : Stream
    {
        protected abstract Stream ftpStream { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ftpStream.Dispose();
        }

        public override bool CanRead
        {
            get
            {
                return ftpStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return ftpStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return ftpStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return ftpStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return ftpStream.Position;
            }

            set
            {
                ftpStream.Position = value;
            }
        }

        public override void Flush()
        {
            ftpStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return ftpStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ftpStream.Write(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return ftpStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ftpStream.SetLength(value);
        }
    }
}
