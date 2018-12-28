using System.Collections.Generic;
using XFilesArchive.Services.Lookups;

namespace XFilesArchive.UI.ViewModel
{
    internal class MultySeltEntityeecsViewModel
    {
        private ICollection<ArchiveEntityDto> _items;
        public ICollection<ArchiveEntityDto> Items { get { return _items; } }

        public MultySeltEntityeecsViewModel(ICollection<ArchiveEntityDto> items)
        {
            this._items = items;
        }
    }
}