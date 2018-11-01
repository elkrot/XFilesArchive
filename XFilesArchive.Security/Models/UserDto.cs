using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Security.Models
{
   public class UserDtoZ
    {
        public UserDtoZ()
        {
            Role = new HashSet<Role>();
        }

        public UserDtoZ(string username, string email, HashSet<Role> roles) : this()
        {
            Username = username;
            Email = email;
            foreach (var role in roles)
            {
                Role.Add(role);
            }
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }

        public virtual ICollection<Role> Role { get; set; }
    }
}
