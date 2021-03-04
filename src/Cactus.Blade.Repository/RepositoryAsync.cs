using Cactus.Blade.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cactus.Blade.Repository
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public RepositoryAsync(DbContext context)
        {
            var dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> queryable = _dbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (!predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (!ignoreQueryFilters)
                queryable = queryable.IgnoreQueryFilters();

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.SingleOrDefaultAsync();
        }

        public Task<IPaginate<T>> SelectAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20,
            bool enableTracking = true, CancellationToken cancellationToken = default, bool ignoreQueryFilters = false)
        {
            IQueryable<T> queryable = _dbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (!predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (!ignoreQueryFilters)
                queryable = queryable.IgnoreQueryFilters();

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.ToPaginateAsync(index, size, cancellationToken: cancellationToken);
        }

        public Task<IPaginate<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20,
            bool enableTracking = true, CancellationToken cancellationToken = default, bool ignoreQueryFilters = false) where TResult : class
        {
            IQueryable<T> queryable = _dbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (!predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (!ignoreQueryFilters)
                queryable = queryable.IgnoreQueryFilters();

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.Select(selector).ToPaginateAsync(index, size, cancellationToken: cancellationToken);
        }

        public ValueTask<EntityEntry<T>> InsertAsync(T entity, CancellationToken cancellationToken = default) => _dbSet.AddAsync(entity, cancellationToken);

        public Task InsertAsync(params T[] entities) => _dbSet.AddRangeAsync(entities);

        public Task InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => _dbSet.AddRangeAsync(entities, cancellationToken);
    }
}
