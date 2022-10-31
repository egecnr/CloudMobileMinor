using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserService
    {
        Task AddUserToQueue(CreateUserDTO user);
        Task CreateUser(CreateUserDTO userDTO);
        Task<bool> CheckIfEmailExist(string email);
        Task<GetUserDTO> GetUserById(Guid Id);
        Task<bool> CheckIfUserNameExist(string userName);
        Task<bool> CheckIfUserExistAndActive(Guid userId);
        Task<bool> CheckIfUserExist(Guid userId);
        Task DeleteUser(string username);
        Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
        Task<bool> CheckIfEmailExist(Guid userId, string wantedEmail);
        Task<bool> CheckIfUserNameExist(Guid userId, string wantedUsername);
        Task DeactivateUserAccount(Guid userId, bool isAccountActive);
        Task UpdateUser(Guid userId, UpdateUserDTO userDTO);

    }
}