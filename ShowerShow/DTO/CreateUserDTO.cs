using Newtonsoft.Json;
using System.Collections.Generic;
using ShowerShow.Models;
using System;
using ShowerShow.Utils;

namespace ShowerShow.DTO
{
    public class CreateUserDTO
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
