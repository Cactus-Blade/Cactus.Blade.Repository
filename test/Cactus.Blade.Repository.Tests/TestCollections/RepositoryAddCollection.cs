using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests.TestCollections
{
    [CollectionDefinition("RepositoryAdd")]
    public class RepositoryAddCollection : ICollectionFixture<SqlLiteWithEmptyDataTestFixture>
    {
    }
}