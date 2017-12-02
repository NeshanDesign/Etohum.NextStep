using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Etohum.NextStep.MQ;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common.WebHost;

namespace Etohum.NextStep.Web
{
    // let Ninject handle HttpApplication -> how app start, end, ...
    public class MvcApplication : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            return new StandardKernel(new StandardNinjectModule());
        }


        protected  override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas(); // we have one area in this app
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);// there is no custom action filter in this app
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);// bundling js & css files 
            Bootstrapper.Run();
        }
    }

    public class StandardNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IQueueProvider>().To<MsmqProvider>(); // simply every where in app you inject  IQueueProvider in controllers, Ninject in instantiated a MsmqProvider for it
        }
    }
}
