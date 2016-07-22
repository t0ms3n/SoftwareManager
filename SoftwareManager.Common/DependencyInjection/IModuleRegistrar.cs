using Microsoft.Extensions.DependencyInjection;

namespace SoftwareManager.Common.DependencyInjection
{
 
    public interface IModuleRegistrar
    {
        void RegisterType<TFrom, TTo>(ServiceLifetime lifeTime) where TTo : class, TFrom where TFrom : class;
    }

}