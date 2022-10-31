using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ShowerShow.Model;

namespace ShowerShow.Repository
{
    internal class ShowerThoughtRepository : IShowerThoughtRepository
    {
        private DatabaseContext dbContext;

        public ShowerThoughtRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateShowerThought(ShowerThoughtDTO thought, Guid showerId, Guid userId)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<ShowerThoughtDTO, ShowerThought>()));
            ShowerThought fullThought = mapper.Map<ShowerThought>(thought);
            fullThought.ShowerId = showerId;
            fullThought.UserId = userId;

            dbContext.ShowerThoughts?.Add(fullThought);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteShowerThought(Guid thoughtId)
        {
            ShowerThought thought = null;

            //this is to give priority to tasks
            Task getId = Task.Run(() =>
            {
                thought = GetShowerThoughtById(thoughtId).Result; //get the thought
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.ShowerThoughts?.Remove(thought); // delete the thought
            });

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShowerThought>> GetAllShowerThoughtsForUser(Guid userId, int limit)
        {
            await dbContext.SaveChangesAsync();
            // get all thoughts with that USER id, and return only the limit amount
            return dbContext.ShowerThoughts.Where(x => x.UserId == userId).Take(limit).ToList();
        }

        public async Task<ShowerThought> GetShowerThoughtById(Guid id)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ShowerThoughts.FirstOrDefault(x => x.Id == id); //return thought by id
        }

        public async Task<IEnumerable<ShowerThought>> GetShowerThoughtsByDate(DateTime date, Guid userId)
        {
            await dbContext.SaveChangesAsync();
            // return all thoughts for user that are for the specified date
            // only year, month and day are relevant
            return dbContext.ShowerThoughts?.Where(x => x.UserId == userId).ToList()
                .Where(d => (d.DateTime.Year == date.Year)
                && (d.DateTime.Month == date.Month)
                && (d.DateTime.Day == date.Day)).ToList();
        }

        public async Task<ShowerThought> GetThoughtByShowerId(Guid showerId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ShowerThoughts.FirstOrDefault(x => x.ShowerId == showerId);
        }

        public async Task<IEnumerable<ShowerThought>> GetThoughtsByContent(string searchWord, Guid userId)
        {
            await dbContext.SaveChangesAsync();
            //search if the title or the content contains the search word
            return dbContext.ShowerThoughts?.Where(x => x.UserId == userId && (x.Title.ToLower().Contains(searchWord) || x.Text.ToLower().Contains(searchWord))).ToList();
        }

        public async Task<ShowerThought> UpdateThought(Guid thoughtId, UpdateShowerThoughtDTO updatedThought)
        {
            ShowerThought thought = null;
            //this is to give priority to tasks
            Task getId = Task.Run(() =>
            {
                // set the new thought
                thought = GetShowerThoughtById(thoughtId).Result;
                thought.IsPublic = updatedThought.IsPublic;
                thought.IsFavorite = updatedThought.IsFavorite;
            });
            await getId.ContinueWith(prev =>
            {
                dbContext.ShowerThoughts?.Update(thought);
            });

            await dbContext.SaveChangesAsync();
            return thought;
        }
    }
}
