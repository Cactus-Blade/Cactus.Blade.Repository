using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Cactus.Blade.Repository
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context) : base(context)
        {
        }

        public void Dispose() => DbContext.Dispose();

        public T SingleOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<T> queryable = DbSet;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (predicate.IsNotNull())
                queryable = queryable.Where(predicate!);

            if (!ignoreQueryFilters)
                queryable = queryable.IgnoreQueryFilters();

            if (include.IsNotNull())
                queryable = include!(queryable);

            if (orderBy.IsNotNull())
                queryable = orderBy!(queryable);

            return queryable.FirstOrDefault();
        }

        public T Insert(T entity) => DbSet.Add(entity).Entity;

        public void Insert(params T[] entities) => DbSet.AddRange(entities);

        public void Insert(IEnumerable<T> entities) => DbSet.AddRange(entities);

        public void Update(T entity) => DbSet.Update(entity);

        public void Update(params T[] entities) => DbSet.UpdateRange(entities);

        public void Update(IEnumerable<T> entities) => DbSet.UpdateRange(entities);

        public void Delete(T entity) => DbSet.Remove(entity);

        public void Delete(params T[] entities) => DbSet.RemoveRange(entities);

        public void Delete(IEnumerable<T> entities) => DbSet.RemoveRange(entities);
    }
}
