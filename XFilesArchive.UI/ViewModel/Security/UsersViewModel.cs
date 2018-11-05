using Prism.Commands;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tulpep.ActiveDirectoryObjectPicker;
using XFilesArchive.Security;
using XFilesArchive.Security.Models;
using XFilesArchive.Security.Repositories;
using XFilesArchive.UI.View.Security;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.Wrapper;

namespace XFilesArchive.UI.ViewModel.Security
{
    public class UsersViewModel : DetailViewModelBase
    {

        public ObservableCollection<User> UsersCollection { get; set; }
        private IUserRepository _repository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageService;


        public ICommand AddNewUserCommand { get; }
        public ICommand RemoveUserCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand SetWindowsAuthenticationCommand { get; }
        public ICommand SetFacebookAuthenticationCommand { get; }



        public UsersViewModel(IUserRepository repository, IEventAggregator eventAggregator
            , IMessageDialogService messageService) : base(eventAggregator, messageService)
        {
            
            UsersCollection = new ObservableCollection<User>();

            _repository = repository;
            _eventAggregator = eventAggregator;
            _messageService = messageService;

            AddNewUserCommand = new Prism.Commands.DelegateCommand(OnAddNewUserExecute);
            RemoveUserCommand = new DelegateCommand<User>(OnRemoveUserExecute);
            ChangePasswordCommand = new DelegateCommand<User>(OnChangePasswordExecute);
            SetWindowsAuthenticationCommand = new DelegateCommand<User>(OnSetWindowsAuthenticationExecute);
            SetFacebookAuthenticationCommand = new DelegateCommand<User>(OnSetFacebookAuthenticationExecute);
        }

        private void OnSetFacebookAuthenticationExecute(User user)
        {
            
        }

        #region WindowsAuthentication
        private async void OnSetWindowsAuthenticationExecute(User user)
        {
            DirectoryObjectPickerDialog picker = new DirectoryObjectPickerDialog()
            {
                AllowedObjectTypes = ObjectTypes.All,
                DefaultObjectTypes = ObjectTypes.All,
                AllowedLocations = Locations.All,
                DefaultLocations = Locations.JoinedDomain,
                MultiSelect = true,
                ShowAdvancedView = true,
                AttributesToFetch = new List<string>() { "objectSid" }
            };

            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryObject[] results = picker.SelectedObjects;
                if (results == null)
                {
                    return;
                }

                var sid = "";
                for (int i = 0; i <= results.Length - 1; i++)
                {
                    string downLevelName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(results[i].Upn))
                            downLevelName = NameTranslator.TranslateUpnToDownLevel(results[i].Upn);
                    }
                    catch (Exception ex)
                    {
                        downLevelName = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                    }

                    for (int j = 0; j < results[i].FetchedAttributes.Length; j++)
                    {
                        object multivaluedAttribute = results[i].FetchedAttributes[j];
                        if (!(multivaluedAttribute is IEnumerable) || multivaluedAttribute is byte[] || multivaluedAttribute is string)
                            multivaluedAttribute = new[] { multivaluedAttribute };

                        foreach (object attribute in (IEnumerable)multivaluedAttribute)
                        {
                            if (attribute is byte[])
                            {
                                byte[] bytes = (byte[])attribute;
                                sid = BytesToString(bytes);
                            }
                        }
                    }
                }
                user.Sid = sid;
               await _repository.UpdateUserAsync(user);
                await LoadAsync(0);

            }
        }

        private string BytesToString(byte[] bytes)
        {
            try
            {
                Guid guid = new Guid(bytes);
                return guid.ToString("D");
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            try
            {
                SecurityIdentifier sid = new SecurityIdentifier(bytes, 0);
                return sid.ToString();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            return "0x" + BitConverter.ToString(bytes).Replace('-', ' ');
        }
        #endregion


        private async void OnChangePasswordExecute(User user)
        {

            // Set passs sh1
            await _repository.UpdateUserAsync(user);
        }

        

        private async void OnRemoveUserExecute(User user)
        {
            if (_messageService.ShowOKCancelDialog("Удалить пользователя", user.Username)==MessageDialogResult.OK)
            {
                await _repository.RemoveUserAsync(user);
                await LoadAsync(0);
            }
        }

        private async void OnAddNewUserExecute()
        {
            var userZ = new UserDtoWrapper(new UserDtoZ() { Username = "Anonimus" });
            var w = new NewUserView(userZ);
            var result = w.ShowDialog();
            if (result.Value==true)
            {
                var userDtoZ = w.DataContext as UserDtoWrapper;
                var _password = Infrastructure.Utilites.Security.CalculateHash(userDtoZ.Password, userDtoZ.Username);
                var user = new User() { Username = userDtoZ.Username, Password = _password, Email = userDtoZ.Email };
                await _repository.AddNewUserAsync(user);
                await LoadAsync(0);
            }
        }

        public override async Task LoadAsync(int id)
        {
            var users = await  _repository.GetAllUsersAsync();
            UsersCollection.Clear();
            foreach (var item in users)
            {
                UsersCollection.Add(item);
            }

        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveExecute()
        {
            throw new NotImplementedException();
        }
    }
}
