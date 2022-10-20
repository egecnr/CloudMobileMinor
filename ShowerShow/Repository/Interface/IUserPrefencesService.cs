using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserPrefencesService
    {
        public Task CreateUserPreferences(CreatePreferencesDTO createPreferencesDTO);
        public Task<bool> CheckIfUserExistAndActive(Guid userId);

        public Task<IEnumerable<Preferences>> GetUserPreferencesById(Guid userId);
        public Task UpdatePreferenceById(Guid userId, UpdatePreferencesDTO updatePreferencesDTO);
    }
}
