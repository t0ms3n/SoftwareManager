namespace SoftwareManager.Common.DependencyInjection
{

    public interface IModule
    {
        void Initialize(IModuleRegistrar moduleRegistrar);
    }

}