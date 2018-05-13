using Prism.Events;

namespace XFilesArchive.UI.Event
{
    public class AfterCollectionSavedEvent:PubSubEvent<AfterCollectionSavedEventArgs>
    {
    }

    public class AfterCollectionSavedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}
