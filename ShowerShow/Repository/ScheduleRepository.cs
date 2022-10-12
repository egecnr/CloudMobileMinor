using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    internal class ScheduleRepository :IScheduleRepository
    {
        private DatabaseContext dbContext;

        public ScheduleRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateSchedule(CreateScheduleDTO schedule, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateScheduleDTO, Schedule>()));
            Schedule fullSchedule = mapper.Map<Schedule>(schedule);
            fullSchedule.UserId = userId;
            dbContext.Schedules?.Add(fullSchedule);
            await dbContext.SaveChangesAsync();
        }
    }
}
