using SoftwareManager.Entities;

namespace SoftwareManager.DAL.EF6.Configurations
{

    public class ApplicationApplicationManagerConfiguration : DateTrackedEntityConfiguration<ApplicationApplicationManager>
    {
        public ApplicationApplicationManagerConfiguration() : base("ApplicationApplicationManager")
        {
            HasRequired(a => a.Application).WithMany(a => a.ApplicationApplicationManagers).HasForeignKey(a => a.ApplicationId);
            HasRequired(a => a.ApplicationManager).WithMany(a => a.Applications).HasForeignKey(a => a.ApplicationManagerId);

            HasRequired(a => a.CreateBy).WithMany(av => av.CreatedApplicationApplicationManagers).HasForeignKey(av => av.CreateById);
            HasOptional(a => a.ModifyBy).WithMany(av => av.ModifiedApplicationApplicationManagers).HasForeignKey(av => av.ModifyById);
                  
        }
    }

}