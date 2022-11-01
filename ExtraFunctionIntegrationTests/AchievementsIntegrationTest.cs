using ExtraFunction.DTO_;
using Microsoft.AspNetCore.Mvc;
using ExtraFunctionIntegrationTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ControllerBase = ExtraFunctionIntegrationTest.ControllerBase;
using Xunit.Abstractions;
using FluentAssertions;
using ExtraFunction.Model;

namespace ExtraFunctionIntegrationTests
{
    public class AchievementsIntegrationTest : ControllerBase
    {

        //string requestUr = "\"user/{UserId:Guid}/achievements\"";
        Guid testId = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");

        public AchievementsIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task Get_Achievement_By_Id_Should_Return_A_List_Of_Achievements() //id wrong, bad request, not authorized
        {
            string requestUri = $"user/{testId}/achievements";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<Achievement>>();


            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK); //no assert true. this is the reccomended approach. 
        }

    }
}
