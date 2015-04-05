using System.Web.Mvc;

namespace Zombie
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahExceptionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
