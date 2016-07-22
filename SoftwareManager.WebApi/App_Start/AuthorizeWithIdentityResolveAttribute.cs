using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using SoftwareManager.BLL.Services;
using SoftwareManager.Entities;
using SoftwareManager.WebApi.Controllers;
using SoftwareManager.WebApi.Services;
using Microsoft.OData.Edm;
using Microsoft.Practices.Unity;
using Unity.WebApi;

namespace SoftwareManager.WebApi
{

    public class AuthorizeWithIdentityResolveAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var isAuthorized = base.IsAuthorized(actionContext);
            IIdentityService identyService = null;

            identyService = actionContext.Request.GetDependencyScope().GetService(typeof(IdentityService)) as IdentityService;

            if (isAuthorized)
            {
                var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
                string loginName = identity.Name;
                isAuthorized = AuthenticateUser(loginName, identyService);
            }
            else
            {
                // Just for Demo
                string loginName = "domain.de\\AdminDemo";
                isAuthorized = AuthenticateUser(loginName, identyService);
            }

            return isAuthorized;
        }

        private bool AuthenticateUser(string loginName, IIdentityService identityService)
        {
            try
            {
                if (identityService != null && !identityService.IsAuthenticated)
                {
                    identityService.CurrentUser =
                        Task.Run(async () => await identityService.Authenticate(loginName)).Result;
                    return identityService.IsAuthenticated;
                }
            }
            catch (Exception)
            {
                //Log unauthorized
            }
            return false;
        }
    }



}