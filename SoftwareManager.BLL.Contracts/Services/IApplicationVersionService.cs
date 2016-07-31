using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Contracts.Services
{
    public interface IApplicationVersionService
    {

        IQueryable<ApplicationVersion> FindApplicationVersions(params string[] membersToExpand);

        IQueryable<ApplicationVersion> FindApplicationVersions(Expression<Func<ApplicationVersion, bool>> query, params string[] membersToExpand);

        IQueryable<ApplicationVersion> FindApplicationVersion(params string[] membersToExpand);

        IQueryable<ApplicationVersion> FindApplicationVersion(Expression<Func<ApplicationVersion, bool>> query, params string[] membersToExpand);

        Task<ApplicationVersion> GetApplicationVersionAsync(int id);

        Task CreateApplicationVersion(ApplicationVersion applicationVersion);

        Task<ApplicationVersion> UpdateApplicationVersion(int key, ApplicationVersion applicationVersion);

        Task DeleteApplicationVersion(int key);
    }
}