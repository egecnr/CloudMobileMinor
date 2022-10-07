using ShowerShow.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DayOfWeek = ShowerShow.Models.DayOfWeek;

namespace ShowerShow.DTO
{
    internal class CreateScheduleDTO
    {

        [JsonRequired]
        public Guid UserId { get; set; }
        [JsonRequired]
        public List<DayOfWeek> DaysOfWeek { get; set; }
        [JsonRequired]
        public List<ScheduleTag> Tags { get; set; }
    }
}
