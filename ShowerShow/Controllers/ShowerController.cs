using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FunctionApp1.DTO;
using FunctionApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FunctionApp1.Controllers
{
    public class ShowerController
    {
        private readonly ILogger<ShowerController> _logger;

        public ShowerController(ILogger<ShowerController> log)
        {
            _logger = log;
        }
        [FunctionName("CreateShower")]
        [OpenApiOperation(operationId: "CreateShower", tags: new[] { "Showers" })]
        [OpenApiRequestBody("application/json", typeof(CreateShowerDataDTO), Description = "The shower data.")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ShowerData), Description = "The OK response with the new shower.")]
        public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "showerdata/{UserId:Guid}")] HttpRequest req, Guid UserId)
        {
            _logger.LogInformation("Creating new shower.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                CreateShowerDataDTO data = JsonConvert.DeserializeObject<CreateShowerDataDTO>(requestBody);

                ShowerData shower = new();
                //shower.UserId = Guid.Parse(req.Path.Value);
                shower.UserId = UserId;
                shower.Duration = data.Duration;
                shower.WaterUsage = data.WaterUsage;
                shower.GasUsage = data.GasUsage;
                shower.WaterCost = data.WaterCost;
                shower.GasCost = data.GasCost;
                shower.Date = data.Date;
                shower.Schedule = data.Schedule;


                return new OkObjectResult(shower);
            }
            catch (Exception ex)
            {
                // DEV ONLY
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}

