using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Zombie.Services;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Zombie.AutofacConfig), "Start")]

namespace Zombie
{
    public static class AutofacConfig 
    {
        public static void Start() 
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterApiControllers(assembly);
            builder.Register(c => new SiteContext()).InstancePerRequest();
            builder.RegisterAssemblyTypes(assembly)
                .Where(x => typeof(IDbService).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
