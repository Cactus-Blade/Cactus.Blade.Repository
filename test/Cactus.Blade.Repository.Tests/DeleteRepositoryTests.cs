using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using System;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class DeleteRepositoryTests : IDisposable
    {
        private readonly SqlLiteWith20ProductsTestFixture _fixture;
        public DeleteRepositoryTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ShouldDeleteProduct()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);

            var getRepo = uow.GetRepository<TestProduct>();
            var delRepo = uow.DeleteRepository<TestProduct>();

            uow.Commit();

            var prod = getRepo.SingleOrDefault(x => x.Id == 1);
            delRepo.Delete(prod);
            uow.Commit();

            prod = getRepo.SingleOrDefault(x => x.Id == 1);
            Assert.Null(prod);
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}
