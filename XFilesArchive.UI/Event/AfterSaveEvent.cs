using Prism.Events;
using System;

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
        public Boolean IsSecret { get; set; }
        public string DriveCode { get; set; }
    }
}
