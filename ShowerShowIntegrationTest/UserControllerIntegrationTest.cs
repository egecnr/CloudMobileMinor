using FluentAssertions;
using ShowerShow.DTO;
using Xunit.Abstractions;

namespace ShowerShowIntegrationTest
{
    public class UserControllerIntegrationTest :ControllerBase
    {
        public UserControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper){}

        [Fact]
        public async Task GetUserByNameShouldReturnAListOfUsersWithStatusCodeOk()
        {
            string userName = "te"; //For test
            string requestUri = $"/user/{userName}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<GetUserDTO>>();
            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(1);

        }
    }
}