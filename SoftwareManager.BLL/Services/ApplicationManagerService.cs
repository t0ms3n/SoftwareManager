using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.BLL.Extensions;
using SoftwareManager.DAL.Contracts;
using DataModels = SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.BLL.Services
{
    public class ApplicationManagerService : ServiceBase, IApplicationManagerService
    {
        private readonly IValidator<ApplicationManager> _applicationManagerValidator;

        public ApplicationManagerService(ISoftwareManagerUoW softwareManagerUoW, IIdentityService identityService, IValidator<ApplicationManager> applicationManagerValidator ) : base(identityService, softwareManagerUoW)
        {
            _applicationManagerValidator = applicationManagerValidator;
        }

        public IQueryable<ApplicationManager> FindApplicationManagers(params string[] membersToExpand)
        {
            var resultQuery = SoftwareManagerUoW.ApplicationManagerRepository.FindAll();
            return ProjectQuery<ApplicationManager, DataModels.ApplicationManager>(membersToExpand, resultQuery);
        }

        public IQueryable<ApplicationManager> FindApplicationManagers(Expression<Func<ApplicationManager, bool>> query, params string[] membersToExpand)
        {
            return FindApplicationManagers(membersToExpand).Where(query);
        }

        public IQueryable<ApplicationManager> FindApplicationManager(params string[] membersToExpand)
        {
            var resultQuery = SoftwareManagerUoW.ApplicationManagerRepository.FindOne();
            return ProjectQuery<ApplicationManager, DataModels.ApplicationManager>(membersToExpand, resultQuery);
        }

        public IQueryable<ApplicationManager> FindApplicationManager(Expression<Func<ApplicationManager, bool>> query, params string[] membersToExpand)
        {
            return FindApplicationManagers(membersToExpand: membersToExpand).Where(query).Take(1);
        }

        public async Task<ApplicationManager> GetApplicationManagerAsync(int id)
        {
            var applicationManager = await SoftwareManagerUoW
                                            .ApplicationManagerRepository
                                            .FirstOrDefaultAsync(f => f.Id == id, e => e.Applications);

            return Mapper.Map<ApplicationManager>(applicationManager);
        }

        public async Task CreateApplicationManager(ApplicationManager applicationManager)
        {
            // Validate model
            var validationResult = await _applicationManagerValidator.ValidateAsync(new ValidationContext<ApplicationManager>(applicationManager));
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException("Model is invalid", validationResult.ExtractModelErrors());
            }

            IdentityService.CheckAdminRole();

            var internalApplication = Mapper.Map<DataModels.ApplicationManager>(applicationManager);

            await CreateOrUseTransaction(async () =>
            {
                SoftwareManagerUoW.ApplicationManagerRepository.Add(internalApplication);
                await SoftwareManagerUoW.SaveAsync();
            });

            Mapper.Map(internalApplication, applicationManager);
        }

        public async Task<ApplicationManager> UpdateApplicationManager(int key, ApplicationManager applicationManager)
        {
            // Validate model
            var validationResult = await _applicationManagerValidator.ValidateAsync(new ValidationContext<ApplicationManager>(applicationManager));
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException("Model is invalid", validationResult.ExtractModelErrors());
            }

            // Check admin access
            IdentityService.CheckAdminRole();

            var currentApplicationManager = await SoftwareManagerUoW.ApplicationManagerRepository.GetAsync(key);
            if (currentApplicationManager == null)
                throw new ItemNotFoundException($"ApplicationManager with id {key} not found");

            applicationManager.Id = currentApplicationManager.Id;
            Mapper.Map(applicationManager, currentApplicationManager);

            await CreateOrUseTransaction(async () =>
            {
                SoftwareManagerUoW.ApplicationManagerRepository.Update(currentApplicationManager);
                await SoftwareManagerUoW.SaveAsync();
            });

            var updatedManager = Mapper.Map<ApplicationManager>(currentApplicationManager);
            return updatedManager;
        }

        public async Task DeleteApplicationManager(int key)
        {
            IdentityService.CheckAdminRole();

            var currentApplicationManager = await SoftwareManagerUoW.ApplicationManagerRepository.GetAsync(key);

            if (currentApplicationManager == null)
                throw new ItemNotFoundException($"ApplicationManager with id {key} not found");

            await CreateOrUseTransaction(async () =>
            {
                SoftwareManagerUoW.ApplicationManagerRepository.Remove(currentApplicationManager);
                await SoftwareManagerUoW.SaveAsync();
            });
        }
    }
}