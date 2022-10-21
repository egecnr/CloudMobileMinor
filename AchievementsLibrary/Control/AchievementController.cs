using ExtraFunction.Model_;
using ExtraFunction.Repository_.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ShowerShow.Authorization;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ExtraFunction.Control
{
    public class AchievementController
    {
        private readonly ILogger<AchievementController> _logger;
        private readonly IAchievementService _achievementService;
        private readonly IUserRepository _userRepository;

        public AchievementController(ILogger<AchievementController> logger, IAchievementService service, IUserRepository userRepository)
        {
            _logger = logger;
            _achievementService = service;
            _userRepository = userRepository;
        }

        [Function(nameof(GetAchievementsById))]
        [OpenApiOperation(operationId: "GetUserAchievements", tags: new[] { "Achievement" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Achievement>), Description = "The OK response with all achievements from user.")]
        public async Task<HttpResponseData> GetAchievementsById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId}/achievements")] HttpRequestData req, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Getting achievement by Id");
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }

            if (await _userRepository.CheckIfUserExistAndActive(UserId))
            {
                var res = _achievementService.GetAchievementsById(UserId);
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(res);
                return response;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }

        }

        [Function(nameof(GetAchievementByTitle))]
        [OpenApiOperation(operationId: "GetUserAchievement", tags: new[] { "Achievement" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user id parameter")]
        [OpenApiParameter(name: "Title", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Achievement Title")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Achievement), Description = "The OK response with userId and id of requested achievement.")]
        public async Task<HttpResponseData> GetAchievementByTitle([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId}/achievement/{Title}")] HttpRequestData req, Guid UserId, string achievementTitle, FunctionContext functionContext)
        {
            _logger.LogInformation("Getting achievement by title");
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }

            if (await _userRepository.CheckIfUserExistAndActive(UserId))
            {
                var res = _achievementService.GetAchievementByTitle(achievementTitle, UserId);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(res);
                return response;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }




        }
        [Function(nameof(UpdateAchievementById))]
        [OpenApiOperation(operationId: "UpdateAchievement", tags: new[] { "Achievement" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "CurrentValue", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The cuurent value you want to change")]
        [OpenApiParameter(name: "achievementTitle", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Title of the requested achievement")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Achievement), Description = "Achievement updated")]
        public async Task<HttpResponseData> UpdateAchievementById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{UserId}/achievement/{achievementTitle}")] HttpRequestData req, Guid UserId, string achievementTitle, int currentValue, FunctionContext functionContext)
        {
            _logger.LogInformation("Updating achievement by id");
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }

            if (await _userRepository.CheckIfUserExistAndActive(UserId))
            {
                var res = _achievementService.UpdateAchievementById(achievementTitle, UserId, currentValue);
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(res);
                return response;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }



        }


    }
}
