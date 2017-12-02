using System.Web;
using System.Web.Optimization;

namespace Etohum.NextStep.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/statics/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/statics/scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/statics/scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/statics/components/bootstrap-3.3.7/js/bootstrap.js",
                      "~/statics/scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/statics/components/bootstrap-3.3.7/css/bootstrap.css",
                      "~/statics/styles/home.css"));
        }
    }
}
