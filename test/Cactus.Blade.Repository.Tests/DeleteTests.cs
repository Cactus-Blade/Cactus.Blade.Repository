using System;
using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class DeleteTests : IDisposable
    {
        public DeleteTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }


        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public void ShouldDeleteProduct()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);

            var get = uow.GetRepository<TestProduct>();


            uow.Commit();

            var prod = get.SingleOrDefault(x => x.Id == 1);
            get.Delete(prod);
            uow.Commit();

            prod = get.SingleOrDefault(x => x.Id == 1);
            Assert.Null(prod);
        }
    }
}