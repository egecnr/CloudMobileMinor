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
        public Task CreateUser(CreateUserDTO user);
        public Task<bool> CheckIfUserExist(Guid userId);
    }
}
