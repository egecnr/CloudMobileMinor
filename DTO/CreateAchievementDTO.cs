using Newtonsoft.Json;

namespace FunctionApp1.DTO
{
    internal class CreateAchievementDTO
    {
        [JsonRequired]
        public string Title { get; set; }
        [JsonRequired]
        public string Description { get; set; }
        [JsonRequired]
        public int CurrentValue { get; set; } = 0;
        [JsonRequired]
        public int TargetValue { get; set; }
    }
}
