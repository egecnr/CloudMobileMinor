using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShowerShowIntegrationTest
{
    public class UserFriendControllerIntegrationTest : ControllerBase
    {
        //Endpoints to test: ,FavoriteFriendById,,,,DeleteUserFriend
        //Tested endpoints:CreateUserFriend,AcceptFriendRequest,GetUserFriends,GetUserFriendById
        Guid testuserId1 = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
        Guid testuserId2 = Guid.Parse("ff7a22ec-5114-44a8-b420-441132847b12");

        public UserFriendControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper){}

        //Create user endpoint
        [Fact]
        public async Task CreateUserShouldReturnStatusCreated()
        {
            string requestUri = $"user/{testuserId1}/friends/{testuserId2}";
            await Authenticate();
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateUserShouldReturnStatusNotAuthorizedDueToNotHavingAuthenticatedUser()
        {
            string requestUri = $"user/{testuserId1}/friends/{testuserId2}";
            //Authentication ommitted
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task CreateUserShouldReturnStatusBadRequestUponTryingToAddFriendForTheSecondTime()
        {
            string requestUri = $"user/{testuserId1}/friends/{testuserId2}";
            await Authenticate();
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateUserShouldReturnStatusBadRequestIfIdIsIncorrect()
        {
            string requestUri = $"user/{Guid.NewGuid()}/friends/{testuserId2}";
            await Authenticate();
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //Endpoint: AcceptFriendRequest
        [Fact]
        public async Task UpdateUserShouldReturnWithResponseStatusCodeBadRequestDueToNotAddingTheAddressCorrectly()
        {

            string requestUri = $"user/{testuserId1}/friends/{testuserId2}";
            await Authenticate();
           
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UpdateUserShouldReturnWithResponseStatusCodeAccepted()
        {

            string requestUri = $"user/{testuserId2}/friends/{testuserId1}";
            await Authenticate();

            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
        [Fact]
        public async Task UpdateUserShouldWithCorrectPathShouldReturnBadRequestIfUsersAreAlreadyFriends()
        {
            string requestUri = $"user/{testuserId2}/friends/{testuserId1}";
            await Authenticate();
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UpdateUserShouldReturnBadRequestIfUserDoesntExist()
        {
            string requestUri = $"user/{Guid.NewGuid()}/friends/{testuserId1}";
            await Authenticate();
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UpdateUserShouldWithCorrectPathShouldReturnUnauthorized()
        {
            string requestUri = $"user/{testuserId2}/friends/{testuserId1}";
            //Authentication ommitted
            HttpContent http = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //Endpoint:GetAllFriendsOfUser
        [Fact]
        public async Task GetAllFriendsOfUser1ByIdShouldReturnUnauthorized()
        {
            string requestUri = $"user/{testuserId1}/friends";
            //Authentication ommitted
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<GetUserFriendDTO>>();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task GetAllFriendsOfUser1ByIdShouldReturnAListOfFriendsWithStatusCodeOk()
        {
            string requestUri = $"user/{testuserId1}/friends";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<GetUserFriendDTO>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            assertVar.Count.Should().Be(1);
        }
        [Fact]
        public async Task GetAllFriendsOfUser2ByIdShouldReturnAListOfFriendsWithStatusCodeOk()
        {
            string requestUri = $"user/{testuserId2}/friends";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<GetUserFriendDTO>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            assertVar.Count.Should().Be(1);
        }
        [Fact]
        public async Task GetAllFriendsOfUserShouldReturnBadRequestIfUserDoesntExist()
        {
            Guid id = Guid.NewGuid();
            string requestUri = $"user/{id}/friends";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        //Endpoint: GetFriendById
        [Fact]
        public async Task GetFriendOfUserByIdShouldReturnBadRequestIfUserDoesntExist()
        {
            Guid id = Guid.NewGuid();
            string requestUri = $"user/{id}/friends/{testuserId1}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetFriendOfUserByIdShouldReturnStatusOkIfUsersExist()
        {
            string requestUri = $"user/{testuserId2}/friends/{testuserId1}";
            await Authenticate();

            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<GetUserFriendDTO>();
            assertVar.FriendId.Should().Be(testuserId1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetFriendOfUserByIdShouldReturnUnauthorized()
        {
            string requestUri = $"user/{testuserId2}/friends/{testuserId1}";
           //Authentication ommitted
            var response = await client.GetAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //Endpoint: FavoriteUserById


    }
}
