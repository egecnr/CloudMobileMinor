using Newtonsoft.Json;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class UpdateUserFriendDTO
    {
        [JsonRequired]
        public Guid MainUserId { get; set; }

        [JsonRequired]
        public Guid FriendId { get; set; }

        [JsonRequired]
        public bool IsFavorite { get; set; }
    }
}
