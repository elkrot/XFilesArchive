using Autofac;
using XFilesArchive.DataAccess;
using XFilesArchive.UI.Services;
using XFilesArchive.UI.Services.Data;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap() {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<ArchiveDataService>().As<IArchiveDataService>();
            builder.RegisterType<XFilesArchiveDataContext>().AsSelf();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            return builder.Build();
        }


    }
}
