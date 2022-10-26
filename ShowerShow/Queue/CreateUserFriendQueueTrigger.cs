using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ShowerShow.Repository.Interface;

namespace ShowerShow.Queue
{
    public class CreateUserFriendQueueTrigger
    {
        private readonly ILogger _logger;
        private IUserFriendService userFriendService;

        public CreateUserFriendQueueTrigger(ILoggerFactory loggerFactory, IUserFriendService userFriendService)
        {
            _logger = loggerFactory.CreateLogger<CreateUserFriendQueueTrigger>();
            this.userFriendService = userFriendService;
        }

        [Function("CreateUserFriendQueueTrigger")]
        public void Run([QueueTrigger("create-user-friend-queue", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
