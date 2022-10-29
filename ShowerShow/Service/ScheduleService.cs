using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class ScheduleService : IScheduleService
    {
        private IScheduleRepository scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
        }

        public async Task AddScheduleToQueue(CreateScheduleDTO schedule, Guid userId)
        {
            await scheduleRepository.AddScheduleToQueue(schedule, userId);
        }

        public async Task CreateSchedule(Schedule schedule)
        {
            await scheduleRepository.CreateSchedule(schedule);
        }

        public async Task DeleteSchedule(Guid scheduleId)
        {
            await scheduleRepository.DeleteSchedule(scheduleId);
        }

        public async Task<bool> DoesScheduleExist(Guid scheduleId)
        {
           return await scheduleRepository.DoesScheduleExist(scheduleId);
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules(Guid UserId)
        {
            return await scheduleRepository.GetAllSchedules(UserId);
        }

        public async Task<Schedule> GetScheduleById(Guid scheduleId)
        {
            return await scheduleRepository.GetScheduleById(scheduleId);
        }

        public async Task<Schedule> UpdateSchedule(Guid scheduleId, UpdateScheduleDTO newSchedule)
        {
            return await scheduleRepository.UpdateSchedule(scheduleId, newSchedule);
        }
    }
}
