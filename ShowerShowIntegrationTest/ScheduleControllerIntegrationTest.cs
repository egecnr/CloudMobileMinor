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
        public SchedulecontrollerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper) { }
        //Get User By name endpoint
        private Guid testUserId = Guid.Parse("3c37e2a9-b4e5-402f-aabe-1ad16810f81f");
        private Guid badUserId = Guid.Parse("3c37e2a9-b4e5-402f-aabe-1ad16810f82d");


        //Create schedule endpoint
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
            string requestUri = $"schedule/{badUserId}/schedules";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<Schedule>>();
            assertVar.Count.Should().Be(0);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        public async Task GetScheduleByIdShouldReturnScheduleWithStatusCodeOk()
        {
            string requestUri = $"schedule/{badUserId}/schedules";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<Schedule>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetUserByNameShouldNotReturnUsersWithStatusCodeBadrequest()
        {
            string userName = "  asdas  asdasd  asdasd"; //This username would never exist in db so this will always return an empty list
            string requestUri = $"user/{userName}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<List<GetUserDTO>>();
            assertVar.Count.Should().Be(0);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Fact]
        public async Task GetUserByNameShouldReturnResponseWithStatusCodeUnauthorized()
        {
            string userName = "te"; //For test
            string requestUri = $"user/{userName}";
            //Authentication ommitted.
            var response = await client.GetAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        //Get user by id endpoint
        [Fact]
        public async Task GetUserByIdShouldReturnASingleRecordWithStatusCodeOk()
        {
            string requestUri = $"user/{testUserId}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<GetUserDTO>();
            assertVar.Should().NotBeNull();
            assertVar.UserName.Should().Be("test");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetUserByIdShouldReturnWithResponseStatusCodeUnauthorized()
        {

            string requestUri = $"user/{testUserId}";
            //Authentication ommited.
            var response = await client.GetAsync(requestUri);
            ;
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUserByIdShouldNotReturnASingleRecordWithStatusCodeBadrequest()
        {
            Guid id = Guid.NewGuid();
            string requestUri = $"user/{id}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<GetUserDTO>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }





        [Fact]
        public async Task CreateUserShouldReturnStatusBadRequest()
        {
            string requestUri = $"user/register";

            CreateUserDTO userDto = new CreateUserDTO()
            {
                UserName = "test",
                PasswordHash = "++!_@#()!+#)@#+)_!@",  // Email and Username already exists in database
                Email = "test",
                Name = "George Costanza"
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        //Deactivate User endpoint
        [Fact]
        public async Task DeactivateUserShouldNotUpdateAccountWithStatusCodeBadRequest()
        {
            Guid id = Guid.NewGuid();
            string requestUri = $"user/{id}/true";
            await Authenticate();

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task DeactivateUserShouldUpdateAccountStatusWithStatusCodeOK()
        {

            string requestUri = $"user/{testUserId}/true";
            await Authenticate();

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task DeactivateUserShouldReturnWithResponseStatusCodeUnauthorized()
        {

            string requestUri = $"user/{testUserId}/true";
            //Authentication method ommited.
            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        ////Update User endpoint

        [Fact]
        public async Task UpdateUserShouldReturnWithResponseStatusCodeUnauthorized()
        {

            string requestUri = $"user/{testUserId}";
            //Authentication method ommited.
            Random random = new Random();
            UpdateUserDTO updateUserDTO = new UpdateUserDTO()
            {
                Name = "ChangedName",
                UserName = "test",
                Email = "test",
                PasswordHash = "ChangedPassword"
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateUserDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task UpdateUserShouldReturnWithResponseStatusCodeOk()
        {

            string requestUri = $"user/{testUserId}";
            await Authenticate();
            Random random = new Random();
            UpdateUserDTO updateUserDTO = new UpdateUserDTO()
            {
                Name = "ChangedName",
                UserName = "test",
                Email = "test",
                PasswordHash = "test"
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateUserDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateUserShouldReturnWithResponseStatusCodeBadRequestDueToNotHavingRightId()
        {
            Guid id = Guid.NewGuid();
            string requestUri = $"user/{id}";
            await Authenticate();
            Random random = new Random();
            UpdateUserDTO updateUserDTO = new UpdateUserDTO()
            {
                Name = "ChangedName",
                UserName = "test",
                Email = "test",
                PasswordHash = "test"
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateUserDTO), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}