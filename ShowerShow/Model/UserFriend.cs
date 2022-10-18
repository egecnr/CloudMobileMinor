using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
