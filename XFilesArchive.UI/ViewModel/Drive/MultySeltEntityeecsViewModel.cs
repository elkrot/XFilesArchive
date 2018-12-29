using System.Collections.Generic;
using System.Collections.ObjectModel;
using XFilesArchive.Services.Lookups;

namespace XFilesArchive.UI.ViewModel
{
    internal class MultySeltEntityeecsViewModel
    {
        private ObservableCollection<ArchiveEntityLookupDto> _items;
        public ObservableCollection<ArchiveEntityLookupDto> Items { get { return _items; } }

        public MultySeltEntityeecsViewModel(ObservableCollection<ArchiveEntityLookupDto> items)
        {
            this._items = items;
        }
    }
}