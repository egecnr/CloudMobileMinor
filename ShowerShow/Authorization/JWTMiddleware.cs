using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Authorization
{
    public class JWTMiddleware : IFunctionsWorkerMiddleware
    {
        ITokenService TokenService { get; }
        ILogger Logger { get; }

        public JWTMiddleware(ITokenService TokenService, ILogger<JWTMiddleware> Logger)
        {
            this.TokenService = TokenService;
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext Context, FunctionExecutionDelegate Next)
        {
            string headerString;
            Dictionary<string, string> Headers = new Dictionary<string, string>();
            if (Context.BindingContext.BindingData.TryGetValue("Headers", out object? headerValues))
            {
                headerString = headerValues.ToString();
                Headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerString);
            }



            if (Headers.TryGetValue("Authorization", out string AuthorizationHeader))
            {
                try
                {
                    AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(AuthorizationHeader);

                    ClaimsPrincipal User = await TokenService.GetByValue(BearerHeader.Parameter);

                    Context.Items["User"] = User;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                }
            }

            await Next(Context);
        }

    }
}
