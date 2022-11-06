using FluentAssertions;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static ShowerShow.Models.Preferences;

namespace ShowerShowIntegrationTest
{
    public class PreferencesIntegrationTests : ControllerBase
    {
        Guid testId = Guid.Parse("ff7a22ec-5114-44a8-b420-441132847b12");
        PreferencesDTO mockdto;

        public PreferencesIntegrationTests(ITestOutputHelper outputHelper) : base(outputHelper){
            mockdto = new PreferencesDTO()
            {
                Currency = AvailableCurrencies.EUR,
                WaterType = AvailableWaterTypes.Liters,
                SelectedLanguage = AvailableLanguages.English,
                SelectedVoice = "voiceid",
                Notification = true,
                Theme = AvailableThemes.Dark
            };
        }
        
        [Fact]
        public async Task GetPreferencesByUserIdShouldReturnPreferencesDTOWithStatusCodeOk()
        {
            string requestUri = $"user/{testId}/preferences";
            await Authenticate();

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<PreferencesDTO>();
            assertVar.Should().NotBeNull();
            assertVar.SelectedLanguage.Should().Be(AvailableLanguages.English);
            assertVar.Currency.Should().Be(AvailableCurrencies.EUR);
            assertVar.Theme.Should().Be(AvailableThemes.Dark);
            assertVar.SelectedVoice.Should().Be("voiceid");
            assertVar.Notification.Should().Be(true);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetPreferencesByIdShouldNotReturnPreferencesWithStatusCodeBadrequest()
        {
            string requestUri = $"user/{Guid.NewGuid()}/preferences";
            await Authenticate();

            var response = await client.GetAsync(requestUri);         
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetPreferencesByIdShouldNotReturnPreferencesWithStatusCodeUnauthorized()
        {
            string requestUri = $"user/{testId}/preferences";
            //Authentication ommitted

            var response = await client.GetAsync(requestUri);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task UpdatePreferencesShouldReturnWithResponseStatusCodeOk()
        {

            string requestUri = $"user/{testId}/preferences";
            await Authenticate();
           
            HttpContent http = new StringContent(JsonConvert.SerializeObject(mockdto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdatePreferencesShouldReturnWithResponseStatusCodeBadRequestDueToUserNotExistingInDb()
        {

            string requestUri = $"user/{Guid.NewGuid()}/preferences";
            await Authenticate();

            HttpContent http = new StringContent(JsonConvert.SerializeObject(mockdto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UpdatePreferencesShouldReturnWithResponseStatusCodeUnauthorized()
        {

            string requestUri = $"user/{testId}/preferences";
            //Authorization ommitted

            HttpContent http = new StringContent(JsonConvert.SerializeObject(mockdto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUri, http);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
