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
        private IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
     
        public async Task CreateUser(CreateUserDTO user)
        {
            await userRepository.CreateUser(user);
        }
        public async Task<GetUserDTO> GetUserById(Guid Id)
        {
            return await userRepository.GetUserById(Id);
        }
        public async Task<bool> CheckIfUserExistAndActive(Guid userId)
        {
            return await userRepository.CheckIfUserExistAndActive(userId);
        }
        public async Task<bool> CheckIfEmailExist(string email)
        {
            return await userRepository.CheckIfEmailExist(email);
        }

        public async Task UpdateUser(Guid userId, UpdateUserDTO userDTO)
        {
            await userRepository.UpdateUser(userId,userDTO);
        }

        public async Task DeactivateUserAccount(Guid userId, bool isAccountActive)
        {
            await userRepository.DeactivateUserAccount(userId, isAccountActive);
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName)
        {
            return await userRepository.GetUsersByName(userName);
        }

        public async Task<bool> CheckIfUserExist(Guid userId)
        {
            return await userRepository.CheckIfUserExist(userId);
        }

        public async Task<bool> CheckIfUserNameExist(string userName)
        {
            return await userRepository.CheckIfUserNameExist(userName);
        }
        public async Task<bool> CheckIfEmailExist(Guid userId, string wantedEmail)
        {
            return await userRepository.CheckIfEmailExist(userId, wantedEmail);
        }

        public async Task<bool> CheckIfUserNameExist(Guid userId, string wantedUsername)
        {
            return await userRepository.CheckIfUserNameExist(userId, wantedUsername);
        }
    }
}
