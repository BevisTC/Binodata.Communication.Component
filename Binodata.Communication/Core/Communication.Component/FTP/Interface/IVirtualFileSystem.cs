using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.FTP.Interface
{
    public interface IVirtualFileSystem
    {
        bool IsFile(string Path);
        bool IsDirectory(string Path);
        long Size(string Path);
        IEnumerable<string> ListRoot();
        IEnumerable<string> ListDirectory(string DirectoryPath);
        Stream Open(string FilePath, FileAccess Access = FileAccess.Read, FileMode Mode = FileMode.Open);
        void Delete(string FilePath);
        void MakeDirectory(string DirectoryPath);
        void DeleteDirectory(string DirectoryPath);
    }
}
