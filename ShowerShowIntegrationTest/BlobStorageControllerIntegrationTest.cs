using Azure.Core;
using FluentAssertions;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;
using System.Net;
using System.Net.Mime;
using System.Text;
using Xunit.Abstractions;
using DayOfWeek = ShowerShow.Models.DayOfWeek;

namespace ShowerShowIntegrationTest
{
    public class BlobStorageControllerIntegrationTest : ControllerBase
    {
        public BlobStorageControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
            };
        }
        private Guid testUserId = Guid.Parse("3c37e2a9-b4e5-402f-aabe-1ad16810f81f");
        private Guid badUserId = Guid.Parse("3c37e2a9-b4e5-402f-aabe-1ad16810f82a");

        [Fact]
        public async Task UploadProfilePictureShouldReturnStatusOK()
        {
            string requestUri = $"user/{testUserId}/profile/uploadpic";

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "image/jpeg");
            http.Headers.Add("Content-Type", "image/jpeg");
            http.Headers.Add("Content-Disposition", $"attachment; filename={"pic.png"}; filename*=UTF-8'pic.png");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UploadProfilePictureShouldReturnStatusBadRequestBecauseWrongUserId()
        {
            string requestUri = $"user/{badUserId}/profile/uploadpic";

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "image/jpeg");
            http.Headers.Add("Content-Type", "image/jpeg");
            http.Headers.Add("Content-Disposition", $"attachment; filename={"pic.png"}; filename*=UTF-8'pic.png");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UploadProfilePictureShouldReturnStatusBadRequestBecauseWrongformat()
        {
            string requestUri = $"user/{testUserId}/profile/uploadpic";

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "image/jpeg");
            http.Headers.Add("Content-Type", "image/jpeg");
            http.Headers.Add("Content-Disposition", $"attachment; filename={"pic.mp4"}; filename*=UTF-8'pic.mp4");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UploadVoiceSoundShouldReturnStatusOK()
        {
            string requestUri = $"user/defaultvoices";

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "audio/mpeg");
            http.Headers.Add("Content-Type", "audio/mpeg");
            http.Headers.Add("Content-Disposition", $"attachment; filename={"audio.mp3"}; filename*=UTF-8'audio.mp3");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UploadVoiceSoundShouldReturnStatusBadRequest()
        {
            string requestUri = $"user/defaultvoices";

            HttpContent http = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "audio/mpeg");
            http.Headers.Add("Content-Type", "audio/mpeg");
            http.Headers.Add("Content-Disposition", $"attachment; filename={"audio.png"}; filename*=UTF-8'audio.png");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetVoiceSoundShouldReturnStatusOk()
        {
            string requestUri = $"user/defaultvoices/e_04ZrNroTo_48.mp3";

            var response = await client.GetAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetVoiceSoundShouldReturnStatusBadRequest()
        {
            string requestUri = $"user/defaultvoices/e_04ZrNroTo_48.png";

            var response = await client.GetAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task DeleteProfilePictureShouldReturnStatusOK()
        {
            string requestUri = $"user/{testUserId}/profile/deletepic";

            var response = await client.DeleteAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        public async Task DeleteProfilePictureShouldReturnStatusBadRequest()
        {
            string requestUri = $"user/{badUserId}/profile/deletepic";

            var response = await client.DeleteAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}