using System.ComponentModel.Composition;
using SoftwareManager.Common.DependencyInjection;

namespace SoftwareManager.WebApi
{
    [Export(typeof(IModule))]
    public class ModuleInit : IModule
    {
        public void Initialize(IModuleRegistrar moduleRegistrar)
        {
            
        }
    }
}
