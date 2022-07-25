using CarbonaraWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarbonaraWebAPITest.Util
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class TestCollection : ICollectionFixture<CustomWebApplicationFactory<Startup>>
    {
    }
}
/*
 * dotnet test --collect:"XPlat Code Coverage"
 * reportgenerator -reports:.\TestResults\5093b5da-3bbc-4413-9ecb-830ff4a9a9a6\coverage.cobertura.xml -targetdir:"coveragereport" -reporttypes:Html
 */