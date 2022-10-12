using ShowerShow.DTO;
using System;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IScheduleRepository
    {
        public Task CreateSchedule(CreateScheduleDTO schedule, Guid id);
    }
}
