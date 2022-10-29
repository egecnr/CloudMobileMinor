using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    internal class UserStatisticsRepository : IUserStatisticsRepository
    {
        private DatabaseContext dbContext;
        private IUserFriendRepository userFriendRepository;
        private IUserRepository userRepository;

        public UserStatisticsRepository(DatabaseContext dbContext, IUserRepository userRepository, IUserFriendRepository userFriendRepository)
        {
            this.dbContext = dbContext;
            this.userFriendRepository = userFriendRepository;
            this.userRepository = userRepository;
        }
        public async Task<Dictionary<Guid, double>> GetFriendRanking(Guid userId, int limit)
        {
            if (!await userRepository.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");
            // get all friends of user
            List<GetUserFriendDTO> friends = (List<GetUserFriendDTO>)await userFriendRepository.GetAllFriendsOfUser(userId);
            Dictionary<Guid, double> ranking = new Dictionary<Guid, double>();
            // get all showers of user for the past 7 days
            List<ShowerData> showers = GetUserShowers(userId, 7);

            // calculate total water usage for main user
            double litersAmount = 0;
            foreach (ShowerData shower in showers)
                litersAmount += shower.WaterUsage;

            ranking.Add(userId, litersAmount); // add main user (calling user) to ranking list

            foreach (GetUserFriendDTO friend in friends)
            {
                //loop through the user's friend
                //for each friend calculate total water usage for the past 7 days
                litersAmount = 0;
                showers = GetUserShowers(friend.FriendId, 7); // get showers for friend
                foreach (ShowerData shower in showers)
                    litersAmount += shower.WaterUsage;

                ranking.Add(friend.FriendId, litersAmount); // add to ranking list
            }
            // order the ranking list ascending based on water usage and convert back to dictionary
            Dictionary<Guid, double> rankingOrderedAscending = ranking.OrderByDescending(x => x.Value).Reverse().ToDictionary(k => k.Key, v => v.Value);
            ranking.Clear();

            int count = 0;
            // loop through the ordered list (dictionary), and add them to the original ranking list
            // human language = list; code language = dictionarý; 
            foreach (KeyValuePair<Guid, double> kvp in rankingOrderedAscending)
            {
                ranking.Add(kvp.Key, kvp.Value);
                if (++count >= limit) break; //stop when the limit is reached 
            }
            return ranking;
        }
        public async Task<UserDashboard> GetUserDashboard(Guid userId, int amountOfDays)
        {
            if (!await userRepository.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            List<ShowerData> showers = GetUserShowers(userId, amountOfDays); //get all showers for user for specified amount of days
                                                                             // init variables
            double litersAmount = 0, gasAmount = 0, totalPrice = 0, avgShowerLiters = 0, avgGasUsage = 0, avgShowerTime = 0, avgShowerPrice = 0, totalOvertime = 0;

            //loop through the user's showers and calculate the necessary variables
            foreach (ShowerData shower in showers)
            {
                litersAmount += shower.WaterUsage;
                gasAmount += shower.GasUsage;
                totalPrice += shower.WaterCost + shower.GasCost;
                avgShowerTime += shower.Duration;
                avgShowerLiters += shower.WaterUsage;
                avgGasUsage += shower.GasUsage;
                avgShowerPrice += shower.WaterCost + shower.GasCost;
                totalOvertime += shower.Overtime;

            }
            // init and return the completed dashboard
            return new UserDashboard()
            {
                UserId = userId,
                TotalWaterUsage = litersAmount,
                TotalGasUsage = gasAmount,
                TotalPrice = totalPrice,
                TotalOvertime = totalOvertime,
                AvgShowerTime = avgShowerTime / showers.Count,
                AvgShowerLiters = avgShowerLiters / showers.Count,
                AvgShowerGas = avgGasUsage / showers.Count,
                AvgShowerPrice = avgShowerPrice / showers.Count,
                AvgShowerOvertime = totalOvertime / showers.Count
            };
        }
        public List<ShowerData> GetUserShowers(Guid userId, int amountOfDays)
        {
            // set how far back you want the dashboard data to be 
            DateTime minimumDate = DateTime.Now.AddDays(-amountOfDays); //remove amount of days
            // return all showers for specified user that are more recent than the minimum date
            return dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
        }
    }
}
