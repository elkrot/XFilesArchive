using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.UI.Event
{
    public class AfterSearchDetailClosedEvent : PubSubEvent<AfterSearchDtailClosedEventArgs>
    {
    }

    public class AfterSearchDtailClosedEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
