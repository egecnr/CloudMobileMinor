using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShowIntegrationTest;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace ShowerShowIntegrationTest
{
    public class ShowerDataControllerIntegrationTest : ControllerBase
    {
        public ShowerDataControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
            };
        }
        private Guid testUserId = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
        private Guid testShowerId = Guid.Parse("cbafec3c-fd12-45d1-b3c5-9fff052887e0");

        [Fact]
        public async Task CreateShowerDataShouldReturnStatusCreated()
        {
            string requestUri = $"user/{testUserId}/showerdata";
            await Authenticate();
            CreateShowerDataDTO showerDataDTO = new CreateShowerDataDTO()
            {
                Duration = 30,
                WaterUsage = 15,
                WaterCost = 10,
                GasUsage = 10,
                GasCost = 10,
                Date = DateTime.Now,
                ScheduleId = Guid.NewGuid(),
                Overtime = 15
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(showerDataDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task CreateShowerDataShouldReturnBadRequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/showerdata";
            await Authenticate();
            CreateShowerDataDTO showerDataDTO = new CreateShowerDataDTO()
            {
                Duration = 30,
                WaterUsage = 15,
                WaterCost = 10,
                GasUsage = 10,
                GasCost = 10,
                Date = DateTime.Now,
                ScheduleId = Guid.NewGuid(),
                Overtime = 15
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(showerDataDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetShowerDataByIdShouldReturnOK()
        {
            string requestUri = $"user/{testUserId}/showerdata/{testShowerId}";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<ShowerData>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetShowerDatatByIdShouldReturnBadRequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/showerdata/{testShowerId}";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<ShowerData>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
       
    }
}