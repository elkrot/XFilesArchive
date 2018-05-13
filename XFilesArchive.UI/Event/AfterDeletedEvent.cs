using Prism.Events;
namespace XFilesArchive.UI.Event
{
    public class AfterDeletedEvent: PubSubEvent<AfterDeletedEventArgs>
    {
    }
    public class AfterDeletedEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
