using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoftwareManager.DAL.Contracts.Models;
using SoftwareManager.DAL.Contracts.Repositories;


namespace SoftwareManager.DAL.EF6.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ISoftwareManagerContext _context;
        public GenericRepository(ISoftwareManagerContext context)
        {
            _context = context;
        }

        public Task<TEntity> GetAsync(int id)
        {
            return _context.Set<TEntity>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _context.Set<TEntity>().ToListAsync();

        }
        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes).Where(predicate);
        }

        public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes);
        }

        public IQueryable<TEntity> FindOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes).Where(predicate).OrderBy(o => o.Id).Take(1);
        }

        public IQueryable<TEntity> FindOne(params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes).OrderBy(o => o.Id).Take(1);
        }

        private IQueryable<TEntity> ApplyIncludes(Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }

        public IQueryable<TEntity> FindById(int id)
        {
            return _context.Set<TEntity>().Where(w => w.Id == id);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return FirstOrDefaultAsync(predicate, includes: null);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return ApplyIncludes(includes).Where(predicate).FirstOrDefaultAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AnyAsync();
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
