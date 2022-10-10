using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShowerShow.Models
{
    public class Schedule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonRequired]
        public Guid UserId { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
