using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;

namespace ShowerShow.Model
{
    public class UserFriend
    {
        public Guid Id { get; set; }
        public UserFriend() { }
        public UserFriend(Guid id)
        {
            Id = id;
        }
    }
}
