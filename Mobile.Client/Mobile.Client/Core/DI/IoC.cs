using AvantCredit.Uploader.DI;
using Ninject;

namespace AvantCredit.Uploader.Core.DI
{
    public static class IoC
    {
        public static StandardKernel Container { get; set; }

        public static void Initialize()
        {
            var kernel = new StandardKernel(new AppBindingsModule());
            Container = kernel;
        }

        public static T Get<T>()
        {
            var item = Container.Get<T>();
            return item;
        }
    }
    
}