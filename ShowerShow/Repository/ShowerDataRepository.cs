using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ShowerDataRepository : IShowerDataRepository
    {

        private DatabaseContext dbContext;
        public ShowerDataRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateShower(CreateShowerDataDTO shower, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateShowerDataDTO, ShowerData>()));
            ShowerData showerData = mapper.Map<ShowerData>(shower);
            showerData.Id = userId;
            dbContext.ShowerInstances.Add(showerData);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {

            var showerData = dbContext.ShowerInstances.Where(x => x.Id == userId).Where(y => y.Id == showerId).FirstOrDefault();
            return showerData;
        }

        
    }
}
