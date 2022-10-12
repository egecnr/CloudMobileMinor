using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;

namespace ShowerShow.Control
{
    public class UserFriendController
    {
        private readonly ILogger _logger;

        public UserFriendController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserFriendController>();
        }

        //This is not complete at all yet.
        [Function("GetUserFriends")]
        [OpenApiOperation(operationId: "GetUserFriends", tags: new[] { "Users" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUserFriends([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId:Guid}/friends")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Creating new schedule.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                return null;
            }
            catch (Exception ex)
            {
                // DEV ONLY
                throw new Exception(ex.Message);
            }
        }
    }
}
