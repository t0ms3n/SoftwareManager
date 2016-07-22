using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity;

namespace SoftwareManager.Common.DependencyInjection
{
    internal class UnityModuleRegistrar : IModuleRegistrar
    {
        private readonly IUnityContainer _container;
        public UnityModuleRegistrar(IUnityContainer container)
        {
            _container = container;
        }

        public void RegisterType<TFrom, TTo>(ServiceLifetime lifetime) where TTo : class, TFrom where TFrom : class
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    _container.RegisterType<TFrom, TTo>(new HierarchicalLifetimeManager());
                    break;
                case ServiceLifetime.Singleton:
                    _container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
                    break;
                case ServiceLifetime.Transient:
                    _container.RegisterType<TFrom, TTo>(new TransientLifetimeManager());
                    break;
            }

        }
    }
}