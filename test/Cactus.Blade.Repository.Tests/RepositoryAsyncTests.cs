using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class RepositoryAsyncTests : IDisposable
    {
        public RepositoryAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;


        [Fact]
        public async Task ShouldGet5ProductsAndIncludeCategoryInfo()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetRepositoryAsync<TestProduct>();

            var results = await repo.SelectAsync(t => t.InStock == true && t.CategoryId == 1,
                include: category => category.Include(x => x.Category),
                size: 5);

            Assert.Equal(5, results.Items.Count);
            Assert.Equal(4, results.Pages);
            Assert.IsAssignableFrom<TestCategory>(results.Items[0].Category);
            Assert.Equal("Name9", results.Items[0].Category.Name);
        }

        [Fact]
        public async Task ShouldGetFiveProductsInStockOnePage()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetRepositoryAsync<TestProduct>();

            var results = await repo.SelectAsync(t => t.InStock == true && t.CategoryId == 1,
                size: 5);

            Assert.Equal(5, results.Items.Count);
            Assert.Equal(4, results.Pages);
        }


        [Fact]
        public async Task ShouldGetSingleValueAsync()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetRepositoryAsync<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 3);

            Assert.NotNull(product);
            Assert.IsAssignableFrom<TestProduct>(product);
        }
    }
}