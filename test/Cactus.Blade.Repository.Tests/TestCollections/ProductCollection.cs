using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests.TestCollections
{
    [CollectionDefinition(GlobalTestStrings.ProductCollectionName)]
    public class ProductCollection : ICollectionFixture<SqlLiteWith20ProductsTestFixture>
    {
    }
}