using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Services;

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
                if (identityService != null)
                {
                    identityService.CurrentUser =
                        Task.Run(async () => await identityService.Authenticate(loginName)).Result;
                    return identityService.IsAuthenticated;
                }
            }
            catch (Exception ex)
            {
                //Log unauthorized
                Debug.WriteLine($"Exception during authentication of {loginName}: {ex.Message}");
            }
            return false;
        }
    }
}