using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoftwareManager.Entities;
using SoftwareManager.DAL.EF6.Models;
using SoftwareManager.DAL.EF6.Repositories;

namespace SoftwareManager.DAL.EF6
{
    public class SoftwareManagerUoW : ISoftwareManagerUoW
    {
        private readonly ISoftwareManagerContext _context;

        public DbContextTransaction Transaction { get; private set; }
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

        public DbContextTransaction Begin()
        {
            Transaction = _context.BeginTransaction();
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

        public async Task<DbSaveResult> SaveAsync()
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
