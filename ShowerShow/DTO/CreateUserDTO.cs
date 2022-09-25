using Newtonsoft.Json;
using System.Collections.Generic;
using FunctionApp1.Models;

namespace FunctionApp1.DTO
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
