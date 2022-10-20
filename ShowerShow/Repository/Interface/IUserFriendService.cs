using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserFriendService
    {
        Task<bool> CheckIfUserIsAlreadyFriend(Guid userId, Guid friendId);
        Task CreateUserFriend(Guid userId, Guid friendId);
        Task<IEnumerable<GetUserFriendDTO>> GetUserFriendsByName(Guid userId, string userName);
        Task<IEnumerable<GetUserFriendDTO>> GetAllFriendsOfUser(Guid userId);
    }
}
