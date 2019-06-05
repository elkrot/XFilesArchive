using XFilesArchive.Security.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using XFilesArchive.Infrastructure;

namespace XFilesArchive.Security
{
    public interface IAuthenticationService
    {
        UserDto AuthenticateUser(string _username, string _clearTextPassword);
        MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles);
        Role GetRole(string RoleTitle);
    }

    public class AuthenticationService : IAuthenticationService
    {
 
        public AuthenticationService()
        {

        }
        #region AuthenticateUser
        public UserDto AuthenticateUser(string _username, string _clearTextPassword)
        {

            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);
                var CashedPassword = Infrastructure.Utilites.Security.CalculateHash(_clearTextPassword, _username);

                User userData = repo.Find(u => u.Username.Equals(_username)
                && u.Password.Equals(CashedPassword), null).FirstOrDefault();

                if (userData == null)
                    throw new UnauthorizedAccessException("Доступ запрещен. Отредактируйте учетные данные.");
                var roles = new string[] { };
                if (userData.Role != null)
                {
                    roles = userData.Role.Select(x => x.RoleTitle).ToArray();
                }
                return new UserDto(userData.Username, userData.Email, roles);

            }



        }
        #endregion



        #region GetUserById
        public User GetUserById(int id)
        {
            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);

                return repo.Find(x => x.UserId == id, null).FirstOrDefault();
            }
        }
        #endregion

        #region NewUser
        public MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles)
        {

            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);
                User user = repo.Find(x => x.Username == username, null).FirstOrDefault();

                if (user == null)
                {
                    HashSet<Role> _roles = new HashSet<Role>();

                    foreach (var ro in roles)
                    {
                        var _role = repo.GetRole(ro.RoleTitle);
                        if (_role == null)
                        {
                            _role = new Role() { RoleTitle = ro.RoleTitle };
                        }
                        _roles.Add(_role);
                    }




                    user = new User(username, email, _roles);
                    user.Password = Infrastructure.Utilites.Security.CalculateHash(password, user.Username);
                    repo.Add(user);
                }
                else
                {
                    throw new ArgumentException("Пользователь с таки именем существует", "User");
                }
                repo.Save();
                var result = new MethodResult<int>(0);
                return result;
            }
        }
        #endregion

        #region GetRole
        public Role GetRole(string RoleTitle)
        {
            using (var context = new SecurityContext())
            {
                var repo = new RoleRepository(context);
                Role role = repo.Find(x => x.RoleTitle == RoleTitle, null).FirstOrDefault();

                if (role == null)
                {
                    role = new Role() { RoleTitle = RoleTitle };
                    repo.Add(role);
                    repo.Save();
                }
                else
                {
                    //throw new ArgumentException("Пользователь с таки именем существует", "User");
                }


                return role;
            }
        }
        #endregion


        #region SaveUser
        public MethodResult<int> SaveUser(string username, string email, string password)
        {
            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);
                User user = repo.Find(x => x.Username == username, null).FirstOrDefault();

                if (user != null)
                {
                    repo.Add(user);
                }
                else
                {
                    repo.Add(user);
                }
                repo.Save();
                var result = new MethodResult<int>(0);
                return result;
            }
        }

        #endregion


    }

    #region UserDto
    public class UserDto
    {
        public UserDto(string username, string email, string[] roles)
        {
            Username = username;
            Email = email;
            Roles = roles;
        }

        public string Username
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string[] Roles
        {
            get;
            set;
        }
    }

    #endregion
}
