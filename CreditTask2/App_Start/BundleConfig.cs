using System.Web;
using System.Web.Optimization;

namespace CreditTask2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //Add required js for datatables
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/bootstrap-datepicker.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js"));

            //Added bootstrap for data tables here. Make sure path is correct!
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/bootstrap-datepicker3.standalone.css",
                      "~/Content/site.css"));

            //Custom Javascript for the site.
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/Scripts/site.js"
                ));

        }
    }
}
