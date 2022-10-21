using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class UserFriendRepository :IUserFriendRepository
    {
        private DatabaseContext dbContext;

        public UserFriendRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> CheckIfUserIsAlreadyFriend(Guid userId, Guid friendId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.UserFriends.Where(a=>a.FriendId==friendId).Count(x => x.MainUserId==userId) > 0)
                return true;
            else
                return false;
        }
        public async Task<bool> CheckFriendStatusIsResponseRequired(Guid userId, Guid friendId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.UserFriends.Where(a => a.FriendId == friendId).Where(a=>a.status==FriendStatus.ResponseRequired).Count(x => x.MainUserId == userId) > 0)
                return true;
            else
                return false;
        }

        public async Task CreateUserFriend(Guid userId, Guid friendId)
        {
            //Whether both users exist or not are already checked
            User userFriend = dbContext.Users.FirstOrDefault(f => f.Id==friendId);
            UserFriend newUserFriend = new UserFriend() {
                Id = Guid.NewGuid(),
                MainUserId = userId,
                FriendId = friendId,
                IsFavorite = false,
                status = FriendStatus.Pending,
            };
            UserFriend newUserFriend2 = new UserFriend()
            {
                Id = Guid.NewGuid(),
                MainUserId = friendId,
                FriendId = userId,
                IsFavorite = false,
                status = FriendStatus.ResponseRequired,
            };

            dbContext.UserFriends?.Add(newUserFriend);
            dbContext.UserFriends?.Add(newUserFriend2);
            await  dbContext.SaveChangesAsync();

        }
        public async Task AcceptFriendRequest(Guid userId, Guid friendId)
        {
            //Whether both users exist or not are already checked
            UserFriend userFriend = dbContext.UserFriends.Where(f=> f.FriendId==friendId).FirstOrDefault(u=> u.MainUserId==userId);
            userFriend.status = FriendStatus.Accepted; 
            UserFriend userFriend2 = dbContext.UserFriends.Where(f=> f.FriendId==userId).FirstOrDefault(u=> u.MainUserId==friendId);
            userFriend2.status = FriendStatus.Accepted;
            dbContext.UserFriends.Update(userFriend);
            dbContext.UserFriends.Update(userFriend2);
            await dbContext.SaveChangesAsync();

        }
        public async Task DeleteFriend(Guid userId, Guid friendId)
        {
            //Whether both users exist or not are already checked
            UserFriend userFriend = dbContext.UserFriends.Where(f => f.FriendId == friendId).FirstOrDefault(u => u.MainUserId == userId);
            UserFriend userFriend2 = dbContext.UserFriends.Where(f => f.FriendId == userId).FirstOrDefault(u => u.MainUserId == friendId);
            dbContext.UserFriends?.Remove(userFriend);
            dbContext.UserFriends?.Remove(userFriend2);
            await dbContext.SaveChangesAsync();

        }

        public async Task ChangeFavoriteStateOfFriend(Guid userId, Guid friendId,bool isFavorite)
        {
            //Whether both users exist or not are already checked
            UserFriend userFriend = dbContext.UserFriends.Where(f => f.FriendId == friendId).FirstOrDefault(u => u.MainUserId == userId);
            userFriend.IsFavorite = isFavorite;
            dbContext.UserFriends.Update(userFriend);
            await dbContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<GetUserFriendDTO>> GetAllFriendsOfUser(Guid userId)
        {
            List<UserFriend> allFriends = dbContext.UserFriends.Where(u => u.MainUserId == userId).ToList();
            //This has to be done in order to always have the most up to date user info.
            List<User> userDataRequired = new List<User>();
            for(int i = 0; i < allFriends.Count; i++)
            {
                userDataRequired.Add(dbContext.Users.FirstOrDefault(f => f.Id == allFriends[i].FriendId));
            }

            List<GetUserFriendDTO> getUserFriendDTOs = new List<GetUserFriendDTO>();

            for (int i = 0; i < allFriends.Count; i++)
            {
                getUserFriendDTOs.Add(
                    new GetUserFriendDTO {
                        MainUserId = allFriends[i].MainUserId,
                        FriendId = allFriends[i].FriendId,
                        IsFavorite= allFriends[i].IsFavorite, 
                        status= allFriends[i].status,
                        UserNameOfFriend = userDataRequired[i].UserName,
                        FullNameOfFriend = userDataRequired[i].Name,
                        UserPicture = userDataRequired[i].ProfilePicture
                    });
            }
            return getUserFriendDTOs;

        }

        public async Task<GetUserFriendDTO> GetUserFriendsById(Guid userId, Guid friendId)
        {
            UserFriend userFriend = dbContext.UserFriends.Where(f => f.FriendId == friendId).FirstOrDefault(u => u.MainUserId == userId);
            User friendFullObj = dbContext.Users.FirstOrDefault(u => u.Id == friendId);

            return new GetUserFriendDTO() {
                MainUserId = userId,
                FriendId = friendId,
                IsFavorite=userFriend.IsFavorite,
                status=userFriend.status,
                UserNameOfFriend=friendFullObj.UserName,
                FullNameOfFriend=friendFullObj.Name,
                UserPicture = friendFullObj.ProfilePicture
            };
        }
    }
}
