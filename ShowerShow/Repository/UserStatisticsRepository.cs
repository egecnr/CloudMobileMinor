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
            double litersAmount = await GetWaterUsage(userId, 7);
            ranking.Add(userId, litersAmount);

            foreach (GetUserFriendDTO friend in friends)
            {
                litersAmount = await GetWaterUsage(friend.FriendId, 7);
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
            return new UserDashboard()
            {
                UserId = userId,
                TotalWaterUsage = await GetWaterUsage(userId, amountOfDays),
                TotalGasUsage = await GetGasUsage(userId, amountOfDays),
                TotalPrice = await GetShowerPrice(userId, amountOfDays),
                AvgShowerTime = await GetAvgShowerTime(userId, amountOfDays),
                AvgShowerLiters = await GetAvgShowerLiters(userId, amountOfDays),
                AvgShowerGas = await GetAvgShowerGas(userId, amountOfDays),
                AvgShowerPrice  = await GetAvgShowerPrice(userId, amountOfDays),
            };
        }
        public async Task<double> GetWaterUsage(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double litersAmount = 0;
            foreach (ShowerData shower in showers)
                litersAmount += shower.WaterUsage;

            return litersAmount;
        }
        public async Task<double> GetGasUsage(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double gasAmount = 0;
            foreach (ShowerData shower in showers)
                gasAmount += shower.GasUsage;

            return gasAmount;
        }
        public async Task<double> GetShowerPrice(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double totalPrice = 0;
            foreach (ShowerData shower in showers)
                totalPrice += shower.WaterCost + shower.GasCost;

            return totalPrice;
        }

        public async Task<double> GetAvgShowerTime(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double avgShowerTime = 0;
            foreach (ShowerData shower in showers)
                avgShowerTime += shower.Duration;

            return avgShowerTime / showers.Count;
        }

        public async Task<double> GetAvgShowerLiters(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double avgShowerLiters = 0;
            foreach (ShowerData shower in showers)
                avgShowerLiters += shower.WaterUsage;

            return avgShowerLiters / showers.Count;
        }

        public async Task<double> GetAvgShowerGas(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double avgGasUsage = 0;
            foreach (ShowerData shower in showers)
                avgGasUsage += shower.GasUsage;

            return avgGasUsage / showers.Count;
        }

        public async Task<double> GetAvgShowerPrice(Guid userId, int amountOfDays)
        {
            DateTime minimumDate = await Task.FromResult(DateTime.Now.AddDays(-amountOfDays));
            List<ShowerData> showers = dbContext.ShowerInstances.Where(s => s.UserId == userId).Where(d => d.Date > minimumDate).ToList();
            double avgShowerPrice = 0;
            foreach (ShowerData shower in showers)
                avgShowerPrice += shower.WaterCost + shower.GasCost;

            return avgShowerPrice / showers.Count;
        }
    }
}
