using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SoftwareManager.DAL.Contracts.Models;
using SoftwareManager.DAL.Contracts.Repositories;

namespace SoftwareManager.DAL.Contracts
{
    public interface ISoftwareManagerUoW: IDisposable
    {
        IDbTransaction Transaction { get; }

        IGenericRepository<Application> ApplicationRepository { get; set; }
        IGenericRepository<ApplicationApplicationManager> ApplicationApplicationManagerRepository { get; set; }
        IGenericRepository<ApplicationManager> ApplicationManagerRepository { get; set; }
        IGenericRepository<ApplicationVersion> ApplicationVersionRepository { get; set; }

        /// <summary>
        /// Beginnt eine Transaktion
        /// </summary>
        /// <returns></returns>
        IDbTransaction Begin();
        
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
        Task<IDbSaveResult> SaveAsync();
    }
}
