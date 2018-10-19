//using Microsoft.IdentityModel.Claims;
using System.Security.Principal;
using System;
using Microsoft.IdentityModel.Claims;

namespace XFilesArchive.Security
{

    public class CustomIdentity : ClaimsIdentity, IIdentity
    {
        public CustomIdentity(string name, string email, string[] roles):base()
        {
            //Name = name;
            Email = email;
            Roles = roles;
        }

       // public string Name { get; private set; }
        public string Email { get; private set; }
        public string[] Roles { get; private set; }

        #region IIdentity Members
        //public string AuthenticationType { get { return "Custom authentication"; } }

        //public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }

        //public ClaimCollection Claims => throw new NotImplementedException();

        //public IClaimsIdentity Actor { get ; set; }
        //public string Label { get; set; }
        //public string NameClaimType { get; set; }
        //public string RoleClaimType { get; set; }
        //public System.IdentityModel.Tokens.SecurityToken BootstrapToken { get ; set; }

        //public IClaimsIdentity Copy()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}
