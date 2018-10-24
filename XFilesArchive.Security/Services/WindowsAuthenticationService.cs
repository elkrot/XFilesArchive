using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure;

namespace XFilesArchive.Security.Services
{
    public class WindowsAuthenticationService : IAuthenticationService
    {
        public UserDto AuthenticateUser()
        {
            return null;
        }

        public Role GetRole(string RoleTitle)
        {
            throw new NotImplementedException();
        }

        public MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles)
        {
            throw new NotImplementedException();
        }
    }
}
