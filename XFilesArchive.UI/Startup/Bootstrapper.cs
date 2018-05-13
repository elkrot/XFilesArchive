using Autofac;
using Prism.Events;
using XFilesArchive.DataAccess;
using XFilesArchive.UI.Services;
using XFilesArchive.UI.Services.Data;
using XFilesArchive.UI.Services.Lookups;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap() {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            //builder.RegisterType<ArchiveDataService>().As<IArchiveDataService>();
            builder.RegisterType<XFilesArchiveDataContext>().AsSelf();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<DriveDetailViewModel>().As<IDriveDetailViewModel>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            return builder.Build();
        }


    }
}
