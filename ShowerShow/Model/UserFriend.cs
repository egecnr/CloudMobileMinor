using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;

namespace ShowerShow.Model
{
    public class UserFriend
    {
        public Guid Id { get; set; }
        [JsonRequired]
        public string UserName { get; set; }
        public UserFriend() { }
        public UserFriend(Guid id,string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
