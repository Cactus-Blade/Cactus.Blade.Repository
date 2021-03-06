using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cactus.Blade.Repository
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        private Dictionary<(Type type, string name), object> _repositories;

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class =>
            GetOrAddRepository(typeof(TEntity), new Repository<TEntity>(Context))
                .AsOrDefault<IRepository<TEntity>>();

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class =>
                GetOrAddRepository(typeof(TEntity), new RepositoryAsync<TEntity>(Context))
                    .AsOrDefault<IRepositoryAsync<TEntity>>();

        public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class =>
            GetOrAddRepository(typeof(TEntity), new RepositoryReadOnly<TEntity>(Context))
                .AsOrDefault<IRepositoryReadOnly<TEntity>>();

        public IRepositoryReadOnlyAsync<TEntity> GetReadOnlyRepositoryAsync<TEntity>() where TEntity : class =>
            GetOrAddRepository(typeof(TEntity), new RepositoryReadOnlyAsync<TEntity>(Context))
                .AsOrDefault<IRepositoryReadOnlyAsync<TEntity>>();

        public IDeleteRepository<TEntity> DeleteRepository<TEntity>() where TEntity : class =>
            GetOrAddRepository(typeof(TEntity), new DeleteRepository<TEntity>(Context))
                .AsOrDefault<IDeleteRepository<TEntity>>();

        public int Commit(bool autoHistory = false)
        {
            if (autoHistory)
                Context.EnsureAutoHistory();

            return Context.SaveChanges();
        }

        public Task<int> CommitAsync(bool autoHistory = false)
        {
            if (autoHistory)
                Context.EnsureAutoHistory();

            return Context.SaveChangesAsync();
        }

        public void Dispose() => Context?.Dispose();

        public TContext Context { get; }

        public object GetOrAddRepository(Type type, object repo)
        {
            _repositories ??= new Dictionary<(Type type, string Name), object>();

            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository))
                return repository;

            _repositories.Add((type, repo.GetType().FullName), repo);

            return repo;
        }
    }
}
