using System.Web.Mvc;
using Elmah;

namespace Zombie
{
    public class ElmahExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled) ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
        }
    }
}
