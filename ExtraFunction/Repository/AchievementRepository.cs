using ExtraFunction.Repository_.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExtraFunction.Model;
using ExtraFunction.DAL;
using System.Configuration;

namespace ExtraFunction.Repository_
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AchievementRepository(DatabaseContext databasecontext)
        {
            _databaseContext = databasecontext;
        }

        public async Task<Achievement> GetAchievementByIdAndTitle(string achievementTitle, Guid userId)
        {
            await _databaseContext.SaveChangesAsync();
            return _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.FirstOrDefault(y => y.Title == achievementTitle) ?? null;  
        }

        public async Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            await _databaseContext.SaveChangesAsync();
            List<Achievement> ach = _databaseContext.Users.FirstOrDefault(x => x.Id == userId).Achievements;
            return ach;
        }

        public async Task UpdateAchievementByIdAndTitle(string achievementTitle, Guid userId, int currentvalue)
        {
            //get user instead, update the achievement current value locally and update the user instead of only achievement.

            User user = _databaseContext.Users.FirstOrDefault(x => x.Id == userId);
            user.Achievements.ForEach(delegate (Achievement achievement1)
            {
                if (achievement1.Title == achievementTitle)
                    achievement1.CurrentValue = currentvalue;
            });
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

    }
}
