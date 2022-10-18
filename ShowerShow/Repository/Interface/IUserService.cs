using ShowerShow.DTO;
using System;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserService
    {
        public Task CreateUser(CreateUserDTO user);
        public Task<bool> CheckIfUserExist(Guid userId);
    }
}