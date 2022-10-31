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
        private IUserService userService;

        public ShowerThoughtService(IShowerThoughtRepository showerThoughtRepository, IUserService userService)
        {
            this.showerThoughtRepository = showerThoughtRepository;
            this.userService = userService;
        }

        public async Task CreateShowerThought(ShowerThoughtDTO thought, Guid showerId, Guid userId)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            await showerThoughtRepository.CreateShowerThought(thought, showerId,userId);
        }

        public async Task DeleteShowerThought(Guid thoughtId)
        {
            await showerThoughtRepository.DeleteShowerThought(thoughtId);
        }

        public async Task<IEnumerable<ShowerThought>> GetAllShowerThoughtsForUser(Guid userId, int limit)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await showerThoughtRepository.GetAllShowerThoughtsForUser(userId,limit);
        }

        public async Task<ShowerThought> GetShowerThoughtById(Guid id)
        {
            return await showerThoughtRepository.GetShowerThoughtById(id);
        }

        public async Task<IEnumerable<ShowerThought>> GetShowerThoughtsByDate(DateTime date, Guid userId)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await showerThoughtRepository.GetShowerThoughtsByDate(date, userId);
        }

        public async Task<ShowerThought> GetThoughtByShowerId(Guid showerId)
        {
            return await showerThoughtRepository.GetThoughtByShowerId(showerId);
        }

        public async Task<IEnumerable<ShowerThought>> GetThoughtsByContent(string searchWord, Guid userId)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await showerThoughtRepository.GetThoughtsByContent(searchWord, userId);
        }

        public async Task<ShowerThought> UpdateThought(Guid thoughtId, UpdateShowerThoughtDTO updatedThought)
        {
            return await showerThoughtRepository.UpdateThought(thoughtId, updatedThought);
        }
    }
}
