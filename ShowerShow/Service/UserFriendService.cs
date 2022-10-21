using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class UserFriendService : IUserFriendService
    {
        private IUserFriendRepository userFriendRepository;

        public UserFriendService(IUserFriendRepository userFriendRepository)
        {
            this.userFriendRepository = userFriendRepository;
        }

        public async Task AcceptFriendRequest(Guid userId, Guid friendId)
        {
           await  userFriendRepository.AcceptFriendRequest(userId, friendId);
        }

        public async Task ChangeFavoriteStateOfFriend(Guid userId, Guid friendId, bool isFavorite)
        {
            await userFriendRepository.ChangeFavoriteStateOfFriend(userId, friendId, isFavorite);  
        }

        public async Task<bool> CheckFriendStatusIsResponseRequired(Guid userId, Guid friendId)
        {
            return await userFriendRepository.CheckFriendStatusIsResponseRequired(userId, friendId);
        }

        public async Task<bool> CheckIfUserIsAlreadyFriend(Guid userId, Guid friendId)
        {
            return await userFriendRepository.CheckIfUserIsAlreadyFriend(userId, friendId);
        }

        public async Task CreateUserFriend(Guid userId, Guid friendId)
        {
            await userFriendRepository.CreateUserFriend(userId, friendId);
        }

        public async Task DeleteFriend(Guid userId, Guid friendId)
        {
            await userFriendRepository.DeleteFriend(userId,friendId);
        }

        public async Task<IEnumerable<GetUserFriendDTO>> GetAllFriendsOfUser(Guid userId)
        {
            return await userFriendRepository.GetAllFriendsOfUser(userId);
        }

        public async Task<GetUserFriendDTO> GetUserFriendsById(Guid userId, Guid friendId)
        {
            return await userFriendRepository.GetUserFriendsById(userId, friendId);
        }
    }
}
