using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShowerShow.Authorization;
using ShowerShow.Controllers;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Control
{
    public class UserPreferencesController
    {
        private readonly ILogger<UserPreferencesController> _logger;
        private IUserPrefencesService _userPrefencesService;
        private IUserService _userService;
        public UserPreferencesController(ILogger<UserPreferencesController> logger, IUserPrefencesService userPrefencesService, IUserService userService)
        {
            _logger = logger;
            this._userPrefencesService = userPrefencesService;
            this._userService = userService;

        }
        [Function("GetUserPreferenceById")]
        [OpenApiOperation(operationId: "GetAllPreferences", tags: new[] { "Preferences" })]
        [ExampleAuth]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PreferencesDTO), Description = "Successfully retrieved the Preferences")]
        public async Task<HttpResponseData> GetUserPreferenceById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/preferences")] HttpRequestData req, Guid userId, FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching the user preference by id {userId}");
            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
                PreferencesDTO preferences = await _userPrefencesService.GetUserPreferencesById(userId);
                await responseData.WriteAsJsonAsync(preferences);
                responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result","Successfully retrieved preferences");
            }
            catch(Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }               
            return responseData;

        }
        [Function("UpdatePreferenceById")]
        [OpenApiOperation(operationId: "UpdatePreferences", tags: new[] { "Preferences" })]
        [ExampleAuth]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "Id of the requested user ")]
        [OpenApiRequestBody("application/json", typeof(PreferencesDTO), Description = "The user data to update.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PreferencesDTO), Description = "The OK response with the updated user preference")]
        public async Task<HttpResponseData> UpdatePreferenceById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/preferences")] HttpRequestData req, Guid userId,FunctionContext functionContext)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PreferencesDTO updatePreferencesDTO = JsonConvert.DeserializeObject<PreferencesDTO>(requestBody);

            HttpResponseData responseData = req.CreateResponse();
            try
            {
                if (AuthCheck.CheckIfUserNotAuthorized(functionContext))
                {
                    responseData.StatusCode = HttpStatusCode.Unauthorized;
                    return responseData;
                }
               
                    await _userPrefencesService.UpdatePreferenceById(userId, updatePreferencesDTO);
                    responseData.StatusCode = HttpStatusCode.OK;
                responseData.Headers.Add("Result", "Successfully updated preferences");
            }
            catch(Exception e)
            {
                responseData.StatusCode = HttpStatusCode.BadRequest;
                responseData.Headers.Add("Reason", e.Message);
            }
            return responseData;

        }
    }
}
