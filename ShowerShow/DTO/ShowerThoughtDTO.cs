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
        public bool ShareWithFriends { get; set; } = false;
        [JsonRequired]
        public DateTime DateTime { get; set; }
    }
}
