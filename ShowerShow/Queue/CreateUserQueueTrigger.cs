using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShowerShow.Queue
{
    public class CreateUserQueueTrigger
    {
        private readonly ILogger _logger;
        private  IUserService userService;
       

        public CreateUserQueueTrigger(ILoggerFactory loggerFactory,IUserService userService)
        {
            _logger = loggerFactory.CreateLogger<CreateUserQueueTrigger>();
            this.userService = userService;
           
        }

        [Function("CreateUserQueueTrigger")]
        public async Task RunAsync([QueueTrigger("create-user-queue", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            
            try
            {
                CreateUserDTO createUserDTO = JsonSerializer.Deserialize<CreateUserDTO>(myQueueItem);
                await userService.CreateUser(createUserDTO);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
