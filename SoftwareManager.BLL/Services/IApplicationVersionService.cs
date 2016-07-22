using System.Linq;
using System.Threading.Tasks;
using SoftwareManager.Entities;

namespace SoftwareManager.BLL.Services
{
    public interface IApplicationVersionService
    {
        IQueryable<ApplicationVersion> FindApplicationVersions(params string[] expand);
        IQueryable<ApplicationVersion> FindApplicationVersion(int id);

        Task<ApplicationVersion> GetApplicationVersionAsync(int id);

        Task CreateApplicationVersion(ApplicationVersion applicationVersion);
        Task<ApplicationVersion> UpdateApplicationVersion(int key, ApplicationVersion applicationVersion);
        Task DeleteApplicationVersion(int key);
    }
}