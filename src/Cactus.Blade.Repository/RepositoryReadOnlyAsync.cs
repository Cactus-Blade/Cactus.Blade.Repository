using Cactus.Blade.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cactus.Blade.Repository
{
    public class RepositoryReadOnlyAsync<T> : RepositoryAsync<T>, IRepositoryReadOnlyAsync<T> where T : class
    {
        public RepositoryReadOnlyAsync(DbContext context) : base(context)
        {
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) =>
            SingleOrDefaultAsync(predicate, orderBy, include, false);

        public Task<IPaginate<T>> SelectAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20) =>
            SelectAsync(predicate, orderBy, include, index, size, false);

        public Task<IPaginate<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20,
            CancellationToken cancellationToken = default,
            bool ignoreQueryFilters = false) where TResult : class =>
            SelectAsync(selector, predicate, orderBy, include, index, size, false, cancellationToken,
                ignoreQueryFilters);
    }
}
