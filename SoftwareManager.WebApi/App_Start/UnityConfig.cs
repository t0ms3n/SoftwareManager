using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Xml.Schema;
using SoftwareManager.BLL.Services;
using SoftwareManager.Common.DependencyInjection;
using SoftwareManager.WebApi.Controllers;
using SoftwareManager.WebApi.Services;
using Unity.WebApi;

namespace SoftwareManager.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            UnityModuleLoader.LoadContainer(container, "./bin", "SoftwareManager.*.dll");
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}