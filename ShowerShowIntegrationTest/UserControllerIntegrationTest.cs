using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.DTO;
using System.Net;
using System.Text;
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
            string requestUri = $"user/{userName}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<GetUserDTO>>();
            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserByNameShouldNotReturnUsersWithStatusCodeBadrequest()
        {
            string userName = "  asdas  asdasd  asdasd"; //For test
            string requestUri = $"user/{userName}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<List<GetUserDTO>>();
            assertVar.Count.Should().Be(0);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]  //Not working
        public async Task CreateUserShouldReturnStatusCreated()
        {
            string requestUri = $"user/register";

            CreateUserDTO userDto = new CreateUserDTO()
            {
                UserName = "uniqueUsername!!!!!!",
                PasswordHash = "++!_@#()!+#)@#+)_!@",
                Email = "++!_@#()!+#)@#+)_!@",
                Name = "George Costanza"
            };
            await FlushUser(userDto.UserName);

            HttpContent http = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri,http);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
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
    }
}