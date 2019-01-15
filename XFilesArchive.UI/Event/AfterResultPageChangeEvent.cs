using Prism.Events;

namespace XFilesArchive.UI.Event
{
    public class AfterResultPageChangeEvent : PubSubEvent<AfterResultPageChangeEventArgs>
    {
    }

    public class AfterResultPageChangeEventArgs
    {
        public int PageNumber { get; set; }
    }
}
