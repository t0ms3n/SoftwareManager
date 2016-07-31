using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.DAL.EF6.Configurations
{

    public class EntityConfiguration<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : class, IEntity
    {
        public EntityConfiguration(string tableName, string schema = "dbo")
            : base()
        {
            ToTable(tableName, schema);
            HasKey(x => x.Id);
            Property(p => p.Id)
                .HasColumnName($"{tableName}Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsConcurrencyToken().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed).IsFixedLength();

        }
    }

}