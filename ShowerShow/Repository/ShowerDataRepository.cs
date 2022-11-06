using AutoMapper;
using Azure.Storage.Queues;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    internal class ShowerDataRepository : IShowerDataRepository
    {
        private DatabaseContext _dbContext;
        public ShowerDataRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;    
        }

        public async Task AddShowerToQueue(CreateShowerDataDTO shower, Guid userId)
        {
            string qName = Environment.GetEnvironmentVariable("CreateShowerData");
            string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            QueueClientOptions clientOpt = new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };

            QueueClient qClient = new QueueClient(connString, qName, clientOpt);
            var jsonOpt = new JsonSerializerOptions() { WriteIndented = true };
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateShowerDataDTO, ShowerData>()));
            ShowerData showerData = mapper.Map<ShowerData>(shower);
            showerData.UserId = userId;
            string showerJson = JsonSerializer.Serialize<ShowerData>(showerData, jsonOpt);
            await qClient.SendMessageAsync(showerJson);
        }

        public async Task CreateShowerData(ShowerData showerData)
        {
            _dbContext.ShowerInstances?.Add(showerData);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {
            await _dbContext.SaveChangesAsync();
            return _dbContext.ShowerInstances.Where(x => x.UserId == userId).FirstOrDefault(y => y.Id == showerId);

        }

    }
}
