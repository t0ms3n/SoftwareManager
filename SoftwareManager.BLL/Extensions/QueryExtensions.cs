using System.Linq;
using AutoMapper.QueryableExtensions;

namespace SoftwareManager.BLL.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<TEntity> ProjectToWithExpand<TEntity>(this IQueryable query,
            params string[] expand)
        {
            expand = expand.Where(w => !string.IsNullOrEmpty(w)).ToArray();

            if (expand.Length > 0)
                return query.ProjectTo<TEntity>(parameters: null, membersToExpand: expand);

            return query.ProjectTo<TEntity>();

        }
    }

}