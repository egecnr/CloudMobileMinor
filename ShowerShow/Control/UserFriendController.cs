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
        [OpenApiOperation(operationId: "AcceptFriendRequest", tags: new[] { "User Friends" },Summary ="Accept friend request", Description = "This endpoint accepts a pending friend request")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of main user")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of friend user")]
        public async Task<HttpResponseData> AcceptFriendRequest([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                    await userFriendService.AcceptFriendRequest(userId, friendId);
                    responseData.StatusCode = HttpStatusCode.Accepted;
                    responseData.Headers.Add("Result","Friend request succesfully accepted");
                    return responseData;
                
            }
            catch(Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
                return responseData;
            }
         
        }

        //done
        [Function("FavoriteFriendById")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "FavoriteFriendById", tags: new[] { "User Friends" }, Summary = "Favorite a friend", Description = "This endpoint favorites a friend")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of main user")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of friend user")]
        [OpenApiParameter(name: "isFavorite", In = ParameterLocation.Path, Required = true, Type = typeof(bool), Description = "Favorite/Unfavorite")]
        public async Task<HttpResponseData> FavoriteFriendById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/friends/{friendId:Guid}/{isFavorite}")] HttpRequestData req, Guid userId, Guid friendId,bool isFavorite, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                await userFriendService.ChangeFavoriteStateOfFriend(userId, friendId, isFavorite);
                responseData.Headers.Add("Result", "Successfully favorited a friend");
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
                return responseData;
            }
        }

        [Function("GetUserFriends")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "GetUserFriends", tags: new[] { "User Friends" }, Summary = "Get all friends", Description = "This endpoint retrieves all friends of user")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<GetUserDTO>), Description = "The OK response with the new schedule.")]
        public async Task<HttpResponseData> GetUserFriends([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends")] HttpRequestData req, Guid userId,FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching friends from the user with id.{userId}");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                IEnumerable<GetUserFriendDTO> friendListToView = await userFriendService.GetAllFriendsOfUser(userId);
                await responseData.WriteAsJsonAsync(friendListToView);
                responseData.Headers.Add("Result", "Successfully fetched all friends");
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
                return responseData;
            }
        }

        [Function("GetUserFriendById")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "GetUserFriendById", tags: new[] { "User Friends" }, Summary = "Get friend by id", Description = "This endpoint retrieves the friend with the specified id")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of main user")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter of friend user")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetUserFriendDTO), Description = "The OK response with get friend by id.")]
        public async Task<HttpResponseData> GetUserFriendById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching friend from the user with id.{userId}");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                GetUserFriendDTO userDTO = await userFriendService.GetUserFriendsById(userId, friendId);
                await responseData.WriteAsJsonAsync(userDTO);
                responseData.Headers.Add("Result", "Successfully retrieved a friend");
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
                return responseData;
            }
        }

        [Function("CreateUserFriend")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "CreateUserFriend", tags: new[] { "User Friends" }, Summary = "Create friend request", Description = "This endpoint sends a friend request")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> CreateUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req,Guid userId, Guid friendId, FunctionContext functionContext)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                await userFriendService.AddFriendToQueue(userId, friendId);
                responseData.Headers.Add("Result", "Successfully created a friend");
                responseData.StatusCode = HttpStatusCode.Created;
                return responseData;
                  
            }catch(Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason",e.Message);
                return responseData;
            }
        }

        [Function("DeleteUserFriend")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "DeleteUserFriend", tags: new[] { "User Friends" }, Summary = "Delete friend by id", Description = "This endpoint deletes the friend with the specified id")]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiParameter(name: "friendId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The OK response with the new user.")]
        public async Task<HttpResponseData> DeleteUserFriend([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{userId:Guid}/friends/{friendId:Guid}")] HttpRequestData req, Guid userId, Guid friendId, FunctionContext functionContext)
        {
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                await userFriendService.DeleteFriend(userId, friendId);
                responseData.Headers.Add("Result", "Successfully deleted a friend");
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
                return responseData;
            }
        }
    }
}
