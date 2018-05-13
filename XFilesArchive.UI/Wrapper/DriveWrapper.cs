using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public int DriveId { get { return Model.DriveId; } }

        public string Title
        {
            get { return GetValue<string>(); }
            set
            { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Title):
                    if (string.Equals(Title, "Test", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "не может быть Test";
                    }
                    break;
            }
        }
    }


}
