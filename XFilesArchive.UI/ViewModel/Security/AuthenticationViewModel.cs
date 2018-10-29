using Autofac;
using XFilesArchive.Security;
using XFilesArchive.UI.Startup;
using XFilesArchive.UI.View.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using XFilesArchive.Infrastructure;
using System.Security.Claims;
using XFilesArchive.Security.Services;

namespace XFilesArchive.UI.ViewModel.Security
{
    public interface IViewModel { }

    public class AuthenticationViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _winLoginCommand;
        private readonly DelegateCommand _fbLoginCommand;
        private readonly DelegateCommand _logoutCommand;
        private readonly DelegateCommand _showViewCommand;
        private readonly DelegateCommand _createAdminCommand;
        private string _username;
        private string _status;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _winLoginCommand = new DelegateCommand(WinLogin, WinCanLogin);
            _fbLoginCommand = new DelegateCommand(FbLogin, FbCanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);
            _showViewCommand = new DelegateCommand(ShowView, null);
            _createAdminCommand = new DelegateCommand(CreateAdmin, null);
            var cmdArgs = Environment.GetCommandLineArgs();
            Username = "Admin";
        }

        private bool FbCanLogin(object obj)
        {
            return true;
        }

        private void FbLogin(object obj)
        {
            var fbLogin = new FacebookLoginWindow();
            fbLogin.ShowDialog();
            var user = fbLogin.User;

        }

        private bool WinCanLogin(object obj)
        {
            return true;
        }

        private void WinLogin(object obj)
        {
            var service = new WindowsAuthenticationService();
            var user = service.AuthenticateUser();
            try
            {
                Auth(user, null);
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Измените учетные данные и повторите попытку.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        #region Properties
        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged("Username"); }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Выполнен вход как {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Administrator") ? "Вы вошли как администратор!"
                              : "Вы не входите в группу администраторов.");

                return "Не аутентфицирован!";
            }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }
        #endregion

        #region Commands
        public DelegateCommand LoginCommand { get { return _loginCommand; } }

        public DelegateCommand WinLoginCommand { get { return _winLoginCommand; } }

        public DelegateCommand FbLoginCommand { get { return _fbLoginCommand; } }


        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }

        public DelegateCommand ShowViewCommand { get { return _showViewCommand; } }

        public DelegateCommand CreateAdminCommand { get { return _createAdminCommand; } }
        #endregion

        #region CreateAdmin
        private void CreateAdmin(object parameter)
        {
            try
            {
                //  var adminRole = _authenticationService.GetRole("Administrator");

                _authenticationService.NewUser("Admin", "", "Pa$$w0rd", new HashSet<Role>() { new Role() { RoleTitle = "Administrator" } });
                Status = "Создан пользователь Admin";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        #endregion

        #region Login
       private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;

//Username, clearTextPassword
            try
            {

                UserDto user = _authenticationService.AuthenticateUser();
                Action resetAction = () =>
                {
                    Username = string.Empty; //reset
                    passwordBox.Password = string.Empty; //reset
                };
                Auth(user, resetAction);

            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Измените учетные данные и повторите попытку.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private void Auth(UserDto user, Action resetAction)
        {
            //--------------------------------------------------------------------------
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Email,user.Email),
                        };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            customPrincipal = new CustomPrincipal(new ClaimsIdentity(claims, "custom"));
            if (customPrincipal == null)
                throw new ArgumentException("Неудача.");
            Thread.CurrentPrincipal = customPrincipal;
            // customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);
            //--------------------------------------------------------------------------
            NotifyPropertyChanged("AuthenticatedUser");
            NotifyPropertyChanged("IsAuthenticated");
            _loginCommand.RaiseCanExecuteChanged();
            _logoutCommand.RaiseCanExecuteChanged();
            if (resetAction!=null) resetAction();
            Status = string.Empty;
            if (IsAuthenticated)
            {
                ShowView(null);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        #endregion

        #region Logout
      private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
               // customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }
        #endregion

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        #region ShowView
     private void ShowView(object parameter)
        {
            try
            {
                Status = string.Empty;

                var bootstrapper = new Bootstrapper();

                Autofac.IContainer container = bootstrapper.Bootstrap();



                var mainWindow = container.Resolve<MainWindow>();


                var window = System.Windows.Application.Current.Windows[0];
                if (window != null)
                    window.Hide();
                mainWindow.ShowDialog();


                

                if (window != null)
                    window.Close();
            }
            catch (SecurityException)
            {
                Status = "Вы не авторизованы!";
            }
        }

        #endregion
   

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
