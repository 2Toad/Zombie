using System.Web.Optimization;

namespace Zombie
{
    public class BundleConfig
    {
        /// <remarks>
        /// The order files are added to bundles are the same order the files are added to the page. 
        /// When making changes to the bundles, make sure bundled dependencies are added first.
        /// </remarks>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCss(bundles);
            RegisterJs(bundles);
        }

        private static void RegisterCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
                // Vendor
                .Include("~/app/css/vendor/bootstrap.css")
                .Include("~/app/css/vendor/font-awesome.css")
                .Include("~/app/css/vendor/toastr.css")

                // Application
                .Include("~/app/css/app.css")
            );
        }

        private static void RegisterJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                // Vendor
                .Include(
                    "~/app/js/vendor/jquery.js",
                    "~/app/js/vendor/lodash.js",
                    "~/app/js/vendor/bootstrap.js",
                    "~/app/js/vendor/spin.js",
                    "~/app/js/vendor/ladda.js",
                    "~/app/js/vendor/toastr.js",

                    // Angular
                    "~/app/js/vendor/angular.js",
                    "~/app/js/vendor/angular-route.js",
                    "~/app/js/vendor/angular-cookies.js",
                    "~/app/js/vendor/angular-sanitize.js",

                    // Angular-dependent
                    "~/app/js/vendor/angular-localizer.js",
                    "~/app/js/vendor/angular-errorz.js",
                    "~/app/js/vendor/barricade-angular.js",
                    "~/app/js/vendor/ui-bootstrap.js",
                    "~/app/js/vendor/ui-bootstrap-tpls.js",
                    "~/app/js/vendor/xtForm.js",
                    "~/app/js/vendor/angular-local-storage.js"
                )

                // Application
                .Include("~/app/js/app.js")
                .IncludeDirectory("~/app/js/services", "*.js", true)
                .IncludeDirectory("~/app/js/controllers", "*.js", true)
                .IncludeDirectory("~/app/js/directives", "*.js", true)
            );
        }
    }
}
