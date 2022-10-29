using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Azure.Storage.Queues;
using System.Text.Json;

namespace ShowerShow.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private DatabaseContext dbContext;
        private IUserRepository userRepository;

        public ScheduleRepository(DatabaseContext dbContext,IUserRepository userRepository)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
        }

        public async Task AddScheduleToQueue(CreateScheduleDTO schedule, Guid userId)
        {
            // get env variables 
            string qName = Environment.GetEnvironmentVariable("CreateScheduleQueue");
            string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            QueueClientOptions clientOpt = new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };

            QueueClient qClient = new QueueClient(connString, qName, clientOpt);
           
            //map DTO to normal Schedule
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateScheduleDTO, Schedule>()));
            Schedule newSchedule = mapper.Map<Schedule>(schedule);
            newSchedule.UserId = userId;
          
            var jsonOpt = new JsonSerializerOptions() { WriteIndented = true };
            string userJson = JsonSerializer.Serialize(newSchedule, jsonOpt);
            await qClient.SendMessageAsync(userJson);


        }

        public async Task CreateSchedule(Schedule schedule)
        {
            if (!await userRepository.CheckIfUserExistAndActive(schedule.UserId))
                throw new ArgumentException("The user does not exist or is inactive.");

            dbContext.Schedules?.Add(schedule);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteSchedule(Guid scheduleId)
        {
            if(!await DoesScheduleExist(scheduleId))
                throw new ArgumentException("The schedule does not exist.");

            Schedule schedule = null;

            //this is to give priority to tasks
            Task getId = Task.Run(() =>
            {
                schedule = GetScheduleById(scheduleId).Result; // get schedule
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.Schedules?.Remove(schedule); // delete schedule

            });
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesScheduleExist(Guid scheduleId) //check if schedule with id exists
        {
            await dbContext.SaveChangesAsync();
            return dbContext.Schedules.FirstOrDefault(x => x.Id == scheduleId) != null;
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules(Guid UserId)
        {
            if(!await userRepository.CheckIfUserExistAndActive(UserId))
                throw new ArgumentException("The user does not exist or is inactive.");

            await dbContext.SaveChangesAsync();
            return dbContext.Schedules.Where(x => x.UserId == UserId).ToList();
        }

        public async Task<Schedule> GetScheduleById(Guid scheduleId)
        {
            if(!await DoesScheduleExist(scheduleId))
                throw new ArgumentException("The schedule does not exist.");

            await dbContext.SaveChangesAsync();
            return dbContext.Schedules.FirstOrDefault(x => x.Id == scheduleId);
        }

        public async Task<Schedule> UpdateSchedule(Guid scheduleId, UpdateScheduleDTO newSchedule)
        {
            if(!await DoesScheduleExist(scheduleId))
                throw new ArgumentException("The schedule does not exist.");

            Schedule schedule = null;
            //this is to give priority to tasks
            Task getId = Task.Run(() =>
            {
                //set the new schedule
                schedule = GetScheduleById(scheduleId).Result; 
                schedule.DaysOfWeek = newSchedule.DaysOfWeek;
                schedule.Tags = newSchedule.Tags;
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.Schedules?.Update(schedule);
            });
            await dbContext.SaveChangesAsync();

            return schedule;
        }
    }
}
