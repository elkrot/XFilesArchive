using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI.Wrapper
{
    public class DriveWrapper : ModelWrapper<Drive>
    {
        public DriveWrapper(Drive model) : base(model)
        {
        }


        protected override IEnumerable<string> ValidateProperty([CallerMemberName]string _propertyName = "")
        {
            switch (_propertyName)
            {
                case nameof(Title):
                    if (string.Equals(Title, "Test", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "не может быть Test";
                    }
                    break;
            }
        }


        public virtual ICollection<ArchiveEntity> ArchiveEntities { get; set; }

        public System.Int32 DriveId
        {
            get { return GetValue<System.Int32>(); }
            set { SetValue(value); }
        }

        public System.Int32 DriveIdOriginalValue => GetOriginalValue<System.Int32>(nameof(DriveId));

        public bool DriveIdIsChanged => GetIsChanged(nameof(DriveId));

        public System.String Title
        {
            get { return GetValue<System.String>().Trim(); }
            set { SetValue(value); }
        }

        public System.String TitleOriginalValue => GetOriginalValue<System.String>(nameof(Title));

        public bool TitleIsChanged => GetIsChanged(nameof(Title));



        public System.String DriveCode
        {
            get { return GetValue<System.String>().Trim(); }
            set { SetValue(value); }
        }

        public System.String DriveCodeOriginalValue => GetOriginalValue<System.String>(nameof(DriveCode));

        public bool DriveCodeIsChanged => GetIsChanged(nameof(DriveCode));


        public System.Nullable<System.Int32> HashCode
        {
            get { return GetValue<System.Nullable<System.Int32>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.Int32> HashCodeOriginalValue => GetOriginalValue<System.Nullable<System.Int32>>(nameof(HashCode));

        public bool HashCodeIsChanged => GetIsChanged(nameof(HashCode));

        public System.Nullable<System.DateTime> CreatedDate
        {
            get { return GetValue<System.Nullable<System.DateTime>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<System.DateTime> CreatedDateOriginalValue => GetOriginalValue<System.Nullable<System.DateTime>>(nameof(CreatedDate));

        public bool CreatedDateIsChanged => GetIsChanged(nameof(CreatedDate));




        public System.Nullable<bool> IsSecret
        {
            get { return GetValue<System.Nullable<bool>>(); }
            set { SetValue(value); }
        }

        public System.Nullable<bool> IsSecretOriginalValue => GetOriginalValue<System.Nullable<bool>>(nameof(IsSecret));

        public bool IsSecretIsChanged => GetIsChanged(nameof(IsSecret));
        

    }


}
