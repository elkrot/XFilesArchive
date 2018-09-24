using XFilesArchive.Security.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure;

namespace XFilesArchive.Security
{
    public interface IAuthenticationService
    {
        UserDto AuthenticateUser(string username, string password);
        MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles);
        Role GetRole(string RoleTitle);
    }

    public class AuthenticationService : IAuthenticationService
    {
        #region AuthenticateUser
        public UserDto AuthenticateUser(string username, string clearTextPassword)
        {

            using (var uofw = new EFUnitOfWorkSecurity(new SecurityContext()))
            {
                var repo = uofw.GetRepository<User>();
                var CashedPassword = CalculateHash(clearTextPassword, username);

                User userData = repo.Find(u => u.Username.Equals(username)
                && u.Password.Equals(CashedPassword)).FirstOrDefault();

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

        #region CalculateHash
        private string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
        #endregion

        #region GetUserById
        public User GetUserById(int id)
        {
            using (var uofw = new EFUnitOfWorkSecurity(new SecurityContext()))
            {
                var repo = uofw.GetRepository<User>();
                return repo.Find(x => x.UserId == id).FirstOrDefault();
            }
        }
        #endregion

        #region NewUser
        public MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles)
        {



            using (var uofw = new EFUnitOfWorkSecurity(new SecurityContext()))
            {
                var repo = uofw.GetRepository<User>();
                User user = repo.Find(x => x.Username == username).FirstOrDefault();

                if (user == null)
                {
                    user = new User(username, email, roles);
                    user.Password = CalculateHash(password, user.Username);
                    repo.Add(user);
                }
                else
                {
                    throw new ArgumentException("Пользователь с таки именем существует", "User");
                }
                return uofw.Complete();
            }
        }
        #endregion

        #region GetRole
        public Role GetRole(string RoleTitle)
        {
            using (var uofw = new EFUnitOfWorkSecurity(new SecurityContext()))
            {
                var repo = uofw.GetRepository<Role>();
                Role role = repo.Find(x => x.RoleTitle == RoleTitle).FirstOrDefault();

                if (role == null)
                {
                    role = new Role() { RoleTitle=RoleTitle};
                    repo.Add(role);
                    uofw.Complete();
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
            using (var uofw = new EFUnitOfWorkSecurity(new SecurityContext()))
            {
                var repo = uofw.GetRepository<User>();
                User user = repo.Find(x => x.Username == username).FirstOrDefault();

                if (user != null)
                {
                    repo.Add(user);
                }
                else
                {
                    repo.Add(user);
                }
                return uofw.Complete();
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
