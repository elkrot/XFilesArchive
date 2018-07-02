using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Wrapper
{
    public class ImageWrapper : ModelWrapper<Image>
    {
        public ImageWrapper(Image model) : base(model)
        {
        }
        public System.Int32 ImageKey
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }

        public System.Int32 ImageKeyOriginalValue => GetOriginalValue<System.Int32>(nameof(ImageKey));

        public bool ImageKeyIsChanged => GetIsChanged(nameof(ImageKey));

        public System.String ImagePath
        {
            get
            {
                var spath = GetValue<System.String>();
                var conf = new ConfigurationData();
                var imgdir = conf.GetTargetImagePath();
                return Path.Combine(imgdir, spath);
            }
            set { SetValue(value); }
        }

        public System.String ImagePathOriginalValue => GetOriginalValue<System.String>(nameof(ImagePath));

        public bool ImagePathIsChanged => GetIsChanged(nameof(ImagePath));

        public System.String ThumbnailPath
        {
            get
            {
                var spath = GetValue<System.String>();
                var conf = new ConfigurationData();
                var imgdir = conf.GetTargetImagePath();
                return Path.Combine(imgdir, spath);
            }
            set { SetValue(value); }
        }

        public System.String ThumbnailPathOriginalValue => GetOriginalValue<System.String>(nameof(ThumbnailPath));

        public bool ThumbnailPathIsChanged => GetIsChanged(nameof(ThumbnailPath));

        public System.String ImageTitle
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String ImageTitleOriginalValue => GetOriginalValue<System.String>(nameof(ImageTitle));

        public bool ImageTitleIsChanged => GetIsChanged(nameof(ImageTitle));

        public System.Nullable<System.Int32> HashCode
        {
            get { return GetValue<System.Nullable<System.Int32>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.Int32> HashCodeOriginalValue => GetOriginalValue<System.Nullable<System.Int32>>(nameof(HashCode));

        public bool HashCodeIsChanged => GetIsChanged(nameof(HashCode));

        public System.DateTime CreatedDate
        {
            get { return GetValue<System.DateTime>(); }
            set { SetValue(value); }
        }

        public System.DateTime CreatedDateOriginalValue => GetOriginalValue<System.DateTime>(nameof(CreatedDate));

        public bool CreatedDateIsChanged => GetIsChanged(nameof(CreatedDate));
    }
}
