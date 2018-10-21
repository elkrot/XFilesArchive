using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace XFilesArchive.Security.Services
{
    class FacebookAuthenticationService
    {
        WebBrowser FbBro;

        public static String GetLoginUrl()
        {
            var client = new FacebookClient();
            var fbLoginUri = client.GetLoginUrl(new
            {
                client_id = "893528717423936",
                redirect_uri =
              "https://www.facebook.com/connect/login_success.html",
                response_type = "code",
                display = "popup",
                scope = "email"
            });
            return fbLoginUri.ToString();
        }
        bool ShowBrowser = false;

        //,publish_stream
        public void Login()
        {
            var loginUrl = GetLoginUrl();
            ShowBrowser = true;
            FbBro.Navigate(loginUrl);
        }

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

        private void LoginFailed(FacebookOAuthResult oauthResult)
        {
            throw new NotImplementedException();
        }
        string Token;

        public dynamic UserName { get; private set; }
        public dynamic UserPicture { get; private set; }
        public bool IsLogged { get; private set; }

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
            UserPicture = user.picture;
            IsLogged = true;
        }


        private async static Task<dynamic> GetUser(String token)
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


        public static String GetAccessToken(FacebookOAuthResult oauthResult)
        {
            var client = new FacebookClient();
            dynamic result = client.Get("/oauth/access_token",
              new
              {

                  client_id = "893528717423936",
                  client_secret = "28a4f9e18f7925c455498ddd538afbc7"
               ,
                  redirect_uri =
                "https://www.facebook.com/connect/login_success.html",
                  code = oauthResult.Code
              });
            return result.access_token;
        }
        //ConfigurationManager.AppSettings["fb_secret"]

        public static void PostWithPhoto(
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
    }
}
