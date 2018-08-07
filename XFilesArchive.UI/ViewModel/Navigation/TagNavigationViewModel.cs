using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.Model;
using XFilesArchive.UI.Event;

namespace XFilesArchive.UI.ViewModel.Navigation
{
    public interface ITagNavigationViewModel
    {
        void Load();
    }
    public class TagNavigationViewModel : ITagNavigationViewModel
    {

        private readonly IEventAggregator _eventAggregator;
        private readonly IEnumerable<Tag> _tags;
        public TagNavigationViewModel(IEventAggregator eventAggregator,
         IEnumerable<Tag> tags)
        {
            _eventAggregator = eventAggregator;
            _tags = tags;
            NavigationItems = new ObservableCollection<NavigationTagItemViewModel>();
        }


        public ObservableCollection<NavigationTagItemViewModel> NavigationItems { get; set; }

        public void Load()
        {

            NavigationItems.Clear();
            foreach (var tag in
                _tags)
            {
                NavigationItems.Add(
                  new NavigationTagItemViewModel(
                    tag.TagKey,
                    tag.TagTitle,
                    _eventAggregator));
            }
        }
    }

    public class NavigationTagItemViewModel : Observable
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayValue;

        #region Constructor
        public NavigationTagItemViewModel(int tagKey,
          string displayValue,
          IEventAggregator eventAggregator)
        {
            TagKey = tagKey;
            DisplayValue = displayValue;
            _eventAggregator = eventAggregator;
            AddTagToConditionCommand = new DelegateCommand(AddTagToConditionCommandExecute);
        }
        #endregion


        public ICommand AddTagToConditionCommand { get; set; }

        public int TagKey { get; private set; }

        #region DisplayValue
        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region OpenDriveEditViewExecute
        private void AddTagToConditionCommandExecute()
        {
            _eventAggregator.GetEvent<AddTagToConditionCommandEvent>().Publish(TagKey);
        }
        #endregion

    }
}
