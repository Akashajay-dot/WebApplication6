using Google;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using WebApplication6.Controllers.Api;
using WebApplication6.Models;

namespace WebApplication6
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<AppdbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<AuthController>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}