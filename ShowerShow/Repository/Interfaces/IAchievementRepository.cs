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
        Task<List<Achievement>> getAchievementsById(Guid userId); //getting a list of achievement
        Task<Achievement> getAchievementById(Guid achievementId); //getting a single achievement
        Task<Achievement> updateAchievementById(Guid achievementId, Guid userId);
  

    }
}
