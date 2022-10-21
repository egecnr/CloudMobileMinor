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
        public Task<bool> CheckIfUserNameExist(string userName);
        public Task<bool> CheckIfUserExistAndActive(Guid userId);
        public Task<bool> CheckIfUserExist(Guid userId);
        public Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
        public Task<bool> CheckIfEmailExist(Guid userId, string wantedEmail);
        public Task<bool> CheckIfUserNameExist(Guid userId, string wantedUsername);
        public Task DeactivateUserAccount(Guid userId, bool isAccountActive);
        public Task UpdateUser(Guid userId, UpdateUserDTO userDTO);

    }
}