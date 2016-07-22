using System.Linq;
using System.Threading.Tasks;
using SoftwareManager.Entities;

namespace SoftwareManager.BLL.Services
{
    public interface IApplicationManagerService
    {
        IQueryable<ApplicationManager> FindApplicationManagers(params string[] expand);
        IQueryable<ApplicationManager> FindApplicationManager(int id);
        Task<ApplicationManager> GetApplicationManagerAsync(int id);
        Task CreateApplicationManager(ApplicationManager applicationManager);
        Task<ApplicationManager> UpdateApplicationManager(int key, ApplicationManager applicationManager);
        Task DeleteApplicationManager(int key);
    }
}