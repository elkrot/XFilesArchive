using System.Collections.ObjectModel;

namespace XFilesArchive.Services.Lookups
{
    public class LookupItem
    {
        public int Id { get; set; }

        public string DisplayMember { get; set; }
    }

    public class LookupItemNode : LookupItem
    {
        public string Name { get; set; }
        public ObservableCollection<LookupItemNode> Nodes { get; set; }
        public byte EntityType { get; set; }
    }

}
