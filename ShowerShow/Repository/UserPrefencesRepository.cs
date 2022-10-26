using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class  UserPrefencesRepository : IUserPreferencesRepository
    {
        private DatabaseContext _dbContext;
        public UserPrefencesRepository(DatabaseContext _dbContext)
        {

            this._dbContext = _dbContext;
        }


        public async Task CreateUserPreferences(Guid userId)
        {

            Preferences userPreferences = Preferences.ReturnDefaultPreference(userId);
            _dbContext.Preferences.Add(userPreferences);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Preferences>> GetUserPreferenceById(Guid userId)
        {
            await _dbContext.SaveChangesAsync();
            return _dbContext.Preferences.Where(x => x.UserId == userId);

        }
        public async Task UpdatePreferenceById(Guid userId, UpdatePreferencesDTO updatePreferencesDTO)
        {
            await _dbContext.SaveChangesAsync();
            Preferences preferences = _dbContext.Preferences.FirstOrDefault(u => u.UserId == userId);
            preferences.SelectedVoice = updatePreferencesDTO.SelectedVoice;
            preferences.SelectedLanguage = updatePreferencesDTO.SelectedLanguage;
            preferences.WaterType = updatePreferencesDTO.WaterType;
            preferences.Currency = updatePreferencesDTO.Currency;
            preferences.Theme = updatePreferencesDTO.Theme;
            preferences.Notification = updatePreferencesDTO.Notification;

            _dbContext.Preferences.Update(preferences);
            await _dbContext.SaveChangesAsync();

        }

    }
}
