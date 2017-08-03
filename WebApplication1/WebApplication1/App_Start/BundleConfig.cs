using System.Web;
using System.Web.Optimization;

namespace DeadLiner
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                      "~/Scripts/jquery-3.2.1.js",
                      "~/Scripts/jquery.validate*",
                      "~/Scripts/jquery.validate.unobtrusive.js",
                      "~/Scripts/DataTables/jquery.dataTables.js",
                      "~/Scripts/jquery-ui-1.12.1.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/easyResponsiveTabs.js",                                                                                       
                      "~/Scripts/DataTables/dataTables.bootstrap.js",                      
                      "~/Scripts/main.js",
                      "~/Scripts/tether/tether.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/respond.js"                      
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/themes/themes/jquery-ui.css",                  
                      "~/Content/edit.css",
                      "~/Content/main.css",
                      "~/Content/tether/tether.css"));
        }
    }
}
