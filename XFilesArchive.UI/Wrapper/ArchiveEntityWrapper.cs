using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Wrapper
{
    public class ArchiveEntityWrapper : ModelWrapper<ArchiveEntity>
    {
        public ArchiveEntityWrapper(ArchiveEntity model) : base(model)
        {
        }
        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
