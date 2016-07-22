using System.ComponentModel.Composition;
using SoftwareManager.BLL.Services;
using SoftwareManager.Common.DependencyInjection;
using SoftwareManager.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity;

namespace SoftwareManager.WebApi
{
    [Export(typeof(IModule))]
    public class ModuleInit : IModule
    {
        public void Initialize(IModuleRegistrar moduleRegistrar)
        {
            moduleRegistrar.RegisterType<IIdentityService, IdentityService>(ServiceLifetime.Scoped);
        }
    }
}
