using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.Security;
using XFilesArchive.Security.Repositories;
using XFilesArchive.UI.View.Security;
using XFilesArchive.UI.View.Services;

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

        private void OnSetWindowsAuthenticationExecute(User user)
        {
            
        }

        private void OnChangePasswordExecute(User user)
        {
            
        }

        public User SelectedUser { get; set; }

        private void OnRemoveUserExecute(User user)
        {
            
        }

        private void OnAddNewUserExecute()
        {
            var w = new NewUserView();
            w.Show();
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
