using System;
using System.Collections.Generic;

namespace Cactus.Blade.Repository.Paging
{
    public static class PaginateExtensions
    {
        public static IPaginate<T> ToPaginate<T>(this IEnumerable<T> sources, int index, int size, int from = 0) =>
            new Paginate<T>(sources, index, size, from);

        public static IPaginate<TResult> ToPaginate<TSource, TResult>(this IEnumerable<TSource> sources,
            Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index, int size, int from = 0) =>
            new Paginate<TSource, TResult>(sources, converter, index, size, from);
    }
}
