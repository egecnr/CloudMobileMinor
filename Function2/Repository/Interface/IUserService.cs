using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserService
    {
        public Task CreateUser(CreateUserDTO user);
        public Task<bool> CheckIfEmailExist(string email);
        public Task<GetUserDTO> GetUserById(Guid Id);
        public Task CreateUserFriend(Guid user1, Guid user2);
        public Task<bool> CheckIfUserExistAndActive(Guid userId);
        public Task<bool> CheckIfUserExist(Guid userId);
        public Task<IEnumerable<GetUserDTO>> GetAllFriendsOfUser(Guid userId);
        public Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
        public Task DeactivateUserAccount(Guid userId, bool isAccountActive);
        public Task DeleteUserFriend(Guid user1, Guid user2);
        public Task<bool> CheckIfUserIsAlreadyFriend(Guid userId1, Guid userId2);
        public Task UpdateUser(Guid userId, UpdateUserDTO userDTO);

    }
}