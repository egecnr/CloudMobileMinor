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
        public Task<Achievement> GetAchievementById(string achievementTitle, Guid userId)
        {
            return _achievementRepository.GetAchievementById(achievementTitle, userId);
        }


        public Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            return _achievementRepository.GetAchievementsById(userId);
        }

        public Task<Achievement> UpdateAchievementById(Guid achievementId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
