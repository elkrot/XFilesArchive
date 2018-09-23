using System.Collections.Generic;

namespace XFilesArchive.Security
{
    public partial class User
    {
         public User()
        {
            Role = new HashSet<Role>();
        }
        public User(string username, string email, HashSet<Role> roles):this()
        {

            Username = username;
            Email = email;
            foreach (var role in roles)
            {
                Role.Add(role);
            }
            
        }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Role> Role { get; set; }
    }
}
