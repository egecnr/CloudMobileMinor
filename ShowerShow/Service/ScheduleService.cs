using ShowerShow.DTO;
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
    }
}
