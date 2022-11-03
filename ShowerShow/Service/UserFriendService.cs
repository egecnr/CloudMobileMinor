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
        private IUserService userService;

        public UserFriendService(IUserFriendRepository userFriendRepository, IUserService userService)
        {
            this.userFriendRepository = userFriendRepository;
            this.userService = userService;
        }

        public async Task AcceptFriendRequest(Guid userId, Guid friendId)
        {
            if (!await userFriendRepository.CheckIfBothUsersExist(userId, friendId))
            {
                throw new Exception("User don't exist.");
            }
            else if (!await CheckIfUserIsAlreadyFriend(userId, friendId))
            {
                throw new Exception("Users dont have pending requests with each other");

            }
            else if (!await CheckFriendStatusIsResponseRequired(userId, friendId))
            {
                throw new Exception($"Only the user with id {friendId} can accept friend requests since user with id {userId} sent the request");
            }
            else 
            { 
                await  userFriendRepository.AcceptFriendRequest(userId, friendId);
            }
        }

        public async Task AddFriendToQueue(Guid userId, Guid friendId)
        {

            if (!await userFriendRepository.CheckIfBothUsersExist(userId, friendId))
            {
                throw new Exception("User don't exist.");
            }
            else if (userId == friendId)
            {
                throw new Exception("User can not add itself as a friend");
            }
            else if (await CheckIfUserIsAlreadyFriend(userId, friendId))
            {
                throw new Exception("Users are already friends or request already sent");
            }
            else {
                await userFriendRepository.AddFriendToQueue(userId, friendId);
            }
        }

        public async Task ChangeFavoriteStateOfFriend(Guid userId, Guid friendId, bool isFavorite)
        {
            if (!await userFriendRepository.CheckIfBothUsersExist(userId, friendId))
            {
                throw new Exception("User don't exist.");
            }
            else if (!await CheckIfUserIsAlreadyFriend(userId, friendId))
            {
                throw new Exception("Users dont have pending requests with each other");
            }
            else
            {
                await userFriendRepository.ChangeFavoriteStateOfFriend(userId, friendId, isFavorite);
            }
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
            if (!await userFriendRepository.CheckIfBothUsersExist(userId, friendId))
            {
                throw new Exception("User don't exist.");
            }
            else if (!await CheckIfUserIsAlreadyFriend(userId, friendId))
            {
                throw new Exception("Users are not friends with each other");
            }
            else 
            {
                await userFriendRepository.DeleteFriend(userId,friendId);
            }
        }

        public async Task<IEnumerable<GetUserFriendDTO>> GetAllFriendsOfUser(Guid userId)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
            {
                throw new Exception("Active user dont exist.");
            }
            else
            {
                return await userFriendRepository.GetAllFriendsOfUser(userId);
            }
        }

        public async Task<GetUserFriendDTO> GetUserFriendsById(Guid userId, Guid friendId)
        {
            if (!await userFriendRepository.CheckIfBothUsersExist(userId, friendId))
            {
                throw new Exception("User don't exist.");
            }
            else if (!await CheckIfUserIsAlreadyFriend(userId, friendId))
            {
                throw new Exception("Users are not friends with each other");
            }
            else
            {
                return await userFriendRepository.GetUserFriendsById(userId, friendId);
            }
        }
    }
}
