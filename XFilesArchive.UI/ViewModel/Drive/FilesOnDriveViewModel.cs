using Prism.Events;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Repositories;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel
{

    
      public class FilesOnDriveViewModel : Observable
    {
        private readonly IArchiveEntityRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        

        #region Конструктор
        public FilesOnDriveViewModel(IEventAggregator eventAggregator,
                   IMessageDialogService messageDialogService,
                   IArchiveEntityRepository _repository)
        {
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






        public void Load(int? DriveId = default(int?))
        {
            
        }





     

    }
     
}