using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Elmah;

namespace Zombie
{
    /// <summary>
    /// Logs all Web API exceptions for viewing through ELMAH
    /// </summary>
    /// <remarks>
    /// http://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/Elmah/Elmah.Server/ExceptionHandling/ElmahExceptionLogger.cs
    /// Repo version: 02/11/2015
    /// </remarks>
    public class ElmahWebApiExceptionLogger : ExceptionLogger
    {
        private const string HttpContextBaseKey = "MS_HttpContext";

        public override void Log(ExceptionLoggerContext context)
        {
            // Retrieve the current HttpContext instance for this request.
            var httpContext = GetHttpContext(context.Request);
            if (httpContext == null) return;

            // Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
            Exception exceptionToRaise = new HttpUnhandledException(null, context.Exception);

            // Send the exception to ELMAH (for logging, mailing, filtering, etc.).
            var signal = ErrorSignal.FromContext(httpContext);
            signal.Raise(exceptionToRaise, httpContext);
        }

        private static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            var contextBase = GetHttpContextBase(request);
            return contextBase == null ? null : ToHttpContext(contextBase);
        }

        private static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null) return null;

            object value;
            return !request.Properties.TryGetValue(HttpContextBaseKey, out value) ? null : value as HttpContextBase;
        }

        private static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }
    }
}
