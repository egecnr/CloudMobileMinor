using ExtraFunction.Model;
using ExtraFunctionIntegrationTest;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace ExtraFunctionIntegrationTests
{
    public class TermsAndConditionsIntegrationTest : ControllerBase
    {
        public TermsAndConditionsIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task Get_TermsAndConditions_Should_Return_TermsAndConditions_Object()
        {
            string requestUri = "TermsAndCondition";

            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<TermsAndConditions>();


            assertVar.Should().NotBeNull();
            //assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);



        }

    }
}
