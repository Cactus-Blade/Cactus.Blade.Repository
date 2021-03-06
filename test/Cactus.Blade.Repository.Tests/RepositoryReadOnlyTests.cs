using System;
using System.Collections.Generic;
using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class RepositoryReadOnlyTests : IDisposable
    {
        public RepositoryReadOnlyTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public void ShouldGetItems()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 1);

            Assert.NotNull(product);
        }

        [Fact]
        public void ShouldGetListOfProducts()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();

            var products = repo.Select();

            Assert.NotNull(products);
        }

        [Fact]
        public void ShouldReturnAListOfProducts()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();

            var products = repo.Select(x => x.CategoryId == 1);

            Assert.NotNull(products);
            Assert.IsAssignableFrom<IEnumerable<TestProduct>>(products.Items);
        }

        [Fact]
        public void ShouldReturnInstanceIfInterface()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();

            Assert.IsAssignableFrom<IRepositoryReadOnly<TestProduct>>(repo);
        }

        [Fact]
        public void ShouldReturnNullObject()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 10001);

            Assert.Null(product);
        }
    }
}