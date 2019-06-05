using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure;
using XFilesArchive.Security.Repositories;

namespace XFilesArchive.Security.Services
{
    public class WindowsAuthenticationService : IAuthenticationService
    {
        public UserDto AuthenticateUser(string _username="", string _clearTextPassword="")
        {

            //var x = Thread.CurrentPrincipal.Identity.Name;

            WindowsIdentity wuser = WindowsIdentity.GetCurrent();
            var groups = wuser.Groups;
            var account = new NTAccount(wuser.Name);
            var sid = account.Translate(typeof(SecurityIdentifier));

            List<string> wroles = new List<string>();
            foreach (var group in wuser.Groups.Translate(typeof(NTAccount)))
            {
                wroles.Add(group.Value);
            }


            var claims = wuser.UserClaims;
            string email = "";
            if (wuser.FindAll(ClaimTypes.Email).Count() > 0)
            {
                email = wuser.FindAll(ClaimTypes.Email).FirstOrDefault().Value;
            }
            else
            {
                email = sid.Value;
            }           

            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);

                User userData = repo.Find(u => u.Sid.Equals(sid.Value)
               , null).FirstOrDefault();

                if (userData == null)
                    throw new UnauthorizedAccessException("Доступ запрещен. Отредактируйте учетные данные.");
                var roles = new string[] { };
                if (userData.Role != null)
                {
                    roles = userData.Role.Select(x => x.RoleTitle).ToArray();
                }
                return new UserDto(userData.Username.Trim(), userData.Email, roles);

            }
            //ps whoami /groups /fo /list



           
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
