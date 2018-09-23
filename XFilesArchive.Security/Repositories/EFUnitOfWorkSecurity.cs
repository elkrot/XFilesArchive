using XFilesArchive.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Security.Repositories
{
    public class EFUnitOfWorkSecurity : EFUnitOfWorkBase
    {
        public EFUnitOfWorkSecurity(IDbContext context):base(context)
        {
            RepositoryDictionary.Add(nameof(User), new UserRepository(_context));
            RepositoryDictionary.Add(nameof(Role), new RoleRepository(_context));
        }
    }
}
