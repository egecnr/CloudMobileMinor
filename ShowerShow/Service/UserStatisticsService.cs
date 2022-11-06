using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class UserStatisticsService : IUserStatisticsService
    {
        private IUserStatisticsRepository userStatisticsRepository;
        private IUserService userService;

        public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository, IUserService userService)
        {
            this.userStatisticsRepository = userStatisticsRepository;
            this.userService = userService;
        }

        public async Task<Dictionary<Guid, double>> GetFriendRanking(Guid userId, int limit)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await userStatisticsRepository.GetFriendRanking(userId, limit);
        }

        public async Task<UserDashboard> GetUserDashboard(Guid userId, int amountOfDays)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await userStatisticsRepository.GetUserDashboard(userId, amountOfDays);
        }
    }
}
