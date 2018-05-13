using Prism.Events;

namespace XFilesArchive.UI.Event
{
    public class AfterDetailClosedEvent : PubSubEvent<AfterDtailClosedEventArgs>
    {
    }

    public class AfterDtailClosedEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
