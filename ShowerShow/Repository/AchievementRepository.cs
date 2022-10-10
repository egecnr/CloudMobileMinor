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

        public Task<Achievement> getAchievementById(Guid achievementId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Achievement>> getAchievementsById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Achievement> updateAchievementById(Guid achievementId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
