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
    public interface IUserStatisticsService
    {
        public Task<Dictionary<Guid, double>> GetFriendRanking(Guid userId, int limit);
        public Task<UserDashboard> GetUserDashboard(Guid userId, int amountOfDays);
    }
}
