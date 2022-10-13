using Newtonsoft.Json;
using ShowerShow.DTO;
using System;
using System.Collections.Generic;

namespace ShowerShow.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public List<UserFriendDTO> Friends { get; set; } = new List<UserFriendDTO>();
        public List<Achievement> Achievements { get; set; } = new List<Achievement>();
        public string UserBadge { get; set; }
        public bool isAccountActive { get; set; } = true;
    }
}
