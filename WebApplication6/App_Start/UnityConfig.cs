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

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<AppdbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<AuthController>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}