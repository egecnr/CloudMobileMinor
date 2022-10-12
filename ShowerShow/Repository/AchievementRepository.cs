using ShowerShow.DAL;
using ShowerShow.Models;
using ShowerShow.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    internal class AchievementRepository : IAchievementRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AchievementRepository(DatabaseContext databasecontext)
        {
            _databaseContext = databasecontext;  
        }

        public Task<Achievement> GetAchievementById(Guid achievementId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Achievement> UpdateAchievementById(Guid achievementId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
