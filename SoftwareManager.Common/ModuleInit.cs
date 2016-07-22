using System.ComponentModel.Composition;
using SoftwareManager.Common.DependencyInjection;
using SoftwareManager.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SoftwareManager.Common
{
    [Export(typeof(IModule))]
    public class ModuleInit : IModule
    {
        public void Initialize(IModuleRegistrar moduleRegistrar)
        {
            moduleRegistrar.RegisterType<IApplicationSettingService, ApplicationSettingService>(lifeTime: ServiceLifetime.Singleton);
            
        }
    }
}
