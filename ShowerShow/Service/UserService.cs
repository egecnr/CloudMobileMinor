using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
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
     
        public async Task AddUserToQueue(CreateUserDTO user)
        {
            if (await userRepository.CheckIfEmailExist(user.Email))
            {
                throw new Exception("Please pick a unique email address");
            }
            else if (await userRepository.CheckIfUserNameExist(user.UserName))
            {
                throw new Exception("Please pick a unique username");
            }
            else
            {
                await userRepository.AddUserToQueue(user);
            }
        }
        public async Task<GetUserDTO> GetUserById(Guid Id)
        {
            if (await CheckIfUserExistAndActive(Id))
            {
                return await userRepository.GetUserById(Id);
            }
            else
            {
                throw new Exception("User does not exist");
            }
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
            if (!await CheckIfUserExistAndActive(userId))
            {
                throw new Exception("User does not exist");
            }
            else if (await CheckIfEmailExist(userId, userDTO.Email))
            {
                throw new Exception("Email has to be unique");
            }
            else if (await CheckIfUserNameExist(userId, userDTO.UserName))
            {
                throw new Exception("Username has to be unique");
            }
            else
            {
                await userRepository.UpdateUser(userId, userDTO);
            }     
        }

        public async Task DeactivateUserAccount(Guid userId, bool isAccountActive)
        {
            if (await CheckIfUserExist(userId))
            {
                await userRepository.DeactivateUserAccount(userId, isAccountActive);
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                throw new Exception("Please input a username");
            }
            else
            {
                return await userRepository.GetUsersByName(userName);
            }
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

        public async Task CreateUser(CreateUserDTO userDTO)
        {
            await userRepository.CreateUser(userDTO);
        }

        public async Task DeleteUser(string username)
        {          
            await userRepository.DeleteUser(username);
            
        }
    }
}
