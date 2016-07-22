using SoftwareManager.Entities;

namespace SoftwareManager.DAL.EF6.Configurations
{

    public class ApplicationManagerConfiguration : EntityConfiguration<ApplicationManager>
    {
        public ApplicationManagerConfiguration() : base("ApplicationManager")
        {
           Property(p => p.IsActive).HasColumnName("IsActive").IsRequired();
           Property(p => p.IsAdmin).HasColumnName("IsAdmin").IsRequired();
           Property(p => p.Name).HasColumnName("Name").HasMaxLength(255).IsRequired();
           Property(p => p.LoginName).HasColumnName("LoginName").HasMaxLength(100).IsRequired();
        }
    }

}