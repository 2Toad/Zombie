using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Barricade;
using Newtonsoft.Json.Serialization;

namespace Zombie
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SecurityContext.Configure(Config.PasswordIterations, Config.PasswordPepper, Config.BearerTokenKey,
                Config.AccessTokenHeader, Config.AccessTokenCacheDuration);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Use camelCase for all property names when serializing objects to JSON
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Check if the database is up-to-date, and apply any pending migrations if it is not
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SiteContext, Migrations.Configuration>());
        }
    }
}
