using Microsoft.EntityFrameworkCore;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
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
            if (dbContext.UserFriends.Where(a=>a.FriendId==friendId).Count(x => x.MainUserId==userId) > 0 || dbContext.UserFriends.Where(a => a.FriendId == userId).Count(x => x.MainUserId == friendId) > 0)
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

            dbContext.UserFriends?.Add(newUserFriend);
            await  dbContext.SaveChangesAsync();

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

        public Task<IEnumerable<GetUserFriendDTO>> GetUserFriendsByName(Guid userId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
