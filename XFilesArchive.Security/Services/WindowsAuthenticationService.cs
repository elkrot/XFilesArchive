using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure;

namespace XFilesArchive.Security.Services
{
    public class WindowsAuthenticationService : IAuthenticationService
    {
        public UserDto AuthenticateUser()
        {

            var x = Thread.CurrentPrincipal.Identity.Name;

            WindowsIdentity wuser = WindowsIdentity.GetCurrent();
            var groups = wuser.Groups;
            var account = new NTAccount(wuser.Name);
            var sid = account.Translate(typeof(SecurityIdentifier));

            List<string> roles = new List<string>();
 foreach (var group in wuser.Groups.Translate(typeof(NTAccount))){
                roles.Add(group.Value);
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

            
            var user = new UserDto(wuser.Name, email, roles.ToArray());
            

  //ps whoami /groups /fo /list



            return user;
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
