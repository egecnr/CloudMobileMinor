using ExtraFunction.DTO_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Repository_.Interface
{
    public interface IUserRepository
    {
        public Task<bool> CheckIfUserExistAndActive(Guid userId);
        public Task<bool> CheckIfUserExist(Guid userId);
        public Task<bool> CheckIfEmailExist(string email);
        public Task<bool> CheckIfUserNameExist(string userName);
        public Task CreateUser(CreateUserDTO user);
        public Task DeactivateUserAccount(Guid userId, bool isAccountActive);
        public Task CreateUserFriend(Guid user1,Guid user2);
        public Task DeleteUserFriend(Guid user1, Guid user2);
        public Task<GetUserDTO> GetUserById(Guid Id);
        public Task<bool> CheckIfEmailExist(Guid userId,string wantedEmail);
        public Task<bool> CheckIfUserNameExist(Guid userId,string wantedUsername);
        public Task<IEnumerable<GetUserDTO>> GetUserFriendsByName(Guid id, string userName);
        public Task<IEnumerable<GetUserDTO>> GetAllFriendsOfUser(Guid userId);
        public Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
        public Task<bool> CheckIfUserIsAlreadyFriend(Guid userId1, Guid userId2);
        public Task UpdateUser(Guid userId, UpdateUserDTO userDTO);

    }
}
