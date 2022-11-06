using ExtraFunction.Model;
using Newtonsoft.Json;
using System.Text;
using Xunit.Abstractions;

namespace ExtraFunctionIntegrationTest
{
    public class LoginResultDTO
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
    public class ControllerBase
    {
        protected HttpClient client { get; }
        protected HttpClient showerShowClient { get; }
        protected ITestOutputHelper outputHelper;
        public ControllerBase(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.showerShowClient = new HttpClient() {
                BaseAddress = new Uri($"http://localhost:7071/api/")
            };
            this.client = new HttpClient() {
                BaseAddress = new Uri($"http://localhost:7075/api/")  // func start --port 7075
                //http://localhost:7071/api/Login"
            };
        }

        protected async Task Authenticate()
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetAuthString());
        }

        private async Task<string> GetAuthString()
        {
            Login loginUser = new Login() {Username="test",Password="test"};
            string requesturi = "Login";
            HttpContent http = new StringContent(JsonConvert.SerializeObject(loginUser),Encoding.UTF8,"application/json");
            showerShowClient.DefaultRequestHeaders.Clear();
            showerShowClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = showerShowClient.PostAsync(requesturi, http).Result;

            var authString = (await response.Content.ReadAsAsync<LoginResultDTO>()).AccessToken;
            return authString;
        }
    }
}
