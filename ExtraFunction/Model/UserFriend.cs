using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ExtraFunction.Model;


namespace ExtraFunction.Model
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
