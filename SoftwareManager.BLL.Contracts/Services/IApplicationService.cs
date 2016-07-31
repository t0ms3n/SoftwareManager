using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Contracts.Services
{
    public interface IApplicationService
    {
        IQueryable<Application> FindApplications(params string[] membersToExpand);
        IQueryable<Application> FindApplications(Expression<Func<Application, bool>> query, params string[] membersToExpand);
        IQueryable<Application> FindApplication(params string[] membersToExpand);
        IQueryable<Application> FindApplication(Expression<Func<Application, bool>> query, params string[] membersToExpand);
        Task<Application> GetApplicationAsync(int id);
        Task CreateApplication(Application applicationManager);
        Task<Application> UpdateApplication(int key, Application applicationManager);
        Task DeleteApplication(int key);

        // Relations to ApplicationManagers
        Task CreateApplicationManagerAssociation(int applicationId, int applicationManagerId);
        Task UpdateApplicationManagerAssociation(int applicationId, int currentApplicationManagerId, int newApplicationManagerId);
        Task DeleteApplicationManagerAssociation(int applicationId, int applicationManagerId);

    }
}