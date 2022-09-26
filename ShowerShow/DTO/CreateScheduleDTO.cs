using ShowerShow.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShowerShow.DTO
{
    internal class CreateScheduleDTO
    {
        public Models.DayOfWeek DayOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
