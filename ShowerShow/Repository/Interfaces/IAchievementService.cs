using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interfaces
{
    internal interface IAchievementService
    {
        Task<List<Achievement>> GetAchievementsById(Guid userId); //getting a list of achievement
        Task<Achievement> GetAchievementById(Guid achievementId); //getting a single achievement
        Task<Achievement> UpdateAchievementById(Guid achievementId, Guid userId);
    }
}
