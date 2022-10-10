using Newtonsoft.Json;
using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Models
{
    public class UserFriend
    {
        public Guid Id { get; set; }
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Username { get; set; }
        [JsonRequired]
        public string UserBadge { get; set; }
    }
}
