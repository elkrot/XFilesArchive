using Prism.Events;

namespace XFilesArchive.UI.Event
{
    public class AfterSaveEvent:PubSubEvent<AfterDriveSavedEventArgs>
    {
    }

    public class AfterDriveSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
        public string ViewModelName { get; set; }
    }
}
