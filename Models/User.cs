using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FunctionApp1.Models
{
    internal class User
    {
        public Guid Id { get; } = Guid.NewGuid();
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Username { get; set; }
        [JsonRequired]
        public string Password { get; set; }
        public string Email { get; set; }
        public List<User> Friends { get; set; }
        public List<Achievement> Achievements { get; set; }
        public string UserBadge { get; set; }

    }
}
