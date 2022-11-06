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
    public class UserPreferencesControllerIntegrationTest : ControllerBase
    {
        public UserPreferencesControllerIntegrationTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
            };
        }
        private Guid testUserId = Guid.Parse("31aa2d55-8eae-4d00-9daa-5be588aba14d");
        private Guid testShowerId = Guid.Parse("cbafec3c-fd12-45d1-b3c5-9fff052887e0");

        [Fact]
        public async Task UpdatePreferencesShouldReturnStatusOK()
        {
            string requestUri = $"user/{testUserId}/preferences";
            await Authenticate();
            
            PreferencesDTO updatePreferences = new PreferencesDTO()
            {
                SelectedVoice = "Default",
                SelectedLanguage = AvailableLanguages.Dutch,
                WaterType = Preferences.AvailableWaterTypes.Liters,
                Currency = Preferences.AvailableCurrencies.EUR,
                Theme = Preferences.AvailableThemes.Dark,
                Notification = true
            };
            HttpContent http = new StringContent(JsonConvert.SerializeObject(updatePreferences), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdatePreferencesShouldReturnBadRequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/preferences";
            await Authenticate();
            PreferencesDTO updatePreferences = new PreferencesDTO()
            {
                SelectedVoice = "Default",
                SelectedLanguage = AvailableLanguages.Dutch,
                WaterType = Preferences.AvailableWaterTypes.Liters,
                Currency = Preferences.AvailableCurrencies.EUR,
                Theme = Preferences.AvailableThemes.Dark,
                Notification = true
            };

            HttpContent http = new StringContent(JsonConvert.SerializeObject(updatePreferences), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, http);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetPreferencesByIdShouldReturnOK()
        {
            string requestUri = $"user/{testUserId}/preferences";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<Preferences>();
            assertVar.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetPreferencesByIdShouldReturnBadRequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/preferences";
            await Authenticate();
            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<Preferences>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
       
    }
}