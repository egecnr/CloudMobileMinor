using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    internal class ShowerThoughtService : IShowerThoughtService
    {
        private IShowerThoughtRepository showerThoughtRepository;

        public ShowerThoughtService(IShowerThoughtRepository showerThoughtRepository)
        {
            this.showerThoughtRepository = showerThoughtRepository;
        }

        public async Task CreateShowerThought(ShowerThoughtDTO thought, Guid showerId, Guid userId)
        {
            await showerThoughtRepository.CreateShowerThought(thought, showerId,userId);
        }

        public async Task DeleteShowerThought(ShowerThought thought)
        {
            await showerThoughtRepository.DeleteShowerThought(thought);
        }

        public async Task<IEnumerable<ShowerThought>> GetAllShowerThoughtsForUser(Guid userId)
        {
           return await showerThoughtRepository.GetAllShowerThoughtsForUser(userId);
        }

        public async Task<ShowerThought> GetShowerThoughtById(Guid id)
        {
            return await showerThoughtRepository.GetShowerThoughtById(id);
        }

        public async Task<IEnumerable<ShowerThought>> GetShowerThoughtsByDate(DateTime date, Guid userId)
        {
            return await showerThoughtRepository.GetShowerThoughtsByDate(date, userId);
        }

        public async Task<ShowerThought> GetThoughtByShowerId(Guid showerId)
        {
            return await showerThoughtRepository.GetThoughtByShowerId(showerId);
        }
    }
}
