using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.DAL.EF6.Configurations
{

    public class ApplicationVersionConfiguration : DateTrackedEntityConfiguration<ApplicationVersion>
    {
        public ApplicationVersionConfiguration() : base("ApplicationVersion")
        {
            Property(p => p.ApplicationId).HasColumnName("ApplicationId").IsRequired();
            Property(p => p.IsActive).HasColumnName("IsActive").IsRequired();
            Property(p => p.IsCurrent).HasColumnName("IsCurrent").IsRequired();
            Property(p => p.VersionNumber).HasColumnName("VersionNumber").HasMaxLength(32).IsRequired();
            Property(p => p.ReleaseDate).HasColumnName("ReleaseDate").IsRequired();

            HasRequired(a => a.CreateBy).WithMany(av => av.CreatedApplicationVersions).HasForeignKey(av => av.CreateById);
            HasOptional(a => a.ModifyBy).WithMany(av => av.ModifiedApplicationVersions).HasForeignKey(av => av.ModifyById);
        }
    }

}