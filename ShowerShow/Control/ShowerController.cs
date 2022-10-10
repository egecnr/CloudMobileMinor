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

namespace ShowerShow.Controllers
{
    public class ShowerController
    {
        private readonly ILogger<ShowerController> _logger;

        public ShowerController(ILogger<ShowerController> log)
        {
            _logger = log;
        }
        [Function("CreateShower")]
        [OpenApiOperation(operationId: "CreateShower", tags: new[] { "Showers" })]
        [OpenApiRequestBody("application/json", typeof(CreateShowerDataDTO), Description = "The shower data.")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ShowerData), Description = "The OK response with the new shower.")]
        public async Task<IActionResult> CreateShower([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "showerdata/{UserId:Guid}")] HttpRequestData req, Guid UserId)
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
                shower.ScheduleId = data.ScheduleId;


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

