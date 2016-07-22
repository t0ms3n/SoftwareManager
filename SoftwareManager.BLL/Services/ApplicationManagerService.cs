using System;
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
    public class ApplicationManagerService : ServiceBase, IApplicationManagerService
    {
        private readonly ISoftwareManagerUoW _softwareUpdateManagerUoW;
        private readonly IIdentityService _identityService;

        public ApplicationManagerService(ISoftwareManagerUoW softwareUpdateManagerUoW, IIdentityService identityService) : base(identityService)
        {
            _softwareUpdateManagerUoW = softwareUpdateManagerUoW;
            _identityService = identityService;
        }

        public IQueryable<ApplicationManager> FindApplicationManagers(params string[] expand)
        {
            var query = _softwareUpdateManagerUoW.ApplicationManagerRepository.FindAll();
            return query;
        }

        public IQueryable<ApplicationManager> FindApplicationManager(int id)
        {
            return _softwareUpdateManagerUoW.ApplicationManagerRepository.FindById(id);
        }

        public Task<ApplicationManager> GetApplicationManagerAsync(int id)
        {
            return _softwareUpdateManagerUoW.ApplicationManagerRepository.GetAsync(id);
        }
        
        public async Task CreateApplicationManager(ApplicationManager applicationManager)
        {
            CheckAdminRole();

            using (_softwareUpdateManagerUoW.Begin())
            {
                _softwareUpdateManagerUoW.ApplicationManagerRepository.Add(applicationManager);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }

        public async Task<ApplicationManager> UpdateApplicationManager(int key, ApplicationManager applicationManager)
        {
            //CheckAdminRole();

            var currentApplicationManager = await _softwareUpdateManagerUoW.ApplicationManagerRepository.GetAsync(key);

            if (currentApplicationManager == null)
                throw new EntityNotFoundException($"ApplicationManager with id {key} not found");

            applicationManager.Id = currentApplicationManager.Id;
            Mapper.Map(applicationManager, currentApplicationManager);

            using (_softwareUpdateManagerUoW.Begin())
            {
                _softwareUpdateManagerUoW.ApplicationManagerRepository.Update(currentApplicationManager);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
            return currentApplicationManager; 
        }

        public async Task DeleteApplicationManager(int key)
        {
            CheckAdminRole();

            var currentApplicationManager = await _softwareUpdateManagerUoW.ApplicationManagerRepository.GetAsync(key);

            if (currentApplicationManager == null)
                throw new EntityNotFoundException($"ApplicationManager with id {key} not found");

            using (_softwareUpdateManagerUoW.Begin())
            {
                _softwareUpdateManagerUoW.ApplicationManagerRepository.Remove(currentApplicationManager);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }
    }


}