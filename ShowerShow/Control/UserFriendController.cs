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
using ShowerShow.Authorization;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;

namespace ShowerShow.Control
{
    public class UserFriendController
    {
        private readonly ILogger _logger;
        private IUserFriendService userFriendService;
        private IUserService userService;

        public UserFriendController(ILoggerFactory loggerFactory,IUserService userService,IUserFriendService userFriendService)
        {
            _logger = loggerFactory.CreateLogger<UserFriendController>();
            this.userFriendService = userFriendService;
            this.userService = userService;
        }
        //done
        [Function("AcceptFriendRequest")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "AcceptFriendRequest", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of main user")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of friend user")]
        public async Task<HttpResponseData> AcceptFriendRequest([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");
            HttpResponseData responseData = req.CreateResponse();

            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            if (await userService.CheckIfUserExist(userId) && await userService.CheckIfUserExist(userId) 
                && await userFriendService.CheckIfUserIsAlreadyFriend(userId,friendId)
                && await userFriendService.CheckFriendStatusIsResponseRequired(userId,friendId))
            {

                await userFriendService.AcceptFriendRequest(userId,friendId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }

        //done
        [Function("FavoriteFriendById")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "FavoriteFriendById", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of main user")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of friend user")]
        [OpenApiParameter(name: "isFavorite", In = ParameterLocation.Path, Required = true, Type = typeof(bool), Description = "Favorite/Unfavorite")]
        public async Task<HttpResponseData> FavoriteFriendById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/friends/{friendId:Guid}/{isFavorite}")] HttpRequestData req, Guid userId, Guid friendId,bool isFavorite, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");
            HttpResponseData responseData = req.CreateResponse();
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            if (await userService.CheckIfUserExist(userId) && await userService.CheckIfUserExist(userId)
                && await userFriendService.CheckIfUserIsAlreadyFriend(userId, friendId))
            {

                await userFriendService.ChangeFavoriteStateOfFriend(userId, friendId,isFavorite);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }

        //done
        [Function("GetUserFriends")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "GetUserFriends", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUserFriends([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends")] HttpRequestData req, Guid userId,FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching friends from the user with id.{userId}");
            HttpResponseData responseData = req.CreateResponse();

            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            if (await userService.CheckIfUserExistAndActive(userId))
            {
                IEnumerable<GetUserFriendDTO> friendListToView = await userFriendService.GetAllFriendsOfUser(userId);

                await responseData.WriteAsJsonAsync(friendListToView);
                responseData.StatusCode = HttpStatusCode.Created;
                return responseData;
            }
            else
            {

                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }
        //done
        [Function("GetUserFriendById")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "GetUserFriendById", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of main user")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of friend user")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetUserFriendDTO), Description = "The OK response with get friend by id.")]
        public async Task<HttpResponseData> GetUserFriendById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching friend from the user with id.{userId}");
            HttpResponseData responseData = req.CreateResponse();

            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            if (await userService.CheckIfUserExistAndActive(userId) && await userService.CheckIfUserExistAndActive(friendId) && 
                await userFriendService.CheckIfUserIsAlreadyFriend(userId,friendId))
            {
                GetUserFriendDTO userDTO = await userFriendService.GetUserFriendsById(userId, friendId);            
                await responseData.WriteAsJsonAsync(userDTO);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            else
            {
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }
        }
        //done
        [Function("CreateUserFriend")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "CreateUserFriend", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req,Guid userId, Guid friendId, FunctionContext functionContext)
        {
            //You cant add the same friend twice. Implement it
            _logger.LogInformation("Creating new friend request.");
            HttpResponseData responseData = req.CreateResponse();

            //Check if both users are present in db
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            if (await userService.CheckIfUserExistAndActive(userId) 
                && await userService.CheckIfUserExistAndActive(friendId))
            {
                //Check if they re already friends or  whether both ids are the same.
                if (await userFriendService.CheckIfUserIsAlreadyFriend(userId, friendId))
                {
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
                else
                {
                    await userFriendService.CreateUserFriend(userId, friendId);
                    responseData.StatusCode = HttpStatusCode.Created;
                    return responseData;
                }        
            }
            else
            {              
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }


        }

        [Function("DeleteUserFriend")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "DeleteUserFriend", tags: new[] { "User Friends" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> DeleteUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId, FunctionContext functionContext)
        {
            //You cant add the same friend twice. Implement it
            _logger.LogInformation("Creating new user.");
            HttpResponseData responseData = req.CreateResponse();
            //Check if both users are present in db
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
              
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            if (await userService.CheckIfUserExistAndActive(userId)
                && await userService.CheckIfUserExistAndActive(friendId))
            {
                //Check if they re already friends
                if (await userFriendService.CheckIfUserIsAlreadyFriend(userId, friendId))
                {
                   await userFriendService.DeleteFriend(userId, friendId);
                    responseData.StatusCode = HttpStatusCode.Accepted;
                    return responseData;
                }
                else
                {     
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
            }
            else
            {
                responseData.StatusCode = HttpStatusCode.NotFound;
                return responseData;
            }


        }
    }
}
