using Newtonsoft.Json;
using ShowerShow.Models;
using System.Collections.Generic;
using DayOfWeek = ShowerShow.Models.DayOfWeek;
namespace ShowerShow.DTO
{
    public class UpdateScheduleDTO
    {
        [JsonRequired]
        public List<DayOfWeek> DaysOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
