using XFilesArchive.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Security.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(IDbContext context) : base(context)
        {
        }
    }
}
