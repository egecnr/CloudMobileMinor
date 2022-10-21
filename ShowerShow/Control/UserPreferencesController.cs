using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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

        [Function("CreateUserPreferences")]
        [OpenApiOperation(operationId: "CreatePreferences", tags: new[] { "Preferences" })]
        [OpenApiRequestBody("application/json", typeof(CreatePreferencesDTO), Description = "Id of the requested user ")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreatePreferencesDTO), Description = "Successfully created the Preferences")]
        public async Task <HttpResponseData> CreateUserPreferences([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId}/preferences")] HttpRequestData req)
        {
            _logger.LogInformation("Create a preference for one user");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CreatePreferencesDTO createPreferencesDTO = JsonConvert.DeserializeObject<CreatePreferencesDTO>(requestBody);

            if (await _userService.CheckIfUserExistAndActive(createPreferencesDTO.UserId))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
            else
            {
                await _userPrefencesService.CreateUserPreferences(createPreferencesDTO);
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.Created;

                return responseData;
            }
        }
        [Function("GetUserPreferenceById")]
        [OpenApiOperation(operationId: "GetAllPreferences", tags: new[] { "Preferences" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Preferences>), Description = "Successfully retrieved the Preferences")]
        public async Task<HttpResponseData> GetUserPreferenceById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userId:Guid}/preferences")] HttpRequestData req, Guid userId)
        {
            _logger.LogInformation($"Fetching the user preference by id {userId}");

            if (await _userService.CheckIfUserExistAndActive(userId))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
            else
            {
                var preferences = await _userPrefencesService.GetUserPreferencesById(userId);
                HttpResponseData responseData = req.CreateResponse();
                await responseData.WriteAsJsonAsync(preferences);
                responseData.StatusCode = HttpStatusCode.OK;

                return responseData;
            }
        }
        [Function("UpdatePreferenceById")]
        [OpenApiOperation(operationId: "UpdatePreferences", tags: new[] { "Preferences" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "Id of the requested user ")]
        [OpenApiRequestBody("application/json", typeof(UpdatePreferencesDTO), Description = "The user data to update.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UpdatePreferencesDTO), Description = "The OK response with the updated user preference")]
        public async Task<HttpResponseData> UpdatePreferenceById([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId:Guid}/preferences")] HttpRequestData req, Guid userId)
        {
            _logger.LogInformation($"Fetching the user by id {userId}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UpdatePreferencesDTO updatePreferencesDTO = JsonConvert.DeserializeObject<UpdatePreferencesDTO>(requestBody);

            if (await _userService.CheckIfUserExistAndActive(userId))
            {
                HttpResponseData responseData = req.CreateResponse();
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
            else
            {
                HttpResponseData responseData = req.CreateResponse();
                await _userPrefencesService.UpdatePreferenceById(userId, updatePreferencesDTO);
                responseData.StatusCode = HttpStatusCode.OK;

                return responseData;
            }





        }
    }
}
