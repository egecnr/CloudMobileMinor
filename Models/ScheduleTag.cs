using Newtonsoft.Json;

namespace FunctionApp1.Models
{
    internal class ScheduleTag
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public double ActivityDuration { get; set; }

    }
}
