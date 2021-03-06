using Cactus.Blade.Repository.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cactus.Blade.Repository.Tests.TestFixtures
{
    public class InMemoryTestFixture : IDisposable
    {
        public TestDbContext Context => InMemoryContext();

        public void Dispose()
        {
            Context?.Dispose();
        }

        private static TestDbContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var context = new TestDbContext(options);

            return context;
        }
    }
}
