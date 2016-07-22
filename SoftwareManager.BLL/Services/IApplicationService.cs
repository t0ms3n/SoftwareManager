using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.Entities;

namespace SoftwareManager.BLL.Services
{
    public interface IApplicationService
    {
        IQueryable<Application> FindApplications(params string[] expand);
        IQueryable<Application> FindApplication(int id);

        Task<Application> GetApplicationAsync(int id);

        Task CreateApplication(Application application);
        Task<Application> UpdateApplication(int key, Application application);
        Task DeleteApplication(int key);

        Task CreateApplicationManagerAssociation(int applicationId, int applicationManagerId);
        Task UpdateApplicationManagerAssociation(int applicationId, int currentApplicationManagerId, int newApplicationManagerId);
        Task DeleteApplicationManagerAssociation(int applicationId, int applicationManagerId);
    }

    
}
