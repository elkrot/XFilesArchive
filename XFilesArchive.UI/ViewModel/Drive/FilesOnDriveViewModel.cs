using System;
using Prism.Events;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Repositories;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.Wrapper;
using XFilesArchive.Model;
using System.Threading.Tasks;

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

        private async void OnSelectedItemChanged(int obj)
        {

            if (obj != 0)
            {
                int ArchiveEntityKey = 0;
                int.TryParse(obj.ToString(), out ArchiveEntityKey);

               
               await Load(ArchiveEntityKey);

                
            }
        }
        #endregion






        public async Task Load(int? FileOnDriveId = default(int?))
        {
          

var _archEntity =  FileOnDriveId.HasValue ?
               await  _repository.GetByIdAsync((int)FileOnDriveId) :
                 new ArchiveEntity();

            _archiveEntity = new ArchiveEntityWrapper(_archEntity);
            OnPropertyChanged("ArchiveEntity");
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