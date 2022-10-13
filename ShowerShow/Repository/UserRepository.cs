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
                users.Add(mapper.Map<GetUserDTO>(dbContext.Users.Find(user.Id)));
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

        public async Task CreateUserFriend(Guid user1, Guid user2)
        {
            await dbContext.SaveChangesAsync();
            //Whether these users exist has been verified in the controller logic already.
            User user1dto = dbContext.Users.FirstOrDefault(x => x.Id == user1);
            User user2dto = dbContext.Users.FirstOrDefault(x => x.Id == user2);
            //Add each other to each other's friend list.

            
            user1dto.Friends.Add(new UserFriendDTO(user2dto.Id));
            user2dto.Friends.Add(new UserFriendDTO(user1dto.Id));
            dbContext.Users.Update(user1dto);
            dbContext.Users.Update(user2dto);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserIsAlreadyFriend(Guid userId1, Guid userId2)
        {
            await dbContext.SaveChangesAsync();
            //No need to check the other user since they have a duplex friend relationship. Either they re both friends or none are.
            User user1dto =  dbContext.Users.FirstOrDefault(x => x.Id == userId1);
            foreach(UserFriendDTO us in user1dto.Friends)
            {
                if (us.Id == userId2)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task DeleteUserFriend(Guid user1, Guid user2)
        {
            await dbContext.SaveChangesAsync();
            //Whether these users exist has been verified in the controller logic already.
            User user1dto = dbContext.Users.FirstOrDefault(x => x.Id == user1);
            User user2dto = dbContext.Users.FirstOrDefault(x => x.Id == user2);
            removeFriendFromList(user1dto, user2);
            removeFriendFromList(user2dto, user1);
            dbContext.Users.Update(user1dto);
            dbContext.Users.Update(user2dto);
           await dbContext.SaveChangesAsync();

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
        private void removeFriendFromList(User u, Guid otherUserId)
        {
            foreach (UserFriendDTO us in u.Friends.ToList())
            {
                if (us.Id == otherUserId)
                {
                    u.Friends.Remove(us);
                }
            }
        }
    }
}
