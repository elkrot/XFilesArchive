using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.UI.Event
{
    public class OpenSearchDetailViewEvent : PubSubEvent<OpenSearchDetailViewEventArgs>
    {
    }
    public class OpenSearchDetailViewEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }

}
