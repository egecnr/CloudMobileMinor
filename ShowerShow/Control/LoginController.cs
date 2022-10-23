using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShowerShow.Model;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;

namespace ShowerShow.Control
{
    public class LoginController
    {
        private ITokenService tokenService;
        private ILoginService loginService;

        public LoginController(ITokenService tokenService,ILoginService loginService)
        {
            this.tokenService = tokenService;
            this.loginService = loginService;
        }

        //TO DO: Verify credentials via  a method
        [Function("LoginController")]
        [OpenApiOperation(operationId: "Login", tags: new[] { "Login" }, Summary = "Login for a user",
                      Description = "This method logs in the user, and retrieves a JWT bearer token.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Login), Required = true, Description = "The user credentials")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResult), Description = "Login success")]

        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            Login login = JsonConvert.DeserializeObject<Login>(await new StreamReader(req.Body).ReadToEndAsync());
            if (await loginService.CheckIfCredentialsCorrect(login.Username, login.Password))
            {
                LoginResult result = await tokenService.CreateToken(login);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            else
            {

                HttpResponseData response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }
                  
        }
    }
}
