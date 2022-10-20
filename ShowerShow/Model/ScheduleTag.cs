using Newtonsoft.Json;
using ShowerShow.Model;
using System;

namespace ShowerShow.Models
{
    public class ScheduleTag
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public double ActivityDuration { get; set; }

        [JsonRequired]
        public WaterTemperature waterTemperature { get; set; }

        [JsonRequired]
        public bool IsWaterOn { get; set; }


    }
}
