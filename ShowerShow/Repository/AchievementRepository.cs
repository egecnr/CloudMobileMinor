using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
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

        public Task<Achievement> GetAchievementById(string achievementTitle, Guid userId)
        {


            var ach =  _databaseContext.Users.Where(x => x.Id == userId).Include(y => y.Achievements.Where(z => z.Title == achievementTitle));
            

            return (Task<Achievement>)ach;
        }

        public async Task<List<Achievement>> GetAchievementsById(Guid userId)
        {
            var res = _databaseContext.Users.Where(x => x.Id == userId).Include(y => y.Achievements);
            return (List<Achievement>)res;
        }

        public async Task<Achievement> UpdateAchievementById(Guid achievementId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
