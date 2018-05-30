using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.UI.Services.Lookups;

namespace XFilesArchive.UI.ViewModel.Navigation
{
    public class NavigationTreeItemViewModel : Observable
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayValue;

        #region Constructor
        public NavigationTreeItemViewModel(LookupItemNode itemNode,
          IEventAggregator eventAggregator)
        {
            ArchiveEntityKey = itemNode.Id;
            DisplayValue = itemNode.DisplayMember;
            NavigationItems = GetItems(itemNode.Nodes);
            ImagePath = itemNode.EntityType == 1 ? "/HomeArchiveX.WpfUI;component/img/folder.png"
                : "/HomeArchiveX.WpfUI;component/img/document_empty.png";
            //;
            _eventAggregator = eventAggregator;
            //OpenFileOnDriveEditViewCommand = new DelegateCommand(OpenFileOnDriveEditViewExecute);



        }

        private ObservableCollection<NavigationTreeItemViewModel> GetItems(ObservableCollection<LookupItemNode> nodes)
        {
            var retItems = new ObservableCollection<NavigationTreeItemViewModel>();
            foreach (var item in nodes)
            {
                var itm = new NavigationTreeItemViewModel(
                    item,
                    _eventAggregator);
                retItems.Add(itm);

            }
            return retItems;
        }
        #endregion


        // public ICommand OpenFileOnDriveEditViewCommand { get; set; }

        public int ArchiveEntityKey { get; private set; }
        public ObservableCollection<NavigationTreeItemViewModel> NavigationItems { get; set; }
        public string ImagePath { get; set; }
        #region DisplayValue
        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value.Trim();
                OnPropertyChanged();
            }
        }
        #endregion

        #region OpenDriveEditViewExecute
        //private void OpenFileOnDriveEditViewExecute(object obj)
        //{
        //    _eventAggregator.GetEvent<OpenFileOnDriveEditViewEvent>().Publish(ArchiveEntityKey);

        //}


        #endregion

    }
}
