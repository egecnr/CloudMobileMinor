using Newtonsoft.Json;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class UpdateUserDTO
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string ProfilePicture { get; set; }
        [JsonRequired]
        public string PasswordHash { get; set; }
        [JsonRequired]
        public string Email { get; set; }
    }
}
