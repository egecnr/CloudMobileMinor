using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;

namespace ShowerShow.Model
{
    public class UserFriend
    {
        [JsonRequired]
        public Guid Id { get; set; }

        [JsonRequired]
        public Guid MainUserId { get; set; }

    

        [JsonRequired]
        public string UserName { get; set; }

        [JsonRequired]
        public bool IsUserFavorite { get; set; }

        public UserFriend() { }
        public UserFriend(Guid id,string userName)
        {
            MainUserId = id;
            UserName = userName;
        }
    }
}
