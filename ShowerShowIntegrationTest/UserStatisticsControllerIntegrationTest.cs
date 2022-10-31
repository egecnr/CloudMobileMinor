using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.Controllers;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using System.Net;
using System.Text;
using Xunit.Abstractions;
using DayOfWeek = ShowerShow.Models.DayOfWeek;

namespace ShowerShowIntegrationTest
{
    public class UserStatisticsControllerIntegrationTest : ControllerBase
    {
        public UserStatisticsControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper) {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
            };
        }
        private Guid testUserId = Guid.Parse("3c37e2a9-b4e5-402f-aabe-1ad16810f81f");

        #region Get Friends Ranking
        [Fact]
        public async Task GetFriendsRankingShouldReturnStatusOK()
        {
            string requestUri = $"user/{testUserId}/friendRanking/2";
            await Authenticate();
            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<Dictionary<Guid,double>>();
            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(1);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetFriendsRankingShouldReturnStatusBadRequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/friendRanking/2";
            await Authenticate();
            var response = await client.GetAsync(requestUri);
            var assertVar = await response.Content.ReadAsAsync<Dictionary<Guid, double>>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region Get Dashboard
        [Fact]
        public async Task GetDashboardShouldReturnStatusCodeOk()
        {
            string requestUri = $"user/{testUserId}/dashboard/7";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<UserDashboard>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetDashboardShouldReturnStatusCodeBadRequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/dashboard/7";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<UserDashboard>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}