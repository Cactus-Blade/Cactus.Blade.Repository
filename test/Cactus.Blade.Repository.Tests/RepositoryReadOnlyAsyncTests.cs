using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class RepositoryReadOnlyAsyncTests : IDisposable
    {
        public RepositoryReadOnlyAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldGetListOfItems()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepositoryAsync<TestProduct>();

            var results = await repo.SelectAsync(t => t.InStock == true && t.CategoryId == 1,
                size: 5);

            Assert.Equal(5, results.Items.Count);
            Assert.Equal(4, results.Pages);
        }

        [Fact]
        public async Task ShouldGetSingleItem()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepositoryAsync<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 1);

            Assert.NotNull(product);
        }

        [Fact]
        public void ShouldReturnInstanceIfInterface()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepositoryAsync<TestProduct>();

            Assert.IsAssignableFrom<IRepositoryReadOnlyAsync<TestProduct>>(repo);
        }

        [Fact]
        public async Task ShouldReturnNotNullObject()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetReadOnlyRepositoryAsync<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 1200);

            Assert.NotNull(product);
        }
    }
}