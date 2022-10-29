using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShowerShowIntegrationTest
{
    public class ControllerBase
    {
        protected HttpClient client { get; }
        protected ITestOutputHelper outputHelper;
        public ControllerBase(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.client = new HttpClient() {
                BaseAddress = new Uri($"http://localhost:7071/api")
            };
        }

        protected async Task Authenticate()
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAuthString());
        }

        private async Task<string> GetAuthString()
        {
            Login loginUser = new Login() {Username="test",Password="test"};
            string requesturi = "/Login";
            HttpContent http = new StringContent(JsonConvert.SerializeObject(loginUser),Encoding.UTF8,"application/json");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(requesturi, http).Result;
            var authString = (await response.Content.ReadAsAsync<LoginResult>()).AccessToken;
            return authString;
        }
    }
}
