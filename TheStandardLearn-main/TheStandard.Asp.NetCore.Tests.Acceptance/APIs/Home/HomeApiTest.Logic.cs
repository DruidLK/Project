using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TheStandard.Asp.NetCore.Tests.Acceptance.APIs.Home
{
    public partial class HomeApiTest
    {
        [Fact]
        public async Task ShouldGetHome()
        {
            // Arrange - Given
            string expectedString =
                "Thank You Mario! But the princess is in another castle.";

            // Act - When
            string actualString =
                await this.apiBrokerTest.GetHome();

            // Assert - Then
            actualString.Should().BeEquivalentTo(expectedString);
        }
    }
}
