using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Contracts.Services
{
    public interface IApplicationManagerService
    {
        IQueryable<ApplicationManager> FindApplicationManagers(params string[] membersToExpand);
        IQueryable<ApplicationManager> FindApplicationManagers(Expression<Func<ApplicationManager, bool>> query, params string[] membersToExpand);

        IQueryable<ApplicationManager> FindApplicationManager(params string[] membersToExpand);
        IQueryable<ApplicationManager> FindApplicationManager(Expression<Func<ApplicationManager, bool>> query, params string[] membersToExpand);

        Task<ApplicationManager> GetApplicationManagerAsync(int id);
        Task CreateApplicationManager(ApplicationManager applicationManager);
        Task<ApplicationManager> UpdateApplicationManager(int key, ApplicationManager applicationManager);
        Task DeleteApplicationManager(int key);
    }
}