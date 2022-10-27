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
        Task<bool> CheckFriendStatusIsResponseRequired(Guid userId, Guid friendId);
        Task CreateUserFriend(Guid userId, Guid friendId);
        Task AddFriendToQueue(Guid userId, Guid friendId);
        Task<GetUserFriendDTO> GetUserFriendsById(Guid userId, Guid friendId);
        Task<IEnumerable<GetUserFriendDTO>> GetAllFriendsOfUser(Guid userId);
        Task AcceptFriendRequest(Guid userId, Guid friendId);
        Task ChangeFavoriteStateOfFriend(Guid userId, Guid friendId, bool isFavorite);
        Task DeleteFriend(Guid userId, Guid friendId);
    }
}
