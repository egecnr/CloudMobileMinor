using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

        public async Task DeleteShowerThought(ShowerThought thought)
        {
            dbContext.ShowerThoughts?.Remove(thought);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShowerThought>> GetAllShowerThoughtsForUser(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ShowerThoughts.Where(x => x.UserId == userId).ToList();
        }

        public async Task<ShowerThought> GetShowerThoughtById(Guid id)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.ShowerThoughts.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<ShowerThought>> GetShowerThoughtsByDate(DateTime date, Guid userId)
        {
            await dbContext.SaveChangesAsync();
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
            List<ShowerThought> resultsForTitle = dbContext.ShowerThoughts?.Where(x => x.UserId == userId && (x.Title.ToLower().Contains(searchWord) || x.Text.ToLower().Contains(searchWord))).ToList();
/*            List <ShowerThought> resultsForContent = dbContext.ShowerThoughts?.Where(x => x.Text.ToLower().Contains(searchWord)).ToList();

            List<ShowerThought> thoughts = new List<ShowerThought>();
            thoughts.AddRange(resultsForTitle);
            thoughts.AddRange(resultsForContent);*/
            return resultsForTitle;
        }

        public async Task UpdateThought(ShowerThought thought, UpdateShowerThoughtDTO updatedThought)
        {
            thought.IsPublic = updatedThought.IsPublic;
            thought.IsFavorite = updatedThought.IsFavorite;
            dbContext.ShowerThoughts?.Update(thought);
            await dbContext.SaveChangesAsync();
        }
    }
}
