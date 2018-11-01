using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Security;
using XFilesArchive.Security.Models;

namespace XFilesArchive.UI.Wrapper
{
    public class UserDtoWrapper : ModelWrapper<UserDtoZ>
    {
        public UserDtoWrapper(UserDtoZ model) : base(model)
        {
        }



        public System.String Username
    {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String UsernameOriginalValue => GetOriginalValue<System.String>(nameof(Username));

        public bool UsernameIsChanged => GetIsChanged(nameof(Username));


        public System.String Email
    {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String EmailOriginalValue => GetOriginalValue<System.String>(nameof(Email));

        public bool EmailIsChanged => GetIsChanged(nameof(Email));




        public System.String Password
    {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String PasswordOriginalValue => GetOriginalValue<System.String>(nameof(Password));

        public bool PasswordIsChanged => GetIsChanged(nameof(Password));

 


        public System.String Password2
        {
            get { return GetValue<System.String>(); }
            set { SetValue(value); }
        }

        public System.String Password2OriginalValue => GetOriginalValue<System.String>(nameof(Password2));

        public bool Password2IsChanged => GetIsChanged(nameof(Password2));

        public ChangeTrackingCollection<RoleWrapper> Roles { get; private set; }

        protected override void InitializeCollectionProperties(UserDtoZ model)
        {
            if (model.Role == null)
            {
                throw new ArgumentException("Role cannot be null");
            }

            Roles = new ChangeTrackingCollection<RoleWrapper>(
              model.Role.Select(e => new RoleWrapper(e)));
            RegisterCollection(Roles, model.Role.ToList());
        }
    }
}
