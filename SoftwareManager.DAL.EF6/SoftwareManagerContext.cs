using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoftwareManager.Common.Services;
using SoftwareManager.DAL.EF6.Configurations;
using SoftwareManager.Entities;


namespace SoftwareManager.DAL.EF6
{
    public class SoftwareManagerContext : DbContext, ISoftwareManagerContext
    {
        private readonly IApplicationSettingService _settingService;

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationApplicationManager> ApplicationApplicationManagers { get; set; }
        public DbSet<ApplicationManager> ApplicationManagers { get; set; }
        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }
      
        static SoftwareManagerContext()
        {
            Database.SetInitializer<SoftwareManagerContext>(null);
        }

        public SoftwareManagerContext(IApplicationSettingService settingService) : base(new SqlConnection(settingService.AppSettings.SoftwareManagerConnection), true)
        {
            _settingService = settingService;
            base.Database.Log += s =>
            {
                Debug.WriteLine(s);
            };

        }
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new ApplicationApplicationManagerConfiguration());
            modelBuilder.Configurations.Add(new ApplicationManagerConfiguration());
            modelBuilder.Configurations.Add(new ApplicationVersionConfiguration());
        }

        public DbContextTransaction BeginTransaction()
        {
            return base.Database.BeginTransaction();
        }

        public DbContextTransaction BeginTransaction(IsolationLevel level)
        {
            return base.Database.BeginTransaction(level);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return base.Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return base.Entry(entity);
        }


    }








}
