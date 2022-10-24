using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.DTO_
{
    public class UpdateAchievementDTO
    {
        [JsonRequired]
        public int CurrentValue { get; set; }
    }
}
