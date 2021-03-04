using System.Collections.Generic;

namespace Cactus.Blade.Repository.Paging
{
    public interface IPaginate<T>
    {
        public int From { get; }
        public int Index { get; }
        public int Size { get; }
        public int Count { get; }
        public int Pages { get; }
        public IList<T> Items { get; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }
    }
}
