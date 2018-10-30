using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Services.Repositories;

namespace XFilesArchive.Security.Repositories
{
    public interface IUserRepository {
         Task<IEnumerable<User>> GetAllUsersAsync();
    };


    public class UserRepository : GenericRepository<User, SecurityContext>, IUserRepository
    {
        public UserRepository(SecurityContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return  await this.GetAllAsync();
        }
    }
}
