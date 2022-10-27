using ExtraFunction.DAL;
using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Repository_
{
    internal class TermsAndConditionsRepository : ITermsAndConditionRepository
    {
        private DatabaseContext _dbContext;

        public TermsAndConditionsRepository(DatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TermsAndConditions> GetTermsAndConditions()
        {
             TermsAndConditions TermsAndConditions = _dbContext.TermsAndConditions.First();//Check if you need this as a parameter

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
