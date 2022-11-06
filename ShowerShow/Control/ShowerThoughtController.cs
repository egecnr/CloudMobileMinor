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
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using System.Collections.Generic;
using ShowerShow.Model;
using ShowerShow.Authorization;

namespace ShowerShow.Controllers
{
    public class ShowerThoughtController
    {
        private readonly ILogger<ShowerThoughtController> _logger;
        private IShowerThoughtService showerThoughtService;
        private IUserService userService;

        public ShowerThoughtController(ILogger<ShowerThoughtController> log, IShowerThoughtService showerThoughtService, IUserService userService)
        {
            _logger = log;
            this.showerThoughtService = showerThoughtService;
            this.userService = userService;
        }
        [Function("CreateShowerThought")]
        [OpenApiOperation(operationId: "CreateShowerThought", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiRequestBody("application/json", typeof(ShowerThoughtDTO), Description = "The shower thought data.")]
        [OpenApiParameter(name: "ShowerId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The ShowerId parameter")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Query, Required = false, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the new thought.")]
        public async Task<HttpResponseData> CreateShowerThought([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "shower/thoughts/{ShowerId:Guid}")] HttpRequestData req, Guid ShowerId, Guid UserId, FunctionContext functionContext)
        {
            _logger.LogInformation("Creating new thought.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                // passing both shower id and user id because I dont have a ShowerData class
                // later get UserId based on ShowerId
                ShowerThoughtDTO data = JsonConvert.DeserializeObject<ShowerThoughtDTO>(requestBody);
                await showerThoughtService.CreateShowerThought(data, ShowerId, UserId);
                await responseData.WriteAsJsonAsync(data);
                responseData.StatusCode = HttpStatusCode.Created;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("DeleteShowerThought")]
        [ExampleAuth]
        [OpenApiOperation(operationId: "DeleteShowerThought", tags: new[] { "Shower Thoughts" })]
        [OpenApiParameter(name: "ThoughtId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The ShowerThought ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the deleted thought")]
        public async Task<HttpResponseData> DeleteShowerThought([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "shower/thoughts/{ThoughtId:Guid}")] HttpRequestData req, Guid ThoughtId, FunctionContext functionContext)
        {
            _logger.LogInformation("Deleting thought.");
            HttpResponseData responseData = req.CreateResponse();
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            try
            {
                await showerThoughtService.DeleteShowerThought(ThoughtId);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetThoughtById")]
        [OpenApiOperation(operationId: "GetThoughtById", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiParameter(name: "ThoughtId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The thought ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the retrieved thought")]
        public async Task<HttpResponseData> GetThoughtById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shower/thoughts/{ThoughtId:Guid}")] HttpRequestData req, Guid ThoughtId, FunctionContext functionContext)
        {
            _logger.LogInformation("Retrieving schedule.");

            HttpResponseData responseData = req.CreateResponse();
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            try
            {
                ShowerThought thought = await showerThoughtService.GetShowerThoughtById(ThoughtId);
                await responseData.WriteAsJsonAsync(thought);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetThoughtByShowerId")]
        [OpenApiOperation(operationId: "GetThoughtByShowerId", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiParameter(name: "ShowerId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The shower ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the retrieved thoughts")]
        public async Task<HttpResponseData> GetThoughtByShowerId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shower/thoughts/{ShowerId:Guid}/s")] HttpRequestData req, Guid ShowerId, FunctionContext functionContext)
        {
            _logger.LogInformation("Retrieving schedule.");

            HttpResponseData responseData = req.CreateResponse();
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            try
            {
                ShowerThought thought = await showerThoughtService.GetThoughtByShowerId(ShowerId);
                await responseData.WriteAsJsonAsync(thought);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;

            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetThoughtsByUserId")]
        [OpenApiOperation(operationId: "GetThoughtsByUserId", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiParameter(name: "limit", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The limit parameter")]

        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the retrieved thoughts")]
        public async Task<HttpResponseData> GetThoughtsByUserId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shower/thoughts/{UserId:Guid}/u")] HttpRequestData req, Guid UserId, int limit, FunctionContext functionContext)
        {
            _logger.LogInformation("Retrieving thoughts.");


            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }

                List<ShowerThought> thoughts = (List<ShowerThought>)await showerThoughtService.GetAllShowerThoughtsForUser(UserId, limit);
                await responseData.WriteAsJsonAsync(thoughts);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetThoughtsByDate")]
        [OpenApiOperation(operationId: "GetThoughtsByDate", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiParameter(name: "Date", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date parameter")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the retrieved thoughts")]
        public async Task<HttpResponseData> GetThoughtsByDate([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shower/thoughts/{UserId:Guid}/date")] HttpRequestData req, Guid UserId, string Date, FunctionContext functionContext)
        {
            _logger.LogInformation("Retrieving thoughts.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                // parse the date to an easy to work with format (only year, month, day are relevant)
                DateTime dateTime = DateTime.ParseExact(Date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                List<ShowerThought> thoughts = (List<ShowerThought>)await showerThoughtService.GetShowerThoughtsByDate(dateTime, UserId);
                await responseData.WriteAsJsonAsync(thoughts);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("UpdateThought")]
        [OpenApiOperation(operationId: "UpdateThought", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiParameter(name: "ThoughtId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The ThoughtId  ")]
        [OpenApiRequestBody("application/json", typeof(UpdateShowerThoughtDTO), Description = "The shower thought data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the updated thought.")]
        public async Task<HttpResponseData> UpdateThought([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "shower/thoughts/{ThoughtId:Guid}")] HttpRequestData req, Guid ThoughtId, FunctionContext functionContext)
        {
            _logger.LogInformation("Updating.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            HttpResponseData responseData = req.CreateResponse();
            if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
            {
                responseData.StatusCode = HttpStatusCode.Unauthorized;
                return responseData;
            }
            try
            {
                UpdateShowerThoughtDTO updatedThought = JsonConvert.DeserializeObject<UpdateShowerThoughtDTO>(requestBody);
                ShowerThought thought = await showerThoughtService.UpdateThought(ThoughtId, updatedThought);
                responseData.StatusCode = HttpStatusCode.OK;
                await responseData.WriteAsJsonAsync(thought);
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }
        }
        [Function("GetThoughtsByContent")]
        [OpenApiOperation(operationId: "GetThoughtsByContent", tags: new[] { "Shower Thoughts" })]
        [ExampleAuth]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user id ")]
        [OpenApiParameter(name: "SearchWord", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The word to search for ")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerThought), Description = "The OK response with the retrieved thoughts.")]
        public async Task<HttpResponseData> GetThoughtsByContent([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shower/thoughts/{UserId:Guid}/{SearchWord}")] HttpRequestData req, Guid UserId, string SearchWord, FunctionContext functionContext)
        {
            _logger.LogInformation("Getting thoughts.");

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }

                List<ShowerThought> thoughts = (List<ShowerThought>)await showerThoughtService.GetThoughtsByContent(SearchWord, UserId);
                await responseData.WriteAsJsonAsync(thoughts);
                responseData.StatusCode = HttpStatusCode.OK;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", ex.Message);
                return responseData;
            }


        }
    }
}

