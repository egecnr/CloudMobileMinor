using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ShowerShow.DTO;
using ShowerShow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ShowerShow.Controllers
{
    public class ScheduleController
    {
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> log)
        {
            _logger = log;
        }
        [FunctionName("CreateSchedule")]
        [OpenApiOperation(operationId: "CreateSchedule", tags: new[] { "Schedules" })]
        [OpenApiRequestBody("application/json", typeof(CreateScheduleDTO), Description = "The schedule data.")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Schedule), Description = "The OK response with the new schedule.")]
        public async Task<IActionResult> CreateSchedule([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "schedule/{UserId:Guid}")] HttpRequest req, Guid UserId)
        {
            _logger.LogInformation("Creating new schedule.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                CreateScheduleDTO data = JsonConvert.DeserializeObject<CreateScheduleDTO>(requestBody);

                Schedule schedule = new();
                schedule.UserId = UserId;
                schedule.DayOfWeek = data.DayOfWeek;
                schedule.Tags = data.Tags;


                return new OkObjectResult(schedule);
            }
            catch (Exception ex)
            {
                // DEV ONLY
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}

