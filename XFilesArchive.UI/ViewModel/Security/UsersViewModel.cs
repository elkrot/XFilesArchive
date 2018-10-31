using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Security;
using XFilesArchive.Security.Repositories;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel.Security
{
    public class UsersViewModel : DetailViewModelBase
    {

        public ObservableCollection<User> UsersCollection { get; set; }
        private IUserRepository _repository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageService;

        public UsersViewModel(IUserRepository repository, IEventAggregator eventAggregator
            , IMessageDialogService messageService) : base(eventAggregator, messageService)
        {
            
            UsersCollection = new ObservableCollection<User>();

            _repository = repository;
            _eventAggregator = eventAggregator;
            _messageService = messageService;
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
