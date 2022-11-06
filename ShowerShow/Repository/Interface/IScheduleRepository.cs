using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IScheduleRepository
    {
        public Task CreateSchedule(Schedule schedule);
        public Task<IEnumerable<Schedule>> GetAllSchedules(Guid UserId);
        public Task<Schedule> GetScheduleById(Guid scheduleId);
        public Task DeleteSchedule(Guid scheduleId);
        public Task<Schedule> UpdateSchedule(Guid scheduleId, UpdateScheduleDTO newSchedule);
        public Task AddScheduleToQueue(CreateScheduleDTO schedule,Guid userId);
        public Task<bool> DoesScheduleExist(Guid scheduleId);
    }
}
