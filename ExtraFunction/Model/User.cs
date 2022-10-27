using ExtraFunction.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace ExtraFunction.Model
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
        public List<Achievement> Achievements { get; set; } 
        public bool isAccountActive { get; set; } = true;
    }
}
