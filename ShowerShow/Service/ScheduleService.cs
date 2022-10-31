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
        private IUserService userService;

        public ScheduleService(IScheduleRepository scheduleRepository, IUserService userService)
        {
            this.scheduleRepository = scheduleRepository;
            this.userService = userService;
        }

        public async Task AddScheduleToQueue(CreateScheduleDTO schedule, Guid userId)
        {
            await scheduleRepository.AddScheduleToQueue(schedule, userId);
        }

        public async Task CreateSchedule(Schedule schedule)
        {
            if (!await userService.CheckIfUserExistAndActive(schedule.UserId))
                throw new ArgumentException("The user does not exist or is inactive.");

            await scheduleRepository.CreateSchedule(schedule);
        }

        public async Task DeleteSchedule(Guid scheduleId)
        {
            if (!await DoesScheduleExist(scheduleId))
                throw new ArgumentException("The schedule does not exist.");

            await scheduleRepository.DeleteSchedule(scheduleId);
        }

        public async Task<bool> DoesScheduleExist(Guid scheduleId)
        {
           return await scheduleRepository.DoesScheduleExist(scheduleId);
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules(Guid UserId)
        {
            if (!await userService.CheckIfUserExistAndActive(UserId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await scheduleRepository.GetAllSchedules(UserId);
        }

        public async Task<Schedule> GetScheduleById(Guid scheduleId)
        {
            if (!await DoesScheduleExist(scheduleId))
                throw new ArgumentException("The schedule does not exist.");

            return await scheduleRepository.GetScheduleById(scheduleId);
        }

        public async Task<Schedule> UpdateSchedule(Guid scheduleId, UpdateScheduleDTO newSchedule)
        {
            if (!await DoesScheduleExist(scheduleId))
                throw new ArgumentException("The schedule does not exist.");

            return await scheduleRepository.UpdateSchedule(scheduleId, newSchedule);
        }
    }
}
