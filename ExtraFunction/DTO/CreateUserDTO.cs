using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace ExtraFunction.DTO_
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
