using XFilesArchive.DataAccess;
using XFilesArchive.Services.Repositories;

namespace XFilesArchive.Security.Repositories
{
    public class UserRepository : GenericRepository<User, SecurityContext>
    {
        public UserRepository(SecurityContext context) : base(context)
        {
        }
    }
}
