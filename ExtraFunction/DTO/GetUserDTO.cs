using ExtraFunction.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.DTO_
{
    public class GetUserDTO
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string Email { get; set; } //Consider deleting this
        public List<UserFriend> Friends { get; set; }
        public List<Achievement> Achievements { get; set; }

    }
}
