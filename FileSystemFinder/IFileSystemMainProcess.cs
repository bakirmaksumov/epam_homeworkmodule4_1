using FileSystemFinder.items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemFinder
{
    public interface IFileSystemMainProcess
    {
        StatusEnum ProcessItemFinded<TItemInfo>(
           TItemInfo fileInfo,
           Func<FileSystemInfo, bool> currentfilter,
           EventHandler<ItemEventEndedItem<TItemInfo>> itemFinded,
           EventHandler<ItemEventEndedItem<TItemInfo>> filteredItemFinded,
           Action<EventHandler<ItemEventEndedItem<TItemInfo>>, ItemEventEndedItem<TItemInfo>> eventEmitter)
           where TItemInfo : FileSystemInfo;
    }
}
