using ExtraFunction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Repository_.Interface
{
    public interface IAchievementService
    {
        Task<List<Achievement>> GetAchievementsById(Guid userId); //getting a list of achievement
        Task<Achievement> GetAchievementByIdAndTitle(string achievementTitle, Guid userId); //getting a single achievement
        Task UpdateAchievementById(string achievementTitle, Guid userId, int currentValue);
    }
}
