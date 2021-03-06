﻿using Cactus.Blade.Repository.Database;
using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests
{
    public class RepositoryAddTest : IClassFixture<InMemoryTestFixture>
    {
        public RepositoryAddTest(InMemoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly InMemoryTestFixture _fixture;

        [Fact]
        public void ShouldAddNewProduct()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.GetRepository<TestProduct>();
            var newProduct = new TestProduct {Name = GlobalTestStrings.TestProductName};

            // Act
            repo.Insert(newProduct);
            uow.Commit();

            //Assert
            Assert.Equal(1, newProduct.Id);
        }
    }
}