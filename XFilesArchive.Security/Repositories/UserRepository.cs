using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Services.Repositories;
using System.Linq;
using System.Data.Entity;

namespace XFilesArchive.Security.Repositories
{
    public interface IUserRepository {
         Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddNewUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task RemoveUserAsync(User user);
    };


    public class UserRepository : GenericRepository<User, SecurityContext>, IUserRepository
    {
        public UserRepository(SecurityContext context) : base(context)
        {
        }

        public async Task AddNewUserAsync(User user)
        {
            Add(user);

           await SaveAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return  await this.GetAllAsync();
        }

        public async Task RemoveUserAsync(User user)
        {
            Remove(user);
            await SaveAsync();
        }

        public async Task UpdateUserAsync(User _user)
        {
           //var _user = Find(x => x.UserId == user.UserId,x=>x.UserId).FirstOrDefault();
            Context.Set<User>().Attach(_user);
            Context.Entry<User>(_user).State = EntityState.Modified;

            await SaveAsync();
        }

        internal Role GetRole(string roleTitle)
        {
           return Context.Set<Role>().Where(x => x.RoleTitle == roleTitle).FirstOrDefault();
        }
    }
}
