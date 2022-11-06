using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Threading.Tasks;
using System.IO;
using ShowerShow.Model;
using System.Collections.Generic;
using ShowerShow.DTO;

namespace ShowerShow.Repository.Interface
{
    public interface IBubbleMessageRepository
    {
        public Task CreateBubbleMessage(CreateBubbleMessageDTO message);
        public Task DeleteBubbleMessage(Guid messageId);
        public Task<BubbleMessage> GetRandomBubbleMessage();
        public Task<bool> CheckIfMessageExist(Guid messageId);
        public Task<BubbleMessage> GetBubbleMessageById(Guid messageId);
        public Task<IEnumerable<BubbleMessage>> GetListOfRandomBubbleMessages(uint limit);

    }
}
