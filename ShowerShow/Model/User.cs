using Newtonsoft.Json;
using ShowerShow.Model;
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
        public string UserName { get; set; }
        [JsonRequired]
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public List<UserFriend> Friends { get; set; } = new List<UserFriend>();
        public List<Achievement> Achievements { get; set; } = new List<Achievement>();
        public bool isAccountActive { get; set; } = true;
    }
}
