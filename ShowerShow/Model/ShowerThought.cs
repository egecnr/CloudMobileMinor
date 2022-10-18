using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Model
{
    public class ShowerThought
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ShowerId { get; set; }
        [JsonProperty]
        public Guid UserId { get; set; }
        [JsonRequired]
        public string Text { get; set; }
        public bool ShareWithFriends { get; set; } = false;
        [JsonRequired]
        public DateTime DateTime { get; set; }
    }
}
