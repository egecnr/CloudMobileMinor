using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class ShowerThoughtDTO
    {

        [JsonRequired]
        public string Text { get; set; }
        public string Title { get; set; } = "My shower thought";
        public bool ShareWithFriends { get; set; } = false;
        [JsonRequired]
        public DateTime DateTime { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsPublic { get; set; }
    }
}
