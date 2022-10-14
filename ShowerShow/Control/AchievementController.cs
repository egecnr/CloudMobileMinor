using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShowerShow.Control
{
    public class AchievementController
    {
        private readonly ILogger<AchievementController> _logger;
        private readonly IAchievementService _achievementService;

        public AchievementController(ILogger<AchievementController> logger, IAchievementService service)
        {
            _logger = logger;
            _achievementService = service;
        }

        [Function(nameof(GetAchievementsById))] 
        [OpenApiOperation(operationId: "GetUserAchievements", tags: new[] { "Achievement" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Achievement>), Description = "The OK response with all achievements from user.")]
        public async Task<HttpResponseData> GetAchievementsById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId}/achievements")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("");


            var res = _achievementService.GetAchievementsById(UserId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(res);
            return response;
        }

        [Function(nameof(GetAchievementById))]
        [OpenApiOperation(operationId: "GetUserAchievement", tags: new[] { "Achievement" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user id parameter")]
        [OpenApiParameter(name: "Title", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Achievement Title")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Achievement), Description = "The OK response with userId and id of requested achievement.")]
        public async Task<HttpResponseData> GetAchievementById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId}/achievement/{Title}")] HttpRequestData req, Guid UserId, string achievementTitle)
        {
            _logger.LogInformation("");

            var res = _achievementService.GetAchievementById(achievementTitle, UserId);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await  response.WriteAsJsonAsync(res);
            return response;
        }
        [Function(nameof(UpdateAchievementById))]
        [OpenApiOperation(operationId: "UpdateAchievement", tags: new[] { "Achievement" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "CurrentValue", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The cuurent value you want to change")]
        [OpenApiParameter(name: "achievementTitle", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Title of the requested achievement")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Achievement), Description = "Achievement updated")]
        public async Task<HttpResponseData> UpdateAchievementById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{UserId}/achievement/{achievementTitle}")] HttpRequestData req, Guid UserId, string achievementTitle, int currentValue)
        {
            _logger.LogInformation("");

            var res = _achievementService.UpdateAchievementById(achievementTitle, UserId, currentValue);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(res);
            return response;
        }


    }
}
