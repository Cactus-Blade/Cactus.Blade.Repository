using Cactus.Blade.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Cactus.Blade.Repository
{
    public class RepositoryReadOnly<T> : BaseRepository<T>, IRepositoryReadOnly<T> where T : class
    {
        public RepositoryReadOnly(DbContext context) : base(context)
        {
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> queryable = DbSet;

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (orderBy.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.SingleOrDefault();
        }

        public IPaginate<T> Select(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20)
        {
            IQueryable<T> queryable = DbSet;

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (orderBy.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.ToPaginate(index, size);
        }

        public IPaginate<TResult> Select<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0, int size = 20) where TResult : class
        {
            IQueryable<T> queryable = DbSet;

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (orderBy.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.Select(selector).ToPaginate(index, size);
        }
    }
}
