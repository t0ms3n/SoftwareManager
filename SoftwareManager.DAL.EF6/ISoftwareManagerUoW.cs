using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SoftwareManager.DAL.EF6.Models;
using SoftwareManager.DAL.EF6.Repositories;
using SoftwareManager.Entities;


namespace SoftwareManager.DAL.EF6
{
    public interface ISoftwareManagerUoW: IDisposable
    {
        IGenericRepository<Application> ApplicationRepository { get; set; }
        IGenericRepository<ApplicationApplicationManager> ApplicationApplicationManagerRepository { get; set; }
        IGenericRepository<ApplicationManager> ApplicationManagerRepository { get; set; }
        IGenericRepository<ApplicationVersion> ApplicationVersionRepository { get; set; }

        /// <summary>
        /// Beginnt eine Transaktion
        /// </summary>
        /// <returns></returns>
        DbContextTransaction Begin();
        
        /// <summary>
        /// Führt ein Commit auf die zuvor begonnene Transaktion aus
        /// </summary>
        void Commit();

        /// <summary>
        /// Führt ein Rollback auf die zuvor begonnene Transaktion aus
        /// </summary>
        void Rollback();

        /// <summary>
        /// Speichert alle vorgenommenen Änderungen
        /// </summary>
        Task<DbSaveResult> SaveAsync();
    }
}
