using Cactus.Blade.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Cactus.Blade.Repository
{
    public class BaseRepository<T> : IReadRepository<T> where T : class
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            DbSet = dbContext.Set<T>();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool enableTracking = true)
        {
            IQueryable<T> queryable = DbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.FirstOrDefault();
        }

        public IPaginate<T> Select(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20,
            bool enableTracking = true)
        {
            IQueryable<T> queryable = DbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.AsEnumerable().ToPaginate(index, size);
        }

        public IPaginate<TResult> Select<TSource, TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20,
            bool enableTracking = true) where TResult : class
        {
            IQueryable<T> queryable = DbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.Select(selector).AsEnumerable().ToPaginate(index, size);
        }
    }
}
