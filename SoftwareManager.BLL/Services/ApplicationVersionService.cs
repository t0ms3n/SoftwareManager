using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.BLL.Extensions;
using SoftwareManager.DAL.EF6;
using SoftwareManager.Entities;

namespace SoftwareManager.BLL.Services
{
    public class ApplicationVersionService : ServiceBase, IApplicationVersionService
    {

        private readonly ISoftwareManagerUoW _softwareUpdateManagerUoW;
        private readonly IIdentityService _identityService;

        public ApplicationVersionService(ISoftwareManagerUoW softwareUpdateManagerUoW, IIdentityService identityService) : base(identityService)
        {
            _softwareUpdateManagerUoW = softwareUpdateManagerUoW;
            _identityService = identityService;
        }

        public IQueryable<ApplicationVersion> FindApplicationVersions(params string[] expand)
        {
            var query = _softwareUpdateManagerUoW.ApplicationVersionRepository.FindAll();
            return query;
        }

        public IQueryable<ApplicationVersion> FindApplicationVersion(int id)
        {
            return _softwareUpdateManagerUoW.ApplicationVersionRepository.FindById(id);
        }

        public Task<ApplicationVersion> GetApplicationVersionAsync(int id)
        {
            return _softwareUpdateManagerUoW.ApplicationVersionRepository.GetAsync(id);
        }

        public async Task CreateApplicationVersion(ApplicationVersion applicationVersion)
        {
            var newApplicationVersion = applicationVersion;
            newApplicationVersion.CreateById = _identityService.CurrentUser.Id;

            using (_softwareUpdateManagerUoW.Begin())
            {
                _softwareUpdateManagerUoW.ApplicationVersionRepository.Add(newApplicationVersion);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }

        public async Task<ApplicationVersion> UpdateApplicationVersion(int key, ApplicationVersion applicationVersion)
        {
            var currentApplicationVersion = await _softwareUpdateManagerUoW.ApplicationVersionRepository.GetAsync(key);

            if (currentApplicationVersion == null)
                throw new EntityNotFoundException($"ApplicationVersion with id {key} not found");

            applicationVersion.Id = currentApplicationVersion.Id;
            Mapper.Map(applicationVersion, currentApplicationVersion);

            using (_softwareUpdateManagerUoW.Begin())
            {
                currentApplicationVersion.ModifyById = _identityService.CurrentUser.Id;
                _softwareUpdateManagerUoW.ApplicationVersionRepository.Update(currentApplicationVersion);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
            return currentApplicationVersion;
        }

        public async Task DeleteApplicationVersion(int key)
        {
            var currentApplicationVersion = await _softwareUpdateManagerUoW.ApplicationVersionRepository.GetAsync(key);

            if (currentApplicationVersion == null)
                throw new EntityNotFoundException($"ApplicationVersion with id {key} not found");

            using (_softwareUpdateManagerUoW.Begin())
            {
                _softwareUpdateManagerUoW.ApplicationVersionRepository.Remove(currentApplicationVersion);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }
    }
}