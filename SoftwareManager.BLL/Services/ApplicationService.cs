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
using SoftwareManager.BLL.Validators;
using SoftwareManager.DAL.Contracts;
using DataModels = SoftwareManager.DAL.Contracts.Models;


namespace SoftwareManager.BLL.Services
{
    public class ApplicationService : ServiceBase, IApplicationService
    {
        private readonly IValidator<Application> _applicationValidator;

        public ApplicationService(ISoftwareManagerUoW softwareManagerUoW, IIdentityService identityService, IValidator<Application> applicationValidator) : base(identityService, softwareManagerUoW)
        {
            _applicationValidator = applicationValidator;
        }

        public IQueryable<Application> FindApplications(params string[] membersToExpand)
        {
            var resultQuery = SoftwareManagerUoW.ApplicationRepository.FindAll();
            return ProjectQuery<Application, DataModels.Application>(membersToExpand, resultQuery);
        }

        public IQueryable<Application> FindApplications(Expression<Func<Application, bool>> query, params string[] membersToExpand)
        {
            return FindApplications(membersToExpand).Where(query);
        }

        public IQueryable<Application> FindApplication(params string[] membersToExpand)
        {
            var resultQuery = SoftwareManagerUoW.ApplicationRepository.FindOne();
            return ProjectQuery<Application, DataModels.Application>(membersToExpand, resultQuery);
        }

        public IQueryable<Application> FindApplication(Expression<Func<Application, bool>> query, params string[] membersToExpand)
        {
            return FindApplications(membersToExpand: membersToExpand).Where(query).Take(1);
        }

        public async Task<Application> GetApplicationAsync(int id)
        {
            var application = await SoftwareManagerUoW.ApplicationRepository.FirstOrDefaultAsync(f => f.Id == id,
                e => e.ApplicationApplicationManagers
                , e => e.ApplicationApplicationManagers.Select(e2 => e2.ApplicationManager)
                , e => e.ApplicationVersions);

            return Mapper.Map<Application>(application);
        }

        public async Task CreateApplication(Application application)
        {
            if(application.Identifier == default(Guid))
            {
                application.Identifier = Guid.NewGuid();
            }

            var validationResult = await _applicationValidator.ValidateAsync(new ValidationContext<Application>(application));
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException("Model is invalid", validationResult.ExtractModelErrors());
            }

            var internalApplication = Mapper.Map<DataModels.Application>(application);

            await CreateOrUseTransaction(async () =>
            {
                internalApplication.CreateById = IdentityService.CurrentUser.Id;
                SoftwareManagerUoW.ApplicationRepository.Add(internalApplication);
                await SoftwareManagerUoW.SaveAsync();
            });

            Mapper.Map(internalApplication, application);
        }

        public async Task<Application> UpdateApplication(int key, Application application)
        {
            var validationResult = await _applicationValidator.ValidateAsync(new ValidationContext<Application>(application));
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException("Model is invalid", validationResult.ExtractModelErrors());
            }

            //ToDo: Check if User is responsible for application


            // Get application to update
            var currentApplication = await SoftwareManagerUoW.ApplicationRepository.GetAsync(key);

            if (currentApplication == null)
                throw new ItemNotFoundException($"Application with id {key} not found");

            application.Id = currentApplication.Id;
            //Map new values
            Mapper.Map(application, currentApplication);

            await CreateOrUseTransaction(async () =>
            {
                currentApplication.ModifyById = IdentityService.CurrentUser.Id;
                SoftwareManagerUoW.ApplicationRepository.Update(currentApplication);
                await SoftwareManagerUoW.SaveAsync();
            });

            var updateApplication = Mapper.Map<Application>(currentApplication);
            return updateApplication;
        }

        public async Task DeleteApplication(int key)
        {
            //Check if User is responsible for application
            IdentityService.CheckAdminRole();

            var currentApplication = await SoftwareManagerUoW.ApplicationRepository.GetAsync(key);

            if (currentApplication == null)
                throw new ItemNotFoundException($"Application with id {key} not found");

            await CreateOrUseTransaction(async () =>
            {
                SoftwareManagerUoW.ApplicationRepository.Remove(currentApplication);
                await SoftwareManagerUoW.SaveAsync();
            });
        }

        /// <summary>
        /// Creates a new association between the application and the application manager
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationManagerId"></param>
        /// <returns></returns>
        public async Task CreateApplicationManagerAssociation(int applicationId, int applicationManagerId)
        {
            // get application and check if found
            var currentApplication = await SoftwareManagerUoW.ApplicationRepository
                                         .FirstOrDefaultAsync(f => f.Id == applicationId, w => w.ApplicationApplicationManagers);
            if (currentApplication == null)
                throw new ItemNotFoundException();

            // check if manager is already association with the application
            if (currentApplication.ApplicationApplicationManagers.Any(a => a.ApplicationManagerId == applicationManagerId))
                throw new BadRequestException();


            // get and check if manager exists
            var doesManagerExist = await SoftwareManagerUoW.ApplicationManagerRepository.AnyAsync(a => a.Id == applicationManagerId);
            if (!doesManagerExist)
                throw new ItemNotFoundException();

            await CreateOrUseTransaction(async () =>
            {
                // create association between manager and application
                SoftwareManagerUoW.ApplicationApplicationManagerRepository.Add(new DataModels.
                    ApplicationApplicationManager()
                {
                    ApplicationId = applicationId,
                    ApplicationManagerId = applicationManagerId,
                    CreateById = IdentityService.CurrentUser.Id
                });
                await SoftwareManagerUoW.SaveAsync();
            });
        }

        /// <summary>
        /// Deletes an existing association between the application and the current application manager and creates a new association
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="currentApplicationManagerId"></param>
        /// <param name="newApplicationManagerId"></param>
        /// <returns></returns>
        public async Task UpdateApplicationManagerAssociation(int applicationId, int currentApplicationManagerId, int newApplicationManagerId)
        {
            // Get application and check if found
            var currentApplication = await SoftwareManagerUoW.ApplicationRepository
                                          .FirstOrDefaultAsync(f => f.Id == applicationId, w => w.ApplicationApplicationManagers);

            if (currentApplication == null)
                throw new ItemNotFoundException();

            // Get current association and check if found
            var currentManagerAssociation =
                currentApplication.ApplicationApplicationManagers.FirstOrDefault(a => a.ApplicationManagerId == currentApplicationManagerId);
            if (currentManagerAssociation == null)
                throw new BadRequestException();

            // Check if new manager exists
            var doesManagerExist = await SoftwareManagerUoW.ApplicationManagerRepository.AnyAsync(a => a.Id == newApplicationManagerId);
            if (!doesManagerExist)
                throw new ItemNotFoundException();

            await CreateOrUseTransaction(async () =>
            {
                // create new association
                SoftwareManagerUoW.ApplicationApplicationManagerRepository.Add(new DataModels.
                    ApplicationApplicationManager()
                {
                    ApplicationId = applicationId,
                    ApplicationManagerId = newApplicationManagerId,
                    CreateById = IdentityService.CurrentUser.Id
                });
                // delete old association
                SoftwareManagerUoW.ApplicationApplicationManagerRepository.Remove(currentManagerAssociation);

                await SoftwareManagerUoW.SaveAsync();
            });
        }

        /// <summary>
        /// Deletes the association between the application and the application manager
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationManagerId"></param>
        /// <returns></returns>
        public async Task DeleteApplicationManagerAssociation(int applicationId, int applicationManagerId)
        {
            // Get application and check if found
            var currentApplication = await SoftwareManagerUoW.ApplicationRepository
                                            .FirstOrDefaultAsync(f => f.Id == applicationId, w => w.ApplicationApplicationManagers);

            if (currentApplication == null)
                throw new ItemNotFoundException();

            // Get association and check if found
            var currentManagerAssociation =
                currentApplication.ApplicationApplicationManagers.FirstOrDefault(a => a.ApplicationManagerId == applicationManagerId);
            if (currentManagerAssociation == null)
                throw new BadRequestException();

            await CreateOrUseTransaction(async () =>
            {
                // Delete association
                SoftwareManagerUoW.ApplicationApplicationManagerRepository.Remove(currentManagerAssociation);

                await SoftwareManagerUoW.SaveAsync();
                SoftwareManagerUoW.Commit();
            });
        }
    }
}