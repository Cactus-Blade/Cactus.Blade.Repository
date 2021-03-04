using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cactus.Blade.Repository.Paging
{
    public static class QueryablePaginateExtensions
    {
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> sources, int index, int size,
            int from = 0, CancellationToken cancellationToken = default)
        {
            if (from > index)
                throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

            var count = await sources.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await sources.Skip((index - from) * size)
                .Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            var list = new Paginate<T>
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = (count / size.ToDouble()).Ceiling().ToInt32()
            };

            return list;
        }
    }
}
