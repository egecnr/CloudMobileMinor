using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using System.Net;
using System.Text;
using Xunit.Abstractions;
using DayOfWeek = ShowerShow.Models.DayOfWeek;

namespace ShowerShowIntegrationTest
{
    public class ScheduleControllerIntegrationTest : ControllerBase
    {
        public ScheduleControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper) {
        }
        private Guid testUserId = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
        private Guid testScheduleId = Guid.Parse("aa1ab168-b658-4b61-8c89-6899fc21762d");
        private Mock<IScheduleRepository> scheduleRepositoryMock = new Mock<IScheduleRepository>();

        #region Create Schedule

     
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
        [Fact]
        public async Task CreateScheduleShouldReturnStatusCreated()
        {
            string requestUri = $"schedule/{testUserId}";

            CreateScheduleDTO scheduleDTO = new CreateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(scheduleDTO), Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
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