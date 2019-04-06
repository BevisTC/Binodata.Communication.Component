using Communication.Component.FTP.Interface;
using Communication.Component.FTP.Streams;
using Communication.Component.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.FTP
{
    public class FtpFileSystem :  IVirtualFileSystem
    {
        public FtpFileSystem(string connectString)
        {
            Dictionary<string, string> parms = ParserConnectString(connectString);

            this.domain = parms["source"];
            this.user = parms["user"];
            this.password = parms["password"];
        }

        public FtpFileSystem(string domain, string user, string password)
        {
            this.domain = domain;
            this.user = user;
            this.password = password;
        }

        public bool IsFile(string Path)
        {
            FtpWebRequest request = CreateRequest(Path);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            try
            {
                request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsDirectory(string DirPath)
        {
            if (DirPath == "./")
                return true;

            string parent = GetParent(DirPath);
            string basename = GetBase(DirPath);

            return ListDirectoryDetails(parent).Any(s => s.Contains(basename) && s.StartsWith("d"));
        }

        public long Size(string Path)
        {
            try
            {
                FtpWebRequest request = CreateRequest(Path);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                return request.GetResponse().ContentLength;
            }
            catch
            {
                return 0;
            }
        }

        public IEnumerable<string> ListRoot()
        {
            return ListDirectory("./");
        }

        public IEnumerable<string> ListDirectory(string DirectoryPath)
        {
            FtpWebRequest request = CreateRequest(DirectoryPath);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            using (var stream = request.GetResponse().GetResponseStream())
            {
                string result = Encoding.UTF8.GetString(stream.ReadToEnd());

                return result.Split(new char[] { '\n' }).AsEnumerable().Where(d => !string.IsNullOrEmpty(d));
            }
        }

        public Stream Open(string FilePath, FileAccess Access, FileMode Mode)
        {
            FtpWebRequest request = CreateRequest(FilePath);

            switch (Access)
            {
                case FileAccess.Read:
                    return ReadStream(request, Mode);

                case FileAccess.Write:
                    return WriteStream(request, FilePath, Mode);

                default:
                    throw new NotSupportedException("FileAccess Not Supported");
            }
        }

        public void Delete(string FilePath)
        {
            try
            {
                FtpWebRequest request = CreateRequest(FilePath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.GetResponse();
            }
            catch { }
        }

        public void MakeDirectory(string DirectoryPath)
        {
            MakeDirectoryRecusive(DirectoryPath);
        }

        private bool MakeDirectoryRecusive(string DirectoryPath)
        {
            if (IsDirectory(DirectoryPath))
                return true;

            string parent = GetParent(DirectoryPath);
            if (MakeDirectoryRecusive(parent))
                return MakeDirectoryImpl(DirectoryPath);
            else
                return false;
        }

        private bool MakeDirectoryImpl(string DirectoryPath)
        {
            try
            {
                FtpWebRequest request = CreateRequest(DirectoryPath);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }


        public void DeleteDirectory(string DirectoryPath)
        {
            try
            {
                FtpWebRequest request = CreateRequest(DirectoryPath);
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                request.GetResponse();
            }
            catch { }
        }


        private FtpWebRequest CreateRequest(string path)
        {
            FtpWebRequest request = FtpWebRequest.Create(Url(path)) as FtpWebRequest;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(user, password);
            request.UseBinary = true;
            request.UsePassive = true;
            request.Timeout = 60 * 1000;

            return request;
        }

        private List<string> ListDirectoryDetails(string Dir)
        {
            FtpWebRequest request = CreateRequest(Dir);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                string response = Encoding.UTF8.GetString(stream.ReadToEnd());
                return response.Split(new char[] { '\n' }).AsEnumerable().Where(d => !string.IsNullOrEmpty(d)).ToList();
            }
        }

        private string Url(string path)
        {
            return string.Format("ftp://{0}/{1}", domain, path);
        }

        private Stream ReadStream(FtpWebRequest request, FileMode Mode)
        {
            switch (Mode)
            {
                case FileMode.Open:
                    return new DowloadFileStream(request);

                default:
                    throw new NotSupportedException("FileMode " + Mode + " Not Supported");
            }
        }

        private Stream WriteStream(FtpWebRequest request, string FilePath, FileMode Mode)
        {
            CreateDirectoryIfNotExist(FilePath);
            switch (Mode)
            {
                case FileMode.CreateNew:
                    if (IsFile(FilePath))
                        throw new IOException("Can Not Write Exist File");
                    else
                        return new UploadFileStream(request);

                case FileMode.Create:
                case FileMode.Truncate:
                    return new UploadFileStream(request);

                case FileMode.Append:
                    return new AppendFileStream(request);

                default:
                    throw new NotSupportedException("FileMode " + Mode + " Not Supported");
            }
        }

        private void CreateDirectoryIfNotExist(string FilePath)
        {
            string parent = GetParent(FilePath);
            if (parent == "./" || IsDirectory(parent))
                return;

            MakeDirectory(parent);
        }

        private static Dictionary<string, string> ParserConnectString(string connectString)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            foreach (var item in connectString.Split(';'))
            {
                var tokens = item.Split('=');
                if (tokens.Length != 2)
                    throw new Exception("FtpFileSystem Connect String Format Error");
                else
                    parms.Add(tokens[0], tokens[1]);
            }

            if (parms.ContainsKey("source") && parms.ContainsKey("user") && parms.ContainsKey("password"))
                return parms;
            else
                throw new Exception("FtpFileSystem Connect String Lack Information");
        }

        private static string GetParent(string Path)
        {
            string result = System.IO.Path.GetDirectoryName(Path);

            return string.IsNullOrWhiteSpace(result) ? "./" : result;
        }

        private static string GetBase(string Path)
        {
            return System.IO.Path.GetFileName(Path).Trim(new char[] { '/', '\\' });
        }

        private readonly string domain;
        private readonly string user;
        private readonly string password;
    }
}
