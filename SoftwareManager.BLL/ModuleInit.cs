using System.ComponentModel.Composition;
using FluentValidation;
using SoftwareManager.BLL.Services;
using SoftwareManager.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BusinessModels = SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Validators;
using SoftwareManager.DAL.Contracts;
using DataModels = SoftwareManager.DAL.Contracts.Models;
using SoftwareManager.DAL.EF6;

namespace SoftwareManager.BLL
{
    [Export(typeof(IModule))]
    public class ModuleInit : IModule
    {
        public ModuleInit()
        {
            InitializeAutoMapper();
        }
        public void Initialize(IModuleRegistrar moduleRegistrar)
        {
            // Validators
            moduleRegistrar.RegisterType<IValidator<BusinessModels.ApplicationManager>, ApplicationManagerValidator>(ServiceLifetime.Singleton);
            moduleRegistrar.RegisterType<IValidator<BusinessModels.Application>, ApplicationValidator>(ServiceLifetime.Singleton);
            moduleRegistrar.RegisterType<IValidator<BusinessModels.ApplicationVersion>, ApplicationVersionValidator>(ServiceLifetime.Singleton);

            //Services
            moduleRegistrar.RegisterType<IApplicationManagerService, ApplicationManagerService>(ServiceLifetime.Transient);
            moduleRegistrar.RegisterType<IApplicationVersionService, ApplicationVersionService>(ServiceLifetime.Transient);
            moduleRegistrar.RegisterType<IApplicationService, ApplicationService>(ServiceLifetime.Transient);
            moduleRegistrar.RegisterType<IUserProfileService, UserProfileService>(ServiceLifetime.Transient);
            moduleRegistrar.RegisterType<IIdentityService, IdentityService>(ServiceLifetime.Scoped);

            // 
            moduleRegistrar.RegisterType<ISoftwareManagerContext, SoftwareManagerContext>(lifeTime: ServiceLifetime.Scoped);
            moduleRegistrar.RegisterType<ISoftwareManagerUoW, SoftwareManagerUoW>(lifeTime: ServiceLifetime.Scoped);
        }

        public static void InitializeAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DataModels.Application, BusinessModels.Application>()
                    .ForMember(f => f.Version, opt => opt.MapFrom(s => s.RowVersion))
                    .ForMember(f => f.Identifier, opt => opt.MapFrom(s => s.ApplicationIdentifier))
                    .ForMember(f => f.ApplicationVersions, opt => { opt.ExplicitExpansion(); })
                    .ForMember(f => f.ApplicationApplicationManagers, opt =>
                    {
                        opt.ExplicitExpansion();
                    }).MaxDepth(1);

                cfg.CreateMap<BusinessModels.Application, DataModels.Application>()
                    .ForMember(f => f.ApplicationIdentifier, opt => opt.MapFrom(s => s.Identifier))
                    .ForMember(f => f.RowVersion, opt => opt.MapFrom(s => s.Version)).MaxDepth(1);

                cfg.CreateMap<DataModels.ApplicationManager, BusinessModels.ApplicationManager>()
                    .ForMember(f => f.Version, opt => opt.MapFrom(s => s.RowVersion))
                    .ForMember(f => f.CreatedApplicationVersions, opt =>
                    {                
                        opt.ExplicitExpansion();
                    })
                    .ForMember(f => f.CreatedApplications, opt =>
                    {
                        opt.MapFrom(m => m.CreatedApplications);
                        opt.ExplicitExpansion();
                    })
                    .ForMember(f => f.MangerOfApplications, opt =>
                    {
                        opt.MapFrom(m => m.Applications);
                        opt.ExplicitExpansion();
                    })
                    .MaxDepth(1).ReverseMap();

                cfg.CreateMap<DataModels.ApplicationApplicationManager, BusinessModels.ApplicationApplicationManager>()
                    .ForMember(f => f.Version, opt => opt.MapFrom(s => s.RowVersion))
                    .ForMember(f => f.ApplicationManager, opt => { opt.ExplicitExpansion(); })
                    .ForMember(f => f.Application, opt => { opt.ExplicitExpansion(); })
                    .MaxDepth(1);

                cfg.CreateMap<DataModels.ApplicationVersion, BusinessModels.ApplicationVersion>()
                    .ForMember(f => f.Version, opt => opt.MapFrom(s => s.RowVersion)).MaxDepth(1).ReverseMap();

                cfg.CreateMap<DataModels.ApplicationManager, BusinessModels.UserProfile>();
            });
        }
    }
}
