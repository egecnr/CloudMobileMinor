using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShowerShow.Models
{
    internal class Schedule
    {
        public Guid Id { get; } = Guid.NewGuid();
        [JsonRequired]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public List<DayOfWeek> DaysOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
