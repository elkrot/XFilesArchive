using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Event
{
    public class OpenSearchDetailViewEvent : PubSubEvent<OpenSearchDetailViewEventArgs>
    {
    }
    public class OpenSearchDetailViewEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
        public Expression<Func<ArchiveEntity, bool>> Condition { get; set; }
    }

}
