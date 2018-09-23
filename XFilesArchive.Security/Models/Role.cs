    using System;
    using System.Collections.Generic;

namespace XFilesArchive.Security
{

    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleTitle { get; set; }

        
        public virtual ICollection<User> User { get; set; }
    }
}
