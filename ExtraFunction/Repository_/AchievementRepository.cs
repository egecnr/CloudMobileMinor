using ExtraFunction.Repository_.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExtraFunction.Model_;
using ShowerShow.DAL;
using User = ShowerShow.Models.User;
using ShowerShow.Model;
//if I use ShowerShow.DAL I have access to DB context, all good. But then users gets fucked? then I can't have a user class in this solution so Idk what to do

namespace ExtraFunction.Repository_
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AchievementRepository(DatabaseContext databasecontext)
        {
            _databaseContext = databasecontext;
        }

        public async Task<Achievement> GetAchievementByTitle(string achievementTitle, Guid userId)
        {
            await _databaseContext.SaveChangesAsync();
            return _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.FirstOrDefault(y => y.Title == achievementTitle) ?? null; //a long line, just like my tralala

        }

        public async Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            await _databaseContext.SaveChangesAsync();
            return _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.ToList() ?? null;

        }

        public async Task UpdateAchievementById(string achievementTitle, Guid userId, int currentvalue)
        {

            Achievement achievement = _databaseContext.Users.FirstOrDefault(x => x.Id == userId)?.Achievements.FirstOrDefault(y => y.Title == achievementTitle) ?? null;
            achievement.CurrentValue = currentvalue;
            _databaseContext.Update(achievement);
            await _databaseContext.SaveChangesAsync();  
        }

    }
}
