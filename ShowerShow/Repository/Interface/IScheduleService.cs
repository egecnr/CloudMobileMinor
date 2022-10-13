using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IScheduleService
    {
        public Task CreateSchedule(CreateScheduleDTO schedule, Guid id);
        public Task<List<Schedule>> GetAllSchedules(Guid UserId);
        public Task<Schedule> GetScheduleById(Guid scheduleId);
        public Task DeleteSchedule(Schedule schedule);
    }
}
