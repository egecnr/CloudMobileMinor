using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;
using System.Net;
using System.Text;
using Xunit.Abstractions;
using DayOfWeek = ShowerShow.Models.DayOfWeek;

namespace ShowerShowIntegrationTest
{
    public class SchedulecontrollerIntegrationTest : ControllerBase
    {
        public SchedulecontrollerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper) {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
            };
        }
        private Guid testUserId = Guid.Parse("3c37e2a9-b4e5-402f-aabe-1ad16810f81f");
        private Guid testScheduleId = Guid.Parse("a7aa2fbc-0685-4e3f-a733-2fbe16cef7d8");

        #region Create Schedule
        [Fact]
        public async Task CreateScheduleShouldReturnStatusCreated()
        {
            string requestUri = $"schedule/{testUserId}";

            CreateScheduleDTO scheduleDTO = new CreateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday,DayOfWeek.Friday},
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test",ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot} }
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(scheduleDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task CreateScheduleShouldReturnBadRequest()
        {
            string requestUri = $"schedule/{Guid.NewGuid()}";

            CreateScheduleDTO scheduleDTO = new CreateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(scheduleDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Schedules
        [Fact]
        public async Task GetSchedulesShouldReturnAListOfSchedulesWithStatusCodeOk()
        {
            string requestUri = $"schedule/{testUserId}/schedules";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<Schedule>>();
            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetSchedulesShouldNotReturnAnythingWithStatusCodeBadRequest()
        {
            string requestUri = $"schedule/{Guid.NewGuid()}/schedules";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<Schedule>>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Schedule By id
        [Fact]
        public async Task GetScheduleByIdShouldReturnScheduleWithStatusCodeOk()
        {
            string requestUri = $"schedule/{testScheduleId}";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<Schedule>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetScheduleByIdShouldReturnBadRequest()
        {
            string requestUri = $"schedule/{Guid.NewGuid()}";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<Schedule>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Delete Schedule
        [Fact]
        public async Task DeleteScheduleShouldReturnStatusOK()
        {
            string requestUri = $"schedule/{testUserId}";

            var response = await client.DeleteAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task DeleteScheduleShouldReturnStatusBadRequest()
        {
            string requestUri = $"schedule/{Guid.NewGuid()}";

            var response = await client.DeleteAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Update Schedule
        [Fact]
        public async Task UpdateScheduleShouldReturnStatusOK()
        {
            string requestUri = $"schedule/{testScheduleId}";
            UpdateScheduleDTO updateScheduleDTO = new UpdateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Wednesday},
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test22", ActivityDuration = 12, IsWaterOn = false, waterTemperature = ShowerShow.Model.WaterTemperature.Cold } }
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateScheduleDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateScheduleShouldReturnStatusBadRequest()
        {
            string requestUri = $"schedule/{Guid.NewGuid()}";
            UpdateScheduleDTO updateScheduleDTO = new UpdateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Wednesday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test22", ActivityDuration = 12, IsWaterOn = false, waterTemperature = ShowerShow.Model.WaterTemperature.Cold } }
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateScheduleDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}