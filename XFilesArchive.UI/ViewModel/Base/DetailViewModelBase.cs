﻿using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.Infrastructure;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        protected readonly IEventAggregator EventAggregator;
        private IAppLogger _appLogger;
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public IMessageDialogService MessageDialogService { get;  }
        public IAppLogger AppLogger { get { return _appLogger; } }


        public DetailViewModelBase(IEventAggregator eventAggregator
            ,IMessageDialogService _messageDialogService
            ,IAppLogger appLogger)
        {
            _appLogger = appLogger;
            EventAggregator = eventAggregator;
            MessageDialogService = _messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewModelCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        protected async virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges) {
                var result = await MessageDialogService.ShowOKCancelDialogAsync(
                    "?","title"
                    );
                if (result == MessageDialogResult.Cancel) {
                    return;
                }
            }
            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Publish(new AfterDtailClosedEventArgs {
                    Id = this.Id
                    , ViewModelName = this.GetType().Name
                });
            
        }

        public abstract Task LoadAsync(int id);

        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();


        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private int _id;
        
        public int Id
        {
            get
            {
                return _id;
            }

            protected set
            {
                _id = value;

            }
        }


        private string _title;

        public string Title
        {
            get
            {
                return _title.Length>5? _title.Substring(0,5)+"..":_title;
            }

            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }


       

        public string Tooltip
        {
            get
            {
                return _title;
            }

        }



        public ICommand CloseDetailViewModelCommand { get; private set; }

        protected virtual void RaiseDetailDelitedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDeletedEvent>().Publish(
                new AfterDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = this.GetType().Name
                }
                );
        }

        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember, string driveCode="", bool? isSecret = false)
        {
            EventAggregator.GetEvent<AfterSaveEvent>().Publish(new AfterDriveSavedEventArgs
            {
                Id = modelId
                ,
                DisplayMember = displayMember
                ,
                ViewModelName = this.GetType().Name
                , DriveCode = driveCode
                , IsSecret = (bool)isSecret
            });
        }


        protected  async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc
            ,Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var dataBaseValues = ex.Entries.Single().GetDatabaseValues();
                if (dataBaseValues == null)
                {

                    _appLogger.SetLog("Ошибка DbUpdateConcurrencyException"+ex.Message+ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                    await MessageDialogService.ShowInfoDialogAsync("Ошибка DbUpdateConcurrencyException");
                    RaiseDetailDelitedEvent(Id);
                    return;
                }


                var result =await MessageDialogService.ShowOKCancelDialogAsync("Продолжить сохранение?", "Q");
                if (result == MessageDialogResult.OK)
                {
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }

            }

            afterSaveAction();

        }


        protected virtual void RaiseCollectionSavedEvent() {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs { ViewModelName=this.GetType().Name})
                ;
        }
    }
}
