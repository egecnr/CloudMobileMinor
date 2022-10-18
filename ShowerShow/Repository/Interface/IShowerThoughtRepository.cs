using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowerShow.DTO;
using ShowerShow.Model;

namespace ShowerShow.Repository.Interface
{
    public interface IShowerThoughtRepository
    {
        public Task CreateShowerThought(ShowerThoughtDTO thought, Guid showerId, Guid userId);
        public Task<ShowerThought> GetThoughtByShowerId(Guid showerId);
        public Task<IEnumerable<ShowerThought>> GetAllShowerThoughtsForUser(Guid userId);
        public Task<IEnumerable<ShowerThought>> GetShowerThoughtsByDate(DateTime date, Guid userId);
        public Task DeleteShowerThought(ShowerThought thought);
        public Task<ShowerThought> GetShowerThoughtById(Guid id);
    }
}
