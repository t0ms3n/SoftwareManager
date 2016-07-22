using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SoftwareManager.BLL.Exceptions;
using AutoMapper;
using SoftwareManager.DAL.EF6;
using SoftwareManager.Entities;

namespace SoftwareManager.BLL.Services
{
    public class ApplicationService : ServiceBase, IApplicationService
    {
        private readonly ISoftwareManagerUoW _softwareUpdateManagerUoW;
        private readonly IIdentityService _identityService;

        public ApplicationService(ISoftwareManagerUoW softwareUpdateManagerUoW, IIdentityService identityService) : base(identityService)
        {
            _softwareUpdateManagerUoW = softwareUpdateManagerUoW;
            _identityService = identityService;
        }

        public IQueryable<Application> FindApplications(params string[] expand)
        {
            if (_identityService.IsAuthenticated)
            {
                if (_identityService.IsAdmin)
                    return _softwareUpdateManagerUoW.ApplicationRepository.FindAll();

                return _softwareUpdateManagerUoW.ApplicationRepository.FindAll().Where(w => w.ApplicationApplicationManagers.Any(a => a.ApplicationManagerId == _identityService.CurrentUser.Id));
            }

            return new List<Application>().AsQueryable();
        }

        public IQueryable<Application> FindApplication(int id)
        {
            if (_identityService.IsAuthenticated)
            {
                if (_identityService.IsAdmin)
                    return _softwareUpdateManagerUoW.ApplicationRepository.FindById(id);

                return _softwareUpdateManagerUoW.ApplicationRepository.FindAll().Where(w => w.Id == id &&  w.ApplicationApplicationManagers.Any(a => a.ApplicationManagerId == _identityService.CurrentUser.Id));
            }

            return new List<Application>().AsQueryable();
        }

        public async Task<Application> GetApplicationAsync(int id)
        {
            var application = await _softwareUpdateManagerUoW.ApplicationRepository.GetAsync(id);
            return Mapper.Map<Application>(application);
        }

        public async Task CreateApplication(Application application)
        {
            // Entity erstellen
            application.CreateById = _identityService.CurrentUser.Id;

            if (application.ApplicationIdentifier == Guid.Empty)
            {
                application.ApplicationIdentifier = Guid.NewGuid();
            }

            using (_softwareUpdateManagerUoW.Begin())
            {
                // Erstellung durchführen
                _softwareUpdateManagerUoW.ApplicationRepository.Add(application);

                // Ersteller als Manager der Software hinzufügen
                _softwareUpdateManagerUoW.ApplicationApplicationManagerRepository.Add(
                    new ApplicationApplicationManager()
                    {
                        Application = application,
                        CreateById = _identityService.CurrentUser.Id,
                        ApplicationManagerId = _identityService.CurrentUser.Id
                    });

                await _softwareUpdateManagerUoW.SaveAsync();

                _softwareUpdateManagerUoW.Commit();
            }
        }

        public async Task<Application> UpdateApplication(int key, Application application)
        {
            // Applikation abrufen und prüfen ob gefunden
            var currentApplication = await _softwareUpdateManagerUoW.ApplicationRepository.GetAsync(key);

            if (currentApplication == null)
                throw new EntityNotFoundException($"Application with id {key} not found");

            // Id des UpdateItems überschreiben und Werte auf aktuelles Item übertragen
            application.Id = currentApplication.Id;

            Mapper.Map(application, currentApplication);

            using (_softwareUpdateManagerUoW.Begin())
            {
                currentApplication.ModifyById = _identityService.CurrentUser.Id;
                // Aktualisierung durchführen
                _softwareUpdateManagerUoW.ApplicationRepository.Update(currentApplication);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }

            return currentApplication;
        }

        /// <summary>
        /// Löscht die Application
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DeleteApplication(int key)
        {
            // Applikation abrufen und prüfen ob gefunden
            var currentApplication = await _softwareUpdateManagerUoW.ApplicationRepository.GetAsync(key);

            if (currentApplication == null)
                throw new EntityNotFoundException($"Application with id {key} not found");

            using (_softwareUpdateManagerUoW.Begin())
            {
                // Löschen durchführen
                _softwareUpdateManagerUoW.ApplicationRepository.Remove(currentApplication);
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }

        /// <summary>
        /// Erstellt eine neue Verbindung zwischen Application und ApplicationManager
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationManagerId"></param>
        /// <returns></returns>
        public async Task CreateApplicationManagerAssociation(int applicationId, int applicationManagerId)
        {
            // Applikation abrufen und prüfen ob gefunden
            var currentApplication =
                await _softwareUpdateManagerUoW.ApplicationRepository.FindById(applicationId)
                    .Include(w => w.ApplicationApplicationManagers).FirstOrDefaultAsync();

            if (currentApplication == null)
                throw new EntityNotFoundException();

            // Ist der ApplicationManager bereits der Applikation zugewiesen?
            if (currentApplication.ApplicationApplicationManagers.Any(a => a.ApplicationManagerId == applicationManagerId))
                throw new BadRequestException();


            // Prüfen ob der ApplicationManager existiert
            var doesManagerExist = await _softwareUpdateManagerUoW.ApplicationManagerRepository.AnyAsync(a => a.Id == applicationManagerId);
            if (!doesManagerExist)
                throw new EntityNotFoundException();

            using (_softwareUpdateManagerUoW.Begin())
            {
                // Verbindung zwischen Manager und Applikation erstellen und speichern
                _softwareUpdateManagerUoW.ApplicationApplicationManagerRepository.Add(new ApplicationApplicationManager()
                {
                    ApplicationId = applicationId,
                    ApplicationManagerId = applicationManagerId,
                    CreateById = _identityService.CurrentUser.Id
                });
                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }

        /// <summary>
        /// Löscht eine vorhandene Verknüpfung zwischen einem ApplicationManager und einer Application und erstellt eine neue
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="currentApplicationManagerId"></param>
        /// <param name="newApplicationManagerId"></param>
        /// <returns></returns>
        public async Task UpdateApplicationManagerAssociation(int applicationId, int currentApplicationManagerId, int newApplicationManagerId)
        {
            // Applikation abrufen und prüfen ob gefunden
            var currentApplication =
                await _softwareUpdateManagerUoW.ApplicationRepository.FindById(applicationId)
                    .Include(w => w.ApplicationApplicationManagers).FirstOrDefaultAsync();

            if (currentApplication == null)
                throw new EntityNotFoundException();

            // Bisherige Verknüpfung abrufen und prüfen ob gefunden
            var currentManagerAssociation =
                currentApplication.ApplicationApplicationManagers.FirstOrDefault(a => a.ApplicationManagerId == currentApplicationManagerId);
            if (currentManagerAssociation == null)
                throw new BadRequestException();

            // Prüfen ob neuer ApplicationManager exisitiert
            var doesManagerExist = await _softwareUpdateManagerUoW.ApplicationManagerRepository.AnyAsync(a => a.Id == newApplicationManagerId);
            if (!doesManagerExist)
                throw new EntityNotFoundException();

            using (_softwareUpdateManagerUoW.Begin())
            {
                // Neue Verbindung erstellen
                _softwareUpdateManagerUoW.ApplicationApplicationManagerRepository.Add(new ApplicationApplicationManager()
                {
                    ApplicationId = applicationId,
                    ApplicationManagerId = newApplicationManagerId,
                    CreateById = _identityService.CurrentUser.Id
                });
                // Alte Verbindung löschen
                _softwareUpdateManagerUoW.ApplicationApplicationManagerRepository.Remove(currentManagerAssociation);

                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }

        /// <summary>
        /// Löscht eine Verknüfung zwischen einer Application und einem ApplicationManager
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationManagerId"></param>
        /// <returns></returns>
        public async Task DeleteApplicationManagerAssociation(int applicationId, int applicationManagerId)
        {
            // Applikation abrufen und prüfen ob gefunden
            var currentApplication = await _softwareUpdateManagerUoW.ApplicationRepository
                                            .FindById(applicationId)
                                            .Include(w => w.ApplicationApplicationManagers).FirstOrDefaultAsync();

            if (currentApplication == null)
                throw new EntityNotFoundException();

            // Verknüpfung abrufen und prüfen ob gefunden
            var currentManagerAssociation =
                currentApplication.ApplicationApplicationManagers.FirstOrDefault(a => a.ApplicationManagerId == applicationManagerId);
            if (currentManagerAssociation == null)
                throw new BadRequestException();

            using (_softwareUpdateManagerUoW.Begin())
            {
                // Verknüpfung löschen
                _softwareUpdateManagerUoW.ApplicationApplicationManagerRepository.Remove(currentManagerAssociation);

                await _softwareUpdateManagerUoW.SaveAsync();
                _softwareUpdateManagerUoW.Commit();
            }
        }
    }
}