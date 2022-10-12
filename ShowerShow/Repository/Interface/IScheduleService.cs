using ShowerShow.DTO;
using System;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IScheduleService
    {
        public Task CreateSchedule(CreateScheduleDTO schedule, Guid id);
    }
}
