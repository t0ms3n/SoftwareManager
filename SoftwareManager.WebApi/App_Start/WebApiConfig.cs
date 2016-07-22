using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using SoftwareManager.BLL.Services;
using SoftwareManager.Entities;
using SoftwareManager.WebApi.Controllers;
using SoftwareManager.WebApi.Services;
using Microsoft.OData.Edm;

namespace SoftwareManager.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            //corsAttr.SupportsCredentials = true;
            config.EnableCors(corsAttr);

            config.MapODataServiceRoute(
             "ODataRoute",
             "odata",
             GetEdmModel());

            config.EnsureInitialized();

            config.Filters.Add(new AuthorizeWithIdentityResolveAttribute());
            
            // Web API configuration and services
            // Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "SoftwareManager";
            builder.ContainerName = "SoftwareManagerContainer";

            var entityType = builder.EntitySet<Entities.Application>("Applications");
            builder.EntitySet<Entities.ApplicationApplicationManager>("ApplicationApplicationManagers");
            builder.EntitySet<Entities.ApplicationVersion>("ApplicationVersions");
            builder.EntitySet<Entities.ApplicationManager>("ApplicationManagers");
            
            builder.EnableLowerCamelCase();
            var model = builder.GetEdmModel();

            return model;
        }
    }
}
