using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Linq;

namespace ShowerShow.Repository
{
    internal class ScheduleRepository : IScheduleRepository
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

        public async Task DeleteSchedule(Schedule schedule)
        {
            dbContext.Schedules?.Remove(schedule);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules(Guid UserId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.Schedules.Where(x => x.UserId == UserId).ToList();
        }

        public async Task<Schedule> GetScheduleById(Guid scheduleId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.Schedules.FirstOrDefault(x => x.Id == scheduleId);
        }

        public async Task UpdateSchedule(Schedule oldSchedule, UpdateScheduleDTO newSchedule)
        {
            oldSchedule.DaysOfWeek = newSchedule.DaysOfWeek;
            oldSchedule.Tags = newSchedule.Tags;
            dbContext.Schedules?.Update(oldSchedule);

            await dbContext.SaveChangesAsync();
        }
    }
}
