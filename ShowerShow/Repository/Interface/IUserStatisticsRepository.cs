using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserStatisticsRepository
    {

        public Task<Dictionary<Guid, double>> GetFriendRanking(Guid userId, int limit);
        public Task<UserDashboard> GetUserDashboard(Guid userId, int amountOfDays);
        public Task<double> GetWaterUsage(Guid userId, int amountOfDays);
        public Task<double> GetGasUsage(Guid userId, int amountOfDays);
        public Task<double> GetShowerPrice(Guid userId, int amountOfDays);
        public Task<double> GetAvgShowerTime(Guid userId, int amountOfDays); //in seconds
        public Task<double> GetAvgShowerLiters(Guid userId, int amountOfDays);
        public Task<double> GetAvgShowerGas(Guid userId, int amountOfDays);
        public Task<double> GetAvgShowerPrice(Guid userId, int amountOfDays);

    }
}
