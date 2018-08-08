using Autofac;
using Prism.Events;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;
using XFilesArchive.Services.Lookups;
using XFilesArchive.Services.Repositories;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.ViewModel;
using XFilesArchive.UI.ViewModel.Drive;
using XFilesArchive.UI.ViewModel.Navigation;
using XFilesArchive.UI.ViewModel.Search;

namespace XFilesArchive.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            

            builder.RegisterType<XFilesArchiveDataContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<DriveDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(DriveDetailViewModel));
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FilesOnDriveLookupProvider>().As<ITreeViewLookupProvider<ArchiveEntity>>();
            builder.RegisterType<CategoryLookupProvider>().As<ITreeViewLookupProvider<Category>>();
          
            //builder.RegisterType<MeetingDetailViewModel>()
            //    .Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<FilesOnDriveViewModel>().AsSelf();

            builder.RegisterType<DriveRepository>().As<IDriveRepository>();
            builder.RegisterType<ArchiveEntityRepository>().As<IArchiveEntityRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<CategoryNavigationViewModel>().As<ICategoryNavigationViewModel>();
            //builder.RegisterType<ProgrammingLanguageRepository>().As<IProgrammingLanguageRepository>();

            builder.RegisterType<SearchResultViewModel>().As<ISearchResultViewModel>();
            builder.RegisterType<SearchNavigationViewModel>().As<ISearchNavigationViewModel>();
            builder.RegisterType<TagNavigationViewModel>().As<ITagNavigationViewModel>();
            


            builder.RegisterType<SearchEngineViewModel>().AsSelf();

            builder.RegisterType<MainNavigationViewModel>().AsSelf();
            
            return builder.Build();
        }


    }
}
