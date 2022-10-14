using ShowerShow.Models;
using ShowerShow.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    internal class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;

        public AchievementService(IAchievementRepository achievementrepository)
        {
            _achievementRepository = achievementrepository;
        }
        public Task<Achievement> GetAchievementByTitle(string achievementTitle, Guid userId)
        {
            return _achievementRepository.GetAchievementByTitle(achievementTitle, userId);
        }


        public Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            return _achievementRepository.GetAchievementsById(userId);
        }

        public Task UpdateAchievementById(string achievementTitle, Guid userId, int currentValue)
        {
            return _achievementRepository.UpdateAchievementById(achievementTitle, userId, currentValue);
        }
    }
}
