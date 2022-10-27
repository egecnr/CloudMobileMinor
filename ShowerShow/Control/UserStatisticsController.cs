using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using User = ShowerShow.Models.User;
using CreateUserDTO = ShowerShow.DTO.CreateUserDTO;
using AutoMapper;
using ShowerShow.Repository.Interface;
using ShowerShow.DTO;
using ShowerShow.Models;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShowerShow.Utils;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.Authorization;
using System.Security.Claims;
using ShowerShow.Model;

namespace ShowerShow.Controllers
{
    public class UserStatisticsController
    {
        private readonly ILogger<UserStatisticsController> _logger;
        private IUserStatisticsService userStatisticsService;
        private IUserService userService;

        public UserStatisticsController(ILogger<UserStatisticsController> log, IUserService userService, IUserStatisticsService userStatisticsService)
        {
            _logger = log;
            this.userService = userService;
            this.userStatisticsService = userStatisticsService;
        }
        [Function("GetFriendsRanking")]
        [OpenApiOperation(operationId: "GetFriendsRanking", tags: new[] { "User Statistics" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "limit", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The limit")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<Guid, double>), Description = "The OK response with the friends ranking" + ".")]
        public async Task<HttpResponseData> GetFriendsRanking([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId:Guid}/friendRanking/{limit}")] HttpRequestData req, Guid UserId, int limit, FunctionContext functionContext)
        {
            HttpResponseData responseData = req.CreateResponse();

            //TO DO: make this a better function
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }

            if (!await userService.CheckIfUserExistAndActive(UserId))
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
            Dictionary<Guid, double> friendRanking = await userStatisticsService.GetFriendRanking(UserId, limit);
            await responseData.WriteAsJsonAsync(friendRanking);
            responseData.StatusCode = HttpStatusCode.OK;
            return responseData;
        }
        [Function("GetUserDashboard")]
        [OpenApiOperation(operationId: "GetUserDashboard", tags: new[] { "User Statistics" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "Days", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The amount of days to go back")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserDashboard), Description = "The OK response with the User Dashboard")]
        public async Task<HttpResponseData> GetUserDashboard([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId:Guid}/dashboard/{Days}")] HttpRequestData req, Guid UserId, int Days, FunctionContext functionContext)
        {
            HttpResponseData responseData = req.CreateResponse();

            //TO DO: make this a better function
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }

            if (!await userService.CheckIfUserExistAndActive(UserId))
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
            UserDashboard userDashboard = await userStatisticsService.GetUserDashboard(UserId, Days);
            await responseData.WriteAsJsonAsync(userDashboard);
            responseData.StatusCode = HttpStatusCode.OK;
            return responseData;
        }
    }
}

