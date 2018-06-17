using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Wrapper
{
    public class CategoryWrapper : ModelWrapper<Category>
    {
        public CategoryWrapper(Category model) : base(model)
        {
        }
    }
}
