using ExtraFunction.Model;
using ExtraFunction.Repository_;
using ExtraFunction.Repository_.Interface;
using Microsoft.Azure.Cosmos.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Service_
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserRepository _userRepository;

        public AchievementService(IAchievementRepository achievementrepository, IUserRepository userRepository)
        {
            _achievementRepository = achievementrepository;
            _userRepository = userRepository;
        }



        public async Task<Achievement> GetAchievementByIdAndTitle(string achievementTitle, Guid userId)
        {

            if (await _userRepository.CheckIfUserExistAndActive(userId))
            {
                return await _achievementRepository.GetAchievementByIdAndTitle(achievementTitle, userId);
            }


            else
            {
                throw new Exception("User does not exist");
            }

        }


        public async Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            if (await _userRepository.CheckIfUserExistAndActive(userId))
            {
                return await _achievementRepository.GetAchievementsById(userId);
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }

        public async Task UpdateAchievementByIdAndTitle(string achievementTitle, Guid userId, int currentValue)
        {
            if(await _userRepository.CheckIfUserExistAndActive(userId))
            {
                await _achievementRepository.UpdateAchievementByIdAndTitle(achievementTitle, userId, currentValue);
            }
            else
            {
                throw new Exception("User does not exist");
            }

            //await _achievementRepository.UpdateAchievementByIdAndTitle(achievementTitle, userId, currentValue);
        }


    }
}
