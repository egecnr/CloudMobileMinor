using Microsoft.EntityFrameworkCore;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class UserPreferencesService : IUserPrefencesService
    {
        private IUserPreferencesRepository _userPreferencesRepository;
        private IUserService userService;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository, IUserService userService)
        {
            this._userPreferencesRepository = userPreferencesRepository;
            this.userService = userService;
        }
        public async Task CreateUserPreferences(Guid userId)
        {
            await _userPreferencesRepository.CreateUserPreferences(userId);
        }
      


        public async Task<PreferencesDTO> GetUserPreferencesById(Guid userId)
        {
            if (await userService.CheckIfUserExistAndActive(userId))
            {
               return await _userPreferencesRepository.GetUserPreferenceById(userId);
              
            }
            else
            {
                throw new Exception("User doesn't exist");
            }
        }

        public async Task UpdatePreferenceById(Guid userId, PreferencesDTO updatePreferencesDTO)
        {
            if (await userService.CheckIfUserExistAndActive(userId))
            {
                await _userPreferencesRepository.UpdatePreferenceById(userId, updatePreferencesDTO);
            }
            else
            {
                throw new Exception("User doesn't exist");
            }
        }
    }
}
