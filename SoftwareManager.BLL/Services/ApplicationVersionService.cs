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
    public class ApplicationVersionService : ServiceBase, IApplicationVersionService
    {
        private readonly IValidator<ApplicationVersion> _applicationVersionValidator;
        
        public ApplicationVersionService(ISoftwareManagerUoW softwareManagerUoW, IIdentityService identityService, IValidator<ApplicationVersion> applicationVersionValidator ) : base(identityService, softwareManagerUoW)
        {
            _applicationVersionValidator = applicationVersionValidator;
        }

        public IQueryable<ApplicationVersion> FindApplicationVersions(params string[] membersToExpand)
        {
            var resultQuery = SoftwareManagerUoW.ApplicationVersionRepository.FindAll();
            return ProjectQuery<ApplicationVersion, DataModels.ApplicationVersion>(membersToExpand, resultQuery);
        }
        public IQueryable<ApplicationVersion> FindApplicationVersions(Expression<Func<ApplicationVersion, bool>> query, params string[] membersToExpand)
        {
            return FindApplicationVersions(membersToExpand).Where(query);
        }

        public IQueryable<ApplicationVersion> FindApplicationVersion(params string[] membersToExpand)
        {
            var resultQuery = SoftwareManagerUoW.ApplicationVersionRepository.FindOne();
            return ProjectQuery<ApplicationVersion, DataModels.ApplicationVersion>(membersToExpand, resultQuery);
        }

        public IQueryable<ApplicationVersion> FindApplicationVersion(Expression<Func<ApplicationVersion, bool>> query, params string[] membersToExpand)
        {
            return FindApplicationVersions(membersToExpand).Where(query).Take(1);
        }

        public async Task<ApplicationVersion> GetApplicationVersionAsync(int id)
        {
            var applicationVersion = await SoftwareManagerUoW.ApplicationVersionRepository.FirstOrDefaultAsync(f => f.Id == id,
               e => e.Application
               );

            return Mapper.Map<ApplicationVersion>(applicationVersion);
        }

        public async Task CreateApplicationVersion(ApplicationVersion applicationVersion)
        {
            // Validate model
            var validationResult = await _applicationVersionValidator.ValidateAsync(new ValidationContext<ApplicationVersion>(applicationVersion));
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException("Model is invalid", validationResult.ExtractModelErrors());
            }

            var internalApplicationVersion = Mapper.Map<DataModels.ApplicationVersion>(applicationVersion);
            internalApplicationVersion.CreateById = IdentityService.CurrentUser.Id;

            await CreateOrUseTransaction(async () =>
            {
                SoftwareManagerUoW.ApplicationVersionRepository.Add(internalApplicationVersion);
                await SoftwareManagerUoW.SaveAsync();
            });

            Mapper.Map(internalApplicationVersion, applicationVersion);
        }

        public async Task<ApplicationVersion> UpdateApplicationVersion(int key, ApplicationVersion applicationVersion)
        {
            // Validate model
            var validationResult = await _applicationVersionValidator.ValidateAsync(new ValidationContext<ApplicationVersion>(applicationVersion));
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException("Model is invalid", validationResult.ExtractModelErrors());
            }

            var currentApplicationVersion = await SoftwareManagerUoW.ApplicationVersionRepository.GetAsync(key);

            if (currentApplicationVersion == null)
                throw new ItemNotFoundException($"ApplicationVersion with id {key} not found");

            applicationVersion.Id = currentApplicationVersion.Id;
            Mapper.Map(applicationVersion, currentApplicationVersion);

            await CreateOrUseTransaction(async () =>
            {
                currentApplicationVersion.ModifyById = IdentityService.CurrentUser.Id;
                SoftwareManagerUoW.ApplicationVersionRepository.Update(currentApplicationVersion);
                await SoftwareManagerUoW.SaveAsync();
            });

            var updatedApplicationVersion = Mapper.Map<ApplicationVersion>(currentApplicationVersion);
            return updatedApplicationVersion;
        }

        public async Task DeleteApplicationVersion(int key)
        {
            var currentApplicationVersion = await SoftwareManagerUoW.ApplicationVersionRepository.GetAsync(key);

            if (currentApplicationVersion == null)
                throw new ItemNotFoundException($"ApplicationVersion with id {key} not found");

            await CreateOrUseTransaction(async () =>
            {
                SoftwareManagerUoW.ApplicationVersionRepository.Remove(currentApplicationVersion);
                await SoftwareManagerUoW.SaveAsync();
            });
        }
    }
}