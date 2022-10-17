using ShowerShow.DTO;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserService
    {
        public Task CreateUser(CreateUserDTO user);
    }

}