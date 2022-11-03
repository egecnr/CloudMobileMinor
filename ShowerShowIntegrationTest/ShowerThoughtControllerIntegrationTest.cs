using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShowIntegrationTest;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace ShowerThoughtControllerIntegrationTest
{
    public class ShowerThoughtControllerIntegrationTest : ControllerBase
    {
        public ShowerThoughtControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
            };
        }
        private Guid testUserId = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
        private Guid testShowerThoughtId = Guid.Parse("9d3fb42e-1fa2-4b9b-b3f6-876860a1a8ac");
        private Guid testShowerId = Guid.Parse("f9f95c1c-852f-4440-9e6d-742a5005b6db");

        #region Create Shower Thought
        [Fact]
        public async Task CreateThoughtShouldReturnStatusCreated()
        {
            string requestUri = $"shower/thoughts/{testShowerId}?UserId={testUserId}";
            await Authenticate();
            ShowerThoughtDTO showerThoughtDTO = new ShowerThoughtDTO()
            {
                Text = "Integration test thought",
                Title = "Test thought",
                ShareWithFriends = false,
                DateTime = DateTime.Now,
                IsFavorite = false
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(showerThoughtDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task CreateThoughtShouldReturnBadRequest()
        {
            string requestUri = $"shower/thoughts/{Guid.NewGuid()}?UserId={Guid.NewGuid()}";
            await Authenticate();
            ShowerThoughtDTO showerThoughtDTO = new ShowerThoughtDTO()
            {
                Text = "Integration test thought",
                Title = "Test thought",
                ShareWithFriends = false,
                DateTime = DateTime.Now,
                IsFavorite = false
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(showerThoughtDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Thought By id
        [Fact]
        public async Task GetThoughtByIdShouldReturnOK()
        {
            string requestUri = $"shower/thoughts/{testShowerThoughtId}";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<ShowerThought>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetThoughtByIdShouldReturnBadRequest()
        {
            string requestUri = $"shower/thoughts/{Guid.NewGuid()}";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<ShowerThought>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Thought By Shower ID
        [Fact]
        public async Task GetThoughtByShowerIdShouldReturnOK()
        {
            string requestUri = $"shower/thoughts/{testShowerId}/s";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<ShowerThought>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        #endregion
        #region Get Thought By User ID
        [Fact]
        public async Task GetThoughtByUserIdShouldReturnOK()
        {
            string requestUri = $"shower/thoughts/{testUserId}/u";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<ShowerThought>>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetThoughtByUserIdShouldReturnBadRequest()
        {
            string requestUri = $"shower/thoughts/{Guid.NewGuid()}/u";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<ShowerThought>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Thoughts By Date
        [Fact]
        public async Task GetThoughtByDateShouldReturnOK()
        {
            string requestUri = $"shower/thoughts/{testUserId}/date?Date=22-10-2022";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<ShowerThought>>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetThoughtByDateShouldReturnBadRequest()
        {
            string requestUri = $"shower/thoughts/{Guid.NewGuid()}/date?Date=22-10-2022";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<ShowerThought>>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Thoughts By Content
        [Fact]
        public async Task GetThoughtByContentShouldReturnOK()
        {
            string requestUri = $"shower/thoughts/{testUserId}/test";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<ShowerThought>>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetThoughtByContentShouldReturnBadRequest()
        {
            string requestUri = $"shower/thoughts/{Guid.NewGuid()}/test";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<List<ShowerThought>>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Update Thoughts
        [Fact]
        public async Task UpdateThoughtShouldReturnOK()
        {
            string requestUri = $"shower/thoughts/{testShowerThoughtId}";
            await Authenticate();
            UpdateShowerThoughtDTO updateThought = new UpdateShowerThoughtDTO()
            {
                IsFavorite = false,
                ShareWithFriends = true
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateThought), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateThoughtShouldReturnBadRequest()
        {
            string requestUri = $"shower/thoughts/{Guid.NewGuid()}";
            await Authenticate();
            UpdateShowerThoughtDTO updateThought = new UpdateShowerThoughtDTO()
            {
                IsFavorite = false,
                ShareWithFriends = true
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(updateThought), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}