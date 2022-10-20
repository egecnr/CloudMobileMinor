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

        public async Task<bool> CheckIfUserIsAlreadyFriend(Guid userId, Guid friendId)
        {
            return await userFriendRepository.CheckIfUserIsAlreadyFriend(userId, friendId);
        }

        public async Task CreateUserFriend(Guid userId, Guid friendId)
        {
            await userFriendRepository.CreateUserFriend(userId, friendId);
        }

        public async Task<IEnumerable<GetUserFriendDTO>> GetAllFriendsOfUser(Guid userId)
        {
            return await userFriendRepository.GetAllFriendsOfUser(userId);
        }

        public Task<IEnumerable<GetUserFriendDTO>> GetUserFriendsByName(Guid userId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
