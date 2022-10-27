using AutoMapper;
using Azure.Storage.Queues;
using ExtraFunction.Model_;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class UserRepository:IUserRepository
    {
        private DatabaseContext dbContext;
        private IUserPrefencesService userPrefencesService;

        public UserRepository(DatabaseContext dbContext, IUserPrefencesService userPrefencesService)
        {
            this.dbContext = dbContext;
            this.userPrefencesService = userPrefencesService;
        }
        //Move this to the service layer later on
        public async Task AddUserToQueue(CreateUserDTO userDTO)
        {
            if (await CheckIfEmailExist(userDTO.Email))
            {
                throw new Exception("Please pick a unique email address");
            }
            else if(await CheckIfUserNameExist(userDTO.UserName))
            {
                throw new Exception("Please pick a unique username");
            }
            else
            {
                string qName = Environment.GetEnvironmentVariable("CreateUserQueue");
                string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                QueueClientOptions clientOpt = new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };

                QueueClient qClient = new QueueClient(connString, qName, clientOpt);
                var jsonOpt = new JsonSerializerOptions() { WriteIndented = true };
                string userJson = JsonSerializer.Serialize<CreateUserDTO>(userDTO, jsonOpt);
                await qClient.SendMessageAsync(userJson);
              
            }      
        }
        public async Task CreateUser(CreateUserDTO userDTO)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            User fullUser = mapper.Map<User>(userDTO);
            fullUser.PasswordHash = PasswordHasher.HashPassword(fullUser.PasswordHash);
            fullUser.Achievements = Achievement.InitializedAchievements();
            dbContext.Users?.Add(fullUser);
            dbContext.Preferences?.Add(Preferences.ReturnDefaultPreference(fullUser.Id));
            await dbContext.SaveChangesAsync();
        }
        public async Task<GetUserDTO> GetUserById(Guid userId)
        {
            if (await CheckIfUserExistAndActive(userId))
            {
                await dbContext.SaveChangesAsync();
                User user = dbContext.Users.Where(acc => acc.isAccountActive == true).FirstOrDefault(u => u.Id == userId);
                Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
                GetUserDTO userDTO = mapper.Map<GetUserDTO>(user);
                return userDTO;
            }
            else
            {
                throw new Exception("User does not exist");
            }
           
        }

        public async Task<bool> CheckIfUserExistAndActive(Guid userId)
        {
            await dbContext.SaveChangesAsync();          
            if (dbContext.Users.Where(a => a.isAccountActive == true).Count(x => x.Id == userId) > 0)           
                return true;
            else               
                return false;
        }
       
       
     
        public async Task<bool> CheckIfEmailExist(string email)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.Email == email)>0)
                return true;
            else
                return false;
        } 
        

        public async Task UpdateUser(Guid userId, UpdateUserDTO userDTO)
        {
            if (!await CheckIfUserExistAndActive(userId))
            {
                throw new Exception("User does not exist");
            }
            else if (await CheckIfEmailExist(userId,userDTO.Email))
            {
                throw new Exception("Email has to be unique");
            }
            else if (await CheckIfUserNameExist(userId,userDTO.UserName))
            {
                throw new Exception("Username has to be unique");
            }
            else
            {
                await dbContext.SaveChangesAsync();
                User user = dbContext.Users.Where(acc => acc.isAccountActive == true).FirstOrDefault(u => u.Id == userId);
                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.PasswordHash = PasswordHasher.HashPassword(userDTO.PasswordHash);
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            }   
        }
        public async Task DeactivateUserAccount(Guid userId, bool isAccountActive)
        {
            if (await CheckIfUserExist(userId))
            {
                await dbContext.SaveChangesAsync();
                User user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
                user.isAccountActive = isAccountActive;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User does not exist");
            }
            
        }
        private List<GetUserDTO> ConvertGetDtos(List<User> users)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            List<GetUserDTO> userdtos = new List<GetUserDTO>();

            users.ForEach(delegate (User u) {
                userdtos.Add(mapper.Map<GetUserDTO>(u));
            });
            return userdtos;
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                throw new Exception("Please input a username");
            }
            else
            {
                List<User> usersWithName = dbContext.Users.Where(u=> u.isAccountActive==true).Where(u => u.UserName.ToLower().StartsWith(userName.ToLower())).ToList();
                Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
                List<GetUserDTO> dtos = ConvertGetDtos(usersWithName);
                return dtos;
            }          
        }

       

        public async Task<bool> CheckIfUserExist(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.Id == userId) > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> CheckIfUserNameExist(string userName)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.UserName.ToLower() == userName.ToLower()) > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> CheckIfUserNameExist(Guid userId, string wantedUsername)
        {
            await dbContext.SaveChangesAsync();
            User user =  dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user.UserName.ToLower() == wantedUsername.ToLower()) //We want to skip the badrequest if userDTO is inputting the same email.
                return false;
            else
            {
                return await CheckIfUserNameExist(wantedUsername);
            }
        }

        public async Task<bool> CheckIfEmailExist(Guid userId, string wantedEmail)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user.Email == wantedEmail) //We want to skip the badrequest if userDTO is inputting the same email.
                return false;
            else
            {
                return await CheckIfEmailExist(wantedEmail);
            }
        }
    }
}
