using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
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
        [OpenApiOperation(operationId: "GetUserFriends", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUserFriends([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends")] HttpRequestData req, Guid userId)
        {
            _logger.LogInformation($"Fetching friends from the user with id.{userId}");

            if (await userService.CheckIfUserExistAndActive(userId))
            {
                IEnumerable<GetUserDTO> friendListToView = await userService.GetAllFriendsOfUser(userId);
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

        [Function("GetUserFriendsByName")]
        [OpenApiOperation(operationId: "GetUserFriendByName", tags: new[] { "Users" })]
        [OpenApiParameter(name: "userName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The User ID parameter")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUserFriendsByName([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends/{userName}")] HttpRequestData req, string userName,Guid userId)
        {
            _logger.LogInformation($"Fetching users by name {userName}");
            if (!userName.IsNullOrWhiteSpace() && await userService.CheckIfUserExistAndActive(userId))
            {
                HttpResponseData responseData = req.CreateResponse();
                IEnumerable<GetUserDTO> allFriendsWithName = await userService.GetUserFriendsByName(userId, userName);
                await responseData.WriteAsJsonAsync(allFriendsWithName);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }

        [Function("GetUserFriendById")]
        [OpenApiOperation(operationId: "GetUserFriendById", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetUserDTO), Description = "The OK response with get friend by id.")]
        public async Task<HttpResponseData> GetUserFriendById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId)
        {
            _logger.LogInformation($"Fetching friend from the user with id.{userId}");

            if (await userService.CheckIfUserExistAndActive(userId) && await userService.CheckIfUserIsAlreadyFriend(userId,friendId))
            {
                GetUserDTO userDTO = await userService.GetUserById(friendId);            
                HttpResponseData responseData = req.CreateResponse();
                await responseData.WriteAsJsonAsync(userDTO);
                responseData.StatusCode = HttpStatusCode.OK;
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
        [OpenApiOperation(operationId: "CreateUserFriend", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId1", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "userId2", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId1:Guid}/{userId2:Guid}/friends")] HttpRequestData req,Guid userId1,Guid userId2)
        {
            //You cant add the same friend twice. Implement it
            _logger.LogInformation("Creating new user.");
          
            //Check if both users are present in db
            if (await userService.CheckIfUserExistAndActive(userId1) 
                && await userService.CheckIfUserExistAndActive(userId2))
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

        [Function("DeleteUserFriend")]
        [OpenApiOperation(operationId: "DeleteUserFriend", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId1", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "userId2", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> DeleteUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{userId1:Guid}/{userId2:Guid}/friends")] HttpRequestData req, Guid userId1, Guid userId2)
        {
            //You cant add the same friend twice. Implement it
            _logger.LogInformation("Creating new user.");

            //Check if both users are present in db
            if (await userService.CheckIfUserExistAndActive(userId1)
                && await userService.CheckIfUserExistAndActive(userId2))
            {
                //Check if they re already friends
                if (await userService.CheckIfUserIsAlreadyFriend(userId1, userId2))
                {
                    await userService.DeleteUserFriend(userId1, userId2);
                    HttpResponseData responseData = req.CreateResponse();
                    responseData.StatusCode = HttpStatusCode.Accepted;
                    return responseData;
                }
                else
                {     
                    HttpResponseData responseData = req.CreateResponse();
                    responseData.StatusCode = HttpStatusCode.BadRequest;
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
