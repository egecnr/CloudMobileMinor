using Newtonsoft.Json;

namespace ShowerShow.Models
{
    internal class ScheduleTag
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public double ActivityDuration { get; set; }

    }
}
