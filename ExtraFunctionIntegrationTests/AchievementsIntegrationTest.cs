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
        Guid testId = Guid.Parse("10cad932-fb7f-4dcc-abec-58a44e691b15");

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
            //var results = JsonConvert.DeserializeObject<List<Achievement>>(response.Content);
            //var assertVar = await response.Content.ReadAsAsync<List<Achievement>>();
            //assertVar.Should().NotBeNull();
            //assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);











            //string requestUri = $"user/{testId}/achievements";
            //await Authenticate();



            //var response = await client.GetAsync(requestUri);

            //var assertVar = await response.Content.ReadAsAsync<List<Achievement>>();

            //var result = response.Content.ReadAsStringAsync().Result;

            //var caralho = JsonSerializer.DeserializeAsync<List<Achievement>>(result);

            //var result = response.Content.ReadAsStringAsync();

            //var serializerOptions = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = false


            //List<Achievement> achievements = JsonConvert.DeserializeObject<List<Achievement>>(result);

            // Achievement[] achievementsArray = JsonConvert.DeserializeObject<Achievement[]>(assertVar);
            //List<Achievement> myDeserializedObjList = (List<Achievement>)JsonConvert.DeserializeObject(Request[requestUri], typeof(List<Achievement>));


            //achievements.Should().NotBeNull();
            //achievements.Count.Should().BeGreaterThanOrEqualTo(1);
            // response.StatusCode.Should().Be(HttpStatusCode.OK); //no assert true. this is the reccomended approach. 
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
        public async Task Get_Achievements_By_Should_Fail_If_Not_Authorized()
        {
            Guid wrongId = Guid.NewGuid();
            string requestUri = $"user/{wrongId}/achievements";


            var response = await client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
    }
}
