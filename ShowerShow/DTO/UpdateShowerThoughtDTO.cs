using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class UpdateShowerThoughtDTO
    {
        [JsonProperty]
        public bool IsFavorite { get; set; }
        [JsonRequired]
        public bool ShareWithFriends { get; set; }
    }
}
