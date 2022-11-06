using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;

namespace ShowerShow.Model
{
    public class UserFriend
    {
        public Guid Id { get; set; }= Guid.NewGuid();

        [JsonRequired]
        public Guid MainUserId { get; set; }

        [JsonRequired]
        public Guid FriendId { get; set; }

        [JsonRequired]
        public FriendStatus status { get; set; } = FriendStatus.Pending; //Its always pending by default

        [JsonRequired]
        public bool IsFavorite { get; set; }
    }
}
