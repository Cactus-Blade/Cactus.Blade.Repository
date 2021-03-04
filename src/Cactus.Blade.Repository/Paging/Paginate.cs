using System;
using System.Collections.Generic;
using System.Linq;

namespace Cactus.Blade.Repository.Paging
{
    public class Paginate<T> : IPaginate<T>
    {
        internal Paginate(IEnumerable<T> sources, int index, int size, int from)
        {
            var enumerable = sources as T[] ?? sources.ToArray();

            if (from > index)
                throw new ArgumentException($"From: {from} > Index: {index}, Must From <= Index");

            if (sources is IQueryable<T> queryable)
            {
                Index = index;
                Size = size;
                From = from;
                Count = queryable.Count();
                Pages = (Count / Size.ToDouble()).Ceiling().ToInt32();

                Items = queryable.Skip((Index - From) * Size).Take(Size).ToList();
            }
            else
            {
                Index = index;
                Size = size;
                From = from;

                Count = enumerable.Length;
                Pages = (Count / Size.ToDouble()).Ceiling().ToInt32();

                Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
            }
        }

        internal Paginate()
        {
            Items = Array.Empty<T>();
        }

        public int From { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public IList<T> Items { get; set; }
        public bool HasPrevious => Index - From > 0;
        public bool HasNext => Index - From + 1 < Pages;
    }

    internal class Paginate<TSource, TResult> : IPaginate<TResult>
    {
        public Paginate(IEnumerable<TSource> sources, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index,
            int size, int from)
        {
            var enumerable = sources as TSource[] ?? sources.ToArray();

            if (from > index)
                throw new ArgumentException($"From: {from} > Index: {index}, Must From <= Index");

            if (sources is IQueryable<TSource> queryable)
            {
                Index = index;
                Size = size;
                From = from;
                Count = queryable.Count();
                Pages = (Count / Size.ToDouble()).Ceiling().ToInt32();

                var items = queryable.Skip((Index - From) * Size).Take(Size).ToArray();

                Items = new List<TResult>(converter(items));
            }
            else
            {
                Index = index;
                Size = size;
                From = from;

                Count = enumerable.Length;
                Pages = (Count / Size.ToDouble()).Ceiling().ToInt32();

                var items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();

                Items = new List<TResult>(converter(items));
            }
        }

        public Paginate(IPaginate<TSource> sources, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            Index = sources.Index;
            Size = sources.Size;
            From = sources.From;
            Count = sources.Count;
            Pages = sources.Pages;

            Items = new List<TResult>(converter(sources.Items));
        }

        public int From { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public IList<TResult> Items { get; set; }
        public bool HasPrevious => Index - From > 0;
        public bool HasNext => Index - From + 1 < Pages;
    }

    public static class Paginate
    {
        public static IPaginate<T> Empty<T>() => new Paginate<T>();

        public static IPaginate<TResult> From<TSource, TResult>(IPaginate<TSource> sources,
            Func<IEnumerable<TSource>, IEnumerable<TResult>> converter) =>
            new Paginate<TSource, TResult>(sources, converter);
    }
}
