using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.Brokers.ApiBrokers
{
    [CollectionDefinition(nameof(ApiBrokerCollection))]
    public class ApiBrokerCollection : ICollectionFixture<ApiBrokerTest>
    { }
}
