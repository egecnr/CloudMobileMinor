using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class UserService: IUserService
    {
        private DatabaseContext dbContext;

        private IUserRepository userRepository; 

        public UserService(IUserRepository userRepository, DatabaseContext dbContext)
        {
            this.userRepository = userRepository;
            this.dbContext = dbContext;
        }
     
        public async Task CreateUser(CreateUserDTO user)
        {
            await userRepository.CreateUser(user);
        }     
    }
}
