using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;

namespace ShowerShow.DTO
{
    public class UserFriendDTO
    {
        public Guid Id { get; set; }
        public UserFriendDTO() { }
        public UserFriendDTO(Guid id) 
        {
            this.Id = id;
        }
    }
}
