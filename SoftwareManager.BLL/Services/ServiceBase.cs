using System;
using SoftwareManager.BLL.Exceptions;

namespace SoftwareManager.BLL.Services
{
    public abstract class ServiceBase
    {
        private readonly IIdentityService _identityService;

        protected ServiceBase()
        {
            
        }

        protected ServiceBase(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        protected virtual void CheckAdminRole()
        {
            if (_identityService == null)
            {
                throw new NullReferenceException("IIdentityService can not be null. Use the appropriate constructor or override the method.");
            }

            if (!_identityService.IsAdmin)
            {
                throw new UserIsNotAnAdminException();
            }
        }
    }
}