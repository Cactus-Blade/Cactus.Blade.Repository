using System.Collections.Generic;

namespace Cactus.Blade.Repository
{
    public interface IDeleteRepository<in T> where T : class
    {
        void Delete(T entity);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
    }
}
