using ExtraFunction.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;

namespace ExtraFunction.Repository_
{
    public class DisclaimersRepository : IDisclaimersRepository
    {
        private DatabaseContext _dbContext;

        public DisclaimersRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Disclaimers> GetDisclaimers()
        {         
            Disclaimers disclaimers = _dbContext.Disclaimers.First();
            return disclaimers;
        }
        public async Task<Disclaimers> UpdateDisclaimers(Disclaimers disclaimers)
        {
            disclaimers = _dbContext.Disclaimers.FirstOrDefault();
            _dbContext.Update(disclaimers);
            return disclaimers;

        }
    }
}
