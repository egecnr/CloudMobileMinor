using ExtraFunction.Model;
using System.Net;
using Xunit.Abstractions;
using ExtraFunctionIntegrationTest;
using FluentAssertions;

namespace ExtraFunctionIntegrationTests
{
    public class DisclaimersIntegrationTest : ControllerBase
    {
        public DisclaimersIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task Get_Disclaimers_Should_Return_Disclaimers_Object()
        {
            string requestUri = "Disclaimers";

            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<Disclaimers>();


            assertVar.Should().NotBeNull();
            //assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);



        }
    }
}
