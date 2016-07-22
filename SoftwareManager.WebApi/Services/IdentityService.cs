using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.UI.WebControls;
using SoftwareManager.BLL.Models;
using SoftwareManager.BLL.Services;
using Microsoft.Practices.Unity;

namespace SoftwareManager.WebApi.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserProfileService _userProfileService;

        public Guid Id { get; set; }

        public bool IsAuthenticated
        {
            get { return CurrentUser?.Id > 0; }
        }

        public bool IsAdmin
        {
            get { return IsAuthenticated && CurrentUser.IsAdmin && CurrentUser.IsActive; }
        }

        public IUserProfil CurrentUser { get; set; }

        public IdentityService(IUserProfileService userProfileService)
        {
            Id = Guid.NewGuid();
            _userProfileService = userProfileService;
        }

        public async Task<IUserProfil> Authenticate(string userName)
        {
            var userProfile = await _userProfileService.GetUserProfileAsync(userName);
            if (userProfile != null && userProfile.IsActive)
            {
                return userProfile;
            }

            return null;
        }
    }
}