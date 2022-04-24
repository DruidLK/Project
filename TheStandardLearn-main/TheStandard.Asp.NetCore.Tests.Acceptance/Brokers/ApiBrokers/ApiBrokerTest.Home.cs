using System.Threading.Tasks;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.Brokers.ApiBrokers
{
    public partial class ApiBrokerTest
    {
        private const string HomeUrl = "Api/Home";

        public async ValueTask<string> GetHome() =>
           await this.apiFactoryClient.GetContentStringAsync(HomeUrl);
    }
}
