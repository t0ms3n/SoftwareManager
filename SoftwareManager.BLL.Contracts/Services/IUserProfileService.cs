using System.Linq;
using System.Threading.Tasks;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Contracts.Services
{
    public interface IUserProfileService
    {
        Task<IUserProfil> GetUserProfileAsync(string userName);
        IQueryable<IUserProfil> GetAllUserProfiles();
    }

}
