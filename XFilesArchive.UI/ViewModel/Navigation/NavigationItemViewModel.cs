using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.UI.Event;

namespace XFilesArchive.UI.ViewModel
{
    #region NavigationItemViewModel
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

        #region DisplayMember
        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(
                new OpenDetailViewEventArgs { Id = Id, ViewModelName = _detailViewModelName });
        }


    }

    #endregion
 
    public class NavigationDriveItemViewModel : NavigationItemViewModel
    {
        private string _driveCode;
        private bool _isSecret;
        private ObservableCollection<PathInfo> _pathItems;
        public NavigationDriveItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator eventAggregator
            ,string DriveCode, Boolean IsSecret) 
            : base(id, displayMember, detailViewModelName, eventAggregator)
        {
            _driveCode = DriveCode;
            _isSecret = IsSecret;
            _pathItems = new ObservableCollection<PathInfo>();
            if (_isSecret) {
                _pathItems.Add(new PathInfo() {
                    Data = "M 16.0625 2.3320312 C 11.54132 2.3320312 7.8353882 5.9897619 7.7441406 10.490234 C 7.1635648 10.892489 6.7832031 11.561397 6.7832031 12.324219 L 6.7832031 24.90625 C 6.7832031 26.142836 7.7790195 27.138672 9.015625 27.138672 L 22.986328 27.138672 C 24.222933 27.138672 25.216797 26.142836 25.216797 24.90625 L 25.216797 12.324219 C 25.216797 11.622096 24.888579 11.006261 24.384766 10.597656 C 24.351468 6.047672 20.619914 2.3320312 16.0625 2.3320312 z M 16.0625 6.1113281 C 18.403039 6.1113281 20.277474 7.833567 20.554688 10.091797 L 11.572266 10.091797 C 11.849479 7.833567 13.721962 6.1113281 16.0625 6.1113281 z M 16 14.166016 A 2.3991122 2.3991122 0 0 1 18.398438 16.564453 A 2.3991122 2.3991122 0 0 1 17.59375 18.351562 L 18.460938 21.767578 L 13.537109 21.767578 L 14.404297 18.353516 A 2.3991122 2.3991122 0 0 1 13.601562 16.564453 A 2.3991122 2.3991122 0 0 1 16 14.166016 z",
                    Fill="White"
                });
            }
        }

        #region IsSecret
        public Boolean IsSecret
        {
            get { return _isSecret; }
            set
            {
                _isSecret = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DriveCode
        public string DriveCode
        {
            get { return _driveCode; }
            set
            {
                _driveCode = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PathItems
        public ObservableCollection<PathInfo> PathItems
        {
            get { return _pathItems; }
            set
            {
                _pathItems = value;
                OnPropertyChanged();
            }
        }
        #endregion

    }

    public class PathInfo {
        public string Data { get; set; }
        public string Fill { get; set; }
    }
}
