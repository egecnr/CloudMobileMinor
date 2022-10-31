using ShowerShow.Repository.Interface;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using HttpMultipartParser;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using ShowerShow.DAL;
using System.Collections.Generic;
using ShowerShow.Model;
using System.Linq;
using ShowerShow.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DTO;
using AutoMapper;
using ShowerShow.Utils;
using Microsoft.EntityFrameworkCore;

namespace ShowerShow.Repository
{
    public class BubbleMessageRepository : IBubbleMessageRepository
    {
        private DatabaseContext dbContext;
        Random rnd = new Random();

        public BubbleMessageRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateBubbleMessage(CreateBubbleMessageDTO message)
        {
            //map DTO to normal Bubble Message
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateBubbleMessageDTO, BubbleMessage>()));
            BubbleMessage bubbleMessage = mapper.Map<BubbleMessage>(message);
            dbContext.BubbleMessages.Add(bubbleMessage);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteBubbleMessage(Guid messageId)
        {
            BubbleMessage bubbleMessage = null;

            //this is to give priority to tasks
            Task getId = Task.Run(() =>
            {
                bubbleMessage = GetBubbleMessageById(messageId).Result; // get message
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.BubbleMessages?.Remove(bubbleMessage); // delete message

            });
            await dbContext.SaveChangesAsync();
        }

        public async Task<BubbleMessage> GetBubbleMessageById(Guid messageId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.BubbleMessages.FirstOrDefault(x => x.Id == messageId);
        }

        public async Task<IEnumerable<BubbleMessage>> GetListOfRandomBubbleMessages(uint limit)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.BubbleMessages.OrderBy(x=> x.Id).Take((int)limit).ToList();
        }

        public async Task<BubbleMessage> GetRandomBubbleMessage()
        { 
            await dbContext.SaveChangesAsync();
            int skipper = rnd.Next(0, dbContext.BubbleMessages.Count());
            return await dbContext.BubbleMessages.Skip(skipper).FirstOrDefaultAsync();
        }
    }
}
