using Newtonsoft.Json;
using System.Collections.Generic;
using ShowerShow.Models;

namespace ShowerShow.DTO
{
    internal class CreateUserDTO
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Username { get; set; }
        [JsonRequired]
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserBadge { get; set; }

    }
}
