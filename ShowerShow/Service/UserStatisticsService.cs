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

        public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository)
        {
            this.userStatisticsRepository = userStatisticsRepository;
        }

        public async Task<Dictionary<Guid, double>> GetFriendRanking(Guid userId, int limit)
        {
            return await userStatisticsRepository.GetFriendRanking(userId, limit);
        }

        public async Task<UserDashboard> GetUserDashboard(Guid userId, int amountOfDays)
        {
            return await userStatisticsRepository.GetUserDashboard(userId, amountOfDays);
        }
    }
}
