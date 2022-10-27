using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ShowerShow.DTO;
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
        public async Task Run([QueueTrigger("create-user-friend-queue", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            List<Guid> guids = JsonSerializer.Deserialize<List<Guid>>(myQueueItem);
            await userFriendService.CreateUserFriend(guids[0], guids[1]);
        }
    }
}
