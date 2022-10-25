using ShowerShow.DAL;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class DisclaimersRepository
    {
        private DatabaseContext _dbContext;

        public DisclaimersRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Disclaimers> GetDisclaimres(Disclaimers disclaimers)
        {
            var Disclaimers = _dbContext.Disclaimers.FirstOrDefault(disclaimers);

            return Disclaimers;
        }
        public async Task<Disclaimers> UpdateDisclaimers(Disclaimers disclaimers)
        {
            disclaimers = _dbContext.Disclaimers.FirstOrDefault();
            _dbContext.Update(disclaimers);
            return disclaimers;

        }
    }
}
