using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace SoftwareManager.DAL.EF6
{
    [Export(typeof(IModule))]
    public class ModuleInit : IModule
    {
        public void Initialize(IModuleRegistrar moduleRegistrar)
        {
            moduleRegistrar.RegisterType<ISoftwareManagerContext, SoftwareManagerContext>(lifeTime: ServiceLifetime.Scoped);
            moduleRegistrar.RegisterType<ISoftwareManagerUoW, SoftwareManagerUoW>(lifeTime: ServiceLifetime.Scoped);
        }
    }
}
