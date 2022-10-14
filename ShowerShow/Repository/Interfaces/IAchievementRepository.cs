using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interfaces
{
    public interface IAchievementRepository
    {
        Task<List<Achievement>> GetAchievementsById(Guid userId); //getting a list of achievement
        Task<Achievement> GetAchievementById(string achievementTitle, Guid userId); //getting a single achievement
        Task UpdateAchievementById(string achievementTitle, Guid userId, int currentvalue);
  

    }
}
