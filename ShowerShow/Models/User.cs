using Newtonsoft.Json;
using ShowerShow.DTO;
using System;
using System.Collections.Generic;

namespace ShowerShow.Models
{
    internal class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Username { get; set; }
        [JsonRequired]
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public List<UserFriend> Friends { get; set; }
        public List<Achievement> Achievements { get; set; }
        public string UserBadge { get; set; }

    }
}
