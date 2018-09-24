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

namespace XFilesArchive.UI.ViewModel.Security
{
    public interface IViewModel { }

    public class AuthenticationViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        private readonly DelegateCommand _showViewCommand;
        private readonly DelegateCommand _createAdminCommand;

        private string _username;
        private string _status;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);
            _showViewCommand = new DelegateCommand(ShowView, null);
            _createAdminCommand = new DelegateCommand(CreateAdmin, null);
            var cmdArgs = System.Environment.GetCommandLineArgs();
           
                Username = "Admin";
        
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

        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }

        public DelegateCommand ShowViewCommand { get { return _showViewCommand; } }

        public DelegateCommand CreateAdminCommand { get { return _createAdminCommand; } }
        #endregion

        private void CreateAdmin(object parameter) {
            try
            {
              //  var adminRole = _authenticationService.GetRole("Administrator");
              
                _authenticationService.NewUser("Admin", "", "Pa$$w0rd", new HashSet<Role>() { new Role() { RoleTitle="Administrator"} });
                Status = "Создан пользователь Admin";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }


        private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;
            try
            {

                UserDto user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("Неудача.");
                //"The application's default thread principal must be set to a CustomPrincipal object on startup."

                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);

                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
                if (IsAuthenticated) {
                    ShowView(null);
                }

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

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
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

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private void ShowView(object parameter)
        {
            try
            {
                Status = string.Empty;

                var bootstrapper = new Bootstrapper();
                
                Autofac.IContainer container = bootstrapper.Bootstrap();



                var mainWindow = container.Resolve<MainWindow>();
                mainWindow.Show();


                var window = System.Windows.Application.Current.Windows[0];

                if (window != null)
                    window.Close();
            }
            catch (SecurityException)
            {
                Status = "Вы не авторизованы!";
            }
        }


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
