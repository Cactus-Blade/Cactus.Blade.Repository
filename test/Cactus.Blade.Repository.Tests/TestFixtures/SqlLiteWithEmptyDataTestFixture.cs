using System;
using Cactus.Blade.Repository.Database;
using Microsoft.EntityFrameworkCore;

namespace Cactus.Blade.Repository.Tests.TestFixtures
{
    public class SqlLiteWithEmptyDataTestFixture : IDisposable
    {
        public TestDbContext Context => SqlLiteInMemoryContext();

        public void Dispose()
        {
            Context?.Dispose();
        }

        private static TestDbContext SqlLiteInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new TestDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }
    }
}