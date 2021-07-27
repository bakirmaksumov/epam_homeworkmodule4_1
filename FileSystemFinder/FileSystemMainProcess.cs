using FileSystemFinder.items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemFinder
{
    public class FileSystemMainProcess: IFileSystemMainProcess
    {
        public StatusEnum ProcessItemFinded<TItemInfo>(
           TItemInfo fileInfo,
           Func<FileSystemInfo, bool> currentfilter,
           EventHandler<ItemEventEndedItem<TItemInfo>> itemFinded,
           EventHandler<ItemEventEndedItem<TItemInfo>> filteredItemFinded,
           Action<EventHandler<ItemEventEndedItem<TItemInfo>>, ItemEventEndedItem<TItemInfo>> eventEmitter)
           where TItemInfo : FileSystemInfo
        {
            var args = new ItemEventEndedItem<TItemInfo>
            {
                FindedItem = fileInfo,
                ActionType = StatusEnum.ContinueSearch
            };
            eventEmitter(itemFinded, args);

            if (args.ActionType != StatusEnum.ContinueSearch || currentfilter == null)
            {
                return args.ActionType;
            }

            if (currentfilter(fileInfo))
            {
                args = new ItemEventEndedItem<TItemInfo>
                {
                    FindedItem = fileInfo,
                    ActionType = StatusEnum.ContinueSearch
                };
                eventEmitter(filteredItemFinded, args);
                return args.ActionType;
            }

            return StatusEnum.SkipElement;
        }
    }
}
