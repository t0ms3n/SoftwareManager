using System;
using System.Threading.Tasks;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Contracts.Services
{
    public interface IIdentityService
    {
        Guid Id { get; set; }
        bool IsAuthenticated { get; }

        bool IsAdmin { get; }

        IUserProfil CurrentUser { get; set; }
        Task<IUserProfil> Authenticate(string userName);

        void CheckAdminRole();
    }

    
}
