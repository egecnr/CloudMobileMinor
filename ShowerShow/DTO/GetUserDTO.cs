using Newtonsoft.Json;
using ShowerShow.Model;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class GetUserDTO
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Email { get; set; }
        public string UserBadge { get; set; }
        public List<UserFriend> Friends { get; set; }

    }
}
