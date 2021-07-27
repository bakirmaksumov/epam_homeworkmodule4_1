using FileSystemFinder.items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemFinder
{
   public class FileSystemVisitor
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly Func<FileSystemInfo, bool> filter;
        private readonly IFileSystemMainProcess fileSystemMainProcess;

        public event EventHandler<StartEventArgs> Start;
        public event EventHandler<FinishEventArgs> Finish;
        public event EventHandler<ItemEventEndedItem<FileInfo>> FileFinded;
        public event EventHandler<ItemEventEndedItem<FileInfo>> FilteredFileFinded;
        public event EventHandler<ItemEventEndedItem<DirectoryInfo>> DirectoryFinded;
        public event EventHandler<ItemEventEndedItem<DirectoryInfo>> FilteredDirectoryFinded;

        public FileSystemVisitor(string path,
            IFileSystemMainProcess fileSystemMainProcess,
            Func<FileSystemInfo, bool> filter = null)
            : this(new DirectoryInfo(path), fileSystemMainProcess, filter) { }
        public FileSystemVisitor(DirectoryInfo startDirectory,
             IFileSystemMainProcess fileSystemMainProcess,
             Func<FileSystemInfo, bool> filter = null)
            {
               this.directoryInfo = startDirectory;
               this.filter = filter;
               this.fileSystemMainProcess = fileSystemMainProcess;
            }


        public IEnumerable<FileSystemInfo> GetFileSystemInfo()
        {
            OnStartEvent(Start, new StartEventArgs());
            foreach (var result in FileSystemPass(this.directoryInfo, CurrentAction.ContinueSearch))
            {
                yield return result;
            }
            OnStartEvent(Finish, new FinishEventArgs());
        }

        private IEnumerable<FileSystemInfo> FileSystemPass(DirectoryInfo directory, CurrentAction curr)
        {
            foreach (var item in directory.EnumerateFileSystemInfos())
            {
                if (item is FileInfo file)
                { curr.statusEnum = ProcessFile(file);}

                if (item is DirectoryInfo dir)
                {
                    curr.statusEnum = ProcessDirectory(dir);
                    if (curr.statusEnum == StatusEnum.ContinueSearch)
                    {
                        yield return dir;
                        foreach (var innerInfo in FileSystemPass(dir, curr))
                        {
                            yield return innerInfo;
                        }
                        continue;
                    }
                }

                if (curr.statusEnum == StatusEnum.StopSearch)
                {
                    yield break;
                }

                yield return item;

            }
        }

        private StatusEnum ProcessFile(FileInfo file)
        {
            return fileSystemMainProcess
                .ProcessItemFinded(file, filter, FileFinded, FilteredFileFinded, OnStartEvent);

        }
        private StatusEnum ProcessDirectory(DirectoryInfo directory)
        {
            return fileSystemMainProcess
                .ProcessItemFinded(directory, filter, DirectoryFinded, FilteredDirectoryFinded, OnStartEvent);
        }
        private void OnStartEvent<TArgs>(EventHandler<TArgs> someEvent, TArgs args)
        {
            someEvent?.Invoke(this, args);
        }

        private class CurrentAction
        {
            public StatusEnum statusEnum { get; set; }
            public static CurrentAction ContinueSearch
                => new CurrentAction { statusEnum = StatusEnum.ContinueSearch };
        }

    }
}
