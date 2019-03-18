using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.UI.Event;

namespace XFilesArchive.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {

        public NavigationItemViewModel(int id, string displayMember, string detailViewModelName
            , IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
            _detailViewModelName = detailViewModelName;
            _eventAggregator = eventAggregator;
        }

        public int Id { get; }
        private string _displayMember;
        private IEventAggregator _eventAggregator;
        private string _detailViewModelName;

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }
        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(
                new OpenDetailViewEventArgs { Id = Id, ViewModelName = _detailViewModelName });
        }


    }

    public class NavigationDriveItemViewModel : NavigationItemViewModel
    {
        private string _driveCode;
        private bool _isSecret;

        public NavigationDriveItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator eventAggregator
            ,string DriveCode, Boolean IsSecret) 
            : base(id, displayMember, detailViewModelName, eventAggregator)
        {
            _driveCode = DriveCode;
            _isSecret = IsSecret;
        }
        public Boolean IsSecret
        {
            get { return _isSecret; }
            set
            {
                _isSecret = value;
                OnPropertyChanged();
            }
        }
        public string DriveCode
        {
            get { return _driveCode; }
            set
            {
                _driveCode = value;
                OnPropertyChanged();
            }
        }
    }

}
