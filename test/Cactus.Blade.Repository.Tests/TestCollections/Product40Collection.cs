﻿using Cactus.Blade.Repository.Tests.TestFixtures;
using Xunit;

namespace Cactus.Blade.Repository.Tests.TestCollections
{
    [CollectionDefinition(GlobalTestStrings.Product40Collection)]
    public class Product40Collection : ICollectionFixture<SqlLiteWith40ProductsTestFixture>
    {
    }
}
