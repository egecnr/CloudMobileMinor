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
            // solution? make this a list and return it. change query

            //return this.context.Users.Where(x => x.UserId == userId).Select(x => x.Score).FirstOrDefault();

            await _databaseContext.SaveChangesAsync();

            //return _databaseContext.Users.Where(x => x.Id == userId).Select(z => z.Achievements.FirstOrDefault(c => c.Title == achievementTitle)).FirstOrDefault();


            //var user = _databaseContext.Users.FirstOrDefault(x => x.Id == userId);
           //var achievement = user.Achievements.FirstOrDefault(y => y.Title == achievementTitle);

            return _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.FirstOrDefault(y => y.Title == achievementTitle) ?? null;  //a long line, just like my tralala


        }

        public async Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            await _databaseContext.SaveChangesAsync();
            return _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.ToList() ?? null;


        }

        public async Task UpdateAchievementByIdAndTitle(string achievementTitle, Guid userId, int currentvalue)
        {
            //get user instead, update the achievement current value locally and update the user instead of only achievement.

            var user = _databaseContext.Users.FirstOrDefault(x => x.Id == userId);
           // user.Achievements.ForEach(delegate (Achievement) { })

            Achievement achievement = _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.FirstOrDefault(y => y.Title == achievementTitle) ?? null;
            achievement.CurrentValue = currentvalue;
            _databaseContext.Update(achievement);
            await _databaseContext.SaveChangesAsync();
        }

    }
}
