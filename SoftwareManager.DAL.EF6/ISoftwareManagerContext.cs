using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using SoftwareManager.Entities;


namespace SoftwareManager.DAL.EF6
{
    public interface ISoftwareManagerContext : IDisposable
    {
        DbSet<ApplicationApplicationManager> ApplicationApplicationManagers { get; set; }
        DbSet<ApplicationManager> ApplicationManagers { get; set; }
        DbSet<Application> Applications { get; set; }
        DbSet<ApplicationVersion> ApplicationVersions { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity;

        DbContextTransaction BeginTransaction();
        DbContextTransaction BeginTransaction(IsolationLevel level);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}