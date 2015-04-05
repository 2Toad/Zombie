using System.Web.Mvc;
using System.Web.Routing;

namespace Zombie
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{*path}", // Angular SPA
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
