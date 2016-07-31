using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.DAL.Contracts;
using SoftwareManager.DAL.Contracts.Models;
using SoftwareManager.DAL.Contracts.Repositories;

namespace SoftwareManager.DAL.EF6.Repositories
{
    public class TrackedGenericRepository<TEntity> : GenericRepository<TEntity> where TEntity : class, IDateTrackedEntity, IEntity
    {
        
        public TrackedGenericRepository(ISoftwareManagerContext context) : base(context)
        {
            
        }

        public override void Add(TEntity entity)
        {
            //ToDo: Current User Id
            entity.CreateDate = DateTime.UtcNow;
            base.Add(entity);
        }

        public override void Update(TEntity entity)
        {
            //ToDo: Current User Id
            entity.ModifyDate = DateTime.UtcNow;
            base.Update(entity);
        }
    }
}
