using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using XFilesArchive.Infrastructure;

namespace XFilesArchive.Security.Services
{
   public class FacebookAuthenticationService : IAuthenticationService
    {
        WebBrowser _FbBro;
        string _client_id;
        string _redirect_uri;
        bool ShowBrowser = false;
        string Token;
        private string _client_secret;
        public dynamic UserName { get; private set; }
        public dynamic Email { get; private set; }
        public dynamic UserPicture { get; private set; }
        public dynamic Id { get; private set; }
        public bool IsLogged { get; private set; }


        public FacebookAuthenticationService(WebBrowser FbBro)
        {
            _FbBro = FbBro;
            _client_id = "-------------";
            _redirect_uri = "https://www.facebook.com/connect/login_success.html";
            FbBro.Navigated += FbBro_Navigated;
            _client_secret = "--------------";
        }

        #region GetLoginUrl
        public String GetLoginUrl()
        {
            var client = new FacebookClient();
            var fbLoginUri = client.GetLoginUrl(new
            {
                client_id = _client_id,
                redirect_uri = _redirect_uri,
                response_type = "code",
                display = "popup",
                scope = "email"
            });
            return fbLoginUri.ToString();
        }
        #endregion
        //,publish_stream
        #region Login
        public void Login()
        {
            var loginUrl = GetLoginUrl();
            ShowBrowser = true;
            _FbBro.Navigate(loginUrl);
        }
        #endregion

        #region FbBro_Navigated
        private void FbBro_Navigated(object sender, NavigationEventArgs e)
        {
            var fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (!fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
                return;
            if (oauthResult.IsSuccess)
                LoginSucceeded(oauthResult);
            else
                LoginFailed(oauthResult);
        }
        #endregion

        #region LoginFailed
        private void LoginFailed(FacebookOAuthResult oauthResult)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LoginSucceeded
        public async void LoginSucceeded(FacebookOAuthResult oauthResult)
        {
            // Hide the Web browser
            ShowBrowser = false;
            // Grab the access token (necessary for further operations)
            var token = GetAccessToken(oauthResult);
            Token = token;
            // Grab user information
            dynamic user = await GetUser(token);
            // Update the user interface
            UserName = String.Format("{0} ", user.first_name);
            Email = String.Format("{0} ", user.email);
            UserPicture = user.picture;
                Id = user.id;
            IsLogged = true;
        }
        #endregion

        #region GetUser
        private async Task<dynamic> GetUser(String token)
        {
            var client = new FacebookClient();
            dynamic user = await client.GetTaskAsync("/me",
                new
                {
                    fields = "email,name,first_name,middle_name,last_name,picture",
                    access_token = token
                });

            return user;
        }
        #endregion

        #region GetAccessToken
        public String GetAccessToken(FacebookOAuthResult oauthResult)
        {
            var client = new FacebookClient();
            dynamic result = client.Get("/oauth/access_token",
              new
              {

                  client_id = _client_id,
                  client_secret = _client_secret
               ,
                  redirect_uri = _redirect_uri,
                  code = oauthResult.Code
              });
            return result.access_token;
        }
        #endregion
        //ConfigurationManager.AppSettings["fb_secret"]
        #region PostWithPhoto
        public void PostWithPhoto(
  String token, String status, String photoPath)
        {
            var client = new FacebookClient(token);
            using (var stream = File.OpenRead(photoPath))
            {
                client.Post("me/photos",
                new
                {
                    message = status,
                    file = new FacebookMediaStream
                    {
                        ContentType = "image/jpg",
                        FileName = System.IO.Path.GetFileName(photoPath)
                    }.SetValue(stream)
                });
            }
        }

        public UserDto AuthenticateUser(string _username="", string _clearTextPassword="")
        {

            Login();
            var user = new UserDto(UserName, "", new string[] { "" });
            return user;
        }

        public MethodResult<int> NewUser(string username, string email, string password, HashSet<Role> roles)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(string RoleTitle)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
