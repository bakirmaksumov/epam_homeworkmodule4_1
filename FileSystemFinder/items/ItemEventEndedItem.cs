using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemFinder.items
{
    public class ItemEventEndedItem<T> : System.EventArgs
        where T : FileSystemInfo
    {
        public T FindedItem { get; set; }
        public StatusEnum ActionType { get; set; }
    }
}
