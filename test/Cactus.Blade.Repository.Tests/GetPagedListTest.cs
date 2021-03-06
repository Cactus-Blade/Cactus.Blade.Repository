using System;
using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class GetPagedListTest : IDisposable
    {
        public GetPagedListTest(SqlLiteWith20ProductsTestFixture fixture)
        {
            _testFixture = fixture;
        }

        public void Dispose()
        {
            _testFixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _testFixture;

        [Fact]
        public void GetProductPagedListUsingPredicateTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.GetRepository<TestProduct>();
            //Act
            var productList = repo.Select(x => x.CategoryId == 1).Items;
            //Assert
            Assert.Equal(5, productList.Count);
        }

        [Fact]
        public void ShouldBeReadOnlyInterface()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            //Act
            var repo = uow.GetReadOnlyRepository<TestProduct>();
            //Assert
            Assert.IsAssignableFrom<IRepositoryReadOnly<TestProduct>>(repo);
        }

        [Fact]
        public void ShouldGet5ProductsOutOfStockMultiPredicateTest()
        {
            // Arrange
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.GetRepository<TestProduct>();
            //Act
            var productList = repo.Select(x => x.Stock == 0 && x.InStock.Value == false).Items;
            //Assert
            Assert.Equal(5, productList.Count);
        }

        [Fact]
        public void ShouldReadFromProducts()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();
            //Act 
            var products = repo.Select().Items;
            //Assert
            Assert.Equal(20, products.Count);
        }

        [Fact]
        public void ShouldGetListWithSelectedColumns()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.GetReadOnlyRepository<TestProduct>();

            var list = repo.Select(s => new
            {
                ProductName = s.Name,
                StockLevel = s.Stock
            });

            Assert.NotNull(list);
            Assert.Equal(20, list.Items.Count);
        }
    }
}