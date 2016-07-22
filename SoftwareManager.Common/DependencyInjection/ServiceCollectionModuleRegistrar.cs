using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;

namespace SoftwareManager.Common.DependencyInjection
{
    internal class ServiceCollectionModuleRegistrar : IModuleRegistrar
    {
        private readonly IServiceCollection _container;
        public ServiceCollectionModuleRegistrar(IServiceCollection container)
        {
            _container = container;
        }

        public void RegisterType<TFrom, TTo>(ServiceLifetime lifetime) where TTo : class, TFrom where TFrom : class
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    _container.AddScoped<TFrom, TTo>();
                    break;
                case ServiceLifetime.Singleton:
                    _container.AddSingleton<TFrom, TTo>();
                    break;
                case ServiceLifetime.Transient:
                    _container.AddTransient<TFrom, TTo>();
                    break;


            }

        }
    }

}