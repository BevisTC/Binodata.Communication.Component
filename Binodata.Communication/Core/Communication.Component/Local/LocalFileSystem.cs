using Communication.Component.FTP.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.Local
{
    public class LocalFileSystem : IVirtualFileSystem
    {
        public LocalFileSystem(string root)
        {
            this.root = root;
        }

        public bool IsFile(string FilePath)
        {
            return File.Exists(FullPath(FilePath));
        }

        public bool IsDirectory(string Path)
        {
            return Directory.Exists(FullPath(Path));
        }

        public long Size(string Path)
        {
            return new FileInfo(FullPath(Path)).Length;
        }

        public Stream Open(string FilePath, FileAccess Access, FileMode Mode)
        {
            if (Access == FileAccess.Write)
                CreateDirectortyIfNotExist(FilePath);

            return new FileStream(FullPath(FilePath), Mode, Access);
        }

        public IEnumerable<string> ListRoot()
        {
            var files = Directory.GetFiles(root);
            var dirs = Directory.GetDirectories(root);

            return files.Union(dirs).Select(item => item);
        }

        public IEnumerable<string> ListDirectory(string DirectoryPath)
        {
            var files = Directory.GetFiles(FullPath(DirectoryPath));
            var dirs = Directory.GetDirectories(FullPath(DirectoryPath));

            return files.Union(dirs).Select(item => item.Substring(root.Length + 1));
        }

        public void Delete(string FilePath)
        {
            File.Delete(FullPath(FilePath));
        }

        public void MakeDirectory(string DirectoryPath)
        {
            if (IsDirectory(DirectoryPath) == false)
                Directory.CreateDirectory(FullPath(DirectoryPath));
        }

        public void DeleteDirectory(string DirectoryPath)
        {
            if (IsDirectory(DirectoryPath))
                Directory.Delete(FullPath(DirectoryPath));
        }


        private string FullPath(string FilePath)
        {
            return string.Format("{0}/{1}", root, FilePath);
        }

        private void CreateDirectortyIfNotExist(string FilePath)
        {
            string dir = Path.GetDirectoryName(FilePath);
            if ((dir.Length > 0) && IsDirectory(dir) == false)
                Directory.CreateDirectory(FullPath(dir));
        }

        private readonly string root;
    }
}
