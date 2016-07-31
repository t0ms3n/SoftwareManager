using System.Threading.Tasks;

namespace SoftwareManager.BLL.Tests
{
    public abstract class ContextSpecification
    {
        protected ContextSpecification()
        {
            ModuleInit.InitializeAutoMapper();

            EstablishContext();
            Because();
        }

        public abstract void EstablishContext();
        public abstract Task Because();
    }
}