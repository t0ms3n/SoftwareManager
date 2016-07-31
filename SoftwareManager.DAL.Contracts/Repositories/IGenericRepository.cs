﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoftwareManager.DAL.Contracts.Models;

namespace SoftwareManager.DAL.Contracts.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Die Entity mit der Id wird abgerufen.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(int id);

        /// <summary>
        /// Alle Entities werden abgerufen.
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// Alle Entities zu dem Filter passend werden abgerufen.
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Es wird ein Queryable zurückgeliefert, welches bereits über das Predicate gefiltert wurde
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Es wird ein Queryable zurückgeliefert für alle Entities zurückgegeben
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Es wird ein Queryable zurückgeliefert, welches maximal einen Datensatz liefert und bereits über das Predicate gefiltert wurde
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FindOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Es wird ein Queryable zurückgeliefert, welches maximal einen Datensatz liefert
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FindOne(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        ///  Es wird ein Queryable zurückgeliefert, welches bereits auf die angegebene Id eingeschränkt ist
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FindById(int id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
