using ShowerShow.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DayOfWeek = ShowerShow.Models.DayOfWeek;

namespace ShowerShow.DTO
{
    public class CreateScheduleDTO
    {

        [JsonRequired]
        public List<DayOfWeek> DaysOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
