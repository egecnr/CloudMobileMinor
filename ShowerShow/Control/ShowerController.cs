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
using ShowerShow.DTO;
using ShowerShow.Models;
using System.Collections.Generic;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;

namespace ShowerShow.Controllers
{
    public class ShowerController
    {
        private readonly ILogger<ShowerController> _logger;
        private readonly IShowerService _showerService;

        public ShowerController(ILogger<ShowerController> log, ShowerService showerService)
        {
            _logger = log;
            _showerService = showerService;
        }

        [Function(nameof(GetShowerById))]
        [OpenApiOperation(operationId: "getShowerById", tags: new[] { "Shower data" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The user id parameter")]
        [OpenApiParameter(name: "ShowerId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "id of the requested shower")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ShowerData), Description = "Successfully received the shower data.")]
        public async Task<HttpResponseData> GetShowerById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{UserId}/showerdata/{showerId}")] HttpRequestData req, Guid UserId, Guid showerId)
        {

            var response = _showerService.GetShowerDataByUserId(UserId, showerId);

            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.OK);

            await responseData.WriteAsJsonAsync(response);
            return responseData;


        }


        [Function(nameof(CreateShower))]
        [OpenApiOperation(operationId: "CreateShowerDataById", tags: new[] { "Shower data" })]
        [OpenApiRequestBody("application/json", typeof(CreateShowerDataDTO), Description = "Created the shower data object")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ShowerData), Description = "The OK response with the new shower.")]
        public async Task<IActionResult> CreateShower([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId/showerdata")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Creating new shower.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {

                CreateShowerDataDTO showerDataDTO = JsonConvert.DeserializeObject<CreateShowerDataDTO>(requestBody);
                await _showerService.CreateShower(showerDataDTO);

                return new OkObjectResult($"Shower created: {showerDataDTO.Date}");


            }
            catch (Exception ex)
            {
                // DEV ONLY
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}

