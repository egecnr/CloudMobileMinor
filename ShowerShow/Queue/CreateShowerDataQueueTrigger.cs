using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;

namespace ShowerShow.Queue
{
    public class CreateShowerDataQueueTrigger
    {
        private readonly ILogger _logger;
        private IShowerDataService showerDataService;

        public CreateShowerDataQueueTrigger(ILoggerFactory loggerFactory,IShowerDataService showerDataService)
        {
            _logger = loggerFactory.CreateLogger<CreateShowerDataQueueTrigger>();
            this.showerDataService = showerDataService;
        }

        [Function("CreateShowerDataQueueTrigger")]
        public async Task Run([QueueTrigger("create-shower-data", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {

            ShowerData myObj = JsonSerializer.Deserialize<ShowerData>(myQueueItem);

            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            await showerDataService.CreateShowerData(myObj);

        }
    }
}
