using TheStandard.Asp.NetCore.Tests.Acceptance.Brokers.ApiBrokers;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.APIs.Home
{
    [Collection(nameof(ApiBrokerCollection))]
    public partial class HomeApiTest
    {
        private readonly ApiBrokerTest apiBrokerTest;

        public HomeApiTest(ApiBrokerTest apiBrokerTest) =>
            this.apiBrokerTest = apiBrokerTest;
    }
}
