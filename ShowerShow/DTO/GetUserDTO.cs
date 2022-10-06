using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    internal class GetUserDTO
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Username { get; set; }

        [JsonRequired]
        public string Email { get; set; }
        public string UserBadge { get; set; }
        public List<Guid> Friends { get; set; } = new List<Guid>();

    }
}
