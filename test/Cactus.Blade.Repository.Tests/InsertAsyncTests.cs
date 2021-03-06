using System;
using System.Threading.Tasks;
using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using FizzWare.NBuilder;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class InsertAsyncTests : IDisposable
    {
        public InsertAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldInsertNewProductReturnCreatedEntity()
        {
            BuilderSetup.DisablePropertyNamingFor<TestProduct, int>(x => x.Id);
            var prod = Builder<TestProduct>.CreateNew().With(x => x.Name = "Cool Product").With(x => x.CategoryId = 1)
                .Build();
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);

            var repo = uow.GetRepositoryAsync<TestProduct>();

            var newProduct = await repo.InsertAsync(prod);
            await uow.CommitAsync();

            Assert.NotNull(newProduct);
            Assert.IsAssignableFrom<TestProduct>(newProduct.Entity);
            Assert.Equal(21, newProduct.Entity.Id);
        }
    }
}