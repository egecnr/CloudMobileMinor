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
         // Done endpoints : getuserbyname, createuser,getuserById, deactivateAccount
        //Get User By name endpoint
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
        {   Guid id = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
            string requestUri = $"user/{id}";
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
            Guid id = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
            string requestUri = $"user/{id}";
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

      
        //Create user endpoint
        [Fact] 
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
            Guid id = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
            string requestUri = $"user/{id}/true";
            await Authenticate();

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task DeactivateUserShouldReturnWithResponseStatusCodeUnauthorized()
        {
            Guid id = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
            string requestUri = $"user/{id}/true";
            //Authentication method ommited.
            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        ////Update User endpoint

        [Fact]
        public async Task UpdateUserShouldReturnWithResponseStatusCodeUnauthorized()
        {
            Guid id = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
            string requestUri = $"user/{id}";
            //Authentication method ommited.
            Random random = new Random();
            UpdateUserDTO updateUserDTO = new UpdateUserDTO() {
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
            Guid id = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
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