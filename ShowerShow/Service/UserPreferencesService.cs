using ShowerShow.DTO;
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
    public class UserPreferencesService : IUserPrefencesService
    {
        private IUserPreferencesRepository _userPreferencesRepository;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
        {
            this._userPreferencesRepository = userPreferencesRepository;
        }
        public async Task CreateUserPreferences(CreatePreferencesDTO createPreferencesDTO)
        {
            await _userPreferencesRepository.CreateUserPreferences(createPreferencesDTO);
        }
        public async Task<bool> CheckIfUserExistAndActive(Guid userId)
        {
            return await _userPreferencesRepository.CheckIfUserExistAndActive(userId);
        }

        public async Task<IEnumerable<Preferences>> GetUserPreferencesById(Guid userId)
        {
            return await _userPreferencesRepository.GetUserPreferenceById(userId);
        }

        public async Task UpdatePreferenceById(Guid userId, UpdatePreferencesDTO updatePreferencesDTO)
        {
            await _userPreferencesRepository.UpdatePreferenceById(userId, updatePreferencesDTO);
        }
    }
}
