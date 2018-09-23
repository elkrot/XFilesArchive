using XFilesArchive.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Security.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(IDbContext context) : base(context)
        {
        }
    }
}
