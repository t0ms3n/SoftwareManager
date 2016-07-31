using System;
using System.Threading.Tasks;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Exceptions;

namespace SoftwareManager.BLL.Services
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

        public void CheckAdminRole()
        {
            if (!IsAdmin)
            {
                throw new UserIsNotAnActiveAdminException();
            }
        }
    }
}