using Etohum.NextStep.Web.Mapping;

namespace Etohum.NextStep.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            AutoMapperConfiguration.Configure();
           // NinjectWebCommon.Start();
        }
    }
}