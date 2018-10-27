﻿using XFilesArchive.Security.Repositories;
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
        UserDto AuthenticateUser();
        MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles);
        Role GetRole(string RoleTitle);
    }

    public class AuthenticationService : IAuthenticationService
    {
        string _username;
        string _clearTextPassword;
        public AuthenticationService(string username, string clearTextPassword)
        {
            _username = username;
            _clearTextPassword = clearTextPassword;
        }
        #region AuthenticateUser
        public UserDto AuthenticateUser()
        {

            using (var context = new SecurityContext())
            {
                var repo =new UserRepository(context);
                var CashedPassword = CalculateHash(_clearTextPassword, _username);

                User userData = repo.Find(u => u.Username.Equals(_username)
                && u.Password.Equals(CashedPassword),null).FirstOrDefault();

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
            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);
               
                return repo.Find(x => x.UserId == id,null).FirstOrDefault();
            }
        }
        #endregion

        #region NewUser
        public MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles)
        {



            using (var context = new SecurityContext())
            {
                var repo = new UserRepository(context);
                User user = repo.Find(x => x.Username == username,null).FirstOrDefault();

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
                Role role = repo.Find(x => x.RoleTitle == RoleTitle,null).FirstOrDefault();

                if (role == null)
                {
                    role = new Role() { RoleTitle=RoleTitle};
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
                User user = repo.Find(x => x.Username == username,null).FirstOrDefault();

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
