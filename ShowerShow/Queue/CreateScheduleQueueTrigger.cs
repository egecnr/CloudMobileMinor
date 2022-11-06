using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShowerShow.Queue
{
    public class CreateScheduleQueueTrigger
    {
        private readonly ILogger _logger;
        private IScheduleService scheduleService;
       

        public CreateScheduleQueueTrigger(ILoggerFactory loggerFactory,IScheduleService scheduleService)
        {
            _logger = loggerFactory.CreateLogger<CreateScheduleQueueTrigger>();
            this.scheduleService = scheduleService;
           
        }

        [Function("CreateScheduleQueueTrigger")]
        public async Task RunAsync([QueueTrigger("create-schedule-queue", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            
            try
            {
                Schedule schedule = JsonSerializer.Deserialize<Schedule>(myQueueItem);
                await scheduleService.CreateSchedule(schedule);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
