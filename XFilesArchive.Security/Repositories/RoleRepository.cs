using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Services.Repositories;

namespace XFilesArchive.Security.Repositories
{
    public class RoleRepository : GenericRepository<User, SecurityContext>
    {
        public RoleRepository(SecurityContext context) : base(context)
        {
        }
    }
}
