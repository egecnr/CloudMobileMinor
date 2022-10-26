using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task CreateUser(CreateUserDTO user)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            User fullUser = mapper.Map<User>(user);
            fullUser.PasswordHash = PasswordHasher.HashPassword(fullUser.PasswordHash);
            dbContext.Users?.Add(fullUser);
            dbContext.Preferences?.Add(Preferences.ReturnDefaultPreference(fullUser.Id));
            await dbContext.SaveChangesAsync();
        }
        public async Task<GetUserDTO> GetUserById(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.Where(acc => acc.isAccountActive ==true).FirstOrDefault(u=> u.Id==userId);
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            GetUserDTO userDTO = mapper.Map<GetUserDTO>(user);
            return userDTO; 
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
            //Whether user exists or his email is duplicated is already checked in the logic or higher levels.
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.PasswordHash=PasswordHasher.HashPassword(userDTO.PasswordHash);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task DeactivateUserAccount(Guid userId, bool isAccountActive)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            user.isAccountActive = isAccountActive;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
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

            List<User> usersWithName = dbContext.Users.Where(u => u.UserName.ToLower().StartsWith(userName.ToLower())).ToList();
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            List<GetUserDTO> dtos = ConvertGetDtos(usersWithName);            
            return dtos;       
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
            if (user.UserName.ToLower() == wantedUsername.ToLower()) //We want to skip the badrequest if user is inputting the same email.
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
            if (user.Email == wantedEmail) //We want to skip the badrequest if user is inputting the same email.
                return false;
            else
            {
                return await CheckIfEmailExist(wantedEmail);
            }
        }
    }
}
