using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Zombie
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Error logging
            config.Services.Add(typeof(IExceptionLogger), new ElmahWebApiExceptionLogger());

            // Return JSON by default
            // To return XML pass text/xml as the request Accept header
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(h => h.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}");
        }
    }
}
