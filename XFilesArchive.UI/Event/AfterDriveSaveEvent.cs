using Prism.Events;

namespace XFilesArchive.UI.Event
{
    public class AfterDriveSaveEvent: PubSubEvent<AfterDriveSavedEventArgs>
    {
    }

    public class AfterDriveSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }

    }
}
