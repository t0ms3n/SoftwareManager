using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.DAL.EF6.Configurations
{

    public class DateTrackedEntityConfiguration<TEntity> : EntityConfiguration<TEntity> where TEntity : class, IDateTrackedEntity, IEntity
    {
        public DateTrackedEntityConfiguration(string tableName, string schema = "dbo")
            : base(tableName, schema)
        {
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateById).HasColumnName("CreateById").IsRequired();
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsOptional();
            Property(x => x.ModifyById).HasColumnName("ModifyById").IsOptional();
        }
    }

}