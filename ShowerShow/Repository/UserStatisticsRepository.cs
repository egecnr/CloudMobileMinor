using Google.Protobuf.WellKnownTypes;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    internal class UserStatisticsRepository : IUserStatisticsRepository
    {
        private DatabaseContext dbContext;
        private IUserFriendRepository userFriendRepository;

        public UserStatisticsRepository(DatabaseContext dbContext, IUserFriendRepository userFriendRepository)
        {
            this.dbContext = dbContext;
            this.userFriendRepository = userFriendRepository;
        }
        public async Task<Dictionary<Guid, double>> GetFriendRanking(Guid userId, int limit)
        {
            List<GetUserFriendDTO> friends = (List<GetUserFriendDTO>)await userFriendRepository.GetAllFriendsOfUser(userId);
            Dictionary<Guid, double> ranking = new Dictionary<Guid, double>();
            List<ShowerData> showers = GetUserShowers(userId, 7);
            
            double litersAmount = 0;
            foreach (ShowerData shower in showers)
                litersAmount += shower.WaterUsage;

            ranking.Add(userId, litersAmount);

            foreach (GetUserFriendDTO friend in friends)
            {
                litersAmount = 0;
                showers = GetUserShowers(friend.FriendId, 7);
                foreach (ShowerData shower in showers)
                    litersAmount += shower.WaterUsage;

                ranking.Add(friend.FriendId, litersAmount);
            }
            Dictionary<Guid, double> rankingOrderedAscending = ranking.OrderByDescending(x => x.Value).Reverse().ToDictionary(k => k.Key, v => v.Value);
            ranking.Clear();

            int count = 0;
            foreach (KeyValuePair<Guid, double> kvp in rankingOrderedAscending)
            {
                ranking.Add(kvp.Key, kvp.Value);
                if (++count >= limit) break;
            }
            return ranking;
        }
        public async Task<UserDashboard> GetUserDashboard(Guid userId, int amountOfDays)
        {
            await dbContext.SaveChangesAsync();
            List<ShowerData> showers = GetUserShowers(userId, amountOfDays);
            double litersAmount = 0, gasAmount = 0, totalPrice = 0, avgShowerLiters = 0, avgGasUsage = 0, avgShowerTime = 0, avgShowerPrice = 0, totalOvertime = 0;

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
            DateTime minimumDate = DateTime.Now.AddDays(-amountOfDays); //remove amount of days
            return dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
        }
    }
}
