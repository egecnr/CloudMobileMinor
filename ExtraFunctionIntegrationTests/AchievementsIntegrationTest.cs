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
using Newtonsoft.Json;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using Azure.Core;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;

namespace ExtraFunctionIntegrationTests
{
    public class AchievementsIntegrationTest : ControllerBase
    {
        // cd showershow  - > func start --port 7071
        //cd extrafunction -> func start --port 7075

        //string requestUr = "\"user/{UserId:Guid}/achievements\"";
        Guid testId = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");

        public AchievementsIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task Get_Achievements_By_Id_Should_Return_A_List_Of_Achievements() //tests to cover: id wrong, bad request, not authorized 
        {


            string requestUri = $"user/{testId}/achievements";
            await Authenticate();


            var response = await client.GetAsync(requestUri);
            outputHelper.WriteLine(response.Content.ToString());
            var assertVar = await response.Content.ReadAsAsync<List<Achievement>>();
            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(14);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Get_Achievements_By_Id_Should_Return_Bad_Request()
        {

            Guid wrongId = Guid.NewGuid();
            string requestUri = $"user/{wrongId}/achievements";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Get_Achievements_By_Id_Should_Fail_If_Not_Authorized()
        {
            Guid wrongId = Guid.NewGuid();
            string requestUri = $"user/{wrongId}/achievements";


            var response = await client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
        [Fact]
        public async Task Get_Achievement_By_Id_And_Title_Should_Return_One_Achievement()
        {

            string title = "Perfect week";
            string requestUri = $"user/{testId}/achievement/{title}";
            await Authenticate();


            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<Achievement>();

            assertVar.Should().NotBeNull();
            assertVar.Should().BeAssignableTo<Achievement>();  // when comparing one object//!!!! LETS GO
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }

        [Fact]
        public async Task Get_Achievement_By_Id_And_Title_Should_Return_Bad_Request()
        {
            string title = "Perfect week";
            Guid wrongId = Guid.NewGuid();
            string requestUri = $"user/{wrongId}/achievement/{title}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        [Fact]
        public async Task Get_Achievement_By_Id_And_Title_Should_Fail_If_Not_Authorized()
        {
            string title = "Perfect week";
            Guid wrongId = Guid.NewGuid();
            string requestUri = $"user/{wrongId}/achievement/{title}";

            var response = await client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);


        }

        [Fact]
        public async Task Update_Achievement_By_Id_And_Title_Should_Return_Status_Ok() //achievementTitle, UserId, CurrentValue
        {
            int changedValue = 79;

            UpdateAchievementDTO updateAchievementDTO = new UpdateAchievementDTO()
            {

                CurrentValue = changedValue
            };

            string title = "Perfect week";
            string requestUri = $"user/{testId}/achievement/{title}";
            await Authenticate();

            // var response = await client.GetAsync(requestUri);

            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateAchievementDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }

        [Fact]
        public async Task Update_Achievement_By_Id_And_Title_Should_Return_Bad_Request()
        {
            int changedValue = 79;

            UpdateAchievementDTO updateAchievementDTO = new UpdateAchievementDTO()
            {

                CurrentValue = changedValue
            };
            Guid wrongId = Guid.NewGuid();
            string title = "Perfect week";
            string requestUri = $"user/{wrongId}/achievement/{title}";
            await Authenticate();

            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateAchievementDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task Update_Achievement_By_Id_And_Title_Fail_If_Not_Authorized()
        {
            int changedValue = 79;

            UpdateAchievementDTO updateAchievementDTO = new UpdateAchievementDTO()
            {

                CurrentValue = changedValue
            };

            string title = "Perfect week";
            string requestUri = $"user/{testId}/achievement/{title}";

            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateAchievementDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }


    }
}
