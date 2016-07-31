using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoftwareManager.DAL.Contracts;
using SoftwareManager.DAL.Contracts.Models;
using SoftwareManager.DAL.Contracts.Repositories;
using SoftwareManager.DAL.EF6.Models;
using SoftwareManager.DAL.EF6.Repositories;

namespace SoftwareManager.DAL.EF6
{
    public class SoftwareManagerUoW : ISoftwareManagerUoW
    {
        private readonly ISoftwareManagerContext _context;

        private DbContextTransaction _internalTransaction;

        public IDbTransaction Transaction
        {
            get {  return _internalTransaction?.UnderlyingTransaction; }
        }
        public IGenericRepository<Application> ApplicationRepository { get; set; }
        public IGenericRepository<ApplicationApplicationManager> ApplicationApplicationManagerRepository { get; set; }
        public IGenericRepository<ApplicationManager> ApplicationManagerRepository { get; set; }
        public IGenericRepository<ApplicationVersion> ApplicationVersionRepository { get; set; }

        public SoftwareManagerUoW(ISoftwareManagerContext context)
        {
            _context = context;
            ApplicationRepository = new TrackedGenericRepository<Application>(_context);
            ApplicationApplicationManagerRepository = new TrackedGenericRepository<ApplicationApplicationManager>(_context);
            ApplicationManagerRepository = new GenericRepository<ApplicationManager>(_context);
            ApplicationVersionRepository = new TrackedGenericRepository<ApplicationVersion>(_context);
        }

        public IDbTransaction Begin()
        {
            _internalTransaction = _context.BeginTransaction();
            return Transaction;
        }

        public void Commit()
        {
            if (Transaction == null)
            {
                throw new Exception("No transaction started");
            }
            Transaction.Commit();
        }

        public void Rollback()
        {
            if (Transaction == null)
            {
                throw new Exception("No transaction started");
            }
            Transaction.Rollback();
        }

        public async Task<IDbSaveResult> SaveAsync()
        {
            var result = await _context.SaveChangesAsync(CancellationToken.None);
            return new DbSaveResult()
            {
                AffectedRows = result
            };
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            _context.Dispose();
        }
    }
}
