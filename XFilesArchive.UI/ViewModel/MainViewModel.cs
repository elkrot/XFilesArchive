﻿using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel
{
    //TODO: Сделать Выбор расположения
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        public INavigationViewModel NavigationViewModel { get; }
        private IDetailViewModel _selectedDetailViewModel;
        private IMessageDialogService _messageDialogService;
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;
        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }
        private int nextNewItemId = 0;


        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        private async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public void Load()
        {
             NavigationViewModel.Load();
        }

        public MainViewModel(
            INavigationViewModel navigationViewModel
            , IIndex<string, IDetailViewModel> detailViewModelCreator
            , IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;

            _detailViewModelCreator = detailViewModelCreator;
            DetailViewModels = new ObservableCollection<IDetailViewModel>();
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
    .Subscribe(OnOpenDetailView);

            _eventAggregator.GetEvent<AfterDeletedEvent>()
    .Subscribe(OnAfterDeleted);

            _eventAggregator.GetEvent<AfterDetailClosedEvent>()
    .Subscribe(OnAfterDetailClosed);

            CreateNewCommand = new DelegateCommand<Type>(OnCreateNewExecute);

            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);
            NavigationViewModel = navigationViewModel;


        }


        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            { Id = -1, ViewModelName = viewModelType.Name });
        }

        private void OnAfterDetailClosed(AfterDtailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void OnAfterDeleted(AfterDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int? id, string viewModelName)
        {

            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == id
                && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private void OnCreateNewExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            { Id = nextNewItemId--, ViewModelName = viewModelType.Name });
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenSingleDetailViewCommand { get; }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            var detailViewModel = DetailViewModels
                 .SingleOrDefault(vm => vm.Id == args.Id
                 && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch (Exception e)
                {
                    await _messageDialogService.ShowInfoDialogAsync(e.Message);
                    await NavigationViewModel.LoadAsync();
                    return;
                }

                DetailViewModels.Add(detailViewModel);
            }
            SelectedDetailViewModel = detailViewModel;
        }
    }
}
