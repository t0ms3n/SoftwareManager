using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using SoftwareManager.BLL.Services;
using SoftwareManager.WebApi.Controllers;
using Microsoft.OData.Edm;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.WebApi.Handlers;

namespace SoftwareManager.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            //corsAttr.SupportsCredentials = true;
            config.EnableCors(corsAttr);

            ODataBatchHandler odataBatchHandler =
                new EntityFrameworkBatchHandler(GlobalConfiguration.DefaultServer);
            odataBatchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;
            odataBatchHandler.MessageQuotas.MaxPartsPerBatch = 10;


            config.MapODataServiceRoute("ODataRoute", "odata", GetEdmModel(), batchHandler: odataBatchHandler);

            config.EnsureInitialized();

            config.Filters.Add(new AuthorizeWithIdentityResolveAttribute());
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "SoftwareManager";
            builder.ContainerName = "SoftwareManagerContainer";

            builder.EntitySet<Application>("Applications");
            builder.EntitySet<ApplicationVersion>("ApplicationVersions");
            builder.EntitySet<ApplicationApplicationManager>("ApplicationApplicationManagers");
            builder.EntitySet<ApplicationManager>("ApplicationManagers");

            builder.EnableLowerCamelCase();
            var model = builder.GetEdmModel();

            return model;
        }
    }
}
