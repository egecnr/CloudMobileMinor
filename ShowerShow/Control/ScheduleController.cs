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
using ShowerShow.Models;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using System.Collections.Generic;

namespace ShowerShow.Controllers
{
    public class ScheduleController
    {
        private readonly ILogger<ScheduleController> _logger;
        private IScheduleService scheduleService;

        public ScheduleController(ILogger<ScheduleController> log, IScheduleService scheduleService)
        {
            _logger = log;
            this.scheduleService = scheduleService;
        }
        [Function("CreateSchedule")]
        [OpenApiOperation(operationId: "CreateSchedule", tags: new[] { "Schedules" })]
        [OpenApiRequestBody("application/json", typeof(CreateScheduleDTO), Description = "The schedule data.")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Schedule), Description = "The OK response with the new schedule.")]
        public async Task<CreateScheduleDTO> CreateSchedule([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "schedule/{UserId:Guid}")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Creating new schedule.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                CreateScheduleDTO data = JsonConvert.DeserializeObject<CreateScheduleDTO>(requestBody);
                await scheduleService.CreateSchedule(data,UserId);

                return data;
            }
            catch (Exception ex)
            {
                // DEV ONLY
                throw new Exception(ex.Message);
            }
        }
        [Function("GetSchedules")]
        [OpenApiOperation(operationId: "GetSchedules", tags: new[] { "Schedules" })]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The User ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Schedule), Description = "The OK response with the new schedule.")]
        public async Task<List<Schedule>> GetSchedules([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "schedule/{UserId:Guid}")] HttpRequestData req, Guid UserId)
        {
            _logger.LogInformation("Creating new schedule.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                List<Schedule> schedules = await scheduleService.GetAllSchedules(UserId);
                

                return schedules;
            }
            catch (Exception ex)
            {
                // DEV ONLY
                throw new Exception(ex.Message);
            }
        }
        [Function("GetSchedule")]
        [OpenApiOperation(operationId: "GetScheduleById", tags: new[] { "Schedules" })]
        [OpenApiParameter(name: "ScheduleId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The Schedule ID parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Schedule), Description = "The OK response with the retrieved schedule")]
        public async Task<Schedule> GetSchedule([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "schedule/{ScheduleId:Guid}")] HttpRequestData req, Guid ScheduleId)
        {
            _logger.LogInformation("Retrieving schedule.");

            try
            {
                Schedule schedule = await scheduleService.GetScheduleById(ScheduleId);


                return schedule;
            }
            catch (Exception ex)
            {
                // DEV ONLY
                throw new Exception(ex.Message);
            }
        }
    }
}

