using ExtraFunction.Model_;
using Microsoft.EntityFrameworkCore;
using ShowerShow.DAL;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    internal class TermsAndConditionsRepository
    {
        private DatabaseContext _dbContext;

        public TermsAndConditionsRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TermsAndConditions> GetTermsAndConditions(TermsAndConditions termsAndConditions)
        {
            var TermsAndConditions = _dbContext.TermsAndConditions.FirstOrDefault(termsAndConditions);

            return TermsAndConditions;
        }
        public async Task<TermsAndConditions> UpdateTermsAndConditions(TermsAndConditions termsAndConditions)
        {
            termsAndConditions = _dbContext.TermsAndConditions.FirstOrDefault();
            _dbContext.Update(termsAndConditions);
            return termsAndConditions;

        }    
    }
}
