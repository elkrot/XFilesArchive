using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.UI.Event
{
    public class OpenSearchDetailDriveViewEvent : PubSubEvent<OpenSearchDetailDriveViewEventArgs>
    {
    }
    public class OpenSearchDetailDriveViewEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
