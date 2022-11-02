using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExtraFunction.Model;
using ExtraFunction.Authorization;
using ExtraFunction.Repository_.Interface;

namespace ExtraFunction.Control
{
    public class AchievementController
    {
        private readonly ILogger<AchievementController> _logger;
        private readonly IAchievementService _achievementService;

        public AchievementController(ILogger<AchievementController> logger, IAchievementService achievementService)
        {
            _logger = logger;
            _achievementService = achievementService;
        }

        [Function(nameof(GetAchievementsById))]
        [OpenApiOperation(operationId: "GetAchievementsById", tags: new[] { "Achievement" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Achievement>), Description = "The OK response with all achievements from user.")]
        public async Task<HttpResponseData> GetAchievementsById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId:Guid}/achievements")] HttpRequestData req, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Getting achievement by Id");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                List<Achievement> achievements = await _achievementService.GetAchievementsById(UserId);
                responseData.StatusCode = HttpStatusCode.OK;
                await responseData.WriteAsJsonAsync(achievements);

            }
            catch (Exception e)
            {

                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }

            return responseData;

        }

        [Function(nameof(GetAchievementByIdAndTitle))]
        [OpenApiOperation(operationId: "GetUserAchievement", tags: new[] { "Achievement" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user id parameter")]
        [OpenApiParameter(name: "achievementTitle", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Achievement Title")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Achievement), Description = "The OK response with userId and id of requested achievement.")]
        public async Task<HttpResponseData> GetAchievementByIdAndTitle([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId}/achievement/{achievementTitle}")] HttpRequestData req, Guid UserId, string achievementTitle, FunctionContext functionContext)
        {
            _logger.LogInformation("Getting achievement by title");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;

                }
                Achievement achievement = await _achievementService.GetAchievementByIdAndTitle(achievementTitle, UserId);
                if (achievement == null)
                {
                    responseData.StatusCode = HttpStatusCode.NotFound;
                    return responseData;
                }
                responseData.StatusCode = HttpStatusCode.OK;
                await responseData.WriteAsJsonAsync(achievement);
            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;
        }
        [Function(nameof(UpdateAchievementByIdAndTitle))]
        [OpenApiOperation(operationId: "UpdateAchievement", tags: new[] { "Achievement" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "CurrentValue", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The current value you want to change")]
        [OpenApiParameter(name: "achievementTitle", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Title of the requested achievement")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Achievement), Description = "Achievement updated")]
        public async Task<HttpResponseData> UpdateAchievementByIdAndTitle([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{UserId}/achievement/{achievementTitle}")] HttpRequestData req, Guid UserId, string achievementTitle, int CurrentValue, FunctionContext functionContext)
        {
            _logger.LogInformation("Updating achievement by id");

            HttpResponseData responseData = req.CreateResponse();

            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }

                await _achievementService.UpdateAchievementByIdAndTitle(achievementTitle, UserId, CurrentValue);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception e)
            {

                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }

            return responseData;
        }


    }
}
