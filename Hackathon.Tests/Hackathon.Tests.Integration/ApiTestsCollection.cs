using Xunit;

namespace Hackathon.Tests.Integration;

[CollectionDefinition(nameof(ApiTestsCollection))]
public class ApiTestsCollection: ICollectionFixture<TestWebApplicationFactory>
{
}
