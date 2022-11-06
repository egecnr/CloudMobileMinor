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
        public Task CreateUserPreferences(Guid userId);
        public Task<PreferencesDTO> GetUserPreferencesById(Guid userId);
        public Task UpdatePreferenceById(Guid userId, PreferencesDTO updatePreferencesDTO);
    }
}
