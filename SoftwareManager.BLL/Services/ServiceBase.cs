using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.DAL.Contracts;

namespace SoftwareManager.BLL.Services
{
    public abstract class ServiceBase
    {
        protected readonly ISoftwareManagerUoW SoftwareManagerUoW;
        protected readonly IIdentityService IdentityService;

        protected ServiceBase(ISoftwareManagerUoW softwareManagerUoW)
        {
            SoftwareManagerUoW = softwareManagerUoW;
        }

        protected ServiceBase(IIdentityService identityService, ISoftwareManagerUoW softwareManagerUoW) : this(softwareManagerUoW)
        {
            IdentityService = identityService;
        }


        /// <summary>
        /// Automatically uses the current available transaction. If no transaction is available a new transaction will be opened and will automatically commit
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task CreateOrUseTransaction(Func<Task> action)
        {
            if (SoftwareManagerUoW.Transaction != null)
            {
                await action();
            }
            else
            {
                using (SoftwareManagerUoW.Begin())
                {
                    await action();
                    SoftwareManagerUoW.Commit();
                }
            }
        }

        protected IQueryable<TOut> ProjectQuery<TOut, TIn>(
            Expression<Func<TOut, object>>[] membersToExpand, IQueryable<TIn> resultQuery)
        {
            if (membersToExpand.Length > 0)
            {
                return resultQuery.ProjectTo(parameters: null, membersToExpand: membersToExpand);
            }

            return resultQuery.ProjectTo<TOut>();
        }

        protected IQueryable<TOut> ProjectQuery<TOut, TIn>(string[] membersToExpand, IQueryable<TIn> resultQuery)
        {
            if (membersToExpand.Length > 0)
            {
                return resultQuery.ProjectTo<TOut>(parameters: null, membersToExpand: membersToExpand);
            }

            return resultQuery.ProjectTo<TOut>();
        }
    }
}