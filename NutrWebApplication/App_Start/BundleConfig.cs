using System.Web;
using System.Web.Optimization;
using System.Web.Optimization.React;

namespace NutrWebApplication
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #region React 

            bundles.Add(new BabelBundle("~/bundles/react").Include(
                "~/Scripts/react/react.js",
                "~/Scripts/react/react-dom.js"
            ));

            #endregion

            #region CustomScripts 

            bundles.Add(new BabelBundle("~/CustomScripts/HelloWorld").Include(
            // Add your JSX files here 
                "~/CustomScripts/hello-world.jsx"
            ));

            bundles.Add(new BabelBundle("~/CustomScripts/StudentPage").Include(
            // Add your JSX files here 
                "~/CustomScripts/student-page.jsx"
            ));

            bundles.Add(new BabelBundle("~/CustomScripts/DishesPage").Include(
            // Add your JSX files here 
                "~/CustomScripts/dish-page.jsx"
            ));

            #endregion
        }
    }
}
