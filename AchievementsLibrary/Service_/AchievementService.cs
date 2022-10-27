using ExtraFunction.Model_;
using ExtraFunction.Repository_.Interface;
using Microsoft.EntityFrameworkCore;
using ShowerShow.Repository.Interface;
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
