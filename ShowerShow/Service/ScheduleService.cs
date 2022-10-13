using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    internal class ScheduleService : IScheduleService
    {
        private IScheduleRepository scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
        }
        public async Task CreateSchedule(CreateScheduleDTO schedule, Guid id)
        {
            await scheduleRepository.CreateSchedule(schedule, id);
        }

        public async Task DeleteSchedule(Schedule schedule)
        {
            await scheduleRepository.DeleteSchedule(schedule);
        }

        public Task<List<Schedule>> GetAllSchedules(Guid UserId)
        {
            return scheduleRepository.GetAllSchedules(UserId);
        }

        public Task<Schedule> GetScheduleById(Guid scheduleId)
        {
            return scheduleRepository.GetScheduleById(scheduleId);
        }

        public async Task UpdateSchedule(Schedule oldSchedule, UpdateScheduleDTO newSchedule)
        {
            await scheduleRepository.UpdateSchedule(oldSchedule, newSchedule);
        }
    }
}
