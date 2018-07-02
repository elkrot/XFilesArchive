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
        public System.Int32 CategoryKey
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }

        public System.Int32 CategoryKeyOriginalValue => GetOriginalValue<System.Int32>(nameof(CategoryKey));

        public bool CategoryKeyIsChanged => GetIsChanged(nameof(CategoryKey));

        public System.String CategoryTitle
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String CategoryTitleOriginalValue => GetOriginalValue<System.String>(nameof(CategoryTitle));

        public bool CategoryTitleIsChanged => GetIsChanged(nameof(CategoryTitle));

        public System.Nullable<System.Int32> ParentCategoryKey
        {
            get { return GetValue<System.Nullable<System.Int32>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.Int32> ParentCategoryKeyOriginalValue => GetOriginalValue<System.Nullable<System.Int32>>(nameof(ParentCategoryKey));

        public bool ParentCategoryKeyIsChanged => GetIsChanged(nameof(ParentCategoryKey));

        public System.Nullable<System.DateTime> CreatedDate
        {
            get { return GetValue<System.Nullable<System.DateTime>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.DateTime> CreatedDateOriginalValue => GetOriginalValue<System.Nullable<System.DateTime>>(nameof(CreatedDate));

        public bool CreatedDateIsChanged => GetIsChanged(nameof(CreatedDate));
    }
}
