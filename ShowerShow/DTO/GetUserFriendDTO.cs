using Newtonsoft.Json;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class GetUserFriendDTO
    {

        [JsonRequired]
        public Guid MainUserId { get; set; }

        [JsonRequired]
        public Guid FriendId { get; set; }

        [JsonRequired]
        public string UserNameOfFriend { get; set; }

        [JsonRequired]
        public string FullNameOfFriend { get; set; }

        [JsonRequired]
        public byte[] UserPicture { get; set; }

        [JsonRequired]
        public FriendStatus status { get; set; } = FriendStatus.Pending; //Its always pending by default

        [JsonRequired]
        public bool IsFavorite { get; set; }
    }
}
