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
using ShowerShow.Repository.Interface;
using ShowerShow.Service;

namespace ShowerShow.Control
{
    public class UserFriendController
    {
        private readonly ILogger _logger;
        private IUserService userService;

        public UserFriendController(ILoggerFactory loggerFactory,IUserService userService)
        {
            _logger = loggerFactory.CreateLogger<UserFriendController>();
            this.userService = userService;
        }

        //This is not complete at all yet.
        [Function("GetUserFriends")]
        [OpenApiOperation(operationId: "GetUserFriends", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUserFriends([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends")] HttpRequestData req, Guid userId)
        {
            _logger.LogInformation($"Fetching friends from the user with id.{userId}");

            if (await userService.CheckIfUserExist(userId))
            {
                List<GetUserDTO> friendListToView = await userService.GetAllFriendsOfUser(userId);
                _logger.LogDebug(friendListToView.Count.ToString());
                HttpResponseData responseData = req.CreateResponse();
                await responseData.WriteAsJsonAsync(friendListToView);
                responseData.StatusCode = HttpStatusCode.Created;
                return responseData;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }

        [Function("CreateUserFriend")]
        [OpenApiOperation(operationId: "CreateUserFriend", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userId1", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "userId2", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId1:Guid}/{userId2:Guid}/friends")] HttpRequestData req,Guid userId1,Guid userId2)
        {
            //You cant add the same friend twice. Implement it
            _logger.LogInformation("Creating new user.");
          
            //Check if both users are present in db
            if (await userService.CheckIfUserExist(userId1) 
                && await userService.CheckIfUserExist(userId2))
            {
                //Check if they re already friends or  whether both ids are the same.
                if (await userService.CheckIfUserIsAlreadyFriend(userId1, userId2))
                {
                    HttpResponseData responseData = req.CreateResponse();
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                else
                {
                    await userService.CreateUserFriend(userId1, userId2);
                    HttpResponseData responseData = req.CreateResponse();
                    responseData.StatusCode = HttpStatusCode.Created;
                    return responseData;
                }        
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
