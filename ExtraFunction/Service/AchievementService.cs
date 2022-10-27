using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Service_
{
    internal class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserRepository _userRepository;

        public AchievementService(IAchievementRepository achievementrepository, IUserRepository userRepository)
        {
            _achievementRepository = achievementrepository;
            _userRepository = userRepository;
        }



        public Task<Achievement> GetAchievementByIdAndTitle(string achievementTitle, Guid userId)
        {
            return _achievementRepository.GetAchievementByIdAndTitle(achievementTitle, userId);
        }


        public Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            return _achievementRepository.GetAchievementsById(userId);
        }

        public async Task UpdateAchievementById(string achievementTitle, Guid userId, int currentValue)
        {
            await _achievementRepository.UpdateAchievementByIdAndTitle(achievementTitle, userId, currentValue);
        }

    }
}
