using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
//using Microsoft.IdentityModel.Claims;
namespace XFilesArchive.Security
{


    // Principal - Пользователь() сдесь объединяются Identity & Claims, 
    // Identities - Паспорта, Документы(Windows, Facebook, Google)   
    // Claims - Строки паспорта (Роли, Имя, emails...)


    // [PrincipalPermission(SecurityAction.Demand, Role = "Administrator")]
    public class CustomPrincipal : ClaimsPrincipal, IPrincipal 
    {
       // private CustomIdentity _identity;

        public CustomPrincipal(IIdentity identity) :base(identity)
        {
           
        }
        //public CustomIdentity Identity
        //{
        //    get { return _identity ?? new AnonymousIdentity(); }
        //    set { _identity = value; }
        //}

        //  public ClaimsIdentityCollection Identities => throw new NotImplementedException();

        #region IPrincipal Members
        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        //public IClaimsPrincipal Copy()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool IsInRole(string role)
        //{
        //    return _identity.Roles.Contains(role);
        //}
        #endregion
    }
}
