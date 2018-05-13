using Prism.Events;

namespace XFilesArchive.UI.Event
{
    class AfterDetailDelitedEvent : PubSubEvent<AfterDtailDelitedEventArgs>
    {
    }

    internal class AfterDtailDelitedEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
