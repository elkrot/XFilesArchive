using Prism.Events;

namespace XFilesArchive.UI.Event
{
    public class AfterDetailSavedEvent : PubSubEvent<AfterDtailSavedEventArgs>
    {
    }

    public class AfterDtailSavedEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
