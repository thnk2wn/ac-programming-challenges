using AvantCredit.Uploader.Cloud;
using AvantCredit.Uploader.Core.Security;
using AvantCredit.Uploader.Core.Window;
using Ninject.Modules;

namespace AvantCredit.Uploader.DI
{
    class AppBindingsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserAuthService>().To<UserAuthService>().InSingletonScope();
            Bind<ILoginStoreService>().To<LoginStoreService>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IMessageBoxService>().To<MessageBoxService>();
            Bind<IDocumentUploadService>().To<DocumentUploadService>();
        }
    }
}