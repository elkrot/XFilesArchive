using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Security;

namespace XFilesArchive.UI.Wrapper
{
    public class RoleWrapper : ModelWrapper<Role>
    {
        public RoleWrapper(Role model) : base(model)
        {
        }
    }
}
