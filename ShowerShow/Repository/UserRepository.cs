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

namespace ShowerShow.Repository
{
    public class UserRepository:IUserRepository
    {
        private DatabaseContext dbContext;

        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateUser(CreateUserDTO user)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            User fullUser = mapper.Map<User>(user);
            fullUser.PasswordHash = PasswordHasher.HashPassword(fullUser.PasswordHash);
            dbContext.Users?.Add(fullUser);
            await dbContext.SaveChangesAsync();
        }
        public async Task<GetUserDTO> GetUserById(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            GetUserDTO userDTO = mapper.Map<GetUserDTO>(user);
            return userDTO; 
        }

        public async Task<bool> CheckIfUserExist(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            if(dbContext.Users.Count(x => x.Id == userId)>0)           
                return true;
            else               
                return false;
        }
        public async Task<List<GetUserDTO>> GetAllFriendsOfUser(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            List<UserFriendDTO> allFriends = dbContext.Users.Find(userId).Friends;
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));

            List<GetUserDTO> users = new List<GetUserDTO>();
            foreach(UserFriendDTO user in allFriends)
            {
                users.Add(mapper.Map<GetUserDTO>(dbContext.Users.Find(userId)));
            }
            return users;
        }

        public async Task<bool> CheckIfEmailExist(string email)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.Email == email)>0)
                return true;
            else
                return false;
        }
    }
}
