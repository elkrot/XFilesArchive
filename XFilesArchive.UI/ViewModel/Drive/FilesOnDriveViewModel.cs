using System;
using Prism.Events;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Repositories;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.Wrapper;
using XFilesArchive.Model;

namespace XFilesArchive.UI.ViewModel
{

    
      public class FilesOnDriveViewModel : Observable
    {
        private readonly IArchiveEntityRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private ArchiveEntityWrapper _archiveEntity;


        #region Конструктор
        public FilesOnDriveViewModel(IEventAggregator eventAggregator,
                   IMessageDialogService messageDialogService,
                   IArchiveEntityRepository repository)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<SelectedItemChangedEvent>().Subscribe(OnSelectedItemChanged);
          
        }

        private void OnSelectedItemChanged(int obj)
        {

            if (obj != 0)
            {
                int ArchiveEntityKey = 0;
                int.TryParse(obj.ToString(), out ArchiveEntityKey);

               
                Load(ArchiveEntityKey);

                
            }
        }
        #endregion






        public void Load(int? FileOnDriveId = default(int?))
        {
            ArchiveEntityLoad(FileOnDriveId);
        }

        private async void ArchiveEntityLoad(int? FileOnDriveId)
        {
            var _archEntity =  FileOnDriveId.HasValue ?
               await  _repository.GetByIdAsync((int)FileOnDriveId) :
                 new ArchiveEntity();

            _archiveEntity = new ArchiveEntityWrapper(_archEntity);


            //_archiveEntity.PropertyChanged += (s, e) =>
            //{
            //    if (e.PropertyName == nameof(_archiveEntity.IsChanged)
            //    || e.PropertyName == nameof(_archiveEntity.IsValid))
            //    {
            //        InvalidateCommands();
            //    }
            //};
        }

        private void InvalidateCommands()
        {
            throw new NotImplementedException();
        }

        public ArchiveEntityWrapper ArchiveEntity
        {
            get
            {
                return _archiveEntity;
            }
        }




    }
     
}