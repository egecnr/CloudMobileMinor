using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.DTO_
{
    public class CreateAchievementDTO
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
