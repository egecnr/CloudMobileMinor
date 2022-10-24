using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task CreateShower(CreateShowerDataDTO shower, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateShowerDataDTO, ShowerData>()));
            ShowerData showerData = mapper.Map<ShowerData>(shower);
            showerData.UserId = userId;
            //showerData.Id = showerData.Id;
            //showerData.Duration = shower.Duration;
            //showerData.WaterUsage = shower.WaterUsage;
            //showerData.WaterCost = shower.WaterCost;
            //showerData.GasUsage = shower.GasUsage;
            //showerData.GasCost = shower.GasCost;
            //showerData.Date = shower.Date;
            //showerData.ScheduleId = shower.ScheduleId;
            _dbContext.ShowerInstances?.Add(showerData);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {
            var showerData = _dbContext.ShowerInstances.Where(x => x.Id == userId).Where(y => y.Id == showerId).FirstOrDefault();
            return showerData;
        }

    }
}
