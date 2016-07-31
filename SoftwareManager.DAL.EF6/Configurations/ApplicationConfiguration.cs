using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.DAL.EF6.Configurations
{

    public class ApplicationConfiguration : DateTrackedEntityConfiguration<Application>
    {
        public ApplicationConfiguration() : base("Application")
        {
           Property(p => p.ApplicationIdentifier).HasColumnName("ApplicationIdentifier").IsRequired();
           Property(p => p.Name).HasColumnName("Name").HasMaxLength(255).IsRequired();

           HasMany(a => a.ApplicationVersions).WithRequired(av => av.Application).HasForeignKey(av => av.ApplicationId);

           HasRequired(a => a.CreateBy).WithMany(av => av.CreatedApplications).HasForeignKey(av => av.CreateById);
           HasOptional(a => a.ModifyBy).WithMany(av => av.ModifiedApplications).HasForeignKey(av => av.ModifyById);
        }
    }

}