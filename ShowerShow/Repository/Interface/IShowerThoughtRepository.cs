using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShowerShow.DTO;
using ShowerShow.Model;

namespace ShowerShow.Repository.Interface
{
    public interface IShowerThoughtRepository
    {
        public Task CreateShowerThought(ShowerThoughtDTO thought, Guid showerId, Guid userId);
        public Task<ShowerThought> GetThoughtByShowerId(Guid showerId);
        public Task<IEnumerable<ShowerThought>> GetAllShowerThoughtsForUser(Guid userId,uint limit);
        public Task<IEnumerable<ShowerThought>> GetShowerThoughtsByDate(DateTime date, Guid userId);
        public Task DeleteShowerThought(Guid thoughtId);
        public Task<ShowerThought> GetShowerThoughtById(Guid id);
        public Task<IEnumerable<ShowerThought>> GetThoughtsByContent(string searchWord, Guid userId);
        public Task<ShowerThought> UpdateThought(Guid thoughtId, UpdateShowerThoughtDTO updatedThought);
    }
}
