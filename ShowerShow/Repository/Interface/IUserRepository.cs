using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<bool> CheckIfUserExistAndActive(Guid userId);
        public Task<bool> CheckIfUserExist(Guid userId);
        public Task CreateUser(CreateUserDTO userDTO);
        public Task<bool> CheckIfEmailExist(string email);
        public Task<bool> CheckIfUserNameExist(string userName);
        public Task AddUserToQueue(CreateUserDTO user);
        public Task DeactivateUserAccount(Guid userId, bool isAccountActive);
        public Task<GetUserDTO> GetUserById(Guid Id);
        public Task<bool> CheckIfEmailExist(Guid userId,string wantedEmail);
        public Task<bool> CheckIfUserNameExist(Guid userId,string wantedUsername);
        public Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
        public Task UpdateUser(Guid userId, UpdateUserDTO userDTO);

    }
}
