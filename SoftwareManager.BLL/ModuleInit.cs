using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.BLL.Services;
using SoftwareManager.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Entities = SoftwareManager.Entities;

namespace SoftwareManager.BLL
{
    [Export(typeof(IModule))]
    public class ModuleInit : IModule
    {
        public void Initialize(IModuleRegistrar moduleRegistrar)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Application, Entities.Application>()
                .ForMember(f => f.CreateById, opt => opt.Ignore() )
                .ForMember(f => f.CreateDate, opt => opt.Ignore())
                .ForMember(f => f.CreateBy, opt => opt.Ignore())
                .ForMember(f => f.ModifyBy, opt => opt.Ignore())
                .ForMember(f => f.ModifyById, opt => opt.Ignore())
                .ForMember(f => f.ModifyDate, opt => opt.Ignore())
                .ForMember(f => f.ApplicationApplicationManagers, opt => opt.Ignore())
                .ForMember(f => f.ApplicationVersions, opt => opt.Ignore());
                
                cfg.CreateMap<Entities.ApplicationManager, Models.UserProfile>();
            });

            moduleRegistrar.RegisterType<IApplicationManagerService, ApplicationManagerService>(ServiceLifetime.Transient);
            moduleRegistrar.RegisterType<IApplicationService, ApplicationService>(ServiceLifetime.Transient);
            moduleRegistrar.RegisterType<IApplicationVersionService, ApplicationVersionService>(ServiceLifetime.Transient);

            moduleRegistrar.RegisterType<IUserProfileService, UserProfileService>(ServiceLifetime.Transient);
           
        }
    }
}
