using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.BLL.Models;

namespace SoftwareManager.BLL.Services
{
    public interface IIdentityService
    {
        Guid Id { get; set; }
        bool IsAuthenticated { get; }

        bool IsAdmin { get; }

        
        IUserProfil CurrentUser { get; set; }

        Task<IUserProfil> Authenticate(string userName);
    }

    
}
