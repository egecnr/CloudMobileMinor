using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionApp1.Models
{
    internal class Schedule
    {
        public Guid Id { get; } = Guid.NewGuid();
        [JsonRequired]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public DayOfWeek DayOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
