using FunctionApp1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionApp1.DTO
{
    internal class CreateScheduleDTO
    {
        public Models.DayOfWeek DayOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
