using System;
using System.Threading.Tasks;
using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class UpdateAsyncTests : IDisposable
    {
        public UpdateAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldUpdateProductName()
        {
            const string newProductName = "Foo Bar";
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetRepository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 1);

            Assert.IsAssignableFrom<TestProduct>(product);

            product.Name = newProductName;

            repo.Update(product);

            await uow.CommitAsync();

            var updatedProduct = repo.SingleOrDefault(x => x.Id == 1);

            Assert.Equal(updatedProduct.Name, newProductName);
        }
    }
}